namespace TennisScoreboard
{
    public class MatchValidator
    {
        public void ValidateGuid(string guid)
        {
            if (string.IsNullOrWhiteSpace(guid))
            {
                throw new ArgumentException("Некорректное значение UUID: " + guid);
            }
        }
    }
}
