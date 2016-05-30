using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace eStore.Models
{
    public class ProductModel
    {
        private AppDbContext _db;

        public ProductModel(AppDbContext context)
        {
            _db = context;
        }

        /// <summary>
        /// Loads a raw Json, grabs all brands, and stuffs it into the Brands Table
        /// </summary>
        /// <param name="rawJson"></param>
        /// <returns></returns>
        public bool loadBrands(string rawJson)
        {
            bool loadedBrands = false;
            try
            {
                _db.Brands.RemoveRange(_db.Brands);
                _db.SaveChanges();

                dynamic decodedJson = Newtonsoft.Json.JsonConvert.DeserializeObject(rawJson);
                List<String> allBrands = new List<String>();

                foreach(var b in decodedJson)
                {
                    allBrands.Add(Convert.ToString(b["Brand"]));
                }

                IEnumerable<String> brands = allBrands.Distinct<String>();

                foreach(string b in brands)
                {
                    Brand bra = new Brand();
                    bra.Name = b;
                    _db.Brands.Add(bra);
                    _db.SaveChanges();
                }
                loadedBrands = true;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error - " + ex.Message);
            }
            return loadedBrands;
        }

        /// <summary>
        /// Loads Raw Json, grabs all product elements, and stuffs it into the Product Table
        /// </summary>
        /// <param name="rawJson"></param>
        /// <returns></returns>
        public bool loadProducts(string rawJson)
        {
            bool loadedProducts = false;

            try
            {
                List<Brand> brands = _db.Brands.ToList();
                // clear out old data
                _db.Products.RemoveRange(_db.Products);
                _db.SaveChanges();
                string decodedJsonStr = Decoder(rawJson);
                dynamic productJson = Newtonsoft.Json.JsonConvert.DeserializeObject(decodedJsonStr);
                foreach(var p in productJson)
                {
                    Product prod = new Product();
                    prod.Id = Convert.ToString(p["Id"]);
                    prod.CostPrice = Convert.ToDecimal(p["CostPrice"]);
                    prod.Description = Convert.ToString(p["Description"]);
                    prod.GraphicName = Convert.ToString(p["GraphicName"]);
                    prod.MSRP = Convert.ToDecimal(p["MSRP"]);
                    prod.ProductName = Convert.ToString(p["ProductName"]);
                    prod.QtyOnBackOrder = Convert.ToInt32(p["QtyOnBackOrder"]);
                    prod.QtyOnHand = Convert.ToInt32(p["QtyOnHand"]);
                    string bra = Convert.ToString(p["Brand"]);

                    foreach(Brand brand in brands)
                    {
                        if(brand.Name == bra)
                        {
                            prod.BrandId = brand.Id;
                        }
                    }

                    _db.Products.Add(prod);
                    _db.SaveChanges();
                }
                loadedProducts = true;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error - " + ex.Message);
            }

            return loadedProducts;
        }

        public string Decoder(string value)
        {
            Regex regex = new Regex(@"\\u(?<Value>[a-zA-Z0-9]{4})", RegexOptions.Compiled);
            return regex.Replace(value, "");
        }

        /// <summary>
        /// Retrieves all products and inserts them into a list
        /// </summary>
        /// <returns></returns>
        public List<Product> GetAll()
        {
            return _db.Products.ToList();
        }

        /// <summary>
        /// Retrieves all Products via the BrandName
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Product> GetAllByBrand(int id)
        {
            return _db.Products.Where(prod => prod.BrandId == id).ToList();
        }

        /// <summary>
        /// Retrieves all Products via the BrandName
        /// </summary>
        /// <param name="braname"></param>
        /// <returns></returns>
        public List<Product> GetAllByBrandName(string braname)
        {
            Brand brand = _db.Brands.First(bra => bra.Name == braname);
            return _db.Products.Where(prod => prod.BrandId == brand.Id).ToList();
        }

        //public Product GetById(string id)
        //{
        //    //return _db.Products.FirstorDefault(prod => prod.Id == id);
        //}
    }
}
