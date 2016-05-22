using eStore.Models;
using Microsoft.AspNet.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace eStore.ViewModels
{
    public class CartViewModel
    {
        private List<Brand> _brands;
        [Required]
        public int Qty { get; set; }
        public string Id { get; set; }
        ///
        public int BrandId { get; set; }
        public IEnumerable<SelectListItem> GetBrands()
        {
            return _brands.Select(brand => new SelectListItem
            {
                Text = brand.Name, Value = Convert.ToString(brand.Id)
            });
        }
        public void SetBrands(List<Brand> bras)
        {
            _brands = bras;
        }
    }
}