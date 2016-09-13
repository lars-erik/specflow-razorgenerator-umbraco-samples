using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using HtmlAgilityPack;
using Moq;
using RazorGenerator.Testing;

namespace Sample.Tests.MvcBindings
{
    public class ViewsUnderTest
    {
        public HtmlDocument Document { get; private set; }
        public WebViewPage Instance { get; set; }
        public ViewsUnderTestViewEngine ViewEngine { get; private set; }
        public HttpContext HttpContext { get; private set; }
        public HttpContextBase HttpContextBase { get; private set; }

        public string Output => Document.DocumentNode.OuterHtml;

        public HtmlNode DocumentNode => Document.DocumentNode;

        public ViewsUnderTest()
        {
            ViewEngine = new ViewsUnderTestViewEngine();
            HttpContextBase = CreateHttpContext();
        }

        public void SetInstanceFromViewResult(ViewResult viewResult)
        {
            Instance = ViewEngine.CreateView(viewResult.ViewName);
        }

        public void SetInstanceFromViewResult(PartialViewResult viewResult)
        {
            Instance = ViewEngine.CreatePartialView(viewResult.ViewName);
        }

        public HtmlDocument RenderAsHtml()
        {
            Document = ((WebViewPage<object>)Instance).RenderAsHtml(HttpContextBase, null);
            return Document;
        }

        public HtmlDocument RenderAsHtml<TModel>(TModel model)
        {
            Document = ((WebViewPage<TModel>)Instance).RenderAsHtml(HttpContextBase, model);
            return Document;
        }

        /// <summary>
        /// Must be called after BeforeScenario Order 0
        /// </summary>
        /// <typeparam name="TPartial"></typeparam>
        /// <param name="path"></param>
        public void AddPartial<TPartial>(string path)
            where TPartial : WebViewPage
        {
            ViewEngine.AddPartial<TPartial>(path);
        }

        /// <summary>
        /// Must be called after BeforeScenario Order 0
        /// </summary>
        /// <typeparam name="TPartial"></typeparam>
        /// <param name="path"></param>
        public void AddView<TView>(string path)
            where TView : WebViewPage
        {
            ViewEngine.Add<TView>(path);
        }

        public HttpContextBase CreateHttpContext()
        {
            var httpContextBase = Mock.Of<HttpContextBase>();
            var httpContext = new HttpContext(new HttpRequest("x", "http://localhost", ""), new HttpResponse(new StringWriter()));
            HttpContext = httpContext;

            httpContext.Request.Form.GetType().GetMethod("MakeReadWrite", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(httpContext.Request.Form, new object[0]);

            var items = new Dictionary<object, object>();
            Mock.Get(httpContextBase).Setup(c => c.Items).Returns(items);
            Mock.Get(httpContextBase).Setup(c => c.Request).Returns(Mock.Of<HttpRequestBase>());
            Mock.Get(httpContextBase).Setup(c => c.Response).Returns(Mock.Of<HttpResponseBase>());
            Mock.Get(httpContextBase).Setup(c => c.Server).Returns(() => new HttpServerUtilityWrapper(httpContext.Server));
            Mock.Get(httpContextBase.Request).Setup(r => r.QueryString).Returns(new NameValueCollection());
            Mock.Get(httpContextBase.Request).Setup(r => r.Form).Returns(httpContext.Request.Form);
            Mock.Get(httpContextBase.Request).Setup(r => r.Unvalidated).Returns(new UnvalidatedRequestValuesWrapper(httpContext.Request.Unvalidated));
            Mock.Get(httpContextBase.Request).Setup(r => r.Files).Returns(Mock.Of<HttpFileCollectionBase>());
            Mock.Get(httpContextBase.Response).Setup(c => c.Output).Returns(() => WebViewPageExtensions.Writer);

            return httpContextBase;
        }
    }
}
