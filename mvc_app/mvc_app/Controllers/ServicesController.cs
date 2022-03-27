using Microsoft.AspNetCore.Mvc;

namespace mvc_app.Controllers
{
    public class ServicesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
