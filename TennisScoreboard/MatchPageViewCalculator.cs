using Microsoft.Extensions.Options;
using TennisScoreboard.Infrastructure;

namespace TennisScoreboard
{
    public class MatchPageViewCalculator
    {
        private readonly int _matchesPerPage;

        public MatchPageViewCalculator(IOptions<PaginationSettings> matchesPerPage)
        {
            _matchesPerPage = matchesPerPage.Value.MatchesPerPage;
        }

        public int GetStartIndex(int currentPage)
        {
            return (currentPage - 1) * _matchesPerPage;
        }

        public int GetTotalPages(int totalItems)
        {
            int totalPages = (int)Math.Ceiling((double)totalItems / _matchesPerPage);
            return Math.Max(1, totalPages);
        }
    }
}
