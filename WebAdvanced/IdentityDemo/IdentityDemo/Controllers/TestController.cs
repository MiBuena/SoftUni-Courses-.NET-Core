using IdentityDemo.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityDemo.Controllers
{
    //[Authorize]
    public class TestController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public TestController(
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<ActionResult> CreateUser()
        {
            //var identityUser = new ApplicationUser()
            //{
            //    Email = "aa@a.b14",
            //    UserName = "UsernameM14",
            //    EmailConfirmed = true,
            //    PhoneNumber = "+359988567898"
            //};

            //var result = await this._userManager.CreateAsync(identityUser, "1234567");


            //if (!result.Succeeded)
            //{
            //    return this.BadRequest();
            //}

            //await this._userManager.AddToRoleAsync(identityUser, "Admin");



            await this._signInManager.PasswordSignInAsync("UsernameM", "1234567", false, false);

            return this.Ok();
        }

        public async Task<ActionResult> CreateRole()
        {
            await this._roleManager.CreateAsync(new IdentityRole() { Name = "Admin" });

            var user = _userManager.GetUserAsync(this.User);



            await this._userManager.AddToRoleAsync(user.Result, "Admin");


            return this.Ok();

        }

        [Authorize(Roles = "Admin")]
        public IActionResult ShowForAdmins()
        {
            return this.Ok("Welcome, admins");
        }

        public IActionResult UserChecks()
        {
            //var isAuthenticated = this.User.Identity.IsAuthenticated;

            //var name = this.User.Identity.Name;

            //var dbUser = this._userManager.GetUserId(this.User);

            //var currentUser = _userManager.GetUserAsync(this.User);

            //var roles = _userManager.GetRolesAsync(currentUser.Result);

            //await this._userManager.AddToRoleAsync(currentUser.Result, "Admin");


            if (this.User.IsInRole("Admin"))
            {

            }


            return null;
        }
    }
}
