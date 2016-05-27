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
                                _db.CartItems.Add(cItem);
                                _db.SaveChanges();
                            }

                            // Update the Product Table
                            CartItem cartItem = new CartItem();
                            cartItem.Product = _db.Products.FirstOrDefault(p => p.Id == cartItem.ProductId);
                            if(cartItem.Product.QtyOnHand > cartItem.Qty)
                            {

                            }
                            else
                            {
                                //cartItem.
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
    }
}