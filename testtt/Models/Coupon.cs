using System;
using System.ComponentModel.DataAnnotations;

namespace testtt.Models
{
    public class Coupon
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Code is required")]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "Code must be between 3 and 10 characters")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Code must contain only letters and numbers")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Discount amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Discount amount must be greater than zero")]
        public decimal DiscountAmount { get; set; }

        public bool IsPercentage { get; set; }

        [Required(ErrorMessage = "Expiry date is required")]
        [FutureDate(ErrorMessage = "Expiry date must be in the future")]
        public DateTime ExpiryDate { get; set; }

        public bool IsActive { get; set; }
    }

    public class FutureDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime date;
            if (DateTime.TryParse(value.ToString(), out date))
            {
                return date > DateTime.Now;
            }
            return false;
        }
    }
}
