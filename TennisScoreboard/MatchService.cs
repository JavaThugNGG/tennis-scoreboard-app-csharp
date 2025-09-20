using Microsoft.EntityFrameworkCore;

namespace TennisScoreboard
{
    public class MatchService
    {
        private readonly IDbContextFactory<AppDbContext> _contextFactory;
        private readonly MatchDao _matchDao;

        public MatchService(IDbContextFactory<AppDbContext> contextFactory, MatchDao matchDao)
        {
            _contextFactory = contextFactory;
            _matchDao = matchDao;
        }

        public IList<MatchEntity> GetPage(int matchesPerPage, int startIndex)
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();
                return _matchDao.GetPage(context, matchesPerPage, startIndex);
            }
            catch (Exception ex)
            {
                //тут лог
                throw;
            }
        }

        public long Count()
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();
                return _matchDao.Count(context);
            }
            catch (Exception ex)
            {
                //лог
                throw;
            }
        }

        public IList<MatchEntity> GetPageWithPlayerFilter(PlayerEntity player, int matchesPerPage, int startIndex)
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();
                return _matchDao.GetPageWithPlayerFilter(context, player, matchesPerPage, startIndex);
            }
            catch (Exception ex)
            {
                //лог
                throw;
            }
        }

        public long CountWithPlayerFilter(PlayerEntity player)
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();
                return _matchDao.CountWithPlayerFilter(context, player);
            }
            catch (Exception ex)
            {
                //лог
                throw;
            }
        }
    }
}
