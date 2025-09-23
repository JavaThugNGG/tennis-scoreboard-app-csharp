using Microsoft.EntityFrameworkCore;
using TennisScoreboard.Entities;
using TennisScoreboard.Infrastructure;
using TennisScoreboard.Repository;

namespace TennisScoreboard.Services
{
    public class MatchService
    {
        private readonly IDbContextFactory<AppDbContext> _contextFactory;
        private readonly MatchDao _matchDao;
        private readonly ILogger<MatchService> _logger;

        public MatchService(IDbContextFactory<AppDbContext> contextFactory, MatchDao matchDao, ILogger<MatchService> logger)
        {
            _contextFactory = contextFactory;
            _matchDao = matchDao;
            _logger = logger;
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
                _logger.LogError(ex, "error getting page of matches with start index: {}, matches per page: {}", startIndex, matchesPerPage);
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
                _logger.LogError(ex, "Error getting count of matches");
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
                _logger.LogError(ex, "Error getting page of matches with player filter: {}, player id: {}, matches per page: {}, start index: {}", player.Name, player.Id, matchesPerPage, startIndex);
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
                _logger.LogError(ex, "Error getting count matches with player filter: {}, player id: {}", player.Name, player.Id);
                throw;
            }
        }
    }
}
