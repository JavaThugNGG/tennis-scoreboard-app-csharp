namespace TennisScoreboard
{
    public class MatchAlreadyFinishedException : Exception
    {
        public MatchAlreadyFinishedException(string message) : base(message) { }
    }
}
