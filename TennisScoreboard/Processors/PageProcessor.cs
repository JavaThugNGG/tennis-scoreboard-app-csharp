using TennisScoreboard.Parsers;
using TennisScoreboard.Validators;

namespace TennisScoreboard.Processors
{
    public class PageProcessor
    {
        private static readonly int DefaultPageIndex = 1;
        private readonly PageValidator _pageValidator;
        private readonly PageParser _pageParser;

        public PageProcessor(PageValidator pageValidator, PageParser pageParser)
        {
            _pageValidator = pageValidator;
            _pageParser = pageParser;
        }

        public int DeterminePage(string page)
        {
            if (IsPageProvided(page))
            {
                int pageInt = _pageParser.ParsePage(page);
                _pageValidator.ValidatePage(pageInt);
                return pageInt;
            }
            return DefaultPageIndex;
        }

        private bool IsPageProvided(string page)
        {
            return page != null;
        }
    }
}
