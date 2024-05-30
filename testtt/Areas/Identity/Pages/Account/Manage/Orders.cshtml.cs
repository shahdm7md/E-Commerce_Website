using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using testtt.Data;
using testtt.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace testtt.Areas.Identity.Pages.Account.Manage
{
    [Authorize]
    public class OrdersModel : PageModel
    {
        private readonly UserManager<Customer> _userManager;
        private readonly SignInManager<Customer> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _Environment;

        public OrdersModel(ApplicationDbContext context, UserManager<Customer> userManager,
            SignInManager<Customer> signInManager, IWebHostEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _Environment = environment;
        }
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
       
        //public void OnGet()
        //{
        //}

        public IList<Order> Orders { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Get the current user's ID
            var userId = _userManager.GetUserId(User);

            // Filter orders by the current user's ID
            Orders = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderDetails)
                .Where(o => o.Customer.Id == userId)
                .ToListAsync();

            return Page();
        }
    }
}
