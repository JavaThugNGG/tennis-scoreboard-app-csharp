using Microsoft.EntityFrameworkCore;

namespace TennisScoreboard
{
    public class PlayerService
    {
        private readonly IDbContextFactory<AppDbContext> _contextFactory;
        private readonly PlayerDao _playerDao;
        private readonly ILogger<PlayerService> _logger;

        public PlayerService(IDbContextFactory<AppDbContext> contextFactory, PlayerDao playerDao, ILogger<PlayerService> logger)
        {
            _contextFactory = contextFactory;
            _playerDao = playerDao;
            _logger = logger;
        }

        public PlayerEntity GetByName(string name)
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();
                return _playerDao.GetByName(context, name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting player by name: {}", name);
                throw;
            }
        }

        public PlayerEntity Insert(string name)
        {
            try
            {
                PlayerEntity player = new PlayerEntity(name);
                using var context = _contextFactory.CreateDbContext();
                _playerDao.Insert(context, player);
                context.SaveChanges();
                return player;
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message.Contains("UNIQUE") == true)
                {
                    throw new PlayerAlreadyExistsException(name);
                }
                throw; 
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error inserting player by name: {}", name);
                throw;
            }
        }
    }
}
