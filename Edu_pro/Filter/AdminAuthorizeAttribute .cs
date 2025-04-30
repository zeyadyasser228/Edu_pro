using Microsoft.AspNetCore.Authorization;

namespace EduPro.Filter

{
    public class AdminAuthorizeAttribute : AuthorizeAttribute
    {
        public AdminAuthorizeAttribute()
        {
            Roles = "Admin";
        }
    }
}
