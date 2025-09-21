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
            using var context = _factory.CreateDbContext();
            context.Database.EnsureCreated();

            Debug.WriteLine("Таблицы в базе:");//в лог
            var tables = context.Model.GetEntityTypes()
                .Select(t => t.GetTableName())
                .Distinct();

            foreach (var table in tables)
                Debug.WriteLine(table);
        }
    }
}
