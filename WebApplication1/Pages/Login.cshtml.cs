using AspNetCore.ReCaptcha;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.Model;
using WebApplication1.ViewModels;

namespace WebApplication1.Pages
{

	public class LoginModel : PageModel
	{
		[BindProperty]
		public Login LModel { get; set; }

		private readonly UserManager<ApplicationUser> userManager;
		private readonly SignInManager<ApplicationUser> signInManager;
		private readonly AuditLogService auditLogService;

		public LoginModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, AuditLogService auditLogService)
		{
			this.signInManager = signInManager;
			this.userManager = userManager;
			this.auditLogService = auditLogService;
		}

		public void OnGet()
		{
		}

		//prevent injection attack
		[ValidateAntiForgeryToken]

		public async Task<IActionResult> OnPostAsync()
		{
			if (ModelState.IsValid)
			{
				if (await TryLoginAsync())
				{
					return RedirectToPage("Index");
				}
			}

			// If execution reaches here, there are model state errors
			return Page();
		}

		[ValidateReCaptcha]
		private async Task<bool> TryLoginAsync()
		{
			//search if login info is correct
			var result = await signInManager.PasswordSignInAsync(LModel.Email, LModel.Password, LModel.RememberMe, false);
			//find user with matching email
			var user = await userManager.FindByEmailAsync(LModel.Email);
			//if correct
			if (result.Succeeded)
			{

				// Log successful login
				auditLogService.LogSuccessfulLogin(user.Id);


                return true;
			}
			//if locked out
			if (result.IsLockedOut)
			{
				auditLogService.LogFailedLogin(user.Id, "Account locked out");
				ModelState.AddModelError("", "Account locked out. Please try again in 5 minutes.");
				return false;
			}

			//if user exists
			if (user != null)
			{
				//increase no. of times user failed login
				await userManager.AccessFailedAsync(user);

				//find remaining times user have
				var remainingAttempts = await userManager.GetAccessFailedCountAsync(user);

				var maxAttempts = 3;  // Change maxAttempts to 3

				//if exceed 3 attempt
				if (remainingAttempts >= maxAttempts)
				{
					//lockout user and print error
					await userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddMinutes(5));
					ModelState.AddModelError("", $"Account locked out after too many unsuccessful attempts. Please try again in 5 minutes. Remaining attempts: {maxAttempts - remainingAttempts}");
					auditLogService.LogFailedLogin(user.Id, "Account locked out after too many unsuccessful attempts. Please try again in 5 minutes. Remaining attempts: {maxAttempts - remainingAttempts}");
				}
				else
				{
					//print error and countdown attempts
					ModelState.AddModelError("", $"Invalid login attempt. Remaining attempts: {maxAttempts - remainingAttempts}");
					auditLogService.LogFailedLogin(user.Id, "Invalid login attempt. Remaining attempts: {maxAttempts - remainingAttempts}");
				}
			}
            ModelState.AddModelError("", "Email or password is incorrect.");

            return false;
		}
	}
}
