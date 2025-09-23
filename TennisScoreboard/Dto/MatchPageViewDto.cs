namespace TennisScoreboard.Dto
{
    public record MatchPageViewDto(IDictionary<string, string> MatchAttributes, int CurrentPage, int TotalPages);
}
