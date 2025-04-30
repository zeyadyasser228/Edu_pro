using Microsoft.AspNetCore.Authorization;

namespace EduPro.Filer
{
    public class AdminAuthorizeAttribute : AuthorizeAttribute
    {
        public AdminAuthorizeAttribute()
        {
            Roles = "Admin";
        }
    }
}
