using System.Web.Mvc;
using System.Web.Routing;

namespace NSprockets.Example
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
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            // we use web.config configuration
            //NSprocketsTool.Current.ConcatToSingleFile = true;
            //NSprocketsTool.Current.Minify = false;
            //NSprocketsTool.Current.SetWebOutputDirectory("~/scripts");
            //NSprocketsTool.Current.AddServerLookupDirectory("~/scripts");
            //NSprocketsTool.Current.AddServerLookupDirectory("~/scripts2");

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}