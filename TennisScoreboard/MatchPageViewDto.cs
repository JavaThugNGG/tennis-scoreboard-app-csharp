namespace TennisScoreboard
{
    public record MatchPageViewDto(IDictionary<string, string> MatchAttributes, int CurrentPage, int TotalPages);
}
