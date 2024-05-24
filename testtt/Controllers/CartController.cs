using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using System.Dynamic;
using System.Security.Claims;
using testtt.Data;
using testtt.Models;
using testtt.Models.ViewsModels;

namespace testtt.Controllers
{
	public class CartController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<Customer> _userManager;
		private readonly IToastNotification _toastNotification;

		public CartController(ApplicationDbContext context, UserManager<Customer> userManager, IToastNotification toastNotification)
		{
			_context = context;
			_userManager = userManager;
			_toastNotification = toastNotification;
		}

		[Authorize]
		public IActionResult Index(int? id)
		{
			//dynamic viewModel = new ExpandoObject();
			List<CartItemViewModel> cartItemViewModels = new List<CartItemViewModel>();


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

			//var prodmodel = await _context.Products.FindAsync(id);
			//var existingCartItem = _context.CartItems.FirstOrDefault(ci => ci.Cart_ID == cart.Cart_ID && ci.Prod_ID == id);
			decimal total = 0;
			if (userCart != null)
			{
				foreach (var cartItem in userCart.CartItems)
				{
					var cartItemViewModel = new CartItemViewModel
					{
						CartItem = cartItem,
						Product = cartItem.Product,
						Cart= userCart

					};
					cartItemViewModels.Add(cartItemViewModel);

					total += cartItem.Sub_total;
				}
                userCart.Total = total;
            }
			//else
			//{
			//	viewModel.Products = new List<Product>(); // If userCart is null, provide an empty list of products
			//	viewModel.UserCart = null; // Set UserCart to null
			//}
			//         var carts = _context.Carts.ToList();
			//viewModel.Carts = carts;

			//var cartitems = _context.CartItems.Include(c=>c.Product).Include(c=>c.Cart);
			//         viewModel.CartItems = cartitems;

			//userCart.Total = total;
			_context.SaveChanges();

			return View(cartItemViewModels);
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> UpdateQuantity(int productId, int quantity)
		{
			// Retrieve the cart item by product ID
			//var cartItem = _context.CartItems.FirstOrDefault(ci => ci.Prod_ID == productId);

			var user = await _userManager.GetUserAsync(User);

			var cart = _context.Carts.FirstOrDefault(c => c.Cus_ID == user.Id);
			if (cart == null)
			{
				// User does not have a cart, handle this scenario
				return Json(new { success = false, message = "Cart not found for the user." });
			}

			var existingCartItem = _context.CartItems.FirstOrDefault(ci => ci.Cart_ID == cart.Cart_ID && ci.Prod_ID == productId);
			var product = _context.Products.FirstOrDefault(p => p.Prod_ID == productId);

			if (existingCartItem != null && product != null)
			{
				var originalStock = product.Prod_Stock; /*+ existingCartItem.Quantity;*/

				if (quantity <= 0)
				{
					return Json(new { success = false, message = "Quantity must be greater than 0." });
				}
				else if (quantity > originalStock)
				{
					return Json(new { success = false, message = "Requested quantity exceeds available stock." });
				}
				else
				{
					var quantityDifference = quantity - existingCartItem.Quantity;
					existingCartItem.Quantity = quantity;
					existingCartItem.Sub_total = quantity * existingCartItem.Unit_price;

					//var stockChange = quantityDifference;
					//product.Prod_Stock -= stockChange;

					_context.SaveChanges();

					return Json(new { success = true });
				}
				//if (quantity > 0 && quantity <= originalStock)
				//{
				//	var quantityDifference = quantity - existingCartItem.Quantity;
				//	existingCartItem.Quantity = quantity;
				//	existingCartItem.Sub_total = quantity * existingCartItem.Unit_price;

				//	var stockChange = quantityDifference;
				//	product.Prod_Stock -= stockChange;

				//	_context.SaveChanges();

				//	return Json(new { success = true});
				//}
				//else
				//{
				//	return Json(new { success = false});
				//}
			}
			else
			{
				return Json(new { success = false, message = "Cart item not found." });
			}

		}

		[Authorize]
        [HttpPost]
		[Authorize]
		[HttpDelete]
		public IActionResult DeleteCartItem(int productId)
		{
			var user = _userManager.GetUserAsync(User).Result;

			var product = _context.Products.FirstOrDefault(p => p.Prod_ID == productId);

			var cart = _context.Carts.FirstOrDefault(c => c.Cus_ID == user.Id);
			if (cart == null)
			{
				// User does not have a cart, handle this scenario
				return Json(new { success = false, message = "Cart not found for the user." });
			}

			var cartItem = _context.CartItems.FirstOrDefault(ci => ci.Cart_ID == cart.Cart_ID && ci.Prod_ID == productId);
			if (cartItem != null)
			{
				product.Prod_Stock = product.Prod_Stock + cartItem.Quantity;
				_context.CartItems.Remove(cartItem);
				
				_context.SaveChanges();
				return Json(new { success = true, message = "Cart item deleted successfully." });
			}
			else
			{
				return Json(new { success = false, message = "Cart item not found." });
			}
		}






	}
}
