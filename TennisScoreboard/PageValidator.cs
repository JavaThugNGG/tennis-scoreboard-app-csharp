namespace TennisScoreboard
{
    public class PageValidator
    {
        public void ValidatePage(int page)
        {
            if (page < 1)
            {
                throw new ArgumentException("Некорректный аргумент страницы матча");
            }
        }
    }
}
