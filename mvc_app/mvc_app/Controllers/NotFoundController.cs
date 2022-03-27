using Microsoft.AspNetCore.Mvc;

namespace mvc_app.Controllers
{
    public class NotFoundController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
