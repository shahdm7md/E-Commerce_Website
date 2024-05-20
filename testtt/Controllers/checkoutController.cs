using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
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
		public async Task<IActionResult> PlaceOrder([FromForm] OrderDetail checkoutForm)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			// Find the user's cart
			var userCart = await _context.Carts
				.Include(c => c.CartItems)
				.ThenInclude(ci => ci.Product)
				.FirstOrDefaultAsync(c => c.Cus_ID == userId);

			if (userCart == null || userCart.CartItems.Count == 0)
			{
				// Handle the scenario where the user's cart is empty
				return RedirectToAction("Index", "Cart");
			}

			// Create a new order object
			var order = new Order
			{
				Order_date = DateTime.Now,
				Order_Status = "Pending", // Set the order status here as needed
				Total_amount = userCart.Total,
				Shipping_address = $"{checkoutForm.Street}, {checkoutForm.Apartment}, {checkoutForm.Country}", // Set the shipping address based on the data received from the checkout form
				Cus_ID = userId // Set the customer ID
			};

			// Add the order to the database
			_context.Orders.Add(order);
			await _context.SaveChangesAsync();

			// Create order details
			foreach (var cartItem in userCart.CartItems)
			{
				var orderDetail = new OrderDetail
				{
					Quantity = cartItem.Quantity,
					Sub_Total = cartItem.Sub_total,
					Order_ID = order.Order_ID,
					Prod_ID = cartItem.Product.Prod_ID,
					FirstName = checkoutForm.FirstName, // Use data from the checkout form
					SecondName = checkoutForm.SecondName,
					EmailAddress = checkoutForm.EmailAddress,
					PhoneNumber = checkoutForm.PhoneNumber,
					Country = checkoutForm.Country,
					Apartment = checkoutForm.Apartment,
					Street = checkoutForm.Street,
					Company = checkoutForm.Company
				};

				// Add order details to the database
				_context.OrderDetails.Add(orderDetail);
			}

			// Remove cart items
			_context.CartItems.RemoveRange(userCart.CartItems);
			userCart.Total = 0;
			await _context.SaveChangesAsync();

			// Redirect to a thank you page
			return RedirectToAction("thankyou");
		}

		public IActionResult thankyou()
		{
			// يمكنك هنا تحميل أي بيانات إضافية تحتاجها صفحة "thankyou" وتمريرها لها إذا لزم الأمر
			return View(); // عرض صفحة الـ "thankyou"
		}

		public IActionResult Confirmation()
		{
			// You can display a confirmation message or details here
			return View();
		}
	}
}
