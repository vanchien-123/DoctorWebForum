using Microsoft.AspNetCore.Mvc;

namespace DoctorWebForum.Controllers
{
    public class NewsController : Controller
    {
        public IActionResult Index()
        {
            return View("New");
        }
    }
}
