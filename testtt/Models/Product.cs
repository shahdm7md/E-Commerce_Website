﻿using System.ComponentModel.DataAnnotations;

namespace testtt.Models
{
    public class Product
    {
        [Key]
        public int Prod_ID { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string Prod_Name { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Prod_Description { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than or equal to 0.")]
        public decimal Prod_Price { get; set; }

        [Required(ErrorMessage = "Stock is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock must be greater than or equal to 0.")]
        public int Prod_Stock { get; set; }

        [RegularExpression(@"^(https?|ftp):\/\/[^\s\/$.?#].[^\s]*$", ErrorMessage = "Invalid URL.")]
        public string Prod_ImageUrl { get; set; }

        // Navigation property for OrderDetails
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        // Navigation property for CartItems
        public virtual ICollection<CartItem> CartItems { get; set; }
    }
}