namespace TennisScoreboard
{
    public class PlayerParser
    {
        public int ParseId(string playerId)
        {
            if (int.TryParse(playerId, out var result)) {
                return result;
            }
            else
            {
                throw new ArgumentException("Ошибка при парсинге. Некорректный аргумент playerId :" + playerId);
            }
        }
    }
}
