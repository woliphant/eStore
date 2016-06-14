using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eStore.ViewModels
{
    public class CartViewModel
    {
        public string CartId { get; set; }
        public string ProductId { get; set; }
        public string UserId { get; set; }
        public int QTY { get; set; }
        public int QTYBackOrdered { get; set; }
        public int QTYOrdered { get; set; }
        public string ProductName { get; set; }
        public decimal MSRP { get; set; }
        public decimal ExtendedPrice { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Tax { get; set; }
        public string DateCreated { get; set; }
        public decimal OrderTotal { get; set; }
        public string Description { get; set; }
    }
}
