using Microsoft.EntityFrameworkCore;

namespace TennisScoreboard
{
    public class MatchFinishingService
    {
        private readonly IDbContextFactory<AppDbContext> _contextFactory;

        public MatchFinishingService(IDbContextFactory<AppDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public PlayersResultDto PersistMatch(MatchScoreModel match, PlayerSide winner)
        {
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

                Console.WriteLine($"Match persisted successfully: matchId={matchEntity.Id}, winner={winner}");

                return DeterminePlayersResult(winner);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Error persisting match between players {firstPlayerId} and {secondPlayerId}: {e.Message}");
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
