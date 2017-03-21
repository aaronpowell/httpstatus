using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Teapot.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode,
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "I'm a teapot",
                "im-a-teapot",
                new {controller = "Teapot", action = "Teapot"}
            );

            routes.MapRoute(
                "StatusCode",
                "{statusCode}",
                new { controller = "Status", action = "StatusCode" },
                new { statusCode = @"\d{3}" }
            );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}", // URL with parameters
                new { controller = "Status", action = "Index" } // Parameter defaults
            );
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;
            HttpContext context = application.Context;

            context.Response.AddHeader("Access-Control-Allow-Origin", "*");
            // Complete.
            base.CompleteRequest();
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}
