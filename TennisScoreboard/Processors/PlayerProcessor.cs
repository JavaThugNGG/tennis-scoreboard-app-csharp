using TennisScoreboard.Exceptions;
using TennisScoreboard.Models;

namespace TennisScoreboard.Processors
{
    public class PlayerProcessor
    {
        public PlayerSide DetermineScorerSide(MatchScoreModel match, int scorerId)
        {
            int firstPlayerId = match.FirstPlayerId;
            int secondPlayerId = match.SecondPlayerId;

            if (scorerId == firstPlayerId)
            {
                return PlayerSide.First;
            }
            else if (scorerId == secondPlayerId)
            {
                return PlayerSide.Second;
            }
            else
            {
                throw new PlayerNotFoundException("Игрок не найден. scorerId: " + scorerId);
            }
        }
    }
}
