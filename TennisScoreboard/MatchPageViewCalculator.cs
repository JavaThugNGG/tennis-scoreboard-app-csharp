namespace TennisScoreboard
{
    public class MatchPageViewCalculator
    {
        private readonly int _matchesPerPage;

        public MatchPageViewCalculator(int matchesPerPage)
        {
            _matchesPerPage = matchesPerPage;
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
