using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using TennisScoreboard.Infrastructure;

namespace TennisScoreboard
{
    namespace TennisScoreboard
    {
        public class Program
        {
            public static void Main(string[] args)
            {
                var builder = WebApplication.CreateBuilder(args);

                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .WriteTo.Console(theme: AnsiConsoleTheme.Code)
                    .CreateLogger();

                builder.Host.UseSerilog();

                try
                {
                    Log.Information("Starting TennisScoreboard application");

                    using var loggerFactory = LoggerFactory.Create(lb => lb.AddSerilog());
                    var logger = loggerFactory.CreateLogger<Startup>();

                    var startUp = new Startup(logger);
                    startUp.ConfigureServices(builder);

                    var app = builder.Build();

                    using (var scope = app.Services.CreateScope())
                    {
                        Log.Information("Initializing database...");
                        var initializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();
                        initializer.Init(app);
                        Log.Information("Database initialized successfully");
                    }

                    startUp.ConfigureStaticFiles(app);
                    startUp.ConfigureRouting(app);
                    startUp.ConfigureApplicationLifetime(app);
                    Log.Information("Application configured. Running...");

                    app.Run();
                }
                catch (Exception ex)
                {
                    Log.Fatal(ex, "Application terminated unexpectedly");
                    throw;
                }
                finally
                {
                    Log.CloseAndFlush();
                }
            }
        }
    }
}
