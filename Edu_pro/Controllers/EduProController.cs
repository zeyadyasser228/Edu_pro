using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;

namespace Edu_pro.Controllers
{
    public class EduProController : Controller
    {
      

        public IActionResult Index()
        {
            ViewBag.IsAuthenticated = User.Identity.IsAuthenticated;

            if (User.Identity.IsAuthenticated)
            {
                ViewBag.UserName = User.FindFirstValue(ClaimTypes.Name);
                ViewBag.UserEmail = User.FindFirstValue(ClaimTypes.Email);
            }

            return View("edupro");  
        }

      
        public IActionResult Dashboard()
        {
            return View("dashboard");
        }

        public IActionResult signup()
        {
            return View("signup");
        }

        public IActionResult forgetpass()
        {
            return View("forget-password");
        }


        public IActionResult payment()
        {
            return View("payment");
        }


        public IActionResult course()
        {
            return View("course");
        }

        public IActionResult Settings()
        {
            return View("Settings");
        }

        public IActionResult Cart()
        {
            return View("Cart");
        }


        public IActionResult MyCourses()
        {
            return RedirectToAction("MyCourses", "Courses");
        }


    }
}
