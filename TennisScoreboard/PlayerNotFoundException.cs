namespace TennisScoreboard
{
    public class PlayerNotFoundException : Exception
    {
        public PlayerNotFoundException(string message) : base(message) { }
    }
}
