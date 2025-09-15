using System.Diagnostics;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace TennisScoreboard
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            var connection = new SqliteConnection("Data Source=InMemoryDb;Mode=Memory;Cache=Shared");
            connection.Open();

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlite(connection);
            });

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.EnsureCreated();

                Debug.WriteLine("Таблицы в базе:");//в лог потом
                var conn = db.Database.GetDbConnection();
                using var command = conn.CreateCommand();
                command.CommandText = "SELECT name FROM sqlite_master WHERE type='table';";
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Debug.WriteLine(reader.GetString(0));
                }
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");


            app.Lifetime.ApplicationStopping.Register(() =>
            {
                connection.Dispose();
            });

            app.Run();
        }
    }
}
