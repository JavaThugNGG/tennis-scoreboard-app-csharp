namespace TennisScoreboard.Parsers
{
    public class PageParser
    {
        public int ParsePage(string pageParameter)
        {
            if (int.TryParse(pageParameter, out int page))
            {
                return page;
            }
            throw new ArgumentException("Некорректный параметр страницы!");
        }
    }
}
