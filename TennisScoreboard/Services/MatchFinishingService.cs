using Microsoft.EntityFrameworkCore;
using TennisScoreboard.Dto;
using TennisScoreboard.Entities;
using TennisScoreboard.Infrastructure;
using TennisScoreboard.Models;

namespace TennisScoreboard.Services
{
    public class MatchFinishingService
    {
        private readonly IDbContextFactory<AppDbContext> _contextFactory;
        private readonly OngoingMatchesService _ongoingMatchesService;
        private readonly ILogger<MatchFinishingService> _logger;

        public MatchFinishingService(IDbContextFactory<AppDbContext> contextFactory, OngoingMatchesService ongoingMatchesService, ILogger<MatchFinishingService> logger)
        {
            _contextFactory = contextFactory;
            _ongoingMatchesService = ongoingMatchesService;
            _logger = logger;
        }

        public PlayersResultDto PersistMatch(MatchScoreModel match, PlayerSide winner, Guid matchGuid)
        {
            if (_ongoingMatchesService.PersistedMatches.ContainsKey(matchGuid))
            {
                return DeterminePlayersResult(winner);
            }

            int firstPlayerId = match.FirstPlayerId;
            int secondPlayerId = match.SecondPlayerId;

            using var context = _contextFactory.CreateDbContext();

            try
            {
                PlayerEntity firstPlayer = context.Players.Find(firstPlayerId);
                PlayerEntity secondPlayer = context.Players.Find(secondPlayerId);

                MatchEntity matchEntity = BuildMatch(firstPlayer, secondPlayer, winner);

                context.Matches.Add(matchEntity);
                context.SaveChanges();

                _ongoingMatchesService.MarkMatchAsPersisted(matchGuid);
                Console.WriteLine($"Match persisted successfully: matchId={matchEntity.Id}, winner={winner}");

                return DeterminePlayersResult(winner);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error persisting match between players {} and {}", firstPlayerId, secondPlayerId);
                throw;
            }
        }

        private MatchEntity BuildMatch(PlayerEntity firstPlayer, PlayerEntity secondPlayer, PlayerSide winner)
        {
            MatchEntity matchEntity = new MatchEntity();
            matchEntity.Player1 = firstPlayer;
            matchEntity.Player2 = secondPlayer;

            if (winner == PlayerSide.First)
            {
                matchEntity.Winner = firstPlayer;
            }
            else
            {
                matchEntity.Winner = secondPlayer;
            }

            return matchEntity;
        }

        private PlayersResultDto DeterminePlayersResult(PlayerSide winner)
        {
            if (winner == PlayerSide.First)
            {
                return new PlayersResultDto("WIN", "LOS");
            }
            else
            {
                return new PlayersResultDto("LOS", "WIN");
            }
        }
    }
}
