using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using TechTalk.SpecFlow;

namespace Sample.Tests
{
    [Binding]
    [Scope(Tag="Umbraco")]
    public class UmbracoRouteBindings
    {
        // For some reason RouteTable.Routes is reset after this, so have to be called later. (from steps)
        [BeforeScenario(Order=2)]
        public void Setup()
        {
            EnsureRoutes();
        }

        public static void EnsureRoutes()
        {
            RouteTable.Routes.MapRoute(
                "SurfaceControllers",
                "umbraco/surface/{controller}/{action}/{id}",
                new {id = UrlParameter.Optional}
                );
        }
    }
}
