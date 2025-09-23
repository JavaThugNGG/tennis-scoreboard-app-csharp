using TennisScoreboard.Models;
using TennisScoreboard.Processors;

namespace TennisScoreboard.Services
{
    public class MatchScoreCalculationService
    {
        private static readonly int DefaultGames = 0;
        private static readonly int GamesToWinSet = 6;
        private static readonly int GamesWinDiff = 2;

        private static readonly int TiebreakPointsToWin = 7;

        public void Scoring(MatchScoreModel match, PlayerSide scorerSide)
        {
            PlayerSide opponentSide = PlayerSideProcessor.OpponentOf(scorerSide);
            var scorer = new MatchScoreModelWrapper(match, scorerSide);
            var opponent = new MatchScoreModelWrapper(match, opponentSide);

            if (match.Tiebreak)
            {
                HandleTiebreak(match, scorer, opponent);
                return;
            }
            UpdatePoints(scorer, opponent, match);
        }

        private void UpdatePoints(MatchScoreModelWrapper scorer, MatchScoreModelWrapper opponent, MatchScoreModel match)
        {
            if (scorer.Points == TennisPoint.Love)
            {
                scorer.Points = TennisPoint.Fifteen;
            }
            else if (scorer.Points == TennisPoint.Fifteen)
            {
                scorer.Points = TennisPoint.Thirty;
            }
            else if (scorer.Points == TennisPoint.Thirty)
            {
                scorer.Points = TennisPoint.Forty;
            }
            else if (scorer.Points == TennisPoint.Forty)
            {
                HandleFortyPoints(scorer, opponent, match);
            }
            else
            {
                throw new InvalidOperationException($"Неожиданное значение scorer points {scorer.Points} в свитче в методе {nameof(UpdatePoints)}");
            }
        }

        private void HandleFortyPoints(MatchScoreModelWrapper scorer, MatchScoreModelWrapper opponent, MatchScoreModel match)
        {
            if (opponent.Points < TennisPoint.Forty)
            {
                UpdateGames(scorer, opponent, match);
            } else
            {
                HandleDeuce(scorer, opponent, match);
            }
        }

        private void HandleDeuce(MatchScoreModelWrapper scorer, MatchScoreModelWrapper opponent, MatchScoreModel match)
        {
            if (scorer.HasAdvantage) {
                UpdateGames(scorer, opponent, match);
                scorer.HasAdvantage = false;
                opponent.HasAdvantage = false;
            } 
            else if (opponent.HasAdvantage) {
                opponent.HasAdvantage = false;
            } 
            else
            {
                scorer.HasAdvantage = true;
            }
        }

        private void UpdateGames(MatchScoreModelWrapper scorer, MatchScoreModelWrapper opponent, MatchScoreModel match)
        {
            int scorerGames = scorer.Games + 1;
            scorer.Games = scorerGames;
            int opponentGames = opponent.Games;

            if (scorerGames == GamesToWinSet && opponentGames == GamesToWinSet)
            {
                match.Tiebreak = true;
                ResetPointsAndAdvantage(match);
                return;
            }

            if (scorerGames >= GamesToWinSet && scorerGames - opponentGames >= GamesWinDiff)
            {
                UpdateSets(scorer, opponent, match);
            } else
            {
                ResetPointsAndAdvantage(match);
            }
        }

        private void HandleTiebreak(MatchScoreModel match, MatchScoreModelWrapper scorer, MatchScoreModelWrapper opponent)
        {
            int scorerPoints = scorer.Points + 1;
            scorer.Points = scorerPoints;

            int opponentPoints = opponent.Points;
            if (scorerPoints >= TiebreakPointsToWin && scorerPoints - opponentPoints >= GamesWinDiff)
            {
                UpdateSets(scorer, opponent, match);
                match.Tiebreak = false;
            }
        }

        private void UpdateSets(MatchScoreModelWrapper scorer, MatchScoreModelWrapper opponent, MatchScoreModel match)
        {
            int scorerSets = scorer.Sets + 1;
            scorer.Sets = scorerSets;

            scorer.Games = DefaultGames;
            opponent.Games = DefaultGames;

            ResetPointsAndAdvantage(match);
        }

        private void ResetPointsAndAdvantage(MatchScoreModel match)
        {
            match.FirstPlayerPoints = TennisPoint.Love;
            match.SecondPlayerPoints = TennisPoint.Love;
            match.FirstPlayerAdvantage = false;
            match.SecondPlayerAdvantage = false;
        }
    }
}
