// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using testtt.Models;

namespace testtt.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<Customer> _userManager;
        private readonly SignInManager<Customer> _signInManager; 
        private readonly IWebHostEnvironment _Environment;

        public IndexModel(
            UserManager<Customer> userManager,
            SignInManager<Customer> signInManager, IWebHostEnvironment Environment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _Environment = Environment;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
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
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required(ErrorMessage = "First Name is required.")]
            [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "First name must contain only letters.")]
            [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }
            [Required(ErrorMessage = "Last Name is required.")]
            [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "Last name must contain only letters.")]
            [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }
            //[Phone]
            //[Display(Name = "Phone number")]
            //public string PhoneNumber { get; set; }
            //[RegularExpression(@"^(https?|ftp):\/\/[^\s\/$.?#].[^\s]*$", ErrorMessage = "Invalid URL.")]
            [Display(Name = "")]
            public string? Cus_ImageUrl { get; set; }

            [Required(ErrorMessage = "Address is required.")]
            [RegularExpression(@"^(?=.*[a-zA-Z])[a-zA-Z0-9\s.,#-]+$", ErrorMessage = "Invalid address format.")]
            public string Address { get; set; }
        }

        private async Task LoadAsync(Customer user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                FirstName = user.Cus_FName,
                LastName = user.Cus_LName,
                Address=user.Cus_address,
                Cus_ImageUrl = user.Cus_ImageUrl
                //PhoneNumber = phoneNumber
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile img_file)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            //var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            //if (Input.PhoneNumber != phoneNumber)
            //{
            //    var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
            //    if (!setPhoneResult.Succeeded)
            //    {
            //        StatusMessage = "Unexpected error when trying to set phone number.";
            //        return RedirectToPage();
            //    }
            //}
            var firstname = user.Cus_FName;
            var lastname = user.Cus_LName;
            var address = user.Cus_address;
            if (Input.FirstName != firstname)
            {
                user.Cus_FName = Input.FirstName;
                await _userManager.UpdateAsync(user);

            }
            if (Input.LastName != lastname)
            {
                user.Cus_LName = Input.LastName;
                await _userManager.UpdateAsync(user);

            }
            if (Input.Address != address)
            {
                user.Cus_address = Input.Address;
                await _userManager.UpdateAsync(user);

            }


			//        if (Request.Form.Files.Count > 0)
			//        {

			//string path = Path.Combine(_Environment.WebRootPath, "images"); // wwwroot/Img/
			//            if (!Directory.Exists(path))
			//            {
			//                Directory.CreateDirectory(path);
			//            }
			//            if (img_file != null)
			//            {
			//	var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
			//	var fileExtension = Path.GetExtension(img_file.FileName).ToLower();
			//	if (!allowedExtensions.Contains(fileExtension))
			//	{
			//		ModelState.AddModelError("ProfilePicture", "Invalid file type. Only JPG, JPEG, PNG, and GIF files are allowed.");
			//		//return View(model);
			//	}
			//	else
			//	{
			//		// Handle case when no file is uploaded
			//		// For example, return a BadRequest or display an error message
			//		return BadRequest("No file uploaded.");
			//	}

			//	path = Path.Combine(path, img_file.FileName); // for exmple : /Img/Photoname.png
			//                using (var stream = new FileStream(path, FileMode.Create))
			//                {
			//                    await img_file.CopyToAsync(stream);
			//                    user.Cus_ImageUrl = img_file.FileName;
			//                }
			//            }
			//            else
			//            {
			//                user.Cus_ImageUrl = "defult_image.jpg"; // to save the default image path in database.
			//            }

			//                //_context.Add(department);
			//                //_context.SaveChanges();
			//                //return RedirectToAction("Index");
			//                await _userManager.UpdateAsync(user);


			//            //return View(department);
			//        }
			if (Request.Form.Files.Count > 0)
			{
				string path = Path.Combine(_Environment.WebRootPath, "images"); // wwwroot/Img/
				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}

				img_file = Request.Form.Files.FirstOrDefault(); // Get the first uploaded file

				if (img_file != null)
				{
					var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
					var fileExtension = Path.GetExtension(img_file.FileName)?.ToLower();

					if (!allowedExtensions.Contains(fileExtension))
					{
						ModelState.AddModelError("ProfilePicture", "Invalid file type. Only JPG, JPEG, PNG, and GIF files are allowed.");
						return BadRequest(ModelState);
					}

					path = Path.Combine(path, img_file.FileName); // For example: /Img/Photoname.png

					using (var stream = new FileStream(path, FileMode.Create))
					{
						await img_file.CopyToAsync(stream);
						user.Cus_ImageUrl = Path.GetFileName(path); // Save only the file name in the database
					}
				}
				else
				{
					user.Cus_ImageUrl = "default_image.jpg"; // Save the default image path in the database
				}

				await _userManager.UpdateAsync(user);
			}



			await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
