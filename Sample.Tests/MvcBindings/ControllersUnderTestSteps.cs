using System.Web.Mvc;
using TechTalk.SpecFlow;

namespace Sample.Tests.MvcBindings
{
    [Binding]
    public class ControllersUnderTestSteps
    {
        private readonly ControllersUnderTest controllersUnderTest;

        public ControllersUnderTestSteps(ControllersUnderTest controllersUnderTest)
        {
            this.controllersUnderTest = controllersUnderTest;
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            ControllerBuilder.Current.SetControllerFactory(controllersUnderTest.Factory);
        }
    }
}