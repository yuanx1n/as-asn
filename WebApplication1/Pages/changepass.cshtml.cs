using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using WebApplication1.Model;
using WebApplication1.ViewModels;

namespace WebApplication1.Pages
{
	public class changepassModel : PageModel
	{
		private readonly UserManager<ApplicationUser> userManager;
		private readonly SignInManager<ApplicationUser> signInManager;

		public changepassModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
		{
			this.userManager = userManager;
			this.signInManager = signInManager;
		}

		[BindProperty]
		public Changepass ChangePasswordModel { get; set; }

		public void OnGet()
		{
		}

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);

                if (user == null)
                {
                    return NotFound("User not found.");
                }

                // Check if the current password is correct
                var passwordCorrect = await userManager.CheckPasswordAsync(user, ChangePasswordModel.currPass);

                if (!passwordCorrect)
                {
                    ModelState.AddModelError(nameof(ChangePasswordModel.currPass), "Current password is incorrect.");
                    return Page();
                }

                // Check if the new password is different from the current password
                if (ChangePasswordModel.currPass == ChangePasswordModel.newPass)
                {
                    ModelState.AddModelError(nameof(ChangePasswordModel.newPass), "New password must be different from the current password.");
                    return Page();
                }
				// Check if the password change meets the minimum and maximum age requirements
				var minPasswordAge = TimeSpan.FromMinutes(1); // Set your desired minimum password age

				if (user.LastPasswordChange != null)
				{
					var timeSinceLastChange = DateTime.UtcNow - user.LastPasswordChange;

					if (timeSinceLastChange < minPasswordAge)
					{
						ModelState.AddModelError("", $"You cannot change your password within {minPasswordAge.TotalMinutes} minutes of the last change.");
						return Page();
					}

					
				}

				// Change password
				var changePasswordResult = await userManager.ChangePasswordAsync(user, ChangePasswordModel.currPass, ChangePasswordModel.newPass);

				if (changePasswordResult.Succeeded)
				{
					// Update LastPasswordChange to the current date and time
					user.LastPasswordChange = DateTime.UtcNow;

					// Save the updated user information
					var updateResult = await userManager.UpdateAsync(user);

					if (updateResult.Succeeded)
					{
						// Password change successful, sign in with the new password
						await signInManager.SignInAsync(user, isPersistent: false);

						return RedirectToPage("/Index"); // Redirect to the home page or any other page after a successful password change
					}
					else
					{
						// Update failed, add errors to ModelState
						foreach (var error in updateResult.Errors)
						{
							ModelState.AddModelError("", error.Description);
						}
					}
				}
				else
				{
					// Password change failed, add errors to ModelState
					foreach (var error in changePasswordResult.Errors)
					{
						ModelState.AddModelError("", error.Description);
					}
				}

			}

			// If execution reaches here, there are model state errors
			return Page();
        }

    }
}
