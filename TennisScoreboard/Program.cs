using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

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
                    var startUp = new Startup();
                    startUp.ConfigureServices(builder);

                    var app = builder.Build();

                    using (var scope = app.Services.CreateScope())
                    {
                        var initializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();
                        initializer.Init(app);
                    }

                    startUp.ConfigureStaticFiles(app);
                    startUp.ConfigureRouting(app);
                    startUp.ConfigureApplicationLifetime(app);

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
