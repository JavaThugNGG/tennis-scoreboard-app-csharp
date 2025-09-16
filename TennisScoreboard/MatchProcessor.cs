namespace TennisScoreboard
{
    public class MatchProcessor
    {
        public MatchScoreModel FindMatch(IReadOnlyDictionary<Guid, MatchScoreModel> currentMatches, Guid guid)
        {
            MatchScoreModel currentMatch = currentMatches[guid];
            if (currentMatch == null)
            {
                //лог ошибки
                throw new PlayerNotFoundException("Матч не найден! uuid матча: " + guid);
            }

            return currentMatch;
        }
    }
}
