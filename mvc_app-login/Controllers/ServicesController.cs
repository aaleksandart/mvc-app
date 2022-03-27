using Microsoft.AspNetCore.Mvc;

namespace mvc_app_login.Controllers
{
    public class ServicesController : Controller
    {
        [Route("services")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
