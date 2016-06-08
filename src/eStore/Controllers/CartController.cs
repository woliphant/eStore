using System;
using System.Collections.Generic;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Http;
using eStore.Utils;
using eStore.Models;

namespace eStore.Controllers
{
    public class CartController : Controller
    {
        AppDbContext _db;
        public CartController(AppDbContext context)
        {
            _db = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Clear all contents from your Cart
        /// </summary>
        /// <returns></returns>
        public ActionResult ClearCart()
        {
            HttpContext.Session.Remove("cart"); // clear out current tray
            HttpContext.Session.SetString(SessionVars.Message, "Cart Cleared"); // clear out current cart once order has been placed
            return Redirect("/Home");
        }

        /// <summary>
        /// Add your cart to your database, pass the session info to the database.
        /// </summary>
        /// <returns></returns>
        public ActionResult AddCart()
        {
            // they can't add a Tray if they're not logged on
            if (HttpContext.Session.GetString(SessionVars.User) == null)
            {
                return Redirect("/Login");
            }
            CartModel model = new CartModel(_db);
            int retVal = -1;
            string retMessage = "";
            try
            {
                Dictionary<string, object> cartItems = HttpContext.Session.GetObject<Dictionary<string, object>>(SessionVars.Cart);
                retVal = model.AddCart(cartItems, HttpContext.Session.GetString(SessionVars.User));
                if (retVal > 0) // Tray Added
                {
                    retMessage = "Cart " + retVal + " Created!";
                }
                else // problem
                {
                    retMessage = "Cart not added, try again later";
                }
            }
            catch (Exception ex) // big problem
            {
                retMessage = "Cart was not created, try again later! - " + ex.Message;
            }
            HttpContext.Session.Remove(SessionVars.Cart); // clear out current tray once persisted
            HttpContext.Session.SetString(SessionVars.Message, retMessage);
            return Redirect("/Home");
        }

        public IActionResult List()
        {
            if (HttpContext.Session.GetString(SessionVars.User) == null)
            {
                return Redirect("/Login");
            }
            return View("List");
        }
        [Route("[action]")]
        public IActionResult GetCarts()
        {
            CartModel model = new CartModel(_db);
            return Ok(model.GetCarts(HttpContext.Session.GetString(SessionVars.User)));
        }
    }
}
