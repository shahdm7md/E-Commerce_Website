using Microsoft.AspNetCore.Identity;
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
		private readonly UserManager<Customer> _userManager;

		public CartController(ApplicationDbContext context, UserManager<Customer> userManager)
        {
            _context = context;
			_userManager = userManager;
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

			var user = _userManager.GetUserAsync(User).Result;
			var cart = _context.Carts.FirstOrDefault(c => c.Cus_ID == user.Id);
			if (cart == null)
			{
				cart = new Cart { Cus_ID = user.Id };
				_context.Carts.Add(cart);
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
		public async Task<IActionResult> UpdateQuantity(int productId, int quantity)
		{
			// Retrieve the cart item by product ID
			//var cartItem = _context.CartItems.FirstOrDefault(ci => ci.Prod_ID == productId);

			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				return Unauthorized("User not authenticated.");
			}

			var cart = _context.Carts.FirstOrDefault(c => c.Cus_ID == user.Id);
			if (cart == null)
			{
				// User does not have a cart, handle this scenario
				return BadRequest("Cart not found for the user.");
			}

			var existingCartItem = _context.CartItems.FirstOrDefault(ci => ci.Cart_ID == cart.Cart_ID && ci.Prod_ID == productId);

			if (existingCartItem != null)
			{
				// Update the quantity of the cart item
				existingCartItem.Quantity = quantity;

				// Save changes to the database
				_context.SaveChanges();

				// Return a success response if needed
				return Ok("Quantity updated successfully.");
			}
			else
			{
				// Return an error response if the cart item is not found
				return NotFound("Cart item not found.");
			}

		}




	}
}
