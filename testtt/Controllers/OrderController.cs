using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using testtt.Data;

namespace testtt.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
	{
		private readonly ApplicationDbContext _context;

		public OrderController(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task <IActionResult> Index()
		{
			var orders = await _context.Orders
				.Include(o => o.Customer)
				.Include(o => o.OrderDetails)
				.ToListAsync();

			return View(orders);
		}

        public async Task<IActionResult> ChangeStatus(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(o => o.Order_ID == orderId);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStatus(int orderId, string status)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Order_ID == orderId);

            if (order == null)
            {
                return NotFound();
            }

            order.Order_Status = status;
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
