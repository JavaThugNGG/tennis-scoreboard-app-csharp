namespace TennisScoreboard
{
    public class ErrorDtoBuilder
    {
        private readonly StatusCodeProcessor _statusCodeProcessor;

        public ErrorDtoBuilder(StatusCodeProcessor statusCodeProcessor)
        {
            _statusCodeProcessor = statusCodeProcessor;
        }

        public ErrorDto Build(Exception exception)
        {
            int statusCode = _statusCodeProcessor.ResolveStatusCode(exception);
            string message = "ошибка: " + exception.Message;
            return new ErrorDto(statusCode, message);
        }
    }
}
