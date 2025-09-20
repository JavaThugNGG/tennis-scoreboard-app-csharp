namespace TennisScoreboard
{
    public class MatchPageViewService
    {
        private readonly PageProcessor _pageProcessor;
        private readonly MatchPageViewDtoBuilder _matchPageViewDtoBuilder;
        private readonly MatchPageViewCalculator _matchPageViewCalculator;
        private readonly MatchesSummaryService _matchesSummaryService;
        private readonly PlayerValidator _playerValidator;

        public MatchPageViewService(PageProcessor pageProcessor, MatchPageViewDtoBuilder matchPageViewDtoBuilder, MatchPageViewCalculator matchPageViewCalculator,
            MatchesSummaryService matchesSummaryService, PlayerValidator playerValidator)
        {
            _pageProcessor = pageProcessor;
            _matchPageViewDtoBuilder = matchPageViewDtoBuilder;
            _matchPageViewCalculator = matchPageViewCalculator;
            _matchesSummaryService = matchesSummaryService;
            _playerValidator = playerValidator;
        }

        public MatchPageViewDto GetPage(string page, string? playerNameFilter)
        {
            int currentPage = _pageProcessor.DeterminePage(page);
            int paginationStartIndex = _matchPageViewCalculator.GetStartIndex(currentPage);

            MatchesSummaryDto matchesWithCount;

            if (playerNameFilter == null)
            {
                matchesWithCount = _matchesSummaryService.GetWithoutFilter(paginationStartIndex);
            }
            else
            {
                _playerValidator.ValidateNameFilter(playerNameFilter);
                matchesWithCount = _matchesSummaryService.GetWithFilter(playerNameFilter, paginationStartIndex);
            }

            IList<MatchEntity> matches = matchesWithCount.Matches;
            int count = matchesWithCount.TotalCount;
            int totalPages = _matchPageViewCalculator.GetTotalPages(count);

            return _matchPageViewDtoBuilder.Build(matches, currentPage, totalPages);
        }
    }
}
