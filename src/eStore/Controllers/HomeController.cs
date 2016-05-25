using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Http;

namespace eStore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Message = HttpContext.Session.GetString("Message");
            return View();
        }
    }
}
