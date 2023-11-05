using NetC.JuniorDeveloperExam.Web.Services;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Services.Description;
using Unity;
using Unity.AspNet.Mvc; // Import the Unity.Mvc5 namespace

namespace NetC.JuniorDeveloperExam.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var container = new UnityContainer();
            container.RegisterType<BlogPostService>();

            // Set the Unity dependency resolver to use the Unity container
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}
