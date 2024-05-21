namespace testtt.Models.ViewsModels
{
	public class CartItemViewModel
	{
		public CartItem CartItem { get; set; } // Assuming CartItem is the class representing items in the cart
		public Product Product { get; set; }
		public Cart Cart { get; set; }
		public Payment Payment { get; set; }


	}
}
