using Amazon.AspNetCore.Identity.Cognito;
using Amazon.Extensions.CognitoAuthentication;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using UdemyCourse.Models.Accounts;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using System.Net;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;

namespace UdemyCourse.Controllers
{
    public class AccountsController : Controller
    {
        private readonly SignInManager<CognitoUser> _signInManager;
        private readonly UserManager<CognitoUser> _userManager;
        private readonly CognitoUserPool _userPool;
        private readonly IAmazonCognitoIdentityProvider _identityProvider;
        private readonly IConfiguration _config;

        public AccountsController(
            SignInManager<CognitoUser> signInManager,
            UserManager<CognitoUser> userManager,
            CognitoUserPool userPool,
            IAmazonCognitoIdentityProvider identityProvider,
            IConfiguration config
            )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userPool = userPool;
            _identityProvider = identityProvider;
            _config = config;
        }

        [HttpGet]
        // If signature contains SignUpModel, 
        // it will show errors to start with
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [ActionName("SignUp")]
        public async Task<IActionResult> SignUp_Post(SignupModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // You should move this to a service
                    var user = _userPool.GetUser(model.Email);
                    if (user.Status != null)
                    {
                        ModelState.AddModelError("UserFound", "User with this email already exists");
                        return View(model);
                    }

                    // Required attribute that was setup in AWS Cognito under Attributes
                    user.Attributes.Add(CognitoAttribute.Name.AttributeName, model.Email);
                    user.Attributes.Add(CognitoAttribute.PhoneNumber.AttributeName, model.PhoneNumber);
                    var created = await _userManager.CreateAsync(user, model.Password);
                    if (created.Succeeded)
                    {
                        return RedirectToAction("Confirm", "Accounts"); // Or RedirectToPage
                    }
                    else
                    {
                        foreach (var error in created.Errors)
                        {
                            ModelState.AddModelError(error.Code, error.Description);
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError("SignUpFailed", ex.Message);
                }
            }
            return View(model); // error case
        }

        [HttpGet]
        public IActionResult Confirm()
        {
            var model = new ConfirmModel();
            return View(model);
        }

        [HttpPost]
        [ActionName("Confirm")]
        public async Task<IActionResult> Confirm_Post(ConfirmModel model)
        {
            // We setup AWS Cognito to sign in by email
            // In our case Username and Email are the same
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    if (user == null)
                    {
                        ModelState.AddModelError("NotFound", "A user with the given email was not found");
                    }
                    else
                    {
                        var confirmed = await (_userManager as CognitoUserManager<CognitoUser>)
                                              .ConfirmSignUpAsync(
                                                    user,
                                                    model.Code,
                                                    forcedAliasCreation: true
                                               );
                        if (confirmed.Succeeded)
                        {
                            return RedirectToAction("Index", "Home");
                        }

                        foreach (var item in confirmed.Errors)
                        {
                            ModelState.AddModelError(item.Code, item.Description);
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError("ConfirmFailed", ex.Message);
                }
            }

            return View(model); // error case
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            var model = new SignInModel();
            return View(model);
        }

        [HttpPost]
        [ActionName("SignIn")]
        public async Task<IActionResult> SignIn_Post(SignInModel model)
        {
            if (ModelState.IsValid)
            {
                var signedIn = await _signInManager.PasswordSignInAsync(
                    model.Email,
                    model.Password,
                    isPersistent: model.RememberMe,
                    lockoutOnFailure: false
                    );
                if (signedIn.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("SignInFailed", "Email and/or password do not exist");
                }
            }

            return View("SignIn", model);
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            var model = new ForgotPasswordModel();
            return View(model); // error case
        }

        [HttpPost]
        [ActionName("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword_Post(ForgotPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // You should move this to a service
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    if (user.Status == null)
                    {
                        ModelState.AddModelError("UserNotFound", "User with this email does not exist");
                    }
                    else
                    {
                        // Send confirmation code. 
                        await user.ForgotPasswordAsync();
                        return RedirectToAction("ResetPassword", "Accounts");
                    }
                }
                catch(System.Exception ex)
                {
                    ModelState.AddModelError("ForgotPasswordFailed", ex.Message);
                }
            }

            return View(model); // error case
        }

        [HttpGet]
        public IActionResult ResetPassword()
        {
            var model = new ResetPasswordModel();
            return View(model);
        }

        [HttpPost]
        [ActionName("ResetPassword")]
        public async Task<IActionResult> ResetPassword_Post(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // You should move this to a service
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    if (user.Status == null)
                    {
                        ModelState.AddModelError("UserNotFound", "User with this email does not exist");
                    }
                    else
                    {
                        // Reset password
                        await user.ConfirmForgotPasswordAsync(model.Code, model.Password);

                        return RedirectToAction("SignIn", "Accounts");
                    }
                }
                catch(System.Exception ex)
                {
                    ModelState.AddModelError("ResetPasswordFailed", ex.Message);
                }
            }

            return View(model); // error case
        }
    }
}