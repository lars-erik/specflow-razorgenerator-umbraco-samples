using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ASP;
using Moq;
using NUnit.Framework;
using RazorGenerator.Testing;
using Sample.Tests.MvcBindings;
using TechTalk.SpecFlow;
using Umbraco.Core.Configuration.UmbracoSettings;
using Umbraco.Core.IO;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.PropertyEditors;
using Umbraco.Core.PropertyEditors.ValueConverters;
using Umbraco.Tests.TestHelpers;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Routing;
using File = System.IO.File;

namespace Sample.Tests.UmbracoBindings
{
    [Binding]
    public class UmbracoRenderingBindings : BaseDatabaseFactoryTest
    {
        private readonly ViewsUnderTest viewsUnderTest;
        private UmbracoContext umbracoContext;
        private RoutingContext routingContext;
        private IUmbracoSettingsSection settings;

        public UmbracoRenderingBindings(ViewsUnderTest viewsUnderTest)
        {
            this.viewsUnderTest = viewsUnderTest;
            viewsUnderTest.AddPartial<_Views_Partials_Grid_Fanoe_cshtml>("Grid/fanoe");
            viewsUnderTest.AddPartial<_Views_Partials_Grid_Editors_Base_cshtml>("grid/editors/base");
            viewsUnderTest.AddPartial<_Views_Partials_Grid_Editors_Textstring_cshtml>("grid/editors/textstring");
            viewsUnderTest.AddPartial<_Views_Partials_Grid_Editors_Rte_cshtml>("grid/editors/rte");
        }

        [When("I render (.*)")]
        public void RenderHome(string route)
        {
            var content = umbracoContext.ContentCache.GetByRoute(route, true);

            // TODO: Select view for template
            var view = new _Views_TextPage_cshtml();
            var renderModel = new RenderModel<IPublishedContent>(content, CultureInfo.InvariantCulture);
            StubViewContext(view, renderModel);
            viewsUnderTest.Instance = view;
            viewsUnderTest.RenderAsHtml(renderModel);

            Console.WriteLine(viewsUnderTest.Output);
        }

        [Then("the result should contain \"(.*)\"")]
        public void TheResultShouldContain(string value)
        {
            Assert.That(viewsUnderTest.Output, Is.Not.Null.And.ContainsSubstring(value));
        }

        [Then("the result should contain")]
        public void TheResultShouldContainMultiline(string value)
        {
            Assert.That(viewsUnderTest.Output, Is.Not.Null.And.ContainsSubstring(value));
        }

        [BeforeFeature]
        public static void BeforeFeature()
        {
            IOHelper.IAmUnitTestingSoNeverUseHttpContextEver = true;
        }

        [BeforeScenario()]
        public override void Initialize()
        {
            InitializeFixture();

            base.Initialize();

            HttpContext.Current = viewsUnderTest.HttpContext;

            SetupUmbraco();
        }

        [AfterScenario()]
        public override void TearDown()
        {
            HttpContext.Current = null;
            base.TearDown();
        }

        protected override void FreezeResolution()
        {
            var internalShit = BindingFlags.NonPublic | BindingFlags.Instance;
            var activatorCtor = Type.GetType("Umbraco.Core.ActivatorServiceProvider, Umbraco.Core")
                .GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, new Type[0],
                    new ParameterModifier[0]);
            var activator = activatorCtor.Invoke(new object[0]);
            var converterCtor = typeof (PropertyValueConvertersResolver)
                .GetConstructor(internalShit, null, new[]
                {
                    typeof (IServiceProvider),
                    typeof (ILogger),
                    typeof (IEnumerable<Type>)
                }, new ParameterModifier[0]);

            PropertyValueConvertersResolver.Current = (PropertyValueConvertersResolver)converterCtor.Invoke(new object[] { activator, Logger, new Type[0] });
            PropertyValueConvertersResolver.Current.AddType(typeof(GridValueConverter));

            base.FreezeResolution();
        }

        private void SetupUmbraco()
        {
            settings = SettingsForTests.GenerateMockSettings();
            SettingsForTests.ConfigureSettings(settings);

            routingContext = GetRoutingContext("http://localhost", -1, new RouteData());
            umbracoContext = routingContext.UmbracoContext;
            umbracoContext.PublishedContentRequest = new PublishedContentRequest(new Uri("http://localhost"), routingContext,
                settings.WebRouting, (s) => new[] {"Kunde"});
        }

        private void StubViewContext(WebViewPage viewPage, RenderModel<IPublishedContent> renderModel)
        {
            var controller = Mock.Of<Controller>();
            var controllerContext = new ControllerContext(umbracoContext.HttpContext, new RouteData(), controller);
            controller.ControllerContext = controllerContext;
            viewPage.ViewContext = new ViewContext(controllerContext, Mock.Of<IView>(), new ViewDataDictionary {Model = renderModel}, new TempDataDictionary(), new StringWriter());
        }

        protected override string GetDbConnectionString()
        {
            return @"Data Source=..\..\..\Sample.Web\App_Data\Umbraco.sdf;Flush Interval=1;";
        }

        protected RoutingContext GetRoutingContext(string url, int templateId, RouteData routeData = null, bool setUmbracoContextCurrent = false)
        {
            umbracoContext = GetUmbracoContext(url, templateId, routeData);
            var urlProvider = new UrlProvider(umbracoContext, settings.WebRouting, new IUrlProvider[]
            {
                new DefaultUrlProvider(settings.RequestHandler)
            });

            var rcctor = typeof (RoutingContext).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new[]
            {
                typeof (UmbracoContext),
                typeof (IEnumerable<IContentFinder>),
                typeof (IContentFinder),
                typeof (UrlProvider)
            }, new ParameterModifier[0]);

            routingContext = (RoutingContext)rcctor.Invoke(new object[] {
                umbracoContext,
                Enumerable.Empty<IContentFinder>(),
                Mock.Of<IContentFinder>(),
                urlProvider});

            typeof(UmbracoContext).GetProperty("RoutingContext").SetValue(umbracoContext, routingContext);
            typeof(UmbracoContext).GetProperty("Current").SetValue(null, umbracoContext);

            return routingContext;
        }

        protected override string GetXmlContent(int templateId)
        {
            return ContentFile.Contents;
        }
    }

    public class ContentFile
    {
        private static string contents = null;

        public static string Contents
        {
            get
            {
                if (contents == null)
                {
                    contents = File.ReadAllText("umbraco.config");
                }
                return contents;
            }
        }
    }
}
