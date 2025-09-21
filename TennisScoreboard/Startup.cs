using Microsoft.EntityFrameworkCore;

namespace TennisScoreboard
{
    public class Startup
    {
        public void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddControllersWithViews();

            var connectionString = builder.Configuration.GetConnectionString("Default")
                                   ?? throw new InvalidOperationException("Connection string 'Default' not found!");

            var dbConnectionManager = new DatabaseConnectionManager(connectionString);
            dbConnectionManager.OpenPersistent();

            builder.Services.AddSingleton(dbConnectionManager);
            builder.Services.AddSingleton<DatabaseInitializer>();

            builder.Services.AddSingleton<PlayerValidator>();
            builder.Services.AddSingleton<PlayerDao>();
            builder.Services.AddSingleton<PlayerService>();
            builder.Services.AddSingleton<PlayerValidator>();
            builder.Services.AddSingleton<PlayerProcessor>();
            builder.Services.AddSingleton<PlayerParser>();

            builder.Services.AddSingleton<MatchDao>();
            builder.Services.AddSingleton<MatchService>();
            builder.Services.AddSingleton<MatchProcessor>();
            builder.Services.AddSingleton<MatchPreparingService>();
            builder.Services.AddSingleton<OngoingMatchesService>();
            builder.Services.AddSingleton<MatchStateService>();
            builder.Services.AddSingleton<MatchScoreCalculationService>();
            builder.Services.AddSingleton<MatchFinishingService>();
            builder.Services.AddSingleton<FinishedMatchProcessingService>();
            builder.Services.AddSingleton<MatchValidator>();
            builder.Services.AddSingleton<MatchParser>();

            builder.Services.AddSingleton<PageProcessor>();
            builder.Services.AddSingleton<PageValidator>();
            builder.Services.AddSingleton<PageParser>();

            builder.Services.Configure<PaginationSettings>(builder.Configuration.GetSection("Pagination"));
            builder.Services.AddSingleton<MatchPageViewDtoBuilder>();
            builder.Services.AddSingleton<MatchPageViewCalculator>();
            builder.Services.AddSingleton<MatchPageViewService>();
            builder.Services.AddSingleton<MatchesSummaryService>();

            builder.Services.AddDbContextFactory<AppDbContext>(options =>
            {
                options.UseSqlite(connectionString);
            });
        }

        public void ConfigureStaticFiles(WebApplication app)
        {
            app.UseDefaultFiles();
            app.UseStaticFiles();
        }

        public void ConfigureRouting(WebApplication app)
        {
            app.UseRouting();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        }

        public void ConfigureApplicationLifetime(WebApplication app)
        {
            var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
            var dbConnectionManager = app.Services.GetRequiredService<DatabaseConnectionManager>();

            lifetime.ApplicationStopping.Register(() =>
            {
                dbConnectionManager.Dispose();
            });
        }
    }
}
