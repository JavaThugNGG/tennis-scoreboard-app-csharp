using TennisScoreboard.Entities;

namespace TennisScoreboard.Dto
{
    public class MatchPageViewDtoBuilder
    {
        public MatchPageViewDto Build(IList<MatchEntity> matches, int currentPage, int totalPages)
        {
            IDictionary<string, string> matchAttributes;

            if (matches.Count == 0)
            {
                matchAttributes = new Dictionary<string, string>();
            }
            else
            {
                matchAttributes = GetAttributes(matches);
            }

            return new MatchPageViewDto(MatchAttributes: matchAttributes, CurrentPage: currentPage,
                TotalPages: totalPages);
        }

        private IDictionary<string, string> GetAttributes(IList<MatchEntity> matches)
        {
            var attributes = new Dictionary<string, string>();

            for (int i = 0; i < matches.Count; i++)
            {
                var match = matches[i];
                attributes[$"firstPlayerName{i + 1}"] = match.Player1?.Name ?? "empty";
                attributes[$"secondPlayerName{i + 1}"] = match.Player2?.Name ?? "empty";
                attributes[$"winnerName{i + 1}"] = match.Winner?.Name ?? "empty";
            }

            return attributes;
        }
    }
}
