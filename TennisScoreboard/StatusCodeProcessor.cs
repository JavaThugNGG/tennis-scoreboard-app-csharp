namespace TennisScoreboard
{
    public class StatusCodeProcessor
    {
        public int ResolveStatusCode(Exception exception)
        {
            return exception switch
            {
                ArgumentException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };
        }
    }
}
