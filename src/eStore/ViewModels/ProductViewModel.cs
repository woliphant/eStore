using eStore.Models;
using System.Collections.Generic;

namespace eStore.ViewModels
{
    public class ProductViewModel
    {
        public string BrandName { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}