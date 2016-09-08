using System;
using System.Web.Mvc;

namespace Sample.Tests.MvcBindings
{
    public class ControllersUnderTest
    {
        readonly ControllersUnderTestFactory testFactory = new ControllersUnderTestFactory();

        public ControllersUnderTestFactory Factory
        {
            get { return testFactory; }
        }

        public void Add(string name, Func<Controller> factory)
        {
            testFactory.Add(name, factory);
        }
    }
}