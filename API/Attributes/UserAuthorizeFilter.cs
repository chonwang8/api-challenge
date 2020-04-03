using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace API.Attributes
{
    public class UserAuthorizeFilter : Attribute, IAuthorizationFilter
    {
        private readonly string _permission;

        public UserAuthorizeFilter(string permission)
        {
            _permission = permission;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string position = "";
            if (context.HttpContext.User.Claims == null || !context.HttpContext.User.Claims.Any())
            {
                context.Result = new ForbidResult("Unauthorized access - No token was found");
            }
            else
            {
                var emailClaim = context.HttpContext.User.Claims.FirstOrDefault(z => z.Type == "user_email");
                if(emailClaim != null)
                {
                    var email = emailClaim.Value;
                }
                else
                {
                    context.Result = new ForbidResult("Unauthorized access - Faulthy token");
                }

                var positionClaim = context.HttpContext.User.Claims.FirstOrDefault(z => z.Type == "position");
                if (positionClaim != null)
                {
                    position = positionClaim.Value;
                }
                else
                {
                    context.Result = new ForbidResult("Unauthorized access - Faulthy token");
                }

                if (!_permission.Contains(position))
                {
                    context.Result = new ForbidResult();
                }
            }
        }
    }
}
