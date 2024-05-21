using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using testtt.Data;
using testtt.Models;
using testtt.Models.ViewsModels;

namespace testtt.Controllers
{
    
    public class ProductController : Controller
    {

        private readonly ApplicationDbContext _context;

        private readonly IToastNotification _toastNotification;
        private new List<string> _allowedExtenstions = new List<string> { ".jpg", ".jpeg", ".png", ".gif" };
        private long _maxAllowedPosterSize = 1048576;
        private readonly UserManager<Customer> _userManager;

        public ProductController(ApplicationDbContext context, IToastNotification toastNotification, UserManager<Customer> userManager)
        {
            _context = context;
            _toastNotification = toastNotification;
            _userManager = userManager;
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.ToListAsync();
            return View(products);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add_Product()
        {
            var viewModel = new ProductViewModel
            {

            };

            return View(viewModel); 
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add_Product(ProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var files = Request.Form.Files;

            if (!files.Any())
            {
                ModelState.AddModelError("ProdImage", "Please select Product image!");
                return View("Add_Product", model);
            }

            var prodimage = files.FirstOrDefault();

            if (!_allowedExtenstions.Contains(Path.GetExtension(prodimage.FileName).ToLower()))
            {
                ModelState.AddModelError("ProdImage", "Only .PNG, .JPG, .JPEG, GIF images are allowed!");
                return View("Add_Product", model);
            }

            if (prodimage.Length > _maxAllowedPosterSize)
            {
                ModelState.AddModelError("ProdImage", "Product Image cannot be more than 1 MB!");
                return View("Add_Product", model);
            }

            using var dataStream = new MemoryStream();

            await prodimage.CopyToAsync(dataStream);

            var products = new Product
            {
                Prod_Name = model.Prod_Name,
                Prod_Price = model.Prod_Price,
                Prod_Description = model.Prod_Description,
                Prod_Stock = model.Prod_Stock,
                Prod_Image=dataStream.ToArray()
            };

            _context.Products.Add(products);
            _context.SaveChanges();

            _toastNotification.AddSuccessToastMessage("Product Added Successfully");

            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return BadRequest();

            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return NotFound();

            var viewModel = new ProductViewModel
            {
                Id=product.Prod_ID,
                Prod_Name=product.Prod_Name,
                Prod_Price=product.Prod_Price,
                Prod_Description=product.Prod_Description,
                Prod_Stock=product.Prod_Stock,
                Prod_Image=product.Prod_Image
            };

            return View("Add_Product", viewModel);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Add_Product", model);
            }

            var product = await _context.Products.FindAsync(model.Id);

            if (product == null)
                return NotFound();

            var files = Request.Form.Files;

            if (files.Any())
            {
                var prodimage = files.FirstOrDefault();

                using var dataStream = new MemoryStream();
                 
                await prodimage.CopyToAsync(dataStream);

                model.Prod_Image = dataStream.ToArray();

                if (!_allowedExtenstions.Contains(Path.GetExtension(prodimage.FileName).ToLower()))
                {
                    ModelState.AddModelError("ProdImage", "Only .PNG, .JPG, .JPEG, GIF images are allowed!");
                    return View("Add_Product", model);
                }

                if (prodimage.Length > _maxAllowedPosterSize)
                {
                    ModelState.AddModelError("ProdImage", "Product Image cannot be more than 1 MB!");
                    return View("Add_Product", model);
                }
                product.Prod_Image = model.Prod_Image;
            }

            product.Prod_ID=model.Id;
            product.Prod_Name=model.Prod_Name;
            product.Prod_Price=model.Prod_Price;
            product.Prod_Description=model.Prod_Description;
            product.Prod_Stock=model.Prod_Stock;
            _context.SaveChanges();

            _toastNotification.AddSuccessToastMessage("Product Updated Successfully");
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return BadRequest();

            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return NotFound();

            _context.Products.Remove(product);
            _context.SaveChanges();

            return Ok();
        }

        [HttpGet]
        public async Task <IActionResult> UserProducts()
        {
            var products = await _context.Products.ToListAsync(); // استرجاع كافة المنتجات من قاعدة البيانات
            return View("UserProducts", products); // إرسال قائمة المنتجات إلى الصفحة لتتم عرضها هناك
        }


		[Authorize]
		[HttpPost]
        public IActionResult AddToCart(int productId, int quantity)
        {
            if (productId == null || productId <= 0)
                return Json(new { success = false, message = "Invalid product ID" });

            // Get the currently logged-in user
            var user = _userManager.GetUserAsync(User).Result;

			var product = _context.Products.FirstOrDefault(p => p.Prod_ID == productId);
            if (product == null)
            {
                return Json(new { success = false, message = "Product not found." });
            }
            if (product.Prod_Stock <= 0 || product.Prod_Stock < quantity)
            {
                return Json(new { success = false, message = "Product is out of stock." });
            }
            //product.Prod_Stock --;
            //product.Prod_Stock--;

            // Find or create a cart for the user
            var cart = _context.Carts.FirstOrDefault(c => c.Cus_ID == user.Id);
            if (cart == null)
            {
                cart = new Cart { Cus_ID = user.Id };
                _context.Carts.Add(cart);
                _context.SaveChanges();
            }

            var existingCartItem = _context.CartItems.FirstOrDefault(ci => ci.Cart_ID == cart.Cart_ID && ci.Prod_ID == productId);

            if (existingCartItem != null)
            {
                // If the product is already in the cart, increase the quantity
                existingCartItem.Quantity++;
                existingCartItem.Sub_total = existingCartItem.Quantity * existingCartItem.Unit_price;
                product.Prod_Stock--;
                
            }
            else
            {
                  // var prodmodel = new ProductViewModel();
                // If the product is not in the cart, create a new cart item
                //var product1 = _context.Products.Find(productId);
                var newCartItem = new CartItem
                {
                    Cart_ID = cart.Cart_ID,
                    Prod_ID = productId,
                    Quantity = 1,
                    Unit_price = product.Prod_Price,
                    Sub_total = product.Prod_Price
                };
                _context.CartItems.Add(newCartItem);
				product.Prod_Stock--;
			}

            //cart.Total = _context.CartItems.Where(ci => ci.Cart_ID == cart.Cart_ID).Sum(ci => ci.Sub_total);


            _context.SaveChanges();

            _toastNotification.AddSuccessToastMessage("Product Added to cart Successfully");
            return Json(new { success = true, stock = product.Prod_Stock });
        }


    }

}
