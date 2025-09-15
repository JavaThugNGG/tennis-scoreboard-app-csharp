using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace TennisScoreboard
{
    public class DatabaseInitializer
    {
        private readonly IDbContextFactory<AppDbContext> _factory;

        public DatabaseInitializer(IDbContextFactory<AppDbContext> factory)
        {
            _factory = factory;
        }

        public void Init(WebApplication app)
        {
            using var db = _factory.CreateDbContext();
            db.Database.EnsureCreated();

            Debug.WriteLine("Таблицы в базе:");
            var tables = db.Model.GetEntityTypes()
                .Select(t => t.GetTableName())
                .Distinct();

            foreach (var table in tables)
                Debug.WriteLine(table);
        }
    }
}
