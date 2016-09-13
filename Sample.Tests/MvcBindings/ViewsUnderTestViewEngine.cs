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
            return TryFindView(viewTypes, viewName, masterName);
        }

        public WebViewPage CreatePartialView(string partialViewName, string layoutName = null)
        {
            var viewType = FindViewType(partialTypes, partialViewName);
            if (viewType != null)
                return (WebViewPage)Activator.CreateInstance(viewType);
            return null;
        }

        public WebViewPage CreateView(string viewName)
        {
            var viewType = FindViewType(viewTypes, viewName);
            if (viewType != null)
                return (WebViewPage)Activator.CreateInstance(viewType);
            return null;
        }

        public void ReleaseView(ControllerContext controllerContext, IView view)
        {
        }

        private ViewEngineResult TryFindView(Dictionary<string, Type> viewTypeDictionary, string viewPath, string layoutPath = null)
        {
            var view = CreateView(viewTypeDictionary, viewPath, layoutPath);
            if (view != null)
                return new ViewEngineResult(view, this);
            return new ViewEngineResult(new string[0]);
        }

        private static IView CreateView(Dictionary<string, Type> viewTypeDictionary, string viewPath, string layoutPath = null)
        {
            var type = FindViewType(viewTypeDictionary, viewPath);
            if (type != null)
                return new PrecompiledMvcView(viewPath, layoutPath, type, false, new string[0]);
            return null;
        }

        private static Type FindViewType(Dictionary<string, Type> viewTypeDictionary, string viewPath)
        {
            if (String.IsNullOrWhiteSpace(viewPath))
                return null;

            var key = viewTypeDictionary.Keys.FirstOrDefault(k => viewPath.Contains(k) || k.Contains(viewPath));
            if (key != null)
                return viewTypeDictionary[key];
            return null;
        }
    }
}
