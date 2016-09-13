using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASP;
using Sample.Tests.MvcBindings;
using Sample.Tests.UmbracoBindings;
using TechTalk.SpecFlow;

namespace Sample.Tests
{
    [Binding]
    public class UmbracoViewBindings
    {
        private readonly ViewsUnderTest viewsUnderTest;

        public UmbracoViewBindings(ViewsUnderTest viewsUnderTest)
        {
            this.viewsUnderTest = viewsUnderTest;
        }

        [BeforeScenario()]
        public void Setup()
        {
            viewsUnderTest.AddView<_Views_TextPage_cshtml>("TextPage");
            viewsUnderTest.AddPartial<_Views_Partials_Grid_Fanoe_cshtml>("Grid/fanoe");
            viewsUnderTest.AddPartial<_Views_Partials_Grid_Editors_Base_cshtml>("grid/editors/base");
            viewsUnderTest.AddPartial<_Views_Partials_Grid_Editors_Textstring_cshtml>("grid/editors/textstring");
            viewsUnderTest.AddPartial<_Views_Partials_Grid_Editors_Rte_cshtml>("grid/editors/rte");
            viewsUnderTest.AddPartial<_Views_Partials_Grid_Editors_Addition_cshtml>("grid/editors/addition");
            viewsUnderTest.AddPartial<_Views_Addition_Index_cshtml>("Addition/Index");
        }
    }
}
