//using Microsoft.AspNetCore.Mvc;
//using testtt.Models;
//using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

//namespace testtt.Controllers
//{
//	public class checkoutController : Controller
//	{
//		public IActionResult Index()
//		{
//			return View();
//		}


//	}
//}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using testtt.Data;
using testtt.Models;
using testtt.Models.ViewsModels;

namespace testtt.Controllers
{
	public class CheckoutController : Controller
	{
		private readonly ApplicationDbContext _context;

		public CheckoutController(ApplicationDbContext context)
		{
			_context = context;
		}

		[Authorize]
		[Authorize]
		public IActionResult Index()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			// Find the user's cart
			var userCart = _context.Carts
				.Include(c => c.CartItems)
				.ThenInclude(ci => ci.Product)
				.FirstOrDefault(c => c.Cus_ID == userId);

			if (userCart == null || userCart.CartItems.Count == 0)
			{
				// Handle the scenario where the user's cart is empty
				return RedirectToAction("Index", "Cart");
			}

			// Create a list of CartItemViewModel
			var cartItemViewModels = userCart.CartItems.Select(cartItem => new CartItemViewModel
			{
				CartItem = cartItem,
				Product = cartItem.Product,
				Cart = userCart
			}).ToList();

			return View(cartItemViewModels);
		}


		[Authorize]
		[HttpPost]
		public IActionResult PlaceOrder()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			// Find the user's cart
			var userCart = _context.Carts
				.Include(c => c.CartItems)
				.ThenInclude(ci => ci.Product)
				.FirstOrDefault(c => c.Cus_ID == userId);

			if (userCart == null || userCart.CartItems.Count == 0)
			{
				// Handle the scenario where the user's cart is empty
				return RedirectToAction("Index", "Cart");
			}

			// You can add additional logic here to place the order,
			// update inventory, create order records, etc.

			// For example, you might want to clear the cart after placing the order
			_context.CartItems.RemoveRange(userCart.CartItems);
			userCart.Total = 0;
			_context.SaveChanges();

			// Redirect to a confirmation page or display a success message
			return RedirectToAction("Confirmation");
		}

		public IActionResult Confirmation()
		{
			// You can display a confirmation message or details here
			return View();
		}
	}
}

