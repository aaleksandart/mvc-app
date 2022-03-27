using Microsoft.AspNetCore.Mvc;

namespace mvc_app_login.Controllers
{
    public class ContactController : Controller
    {
        [Route("contact")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
