using Microsoft.AspNetCore.Mvc;

namespace DoctorWebForum.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View("Blog");
        }
    }
}
