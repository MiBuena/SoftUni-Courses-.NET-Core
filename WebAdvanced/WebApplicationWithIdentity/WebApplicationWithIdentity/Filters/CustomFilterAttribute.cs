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
        private readonly IConfiguration _configuration;

        public CustomFilterAttribute(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var functionalityConfigValue = _configuration["IsFunctionalityEnabled"];

            var isFunctionalityEnabled = functionalityConfigValue != null && functionalityConfigValue.ToLower() == "true";

            if (!isFunctionalityEnabled)
            {
                context.Result = new RedirectToActionResult("Index", "Home", null);
            }

            base.OnActionExecuting(context);
        }
    }
}
