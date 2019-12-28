using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationWithIdentity.Filters
{
    public class MyResourceFilterAttribute : IResourceFilter
    {
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
        }
    }
}
