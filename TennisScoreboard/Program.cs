namespace TennisScoreboard
{
    namespace TennisScoreboard
    {
        public class Program
        {
            public static void Main(string[] args)
            {
                var builder = WebApplication.CreateBuilder(args);

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
        }
    }
}
