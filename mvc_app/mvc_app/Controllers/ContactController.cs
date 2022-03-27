using Microsoft.AspNetCore.Mvc;

namespace mvc_app.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
