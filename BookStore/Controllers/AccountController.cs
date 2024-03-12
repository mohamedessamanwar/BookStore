using BookStore.BusinessLogic.ViewModels.User;
using BookStore.BusnessLogic.ViewsModels.User;
using BookStore.DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;



namespace BookStore.Controllers
{
    public class AccountController : Controller
    {


        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;


        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;

        }




        public IActionResult Regestration()
        {
            return View();
        }



        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Regestration(SignupView account)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser();

                // Mapping
                user.UserName = account.Email;
                user.Email = account.Email;
                user.FirstName = account.Firstname;
                user.LastName = account.Lastname;
                user.PhoneNumber = account.PhoneNumber;
                user.DateOfBirth = account.DateOfBirth;
                user.Address = account.Address;
                user.City = account.City;
                user.State = account.State;


                var result = await _userManager.CreateAsync(user, account.Password);

                if (result.Succeeded)
                {

                    await _userManager.AddToRoleAsync(user, "Admin");
                    var userClaims = await _userManager.GetClaimsAsync(user);
                    var roles = await _userManager.GetRolesAsync(user);
                    // Add claims

                    var roleClaims = new List<Claim>();

                    foreach (var role in roles)
                        roleClaims.Add(new Claim("roles", role));
                    var claims = new List<Claim>
                    {
                     new Claim(ClaimTypes.Name, user.UserName),
                     new Claim(ClaimTypes.NameIdentifier, user.Id),
                     new Claim(ClaimTypes.Email, user.Email)
                    }.Union(userClaims)
                       .Union(roleClaims);

                    await _userManager.AddClaimsAsync(user, claims);

                    // Create cookie and sign in user
                    await _signInManager.SignInAsync(user, false);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Display errors if the registration failed
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    return View(account);
                }
            }

            // If ModelState is not valid
            return View(account);
        }

        ///*************************************************************************************************************************************
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);

                    if (result.Succeeded)
                    {
                        // Create cookie and sign in user
                        await _signInManager.SignInAsync(user, false);

                        return RedirectToAction("Index", "Home");

                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    }

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            // Sign out the user
            await _signInManager.SignOutAsync();

            // Optionally, you can redirect the user to a specific page after logout
            return RedirectToAction("Index", "Home");
        }
    }




}

