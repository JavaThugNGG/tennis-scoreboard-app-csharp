namespace TennisScoreboard
{
    public record MatchPageViewDto(
        IDictionary<String, String> MatchAttributes,
        int CurrentPage,
        int TotalPages
    );
}
