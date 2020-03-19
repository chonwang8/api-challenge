using Microsoft.AspNetCore.Mvc;

namespace API.Attributes
{
    public class MyAuthorizeAttribute : TypeFilterAttribute
    {
        public MyAuthorizeAttribute(string permission) : base(typeof(MyAuthorizeFilter))
        {
            Arguments = new object[] { permission };
        }
    }
}
