using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Http;

namespace eStore.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public ActionResult ClearCart()
        {
            HttpContext.Session.Remove("cart"); // clear out current tray
            HttpContext.Session.SetString("Message", "Cart Cleared"); // clear out current cart once order has been placed
            return Redirect("/Home");
        }
    }
}