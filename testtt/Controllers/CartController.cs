using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;
using testtt.Data;

namespace testtt.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext context;

        public CartController(ApplicationDbContext context)
        {
            this.context = context;
        }
        
        public  IActionResult Index()
        {
            dynamic viewModel= new ExpandoObject();
            var products =  context.Products.ToList();
            viewModel.Products = products;

			var carts = context.Carts.ToList();
			viewModel.Carts = carts;

			var cartitems = context.CartItems.Include(c=>c.Product).Include(c=>c.Cart);
            viewModel.CartItems = cartitems;
			return View(viewModel);
        }
       

    }
}
