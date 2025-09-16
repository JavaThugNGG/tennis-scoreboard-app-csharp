namespace TennisScoreboard
{
    public class MatchScoreCalculationService
    {
        private static readonly int DefaultPoints = 0;
        private static readonly int PointsFirst = 15;
        private static readonly int PointsSecond = 30;
        private static readonly int PointsThird = 40;

        private static readonly int DefaultGames = 0;
        private static readonly int GamesToWinSet = 6;
        private static readonly int GamesWinDiff = 2;

        private static readonly int TiebreakPointsToWin = 7;

        public void Scoring(MatchScoreModel match, PlayerSide scorerSide)
        {
            PlayerSide opponentSide = OpponentOf(scorerSide);
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
            if (scorer.Points == DefaultPoints)
            {
                scorer.Points = PointsFirst;
            }
            else if (scorer.Points == PointsFirst)
            {
                scorer.Points = PointsSecond;
            }
            else if (scorer.Points == PointsSecond)
            {
                scorer.Points = PointsThird;
            }
            else if (scorer.Points == PointsThird)
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
            if (opponent.Points < PointsThird)
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

            if (scorerGames >= GamesToWinSet && (scorerGames - opponentGames) >= GamesWinDiff)
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
            if (scorerPoints >= TiebreakPointsToWin && (scorerPoints - opponentPoints) >= GamesWinDiff)
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
            match.FirstPlayerPoints = DefaultPoints;
            match.SecondPlayerPoints = DefaultPoints;
            match.FirstPlayerAdvantage = false;
            match.SecondPlayerAdvantage = false;
        }

        private PlayerSide OpponentOf(PlayerSide scorerSide)
        {
            if (scorerSide == PlayerSide.First)
            {
                return PlayerSide.Second;
            } 
            else
            {
                return PlayerSide.First;
            }
        }

    }
}
