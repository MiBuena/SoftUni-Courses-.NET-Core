using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationWithIdentity.Filters
{
    public class AuthorizationTryFilterAttribute : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
        }
    }
}
