using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using Sample.Tests.MvcBindings;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Sample.Tests.UmbracoBindings
{
    public class UmbracoViewsUnderTest
    {
        public event EventHandler LoadedContent;

        private readonly ViewsUnderTest viewsUnderTest;

        public IPublishedContent Content { get; private set; }
        public WebViewPage ViewPage { get; }

        public UmbracoContext UmbracoContext { get; set; }

        public UmbracoViewsUnderTest(ViewsUnderTest viewsUnderTest)
        {
            this.viewsUnderTest = viewsUnderTest;
        }

        public void LoadAtRoute(string route)
        {
            Content = UmbracoContext.ContentCache.GetByRoute(route, true);
            if (Content == null)
                throw new Exception("Can't find content at route " + route);
            LoadedContent?.Invoke(this, EventArgs.Empty);
        }

        public void SetupView()
        {
            string templateAlias = null;
            try
            {
                templateAlias = Content.GetTemplateAlias();
                var view = viewsUnderTest.ViewEngine.CreateView(templateAlias);
                StubViewContext(view, Content);
                viewsUnderTest.Instance = view;
            }
            catch
            {
                throw new Exception("Did you add " + (templateAlias ?? Content?.DocumentTypeAlias ?? "the content template") + " to the views under test?");

            }
        }

        private void StubViewContext(WebViewPage viewPage, IPublishedContent model)
        {
            var controller = Mock.Of<Controller>();
            var controllerContext = new ControllerContext(viewsUnderTest.HttpContextBase, new RouteData(), controller);
            controller.ControllerContext = controllerContext;
            viewPage.ViewContext = new ViewContext(controllerContext, Mock.Of<IView>(), new ViewDataDictionary { Model = model }, new TempDataDictionary(), new StringWriter());
        }

    }
}
