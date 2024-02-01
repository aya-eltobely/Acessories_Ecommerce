using AccessoriesWebsite.Models;
using AccessoriesWebsite.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Policy;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace AccessoriesWebsite.Controllers
{
    public class AccountController : Controller
    {

        private UserManager<ApplicationUser> userManager;
        private SignInManager<ApplicationUser> signInManager;

        public AccountController(UserManager<ApplicationUser> _userManger, SignInManager<ApplicationUser> _signInManager)
        {
            userManager = _userManger;
            signInManager = _signInManager;
        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserViewModel user)
        {
            if (ModelState.IsValid == true)
            {
                ApplicationUser newuser = new ApplicationUser();
                newuser.UserName = user.UserName;
                newuser.Email = user.Email;
                newuser.PhoneNumber = user.PhoneNumber;

                IdentityResult identityresult = await userManager.CreateAsync(newuser, user.Password);
                if (identityresult.Succeeded)
                {
                    //create user cookie 
                    await signInManager.SignInAsync(newuser, false);
                    await userManager.AddToRoleAsync(newuser, "User");
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var item in identityresult.Errors)
                    {
                        ModelState.AddModelError("", item.Description);

                    }
                }
            }
            return View("Register");
        }


        ////////////////////////////////log in//////////////////////////////////////////
        public IActionResult LogIn(string ReturnUrl = "~/Home/Index")
        {
            ModelState.Remove("ReturnUrl");
            ViewData["url"] = ReturnUrl;
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> LogIn(LogInViewModel loginUser, string ReturnedUrl = "~/Home/Index")
        {

            if (ModelState.IsValid == true)
            {

                //find user in db , and in user table where username equal user in login view 
                //return username from database which equal to loginuser 
                ApplicationUser user = await userManager.FindByNameAsync(loginUser.UserName);
                if (user != null)
                {
                    //check for loginuser.password == user.password
                    SignInResult res = await signInManager.PasswordSignInAsync(user, loginUser.Password, true, false);
                    if (res.Succeeded)
                    {

                        //return RedirectToAction("Index", "Student");
                        var roles = await userManager.GetRolesAsync(user);
                        if (roles.Contains("Admin"))
                        {
                            var url = Url.Action( "Index", "Home", new { area = "Admin" });
                            return Redirect(url);
                            //go to admin dashboard
                        }
                        return LocalRedirect(ReturnedUrl);
                    }
                    else
                    {
                        //ex ==> user enter wrong password
                        ModelState.AddModelError("", "Incorrect username or Password");
                    }
                }
                else
                {
                    //username is not in database
                    ModelState.AddModelError("", "Invalid User");
                }

            }
            ViewData["url"] = ReturnedUrl;

            return View("LogIn");
        }


        /////////////////////////////////log out////////////////////////////////////////

        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("LogIn", "Account");
        }


        ///////////////////////////////// Forget Password ////////////////////////////////////////

        public async Task<IActionResult> ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    //TempData["user"] = user;
                    // go to reset password view
                    //var token = await userManager.GeneratePasswordResetTokenAsync(user);
                    //var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = token }, protocol: HttpContext.Request.Scheme);

                    // Send the email with the link
                    // Use your email sending service or IdentityMessage service

                    //return RedirectToAction("ForgotPasswordConfirmation");

                    var token = userManager.GeneratePasswordResetTokenAsync(user);

                    return RedirectToAction("ResetPassword", new { userId = user.Id,code = token.Result } );//,user);

                }
                else
                {
                    //username is not in database
                    ModelState.AddModelError("", "Invalid Email");
                }
                // Don't reveal that the user does not exist or is not confirmed
                //return RedirectToAction("ForgotPasswordConfirmation");
            }

            // If we got this far, something failed; redisplay the form
            return View();
        }


        ///////////////////////////////// Reset Password ////////////////////////////////////////


        public async Task<IActionResult> ResetPassword(string userId, string code)
        {
            //ViewBag.UserId = userId;
            //return View();
            //
            ResetPasswordViewModel model = new ResetPasswordViewModel();
            model.Code = code;
            model.UserId = userId;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByIdAsync(model.UserId);
                if (user != null)
                {
                    var result = await userManager.ResetPasswordAsync(user, model.Code, model.NewPassword);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("ResetPasswordConfirmation");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    return View(model);
                }
                else
                {
                    //username is not in database
                    ModelState.AddModelError("", "Invalid Email");
                }
            }
            
            return View();
        }


        public async Task<IActionResult> ResetPasswordConfirmation()
        {
            return View();
        }
    }
}
