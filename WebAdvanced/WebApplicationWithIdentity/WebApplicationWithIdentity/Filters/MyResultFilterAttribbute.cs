using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationWithIdentity.Filters
{
    public class MyResultFilterAttribbute : ResultFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            base.OnResultExecuting(context);
        }
    }
}
