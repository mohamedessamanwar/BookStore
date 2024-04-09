using BookStore.BusinessLogic.ViewModels.User;
using BookStore.BusnessLogic.Services.Email;
using BookStore.BusnessLogic.ViewsModels.User;
using BookStore.DataAccessLayer.Contexts;
using BookStore.DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;



namespace BookStore.Controllers
{
    public class AccountController : Controller
    {


        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMailingService emailSevice;
        private readonly ApplicationDbContext context;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IMailingService emailSevice, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this.emailSevice = emailSevice;
            this.context = context;
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




        public IActionResult ForgetPassward()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgetPassward(ForgetPassward forgetPassward)
        {
            if (!ModelState.IsValid)
            {
                return View(forgetPassward);
            }

            var user = await _userManager.FindByEmailAsync(forgetPassward.Email);
            if (user == null)
            {
                ModelState.AddModelError("Email", "Email Does Not Exist");
                return View(forgetPassward);
            }
            var token = GenerateUniqueToken();
            context.ResetPasswordSecurty.Add(new ResetPasswordSecurty
            {
                Email = forgetPassward.Email,
                Token = token
            });
            context.SaveChanges();
            // Generate password reset link with email in the URL
            var callbackUrl = Url.Action("ResetPassword", "Account", new { Token = token }, protocol: HttpContext.Request.Scheme);
            // Send email with the password reset link
            await emailSevice.SendEmailAsync(user.Email, "Reset Password", $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");
            // Redirect to a confirmation page or show a message to the user
            return View("ForgotPasswordConfirmation");
        }

        string GenerateUniqueToken()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] tokenData = new byte[32]; // Adjust the length as needed
                rng.GetBytes(tokenData);
                return Convert.ToBase64String(tokenData);
            }
        }


        public IActionResult ResetPassword(string token)
        {
            // Store the encoded code in session
            HttpContext.Session.SetString("Token", token);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(PasswordReset passwordReset)
        {
            if (!ModelState.IsValid)
            {
                return View(passwordReset);
            }
            // Retrieve the encoded code from the session
            string token = HttpContext.Session.GetString("Token");

            if (string.IsNullOrEmpty(token))
            {
                // Handle the case when the code is not found in the session
                ModelState.AddModelError(string.Empty, "Try Again");
                return View(passwordReset);
            }
            var sec = context.ResetPasswordSecurty.FirstOrDefault(s => s.Token == token);
            if (sec == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid Token");
                return View(passwordReset);

            }
            var user = await _userManager.FindByEmailAsync(sec.Email);
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, code, passwordReset.Password);
            if (result.Succeeded)
            {
                context.ResetPasswordSecurty.Remove(sec);
                context.SaveChanges();
                // Password reset successful
                return RedirectToAction("Login", "Account");
            }
            else
            {
                // Add model errors for password reset failure reasons
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(passwordReset);
            }
        }
    }
}

