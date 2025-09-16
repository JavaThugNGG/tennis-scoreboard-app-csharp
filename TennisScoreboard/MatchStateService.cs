namespace TennisScoreboard
{
    public class MatchStateService
    {
        private static readonly int WinningSets = 2;
        private static readonly int MaxOpponentSetsForFinishedMatch = 1;

        public void EnsureNotFinished(MatchScoreModel matchScoreModel)
        {
            if (HasFirstPlayerWin(matchScoreModel) || HasSecondPlayerWin(matchScoreModel)) {
                throw new MatchAlreadyFinishedException();
            }
        }

        private bool HasFirstPlayerWin(MatchScoreModel matchScoreModel)
        {
            return (matchScoreModel.FirstPlayerSets == WinningSets) && (matchScoreModel.SecondPlayerSets <= MaxOpponentSetsForFinishedMatch);
        }

        private bool HasSecondPlayerWin(MatchScoreModel matchScoreModel)
        {
            return (matchScoreModel.SecondPlayerSets == WinningSets) && (matchScoreModel.FirstPlayerSets <= MaxOpponentSetsForFinishedMatch);
        }
    }
}
