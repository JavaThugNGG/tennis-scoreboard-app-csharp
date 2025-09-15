using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace TennisScoreboard
{
    public class DatabaseInitializer
    {
        public void Init(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.EnsureCreated();

            Debug.WriteLine("Таблицы в базе:");//в лог
            var conn = db.Database.GetDbConnection();
            using var command = conn.CreateCommand();

            command.CommandText = @"
               SELECT name 
               FROM sqlite_master 
               WHERE type='table';
            ";

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                Debug.WriteLine(reader.GetString(0));//в лог
            }
        }
    }
}
