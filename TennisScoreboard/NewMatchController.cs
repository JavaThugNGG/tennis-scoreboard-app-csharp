using Microsoft.AspNetCore.Mvc;

namespace TennisScoreboard
{
    [Route("new-match")]
    public class NewMatchController : Controller
    {
        private readonly PlayerValidator _playerValidator;
        private readonly MatchPreparingService _matchPreparingService;
        private readonly OngoingMatchesService _ongoingMatchesService;
        private readonly ILogger<NewMatchController> _logger;

        public NewMatchController(PlayerValidator playerValidator, MatchPreparingService matchPreparingService, OngoingMatchesService ongoingMatchesService, ILogger<NewMatchController> logger)
        {
            _playerValidator = playerValidator;
            _matchPreparingService = matchPreparingService;
            _ongoingMatchesService = ongoingMatchesService;
            _logger = logger;
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
                _logger.LogInformation("Match was created, id: {}, first player id: {}, second player id: {}", pastedMatchId, matchScoreModel.FirstPlayerId, matchScoreModel.SecondPlayerId);
                return RedirectToAction("ShowScore", "MatchScore", new { guid = pastedMatchId });
            }
            catch (IllegalPlayerNameException ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                _logger.LogWarning(ex, "Incorrect name of first or second player: {}, {}", playerOne, playerTwo);
                return View("NewMatch");
            }
        }
    }
}
