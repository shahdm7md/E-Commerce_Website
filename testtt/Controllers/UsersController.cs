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

        public async Task<IActionResult> ManageRoles(string userId)
        {
            var user=await _userManager.FindByIdAsync(userId);

            if(user==null)
            {
                return NotFound();
            }
            
            var roles=await _roleManager.Roles.ToListAsync();

            var ViewModel = new UserRolesViewModel
            {
                UserId=user.Id,
                UserFName=user.Cus_FName,
                UserLName=user.Cus_LName,
                Roles=roles.Select(role => new RoleViewModel
                {
                    RoleName=role.Name,
                    IsSelected=_userManager.IsInRoleAsync(user, role.Name).Result

                }).ToList()  //not async because he doesnot go to database to get data he select DB in roles
            };

            return View(ViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageRoles(UserRolesViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user == null)
                return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user); //select all roles to each user

            foreach (var role in model.Roles)  //loop for roles
            {
                if (userRoles.Any(r => r == role.RoleName) && !role.IsSelected)   //if role is assigned and not selected
                    await _userManager.RemoveFromRoleAsync(user, role.RoleName);  //remove role

                if (!userRoles.Any(r => r == role.RoleName) && role.IsSelected)   //if role is not assigned and selected
                    await _userManager.AddToRoleAsync(user, role.RoleName);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
