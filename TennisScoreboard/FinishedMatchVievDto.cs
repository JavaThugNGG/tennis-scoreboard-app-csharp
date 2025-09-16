namespace TennisScoreboard
{
    public class FinishedMatchViewDto
    {
        public MatchScoreModel CurrentMatch { get; init; }
        public string FirstPlayerResult { get; init; }
        public string SecondPlayerResult { get; init; }

        public FinishedMatchViewDto(MatchScoreModel currentMatch, PlayersResultDto playersResult)
        {
            CurrentMatch = currentMatch;
            FirstPlayerResult = playersResult.FirstPlayerResult;
            SecondPlayerResult = playersResult.SecondPlayerResult;
        }
    }
}
