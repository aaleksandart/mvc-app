using Microsoft.AspNetCore.Mvc;

namespace mvc_app_login.Controllers
{
    public class NotFoundController : Controller
    {
        [Route("notfound")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
