using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using RazorGenerator.Mvc;

namespace Sample.Tests.MvcBindings
{
    public class ViewsUnderTestViewEngine : IViewEngine
    {
        readonly Dictionary<string, Type> partialTypes = new Dictionary<string, Type>();
        readonly Dictionary<string, Type> viewTypes = new Dictionary<string, Type>();

        public void AddPartial<TPartial>(string path)
        {
            partialTypes.Add(path, typeof(TPartial));
        }

        public void Add<TView>(string path)
        {
            viewTypes.Add(path, typeof(TView));
        }

        public ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            return TryFindView(partialTypes, partialViewName);
        }

        public ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            return TryFindView(viewTypes, viewName);
        }

        public void ReleaseView(ControllerContext controllerContext, IView view)
        {
        }

        private ViewEngineResult TryFindView(Dictionary<string, Type> viewTypeDictionary, string viewPath)
        {
            var key = viewTypeDictionary.Keys.FirstOrDefault(k => viewPath.Contains(k) || k.Contains(viewPath));
            if (key != null)
            {
                var view = new PrecompiledMvcView(viewPath, viewTypeDictionary[key], false, new string[0]);
                return new ViewEngineResult(view, this);
            }
            return new ViewEngineResult(new string[0]);
        }
    }
}
