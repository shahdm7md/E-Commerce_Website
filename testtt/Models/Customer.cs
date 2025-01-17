﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace testtt.Models
{
    public class Customer : IdentityUser
    {
        [Required(ErrorMessage = "First Name is required.")]
        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "First name must contain only letters.")]
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        public string Cus_FName { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "Last name must contain only letters.")]
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        public string Cus_LName { get; set; }

        //[Required]
        //public string Cus_Password { get; set; }

        //[Required]
        //[EmailAddress]
        //public string Cus_Email { get; set; }

        //[Required(ErrorMessage = "Address is required.")]
        //[RegularExpression(@"^(?=.*[a-zA-Z])[a-zA-Z0-9\s.,#-]+$", ErrorMessage = "Invalid address format.")]
        //public string Cus_address { get; set; }

        // Navigation property for Orders
        public string? Cus_ImageUrl { get; set; }
		//public string Cus_phonenumber { get; set; }
		public virtual ICollection<Order> Orders { get; set; }

		public ICollection<Address> Addresses { get; set; }

	}
}
