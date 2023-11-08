using System;
using System.Web.Mvc;
using System.Web.Routing;
using Serilog;

namespace NetC.JuniorDeveloperExam.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            try
            {
                routes.MapRoute(
                    name: "Blog",
                    url: "Blog",
                    defaults: new { controller = "Blog", action = "Index" });
            }
            catch (Exception ex)
            {
                Log.Information($"{ex.StackTrace}");
            }
            try
            {
                routes.MapRoute(
                    name: "BlogPost",
                    url: "Blog/{id}",
                    defaults: new { controller = "Blog", action = "BlogPost", id = "Id" });
            }
            catch (Exception ex)
            {
                Log.Information($"{ex.StackTrace}");
            }

            try
            {
                routes.MapRoute(
                    name: "AddComment",
                    url: "Blog/AddComment/{id}",
                    defaults: new { controller = "Blog", action = "AddComment", id = "Id" });
            }
            catch (Exception ex)
            {
                Log.Information($"{ex.StackTrace}");
            }

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Blog", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
