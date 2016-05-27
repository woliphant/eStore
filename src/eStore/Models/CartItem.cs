using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eStore.Models
{
    public class CartItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int CartId { get; set; }
        [Required]
        [StringLength(15)]
        public string ProductId { get; set; }
        [Required]
        public int Qty { get; set; }
        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        public byte[] Timer { get; set; }
        [ForeignKey("CartId")]
        public Cart Cart { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        [Required]
        public int QtyOnHand { get; set; }
        [Required]
        public int QtyOrdered { get; set; }
        [Required]
        public int QtyBackOrdered { get; set; }
    }
}
