using System.ComponentModel.DataAnnotations;
//using System.Collections.Generic;

namespace testtt.Models.ViewsModels
{
    public class ProductViewModel
    {
        [Display(Name = "Product Name")]
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string Prod_Name { get; set; }

        [Display(Name = "Product Description")]
        [Required(ErrorMessage = "Description is required.")]
        public string Prod_Description { get; set; }

        [Display(Name = "Product Price")]
        [Required(ErrorMessage = "Price is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than or equal to 0.")]
        public decimal Prod_Price { get; set; }

        [Display(Name = "Stock")]
        [Required(ErrorMessage = "Stock is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock must be greater than or equal to 0.")]
        public int Prod_Stock { get; set; }

        [Display (Name ="Product Picture")]
        [Required(ErrorMessage = "Prod_Image is required.")]
        public byte[] Prod_Image { get; set; }
        //public byte[] Prod_ImageUrl { get; set; }
    }
}
