using Microsoft.EntityFrameworkCore;

namespace TennisScoreboard
{
    public class PlayerService
    {
        private readonly IDbContextFactory<AppDbContext> _contextFactory;
        private readonly PlayerDao _playerDao;

        public PlayerService(IDbContextFactory<AppDbContext> contextFactory, PlayerDao playerDao)
        {
            _contextFactory = contextFactory;
            _playerDao = playerDao;
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
                //тут лог
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
                //тут лог
                throw;
            }
        }
    }
}
