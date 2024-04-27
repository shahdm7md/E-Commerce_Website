using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace testtt.Models
{
    public class OrderDetail
    {
        [Key]
        public int Order_Details_ID { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Unit price is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Unit price must be greater than or equal to 0.")]
        public decimal Unit_price { get; set; }

        [Required(ErrorMessage = "Subtotal is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Subtotal must be greater than or equal to 0.")]
        public decimal Sub_Total { get; set; }

        public decimal? Discount_rate { get; set; }

        public decimal Total { get; set; }

        [ForeignKey("Order")]
        public int Order_ID { get; set; }

        [ForeignKey("Product")]
        public int Prod_ID { get; set; }

        // Navigation property for Order
        public virtual Order Order { get; set; }

        // Navigation property for Product
        public virtual Product Product { get; set; }
    }
}
