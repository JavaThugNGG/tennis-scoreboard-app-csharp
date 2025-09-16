namespace TennisScoreboard
{
    public class MatchParser
    {
        public Guid ParseGuid(string guidParameter)
        {
            if (Guid.TryParse(guidParameter, out var result))
            {
                return result;
            }
            else
            {
                throw new ArgumentException($"Ошибка при парсинге. Некорректный UUID: {guidParameter}");
            }
        }
    }
}
