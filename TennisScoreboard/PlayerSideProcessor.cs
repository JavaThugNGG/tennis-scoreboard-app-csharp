namespace TennisScoreboard
{
    public class PlayerSideProcessor
    {
        public static PlayerSide OpponentOf(PlayerSide side)
        {
            if (side == PlayerSide.First)
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
