using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eStore.Models
{
    public partial class Product
    {
        public Product()
        {
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(15)]
        public string Id { get; set; }

        [Required]
        public int BrandId { get; set; }

        [Required]
        [StringLength(50)]
        public string ProductName { get; set; }

        [Required]
        [StringLength(20)]
        public string GraphicName { get; set; }

        [Column(TypeName = "money")]
        public decimal CostPrice { get; set; }

        [Column(TypeName = "money")]
        public decimal MSRP { get; set; }

        public int QtyOnHand { get; set; }

        public int QtyOnBackOrder { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }

        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        public byte[] Timer { get; set; }
    }
}
