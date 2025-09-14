namespace TennisScoreboard
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            /*
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }*/


            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();//сопоставляет url с маршрутами(готовься к маршрутизации)

            app.MapControllerRoute(//определение шаблона маршрутов(вместе это все позволяет правильно направлять запросы на нужные ендпоинты)
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}






