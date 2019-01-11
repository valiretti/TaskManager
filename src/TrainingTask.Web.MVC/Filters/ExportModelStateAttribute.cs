using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TrainingTask.Web.MVC.Models.ModelState;

namespace TrainingTask.Web.MVC.Filters
{
    public class ExportModelStateAttribute : ModelStateTransfer
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (!filterContext.ModelState.IsValid)
            {
                if (filterContext.Result is RedirectResult || filterContext.Result is RedirectToRouteResult || filterContext.Result is RedirectToActionResult)
                {
                    if (filterContext.Controller is Controller controller && filterContext.ModelState != null)
                    {
                        var modelState = ModelStateHelpers.SerializeModelState(filterContext.ModelState);
                        controller.TempData[Key] = modelState;
                    }
                }
            }

            base.OnActionExecuted(filterContext);
        }
    }
}
