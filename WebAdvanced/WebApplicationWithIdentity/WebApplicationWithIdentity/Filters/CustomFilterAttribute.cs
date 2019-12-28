using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationWithIdentity.Filters
{
    public class CustomFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            IConfiguration config = (IConfiguration)context.HttpContext.RequestServices.GetService(typeof(IConfiguration));

            var areEnabledConfigValue = config["IsFunctionalityEnabled"];

            var areAdvertsEnabled = areEnabledConfigValue != null && areEnabledConfigValue.ToLower() == "true";

            if (!areAdvertsEnabled)
            {
                context.Result = new RedirectToActionResult("Index", "Home", null);
            }

            base.OnActionExecuting(context);
        }
    }
}
