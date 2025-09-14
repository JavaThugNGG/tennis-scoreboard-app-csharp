namespace TennisScoreboard
{
    public class IllegalPlayerNameFilterException : Exception
    {
        public IllegalPlayerNameFilterException(string message) : base(message) { }
    }
}
