using Microsoft.AspNetCore.Mvc;

namespace TennisScoreboard
{
    [Route("matches")]
    public class MatchesController : Controller
    {
        private readonly MatchPageViewService _matchPageViewService;
        private readonly ILogger<MatchesController> _logger;

        public MatchesController(MatchPageViewService matchPageViewService, ILogger<MatchesController> logger)
        {
            _matchPageViewService = matchPageViewService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Show(string page, string playerNameFilter)
        {
            MatchPageViewDto matchPage;

            try
            {
                matchPage = _matchPageViewService.GetPageWithFilter(page, playerNameFilter);
            }
            catch (IllegalPlayerNameFilterException ex)
            {
                _logger.LogWarning("Incorrect or empty playerNameFilter: {}", playerNameFilter);
                matchPage = _matchPageViewService.GetPageWithoutFilter(page);
                ViewData["ErrorMessage"] = ex.Message;
            }
            return View("Matches", matchPage);
        }
    }
}


