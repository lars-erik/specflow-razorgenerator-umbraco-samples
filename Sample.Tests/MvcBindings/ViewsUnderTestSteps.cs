using System.Web.Mvc;
using System.Web.Routing;
using RazorGenerator.Testing;
//using Sample.Mvc.Web;
using TechTalk.SpecFlow;

namespace Sample.Tests.MvcBindings
{
    [Binding]
    public class ViewsUnderTestSteps
    {
        private readonly ViewsUnderTest viewsUnderTest;

        public ViewsUnderTestSteps(ViewsUnderTest viewsUnderTest)
        {
            this.viewsUnderTest = viewsUnderTest;
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            //HttpContext.Current = new HttpContext(new HttpRequest("", "http://localhost", ""), new HttpResponse(new StringWriter()));

            RouteTable.Routes.Clear();
            //RouteConfig.RegisterRoutes(RouteTable.Routes);

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(viewsUnderTest.ViewEngine);
            ViewEngines.Engines.Add(WebViewPageExtensions.ViewEngine);
        }

        [When(@"I use the RenderAsHtml extension")]
        public void WhenIUseTheRenderAsHtmlExtension()
        {
            viewsUnderTest.RenderAsHtml();
        }

    }
}
