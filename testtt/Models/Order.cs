using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace testtt.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(Order = 1)]
        public int Order_ID { get; set; }

        [Required(ErrorMessage = "Order date is required.")]
        public DateTime Order_date { get; set; }

        [Required(ErrorMessage = "Order status is required.")]
        public string Order_Status { get; set; }

        [Required(ErrorMessage = "Total amount is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Total amount must be greater than or equal to 0.")]
        public decimal Total_amount { get; set; }

        [Required(ErrorMessage = "Shipping address is required.")]
        public string Shipping_address { get; set; }

        public decimal? Discount { get; set; }

        [ForeignKey("Customer")]
        public string Cus_ID { get; set; }

        public virtual Customer Customer { get; set; }

        // Navigation property for OrderDetails
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        // Navigation property for Payment
        public virtual Payment Payment { get; set; }
    }
}
