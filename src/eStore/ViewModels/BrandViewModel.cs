using Microsoft.AspNet.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using eStore.Models;

namespace eStore.ViewModels
{
    public class BrandViewModel
    {
        public BrandViewModel() { }
        public string BrandName { get; set; }
        public List<Brand> Brands { get; set; }
        public IEnumerable<SelectListItem> GetBrandNames()
        {
            return Brands.Select(brand => new SelectListItem
            {
                Text = brand.Name,
                Value = brand.Name
            });
        }
        public IEnumerable<Product> Products { get; set; }
    }
}