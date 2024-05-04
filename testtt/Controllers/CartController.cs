using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;
using System.Security.Claims;
using testtt.Data;
using testtt.Models;

namespace testtt.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public  IActionResult Index()
        {
            dynamic viewModel= new ExpandoObject();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Find the user's cart
            var userCart = _context.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                .FirstOrDefault(c => c.Cus_ID == userId);

            if (userCart == null)
            {
                // If the user doesn't have a cart, create an empty cart
                userCart = new Cart { Cus_ID = userId };
                _context.Carts.Add(userCart);
                _context.SaveChanges();
            }


            //var products =  _context.Products.ToList();
            var productsInCart = userCart.CartItems.Select(ci => ci.Product).ToList();

            //viewModel.Products = products;
            viewModel.Products = productsInCart;
            viewModel.UserCart = userCart;

   //         var carts = _context.Carts.ToList();
			//viewModel.Carts = carts;

			//var cartitems = _context.CartItems.Include(c=>c.Product).Include(c=>c.Cart);
   //         viewModel.CartItems = cartitems;
			return View(viewModel);
        }


		[HttpPost]
		public IActionResult UpdateQuantity(int productId, int quantity)
		{
			// Get the currently logged-in user's ID
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			// Find the user's cart
			var userCart = _context.Carts
				.Include(c => c.CartItems)
					.ThenInclude(ci => ci.Product)
				.FirstOrDefault(c => c.Cus_ID == userId);

			if (userCart == null)
			{
				return NotFound("Cart not found for the current user.");
			}

			// Find the cart item corresponding to the specified product
			var cartItem = userCart.CartItems.FirstOrDefault(ci => ci.Product.Prod_ID == productId);

			if (cartItem == null)
			{
				return NotFound("Product not found in the cart.");
			}

			// Update the quantity of the cart item
			cartItem.Quantity = quantity;

			// Save changes to the database
			_context.SaveChanges();

			// Redirect back to the cart page or return a success message
			return Ok("Quantity updated successfully.");
		}




	}
}
