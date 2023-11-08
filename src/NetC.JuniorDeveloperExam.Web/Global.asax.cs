using Microsoft.Extensions.Caching.Memory;
using NetC.JuniorDeveloperExam.Web.Services;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Services.Description;
using Serilog;
using Unity;
using Unity.AspNet.Mvc; // Import the Unity.Mvc5 namespace
using Unity.Injection;

namespace NetC.JuniorDeveloperExam.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // Set up Serilog for console logging
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("C:\\Logs\\myapp.log")
                .CreateLogger();

            
            var cache = new MemoryCache(new MemoryCacheOptions());
            var container = new UnityContainer();
            container.RegisterInstance<IMemoryCache>(cache);
            container.RegisterType<BlogPostService>();
            container.RegisterType<IJsonService, JsonService>();
            // Set the Unity dependency resolver to use the Unity container
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}
