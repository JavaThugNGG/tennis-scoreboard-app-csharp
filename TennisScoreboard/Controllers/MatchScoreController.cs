using Microsoft.AspNetCore.Mvc;
using TennisScoreboard.Dto;
using TennisScoreboard.Exceptions;
using TennisScoreboard.Models;
using TennisScoreboard.Parsers;
using TennisScoreboard.Processors;
using TennisScoreboard.Services;
using TennisScoreboard.Validators;

namespace TennisScoreboard.Controllers
{
    [Route("match-score")]
    public class MatchScoreController : Controller
    {
        private readonly OngoingMatchesService _ongoingMatchesService;
        private readonly MatchStateService _matchStateService;
        private readonly MatchScoreCalculationService _matchScoreCalculationService;
        private readonly FinishedMatchProcessingService _finishedMatchProcessingService;

        private readonly MatchValidator _matchValidator;
        private readonly PlayerValidator _playerValidator;

        private readonly MatchParser _matchParser;
        private readonly PlayerParser _playerParser;

        private readonly MatchProcessor _matchProcessor;
        private readonly PlayerProcessor _playerProcessor;

        private readonly ILogger<MatchScoreController> _logger;

        public MatchScoreController(OngoingMatchesService ongoingMatchesService, MatchStateService matchStateService,
            MatchScoreCalculationService matchScoreCalculationService,
            FinishedMatchProcessingService finishedMatchProcessingService, MatchValidator matchValidator,
            PlayerValidator playerValidator,
            MatchParser matchParser, PlayerParser playerParser, MatchProcessor matchProcessor,
            PlayerProcessor playerProcessor, ILogger<MatchScoreController> logger)
        {
            _ongoingMatchesService = ongoingMatchesService;
            _matchStateService = matchStateService;
            _matchScoreCalculationService = matchScoreCalculationService;
            _finishedMatchProcessingService = finishedMatchProcessingService;
            _matchValidator = matchValidator;
            _playerValidator = playerValidator;
            _matchParser = matchParser;
            _playerParser = playerParser;
            _matchProcessor = matchProcessor;
            _playerProcessor = playerProcessor;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult ShowScore(string guid)
        {
            try
            {
                _matchValidator.ValidateGuid(guid);
                Guid parsedGuid = _matchParser.ParseGuid(guid);
                MatchScoreModel currentMatch = _matchProcessor.FindMatch(_ongoingMatchesService.CurrentMatches, parsedGuid);
                return View(currentMatch);
            }
            catch (ArgumentException ex)
            {
                return HandleIllegalArguments(ex);
            }
        }

        [HttpPost]
        public IActionResult Scoring(string guid, string scoredPlayerId)
        {
            Guid matchGuid;
            int scoredId;

            try
            {
                _matchValidator.ValidateGuid(guid);
                matchGuid = _matchParser.ParseGuid(guid);
                _playerValidator.ValidateId(scoredPlayerId);
                scoredId = _playerParser.ParseId(scoredPlayerId);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("incorrect match uuid or scoredId: {} {}", guid, scoredPlayerId);
                return HandleIllegalArguments(ex);
            }

            IReadOnlyDictionary<Guid, MatchScoreModel> currentMatches = _ongoingMatchesService.CurrentMatches;
            MatchScoreModel currentMatch = _matchProcessor.FindMatch(currentMatches, matchGuid);
            PlayerSide scorerSide = _playerProcessor.DetermineScorerSide(currentMatch, scoredId);

            try
            {       
                lock (currentMatch.MatchStateLock)
                {
                    _matchScoreCalculationService.Scoring(currentMatch, scorerSide);
                    _matchStateService.EnsureNotFinished(currentMatch);
                    _logger.LogInformation("match is scoring: first player id {}, second player id {}, scoring by id: {}", currentMatch.FirstPlayerId, currentMatch.SecondPlayerId, scoredPlayerId);
                    return RedirectToAction("ShowScore", new { guid = matchGuid });
                }
            }
            catch (MatchAlreadyFinishedException)
            {
                return HandleFinishedMatch(currentMatch, scorerSide, matchGuid);
            }
        }

        private IActionResult HandleFinishedMatch(MatchScoreModel currentMatch, PlayerSide scorerSide, Guid matchGuid)
        {
            lock (currentMatch.MatchStateLock)
            {
                FinishedMatchViewDto finishedMatch = _finishedMatchProcessingService.HandleFinishedMatch(currentMatch, scorerSide, matchGuid);
                currentMatch.Finished = true;
                _logger.LogInformation("match is finished: first player id {}, second player id {}", finishedMatch.CurrentMatch.FirstPlayerId, finishedMatch.CurrentMatch.SecondPlayerId);
                return View("MatchResult", finishedMatch);
            }
        }

        private IActionResult HandleIllegalArguments(Exception ex)
        {
            ViewData["ErrorMessage"] = ex.Message;
            return View();
        }
    }
}
