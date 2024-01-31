using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using WebApplication1.Model;
using WebApplication1.ViewModels;


namespace WebApplication1.Pages
{
    public class ForgotPasswordModel : PageModel
    {
        [BindProperty]
        public ForgotPassword ForgetModel { get; set; }

        private readonly UserManager<ApplicationUser> userManager;
        public ForgotPasswordModel(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        public void OnGet()
        {
        }
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(ForgetModel.Email);
                // If the user is found AND Email is confirmed
                if (user != null)
                {
                    // Generate the reset password token
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);

                    // Build the password reset link
                    var passwordResetLink = Url.Action("ResetPassword", "Account",
                            new { email = ForgetModel.Email, token = token }, Request.Scheme);
                }

            }
            ModelState.AddModelError("", "Email does not exist.");
            return Page();
        }
    }
}
