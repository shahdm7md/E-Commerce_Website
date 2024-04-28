using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace testtt.Models
{
    public class CartItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(Order = 1)]
        public int CartItem_ID { get; set; }

        [ForeignKey("Product")]
        public int Prod_ID { get; set; }

        [ForeignKey("Cart")]
        public int Cart_ID { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
        public int Quantity { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Unit price must be greater than or equal to 0.")]
        public decimal Unit_price { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Subtotal must be greater than or equal to 0.")]
        public decimal Sub_total { get; set; }

        public decimal? Discount_rate { get; set; }

        public decimal Total { get; set; }

        // Navigation property for Product
        public virtual Product Product { get; set; }

        // Navigation property for Cart
        public virtual Cart Cart { get; set; }
    }
}
