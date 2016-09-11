using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sample.Web.Models;
using Umbraco.Web.Mvc;

namespace Sample.Web.Controllers
{
    public class AdditionController : SurfaceController
    {
        public PartialViewResult Index()
        {
            var model = ControllerContext.ParentActionViewContext.ViewData["AdditionModel"]
                        ?? new AdditionModel();

            return PartialView(model);
        }

        [HttpPost]
        public UmbracoPageResult Add(AdditionModel model)
        {
            model.Result = model.A + model.B;
            ViewData["AdditionModel"] = model;

            return CurrentUmbracoPage();
        }
    }
}