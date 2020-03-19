﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace API.Attributes
{
    public class MyAuthorizeFilter : IAuthorizationFilter
    {
        private readonly string _permission;

        public MyAuthorizeFilter(string permission)
        {
            _permission = permission;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var email = context.HttpContext.User.Claims.FirstOrDefault(z => z.Type == "user_email").Value;
            var position = context.HttpContext.User.Claims.FirstOrDefault(z => z.Type == "position").Value;
            if (!_permission.Contains(position))
            {
                context.Result = new ForbidResult();
            }

        }
    }
}
