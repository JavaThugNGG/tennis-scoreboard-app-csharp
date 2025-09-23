namespace TennisScoreboard.Models
{
    public class MatchScoreModel
    {
        public int FirstPlayerId { get; set; }
        public int SecondPlayerId { get; set; }

        public int FirstPlayerSets { get; set; }
        public int SecondPlayerSets { get; set; }

        public int FirstPlayerGames { get; set; }
        public int SecondPlayerGames { get; set; }

        public int FirstPlayerPoints { get; set; }
        public int SecondPlayerPoints { get; set; }

        public bool FirstPlayerAdvantage { get; set; }
        public bool SecondPlayerAdvantage { get; set; }

        public bool Tiebreak { get; set; }

        public bool Finished { get; set; }

        public readonly object MatchStateLock = new object();

        public MatchScoreModel(int firstPlayerId, int secondPlayerId)
        {
            FirstPlayerId = firstPlayerId;
            SecondPlayerId = secondPlayerId;
        }

        public string DisplayFirstPlayerPoints()
        {
            return DisplayPoints(FirstPlayerPoints, FirstPlayerAdvantage);
        }

        public string DisplaySecondPlayerPoints()
        {
            return DisplayPoints(SecondPlayerPoints, SecondPlayerAdvantage);
        }

        private string DisplayPoints(int points, bool hasAdvantage)
        {
            if (hasAdvantage)
                return "adv";

            return points.ToString();
        }
    }
}
