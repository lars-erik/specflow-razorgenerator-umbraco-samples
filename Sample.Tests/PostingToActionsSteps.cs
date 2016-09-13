using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using ASP;
using NUnit.Framework;
using Sample.Tests.MvcBindings;
using Sample.Tests.UmbracoBindings;
using Sample.Web.Controllers;
using Sample.Web.Models;
using TechTalk.SpecFlow;
using umbraco;
using Umbraco.Core.Models;
using Umbraco.Web.Models;

namespace Sample.Tests
{
    [Binding]
    public class PostingToActionsSteps
    {
        private readonly ViewsUnderTest viewsUnderTest;
        private readonly UmbracoViewsUnderTest umbracoViewsUnderTest;
        private readonly ControllersUnderTest controllersUnderTest;
        AdditionModel model = new AdditionModel();

        public PostingToActionsSteps(ViewsUnderTest viewsUnderTest, UmbracoViewsUnderTest umbracoViewsUnderTest, ControllersUnderTest controllersUnderTest)
        {
            this.viewsUnderTest = viewsUnderTest;
            this.umbracoViewsUnderTest = umbracoViewsUnderTest;
            this.controllersUnderTest = controllersUnderTest;
        }

        [Given(@"I have entered (.*) into A")]
        public void GivenIHaveEnteredIntoA(int p0)
        {
            model.A = p0;
        }

        [Given(@"I have entered (.*) into B")]
        public void GivenIHaveEnteredIntoB(int p0)
        {
            model.B = p0;
        }

        [When(@"I press add")]
        public void WhenIPressAdd()
        {
            viewsUnderTest.AddPartial<_Views_Addition_Index_cshtml>("Index");

            umbracoViewsUnderTest.LoadAtRoute("/learn/masterclasses");
            umbracoViewsUnderTest.SetupView();

            var routeData = new RouteData();
            routeData.DataTokens.Add("ParentActionViewContext", viewsUnderTest.Instance.ViewContext);

            var controller = new AdditionController();
            controller.ControllerContext = new ControllerContext(viewsUnderTest.HttpContextBase, routeData, controller);
            controller.Add(model);

            controllersUnderTest.Add("Addition", () => controller);

            UmbracoRouteBindings.EnsureRoutes();
            ControllersUnderTestSteps.EnsureControllerFactory(controllersUnderTest);
            viewsUnderTest.RenderAsHtml(new RenderModel<IPublishedContent>(umbracoViewsUnderTest.Content, CultureInfo.CurrentCulture));
        }

    }
}
