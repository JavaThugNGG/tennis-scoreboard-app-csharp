using TennisScoreboard.Entities;

namespace TennisScoreboard.Dto
{
    public record MatchesSummaryDto(IList<MatchEntity> Matches, int TotalCount);
}
