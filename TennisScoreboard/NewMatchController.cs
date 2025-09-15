using Microsoft.AspNetCore.Mvc;

namespace TennisScoreboard
{
    [Route("new-match")]
    public class NewMatchController : Controller
    {
        private readonly PlayerValidator _playerValidator;
        private readonly ErrorDtoBuilder _errorDtoBuilder;
        private readonly MatchPreparingService _matchPreparingService;
        private readonly OngoingMatchesService _ongoingMatchesService;

        public NewMatchController(PlayerValidator playerValidator, ErrorDtoBuilder errorDtoBuilder, MatchPreparingService matchPreparingService, OngoingMatchesService ongoingMatchesService)
        {
            _playerValidator = playerValidator;
            _errorDtoBuilder = errorDtoBuilder;
            _matchPreparingService = matchPreparingService;
            _ongoingMatchesService = ongoingMatchesService;
        }

        [HttpGet]
        public IActionResult Index() => View("NewMatch");

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

                return Redirect($"/match-score?uuid={pastedMatchId}");//перепишешь потом через RedirectToAction когда контроллер другой появится
            }
            catch (IllegalPlayerNameException e)
            {
                ErrorDto error = _errorDtoBuilder.Build(e);

                ViewData["ErrorMessage"] = error.Message;

                //тут логгер

                return View("NewMatch");
            }
        }
    }
}
