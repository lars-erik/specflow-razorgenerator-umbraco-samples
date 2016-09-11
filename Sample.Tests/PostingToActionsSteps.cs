using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASP;
using NUnit.Framework;
using Sample.Tests.MvcBindings;
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
        AdditionModel model = new AdditionModel();

        public PostingToActionsSteps(ViewsUnderTest viewsUnderTest)
        {
            this.viewsUnderTest = viewsUnderTest;
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
            Assert.Fail("CurrentUmbracoPage just does magic");
            // Can do this, but ViewBag has to be injected into controllercontext
            // see about controllersUnderTest

            //var content =
            //var view = new _Views_TextPage_cshtml();
            //var renderModel = new RenderModel<IPublishedContent>(content, CultureInfo.InvariantCulture);
            //StubViewContext(view, renderModel);
            //viewsUnderTest.Instance = view;
            //viewsUnderTest.RenderAsHtml(renderModel);

            //var controller = new AdditionController();
            //var result = controller.Add(model);


        }

    }
}
