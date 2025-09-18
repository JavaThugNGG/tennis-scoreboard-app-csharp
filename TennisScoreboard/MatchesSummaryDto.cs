namespace TennisScoreboard
{
    public record MatchesSummaryDto(IList<MatchEntity> Matches, int TotalCount);
}
