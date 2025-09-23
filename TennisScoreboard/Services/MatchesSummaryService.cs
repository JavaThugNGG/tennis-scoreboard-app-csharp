using Microsoft.Extensions.Options;
using TennisScoreboard.Dto;
using TennisScoreboard.Entities;
using TennisScoreboard.Infrastructure;
using TennisScoreboard.Validators;

namespace TennisScoreboard.Services
{
    public class MatchesSummaryService
    {
        private readonly int _matchesPerPage;

        private readonly MatchService _matchService;
        private readonly PlayerService _playerService;
        private readonly PlayerValidator _playerValidator;

        public MatchesSummaryService(IOptions<PaginationSettings> matchesPerPage, MatchService matchService, PlayerService playerService, PlayerValidator playerValidator)
        {
            _matchesPerPage = matchesPerPage.Value.MatchesPerPage;
            _matchService = matchService;
            _playerService = playerService;
            _playerValidator = playerValidator;
        }

        public MatchesSummaryDto GetWithoutFilter(int paginationStartIndex)
        {
            return new MatchesSummaryDto(_matchService.GetPage(_matchesPerPage, paginationStartIndex), (int)_matchService.Count());
        }

        public MatchesSummaryDto GetWithFilter(string playerNameFilter, int paginationStartIndex)
        {
            _playerValidator.ValidateNameFilter(playerNameFilter);
            PlayerEntity player = _playerService.GetByName(playerNameFilter);

            return new MatchesSummaryDto(_matchService.GetPageWithPlayerFilter(player, _matchesPerPage, paginationStartIndex), (int)_matchService.CountWithPlayerFilter(player));
        }
    }
}
