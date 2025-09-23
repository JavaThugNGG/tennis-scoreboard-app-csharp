namespace TennisScoreboard.Exceptions
{
    public class IllegalPlayerNameException : Exception
    {
        public IllegalPlayerNameException(string message) : base(message) { }
    }
}
