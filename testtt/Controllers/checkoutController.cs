using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
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

			var userCart = _context.Carts
				.Include(c => c.CartItems)
				.ThenInclude(ci => ci.Product)
				.FirstOrDefault(c => c.Cus_ID == userId);

			if (userCart == null || userCart.CartItems.Count == 0)
			{
				return RedirectToAction("Index", "Cart");
			}

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
		public async Task<IActionResult> PlaceOrder([FromForm] OrderDetail checkoutForm, [FromForm] string PaymentMethod, [FromForm] string couponCode)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			var userCart = await _context.Carts
				.Include(c => c.CartItems)
				.ThenInclude(ci => ci.Product)
				.FirstOrDefaultAsync(c => c.Cus_ID == userId);

			if (userCart == null || !userCart.CartItems.Any())
			{
				return RedirectToAction("Index", "Cart");
			}

			// Apply coupon code
			var coupon = await _context.Coupons.FirstOrDefaultAsync(c => c.Code == couponCode && c.IsActive && c.ExpiryDate > DateTime.Now);
			if (coupon != null)
			{
				userCart.Total -= CalculateDiscount(userCart.Total, coupon);
			}

			// Create a new order object
			var order = new Order
			{
				Order_date = DateTime.Now,
				Order_Status = "Pending",
				Total_amount = userCart.Total,
				Shipping_address = $"{checkoutForm.Street}, {checkoutForm.Apartment}, {checkoutForm.Country}",
				Cus_ID = userId
			};

			_context.Orders.Add(order);
			await _context.SaveChangesAsync();


			foreach (var cartItem in userCart.CartItems)
			{
				var orderDetail = new OrderDetail
				{
					Quantity = cartItem.Quantity,
					Sub_Total = cartItem.Sub_total,
					Unit_price = cartItem.Unit_price,
					Order_ID = order.Order_ID,
					Prod_ID = cartItem.Product.Prod_ID,
					FirstName = checkoutForm.FirstName,
					SecondName = checkoutForm.SecondName,
					EmailAddress = checkoutForm.EmailAddress,
					PhoneNumber = checkoutForm.PhoneNumber,
					Country = checkoutForm.Country,
					Apartment = checkoutForm.Apartment,
					Street = checkoutForm.Street,
					Company = checkoutForm.Company
				};

				_context.OrderDetails.Add(orderDetail);

				var product = await _context.Products.FindAsync(cartItem.Product.Prod_ID);
				if (product != null)
				{
					product.Prod_Stock -= cartItem.Quantity;
					_context.Products.Update(product);
				}
			}

			// Remove cart items
			_context.CartItems.RemoveRange(userCart.CartItems);
			_context.Carts.Remove(userCart);
			await _context.SaveChangesAsync();

			var payment = new Payment
			{
				Method = PaymentMethod,
				Amount = order.Total_amount,
				Pay_Date = DateTime.Now,
				Order_ID = order.Order_ID
			};

			_context.Payments.Add(payment);
			await _context.SaveChangesAsync();

			

			return RedirectToAction("thankyou");
		}

		public async Task<IActionResult> ApplyCoupon([FromForm] string couponCode)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var userCart = await _context.Carts
				.Include(c => c.CartItems)
				.FirstOrDefaultAsync(c => c.Cus_ID == userId);

			var coupon = await _context.Coupons.FirstOrDefaultAsync(c => c.Code == couponCode);

			if (coupon != null)
			{
				var discountAmount = CalculateDiscount(userCart.Total, coupon);
				var updatedTotal = userCart.Total - discountAmount;

				return Json(new { total = updatedTotal });
			}
			else
			{
				return BadRequest("Invalid coupon code.");
			}
		}

		private decimal CalculateDiscount(decimal total, Coupon coupon)
		{
			if (coupon.IsActive && coupon.ExpiryDate > DateTime.Now)
			{
				if (coupon.IsPercentage)
				{
					decimal discount = (coupon.DiscountAmount / 100) * total;
					return Math.Min(discount, total);
				}
				else
				{
					return Math.Min(coupon.DiscountAmount, total);
				}
			}
			else
			{
				return 0;
			}
		}

		public IActionResult thankyou()
		{
			return View();
		}

		public IActionResult Confirmation()
		{
			return View();
		}
	}
}
