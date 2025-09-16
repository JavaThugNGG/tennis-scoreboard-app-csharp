using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace TennisScoreboard
{
    public class FinishedMatchProcessingService
    {
        private readonly MatchFinishingService _matchFinishingService;
        private readonly OngoingMatchesService _ongoingMatchesService;

        public FinishedMatchProcessingService(MatchFinishingService matchFinishingService, OngoingMatchesService ongoingMatchesService)
        {
            _matchFinishingService = matchFinishingService;
            _ongoingMatchesService = ongoingMatchesService;
        }

        public FinishedMatchViewDto HandleFinishedMatch(MatchScoreModel currentMatch, PlayerSide winnerSide, Guid matchGuid)
        {
            PlayersResultDto playersResult = _matchFinishingService.PersistMatch(currentMatch, winnerSide);
            _ongoingMatchesService.RemoveMatchWithDelay(matchGuid, 1);
            return new FinishedMatchViewDto(currentMatch, playersResult);
        }
    }
}
