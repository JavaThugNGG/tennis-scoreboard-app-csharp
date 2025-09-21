using Microsoft.AspNetCore.Mvc;

namespace TennisScoreboard
{
    [Route("new-match")]
    public class NewMatchController : Controller
    {
        private readonly PlayerValidator _playerValidator;
        private readonly MatchPreparingService _matchPreparingService;
        private readonly OngoingMatchesService _ongoingMatchesService;

        public NewMatchController(PlayerValidator playerValidator, MatchPreparingService matchPreparingService, OngoingMatchesService ongoingMatchesService)
        {
            _playerValidator = playerValidator;
            _matchPreparingService = matchPreparingService;
            _ongoingMatchesService = ongoingMatchesService;
        }

        [HttpGet]
        public IActionResult ShowForm() => View("NewMatch");

        [HttpPost]
        public IActionResult Create(string playerOne, string playerTwo)
        {
            try
            {
                _playerValidator.ValidateName(playerOne);
                _playerValidator.ValidateName(playerTwo);

                MatchScoreModel matchScoreModel = _matchPreparingService.PersistPlayers(playerOne, playerTwo);
                Guid pastedMatchId = _ongoingMatchesService.AddMatch(matchScoreModel);
                //тут логгер
                return RedirectToAction("ShowScore", "MatchScore", new { guid = pastedMatchId });
            }
            catch (IllegalPlayerNameException ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                //тут логгер
                return View("NewMatch");
            }
        }
    }
}
