using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using testtt.Data;
using testtt.Models;
using testtt.Models.ViewsModels;

namespace testtt.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        //private readonly IToastNotification _toastNotification;
        private new List<string> _allowedExtenstions = new List<string> { ".jpg", ".jpeg", ".png", ".gif" };
        private long _maxAllowedPosterSize = 1048576;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.ToListAsync();
            return View(products);
        }

        public async Task<IActionResult> Add_Product()
        {
            var viewModel = new ProductViewModel
            {
                
            };

            return View(); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add_Product(ProductViewModel model)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View();
            //}

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

            return RedirectToAction(nameof(Index));
        }

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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductViewModel model)
        {

            var product = await _context.Products.FindAsync(model.Id);

            if (product == null)
                return NotFound();

            var files = Request.Form.Files;

            if (!files.Any())
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

           // _toastNotification.AddSuccessToastMessage("Movie updated successfully");
            return RedirectToAction(nameof(Index));
        }
        }
}
