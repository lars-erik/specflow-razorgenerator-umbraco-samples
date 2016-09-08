using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;

namespace Sample.Tests.MvcBindings
{
    public class ControllersUnderTestFactory : IControllerFactory
    {
        readonly Dictionary<string, Func<Controller>> factories = new Dictionary<string, Func<Controller>>();

        public void Add(string name, Func<Controller> factory)
        {
            factories.Add(name, factory);
        }

        public IController CreateController(RequestContext requestContext, string controllerName)
        {
            if (factories.ContainsKey(controllerName))
            {
                var controller = factories[controllerName]();
                return controller;
            }
            return null;
        }

        public SessionStateBehavior GetControllerSessionBehavior(RequestContext requestContext, string controllerName)
        {
            return default(SessionStateBehavior);
        }

        public void ReleaseController(IController controller)
        {
        }
    }
}