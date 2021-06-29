using Andreys.Data;
using Andreys.Services;
using Microsoft.EntityFrameworkCore;
using MyWebServer;
using MyWebServer.Controllers;
using MyWebServer.Results.Views;
using System;
using System.Threading.Tasks;

namespace Andreys
{
    public class Startup
    {
        public static async Task Main()
               => await HttpServer
                   .WithRoutes(routes => routes
                       .MapStaticFiles()
                       .MapControllers())
                   .WithServices(services => services
                       .Add<AppDbContext>()
                       .Add<IValidator, Validator>()
                       .Add<IPasswordHasher, PasswordHasher>()                       
                       .Add<IViewEngine, CompilationViewEngine>())
                   .WithConfiguration<AppDbContext>(context => context
                       .Database.Migrate())
                   .Start();
    }
}
