using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Entity;
using eStore.ViewModels;

namespace eStore.Models
{
    public class CartModel
    {
        private AppDbContext _db;
        public CartModel(AppDbContext ctx)
        {
            _db = ctx;
        }

        /// <summary>
        /// Adds data into the Cart and CartItems table then updates the Products Table after user adds the cart
        /// </summary>
        /// <param name="items"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public int AddCart(Dictionary<string, object> items, string user)
        {
            int cartId = -1;
            using (_db)
            {
                // we need a transaction as multiple entities involved
                using (var _trans = _db.Database.BeginTransaction())
                {
                    try
                    {
                        Cart cart = new Cart();
                        cart.UserId = user;
                        cart.DateCreated = System.DateTime.Now;
                        cart.PriceTotal = 0;
                        foreach (var key in items.Keys)
                        {
                            ProductViewModel item = JsonConvert.DeserializeObject<ProductViewModel>(Convert.ToString(items[key]));
                            if (item.Qty > 0)
                            {
                                cart.PriceTotal += item.CostPrice * item.Qty;
                            }
                        }
                        _db.Carts.Add(cart);
                        _db.SaveChanges();
                        // then add each item to the trayitems table
                        foreach (var key in items.Keys)
                        {
                            ProductViewModel item = JsonConvert.DeserializeObject<ProductViewModel>(Convert.ToString(items[key]));
                            if (item.Qty > 0)
                            {
                                CartItem cItem = new CartItem();
                                cItem.Qty = item.Qty;
                                cItem.ProductId = item.Id;
                                cItem.CartId = cart.Id;
                                cItem.Product = _db.Products.FirstOrDefault(p => p.Id == item.Id);
                                if(cItem.Product.QtyOnHand > item.Qty)
                                {
                                    cItem.Product.QtyOnHand = cItem.Product.QtyOnHand - item.Qty;
                                    cItem.QtySold = item.Qty;
                                    cItem.QtyOrdered = item.Qty;
                                    cItem.QtyBackOrdered = 0;
                                    _db.Products.Update(cItem.Product);
                                    _db.CartItems.Add(cItem);
                                }
                                else
                                {
                                    cItem.QtyBackOrdered = item.Qty - cItem.Product.QtyOnHand;
                                    cItem.QtySold = cItem.Product.QtyOnHand;
                                    cItem.QtyOrdered = item.Qty;
                                    cItem.Product.QtyOnBackOrder = cItem.Product.QtyOnBackOrder + (item.Qty - cItem.Product.QtyOnHand);
                                    cItem.Product.QtyOnHand = 0;
                                    _db.Products.Update(cItem.Product);
                                    _db.CartItems.Add(cItem);
                                }
                                _db.SaveChanges();
                            }
                        }
                        // test trans by uncommenting out these 3 lines
                        //int x = 1;
                        //int y = 0;
                        //x = x / y;
                        _trans.Commit();
                        cartId = cart.Id;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        _trans.Rollback();
                    }
                }
            }
            return cartId;
        }

        public List<Cart> GetCarts(string uid)
        {
            try
            {
                return _db.Carts.Where(c => c.UserId == uid).ToList();
            }
            catch(Exception e)
            {
                Console.Write(e.Message);
                return null;
            }
        }

        public List<CartViewModel> GetCartDetails(int tid, string uid)
        {
            List<CartViewModel> cartdetails = new List<CartViewModel>();
            var results = from c in _db.Set<Cart>()
                          join ci in _db.Set<CartItem>() on c.Id equals ci.CartId
                          join p in _db.Set<Product>() on ci.ProductId equals p.Id
                          where (c.UserId == uid && c.Id == tid)
                          select new CartViewModel
                          {
                              CartId = p.Id,
                              UserId = uid,
                              OrderTotal = c.PriceTotal * 1.13m,
                              SubTotal = c.PriceTotal,
                              Tax = c.PriceTotal * 0.13m,
                              ProductName = p.ProductName,
                              ExtendedPrice = p.CostPrice * ci.Qty,
                              MSRP = p.CostPrice,
                              Description = p.Description,
                              QTY = ci.QtySold,
                              QTYBackOrdered = ci.QtyBackOrdered,
                              QTYOrdered = ci.QtyOrdered,
                              DateCreated = c.DateCreated.ToString("yyyy/MM/dd - hh:mm tt")
                          };
            cartdetails = results.ToList<CartViewModel>();
            return cartdetails;
        }
    }
}