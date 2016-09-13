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
        // Important to add routes at order 2. Umbraco bootup clears routes.
        [BeforeScenario(Order=2)]
        public void Setup()
        {
            RouteTable.Routes.MapRoute(
                "SurfaceControllers",
                "umbraco/surface/{controller}/{action}/{id}",
                new {id = UrlParameter.Optional}
                );
        }
    }
}
