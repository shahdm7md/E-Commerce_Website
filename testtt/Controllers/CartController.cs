using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using testtt.Data;

namespace testtt.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var carts = await _context.Carts.ToListAsync();
            return View(carts);
        }
    }
}
