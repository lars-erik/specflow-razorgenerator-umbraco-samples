using System;
using System.Globalization;
using System.Web.Routing;
using ASP;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using RazorGenerator.Testing;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Persistence;
using Umbraco.Tests.TestHelpers;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Routing;
using File = System.IO.File;

namespace Sample.Tests
{
    [TestFixture]
    //[DatabaseTestBehavior(DatabaseBehavior)]
    public class MinimalReadContentTypeWithGrid : BaseDatabaseFactoryTest
    {
        private UmbracoContext umbracoContext;

        [SetUp]
        public override void Initialize()
        {
            base.Initialize();

            var settings = SettingsForTests.GenerateMockSettings();
            SettingsForTests.ConfigureSettings(settings);
            umbracoContext = GetUmbracoContext("http://localhost", -1, new RouteData(), true);
        }

        [Test]
        public void GetHome()
        {
            var content = umbracoContext.ContentCache.GetByRoute("/learn/basics", true);

            Assert.That(content, Is.Not.Null.And.Property("Properties").With.Count.GreaterThan(0));
        }

        [Test]
        public void RenderHome()
        {
            var content = umbracoContext.ContentCache.GetByRoute("/learn/basics", true);
            // et yadi yada
            //umbracoContext.PublishedContentRequest = new PublishedContentRequest(new Uri("http://localhost"),  );
            // TODO: Back to routing test

            var view = new _Views_TextPage_cshtml();
            var output = view.Render(umbracoContext.HttpContext, new RenderModel<IPublishedContent>(content, CultureInfo.InvariantCulture));

            Console.WriteLine(output);
        }

        protected override string GetDbConnectionString()
        {
            return @"Data Source=..\..\..\Sample.Web\App_Data\Umbraco.sdf;Flush Interval=1;";
        }

        //protected override ApplicationContext CreateApplicationContext()
        //{
        //    var databaseFactory = Mock.Of<IDatabaseFactory>();
        //    var services = MockHelper.GetMockedServiceContext();
        //    var ctx = new ApplicationContext(
        //        //assign the db context
        //        new DatabaseContext(databaseFactory, Logger, SqlSyntax, GetDbProviderName()),
        //        //assign the service context
        //        services,
        //        CacheHelper,
        //        ProfilingLogger);
        //    return ctx;
        //}

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
