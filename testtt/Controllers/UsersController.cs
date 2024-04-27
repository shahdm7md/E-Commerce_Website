using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using testtt.Models;
using testtt.Models.ViewsModels;

namespace testtt.Controllers
{
    [Authorize(Roles = "Admin")]

    public class UsersController : Controller
    {
        private readonly UserManager<Customer> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(UserManager<Customer> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }


        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.Select(user => new UserViewModel
            {
                Id = user.Id,
                FirstName = user.Cus_FName,
                LastName = user.Cus_LName,
               // UserName = user.UserName,
                Email = user.Email,
                Roles = _userManager.GetRolesAsync(user).Result
               // Roles = user.Roles.Select(r => r.Name).ToList()
            }).ToListAsync();

            return View(users);
        }
    }
}
