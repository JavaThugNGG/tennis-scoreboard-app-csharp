namespace TennisScoreboard
{
    public class MatchProcessor
    {
        private readonly ILogger<MatchProcessor> _logger;

        public MatchProcessor(ILogger<MatchProcessor> logger)
        {
            _logger = logger;
        }

        public MatchScoreModel FindMatch(IReadOnlyDictionary<Guid, MatchScoreModel> currentMatches, Guid guid)
        {
            MatchScoreModel currentMatch = currentMatches[guid];

            if (currentMatch == null)
            {
                _logger.LogError("match not found in current matches, uuid: {}", guid);
                throw new PlayerNotFoundException("Матч не найден! uuid матча: " + guid);
            }

            return currentMatch;
        }
    }
}
