using Microsoft.EntityFrameworkCore;

namespace TennisScoreboard
{
    public class DatabaseInitializer
    {
        private readonly IDbContextFactory<AppDbContext> _factory;
        private readonly ILogger<DatabaseInitializer> _logger;

        public DatabaseInitializer(IDbContextFactory<AppDbContext> factory, ILogger<DatabaseInitializer> logger)
        {
            _factory = factory;
            _logger = logger;
        }

        public void Init(WebApplication app)
        {
            using var context = _factory.CreateDbContext();

            _logger.LogInformation("Ensuring database is created...");
            context.Database.EnsureCreated();
            _logger.LogInformation("Database ensured.");

            _logger.LogInformation("Tables in database:");
            var tables = context.Model.GetEntityTypes()
                .Select(t => t.GetTableName())
                .Distinct();

            foreach (var table in tables)
            {
                _logger.LogInformation(table);
            }
        }
    }
}
