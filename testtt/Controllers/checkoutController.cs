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


        //[Authorize]
        //[HttpPost]
        //[Authorize]
        //[HttpPost]
        //public IActionResult PlaceOrder()
        //{
        //	var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        //	// Find the user's cart
        //	var userCart = _context.Carts
        //		.Include(c => c.CartItems)
        //			.ThenInclude(ci => ci.Product)
        //		.FirstOrDefault(c => c.Cus_ID == userId);

        //	if (userCart == null || userCart.CartItems.Count == 0)
        //	{
        //		// Handle the scenario where the user's cart is empty
        //		return RedirectToAction("Index", "Cart");
        //	}

        //	// Create an Order object
        //	var order = new Order
        //	{
        //		Order_date = DateTime.Now,
        //		Order_Status = "Pending", // You may change this as needed
        //		Total_amount = userCart.Total,
        //		Shipping_address = "Address from form", // Get the shipping address from the form
        //		Cus_ID = userId // Set the customer ID
        //	};

        //	// Add the order to the context
        //	_context.Orders.Add(order);
        //	_context.SaveChanges();

        //	// Create OrderDetail objects
        //	foreach (var cartItem in userCart.CartItems)
        //	{
        //		var orderDetail = new OrderDetail
        //		{
        //			Quantity = cartItem.Quantity,
        //			/*Unit_price = cartItem.Product.Price,*/ // Assuming Product has a Price property
        //			Sub_Total = cartItem.Sub_total,
        //			Order_ID = order.Order_ID, // Set the order ID
        //			Prod_ID = cartItem.Product.Prod_ID // Set the product ID
        //		};

        //		// Add the order detail to the context
        //		_context.OrderDetails.Add(orderDetail);
        //	}

        //	// Remove cart items
        //	_context.CartItems.RemoveRange(userCart.CartItems);
        //	userCart.Total = 0;
        //	_context.SaveChanges();

        //	// Redirect to a confirmation page or display a success message
        //	return RedirectToAction("Confirmation");
        //}
        [Authorize]
        [HttpPost]
        public IActionResult PlaceOrder(Cart cart, OrderDetail checkoutForm)
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
            // Create a new order object
            var order = new Order
            {
                Order_date = DateTime.Now,
                Order_Status = "Pending", // Set the order status here as needed
                Total_amount = userCart.Total,
                Shipping_address = checkoutForm.Street + ", " + checkoutForm.Apartment + ", " + checkoutForm.Country, // Set the shipping address based on the data received from the checkout form
                Cus_ID = userId // You can use the customer's email as an identifier here, but it's better to use a real customer ID if available
            };

            // Add the order to the database
            _context.Orders.Add(order);
            _context.SaveChanges();

            // Create order details
            foreach (var cartItem in userCart.CartItems)
            {
                var orderDetail = new OrderDetail
                {
                    Quantity = cartItem.Quantity,
                    //Unit_price = cartItem.Product.Price,
                    //Discount_rate = cartItem.Discount,
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
            _context.SaveChanges(); // Save changes
            return RedirectToAction("thankyou");
        }

        public ActionResult thankyou()
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

