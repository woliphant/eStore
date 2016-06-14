using Microsoft.AspNet.Mvc;
using eStore.Models;
using eStore.ViewModels;
using Microsoft.AspNet.Http;
using System.Collections.Generic;
using System;

namespace eStore.Controllers
{
    public class BrandController : Controller
    {
        AppDbContext _db;
        public BrandController(AppDbContext context)
        {
            _db = context;
        }

        /// <summary>
        /// Main method for BrandController
        /// By default, creates a catalogue containing all products
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            CartItemsViewModel vm = new CartItemsViewModel();
            if (HttpContext.Session.GetObject<List<Brand>>("brands") == null)
            {
                try
                {
                    BrandModel braModel = new BrandModel(_db);
                    List<Brand> brands = braModel.GetAll();
                    HttpContext.Session.SetObject("brands", brands);
                    ProductModel prodModel = new ProductModel(_db);
                    List<Product> pros = prodModel.GetAllByBrand(vm.BrandId);
                    List<ProductViewModel> vms = new List<ProductViewModel>();
                    if(pros.Count == 0)
                    { 
                        foreach (Brand bra in brands)
                        {
                            List<Product> items = prodModel.GetAllByBrand(bra.Id);
                            foreach(Product item in items)
                            {
                                ProductViewModel mvm = new ProductViewModel();
                                mvm.Qty = 0;
                                mvm.BrandId = item.BrandId;
                                mvm.BrandName = braModel.GetName(item.BrandId);
                                mvm.Description = item.Description;
                                mvm.Id = item.Id;
                                mvm.CostPrice = Convert.ToDecimal(item.CostPrice);
                                mvm.GraphicName = item.GraphicName;
                                mvm.ProductName = item.ProductName;
                                vms.Add(mvm);
                            }
                        }
                        ProductViewModel[] myCart = vms.ToArray();
                        HttpContext.Session.SetObject("order", myCart);

                    }
                    vm.SetBrands(HttpContext.Session.GetObject<List<Brand>>("brands"));
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Catalogue Problem - " + ex.Message;
                }
            }
            else
            {
                vm.SetBrands(HttpContext.Session.GetObject<List<Brand>>("brands"));
            }
            return View("Index", vm);
        }

        /// <summary>
        /// Creates a Catalogue specific to the chosen Brand
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public IActionResult SelectBrand(CartItemsViewModel vm)
        {
            BrandModel braModel = new BrandModel(_db);
            ProductModel prodModel = new ProductModel(_db);
            List<Product> items = prodModel.GetAllByBrand(vm.BrandId);
            List<ProductViewModel> vms = new List<ProductViewModel>();
            if (items.Count > 0)
            {
                foreach (Product item in items)
                {
                    ProductViewModel mvm = new ProductViewModel();
                    mvm.Qty = 0;
                    mvm.BrandId = item.BrandId;
                    mvm.BrandName = braModel.GetName(item.BrandId);
                    mvm.Description = item.Description;
                    mvm.Id = item.Id;
                    mvm.CostPrice = Convert.ToDecimal(item.CostPrice);
                    mvm.GraphicName = item.GraphicName;
                    mvm.ProductName = item.ProductName;
                    vms.Add(mvm);
                }
                ProductViewModel[] myCart = vms.ToArray();
                HttpContext.Session.SetObject("order", myCart);
            }
            vm.SetBrands(HttpContext.Session.GetObject<List<Brand>>("brands"));
            return View("Index", vm);
        }

        /// <summary>
        /// Creates a Cart session if there isn't one yet created. 
        /// Adds or removes items to and from the Cart session.
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SelectItem(CartItemsViewModel vm)
        {
            Dictionary<string, object> cart;
            if (HttpContext.Session.GetObject<Dictionary<String, Object>>("cart") == null)
            {
                cart = new Dictionary<string, object>();
            }
            else
            {
                cart = HttpContext.Session.GetObject<Dictionary<string, object>>("cart");
            }
            ProductViewModel[] order = HttpContext.Session.GetObject<ProductViewModel[]>("order");
            String retMsg = "";
            foreach (ProductViewModel item in order)
            {
                if (item.Id == vm.Id)
                {
                    if (vm.Qty > 0) // update only selected item
                    {
                        item.Qty = vm.Qty;
                        retMsg = vm.Qty + " - item(s) Added!";
                        cart[item.Id] = item;
                    }
                    else
                    {
                        item.Qty = 0;
                        cart.Remove(item.Id);
                        retMsg = "item(s) Removed!";
                    }
                    vm.BrandId = item.BrandId;
                    break;
                }
            }
            ViewBag.AddMessage = retMsg;
            HttpContext.Session.SetObject("cart", cart);
            vm.SetBrands(HttpContext.Session.GetObject<List<Brand>>("brands"));
            return View("Index", vm);
        }
    }
}