using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace testtt.Models
{
    public class Payment
    {
        [Key]
        public int Pay_ID { get; set; }

        [Required(ErrorMessage = "Method is required.")]
        public string Method { get; set; }
        [Required(ErrorMessage = "Amount is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Amount must be greater than or equal to 0.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Payment date is required.")]
        public DateTime Pay_Date { get; set; }

        [ForeignKey("Order")]
        public int Order_ID { get; set; }

        // Navigation property for Order
        public virtual Order Order { get; set; }
    }
}
