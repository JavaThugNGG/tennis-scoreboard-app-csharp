using Microsoft.AspNetCore.Mvc;

namespace TennisScoreboard
{
    [Route("matches")]
    public class MatchesController : Controller
    {
        private readonly MatchPageViewService _matchPageViewService;

        public MatchesController(MatchPageViewService matchPageViewService)
        {
            _matchPageViewService = matchPageViewService;
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
                //лог
                matchPage = _matchPageViewService.GetPageWithoutFilter(page);
                Response.StatusCode = 400;
                ViewData["ErrorMessage"] = ex.Message;
            }
            return View("Matches", matchPage);
        }
    }
}


