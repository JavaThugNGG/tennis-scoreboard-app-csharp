namespace TennisScoreboard
{
    public class PlayerAlreadyExistsException : Exception
    {
        public PlayerAlreadyExistsException(string message) : base(message) { }
    }
}
