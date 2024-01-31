using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.Model;

namespace WebApplication1.Pages
{
    public class LogoutModel : PageModel
    {
		private readonly UserManager<ApplicationUser> userManager;
		private readonly SignInManager<ApplicationUser> signInManager;
		private readonly AuditLogService auditLogService;

		public LogoutModel(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager, AuditLogService auditLogService)
		{
			this.userManager = userManager;
			this.signInManager = signInManager;
			this.auditLogService = auditLogService;
		}
		public void OnGet()
        {
        }
		public async Task<IActionResult> OnPostLogoutAsync()
		{
			// Retrieve the current user
			var user = await userManager.GetUserAsync(User);

			await signInManager.SignOutAsync();
            // Clear session 
            HttpContext.Session.Clear();

            //log logout
            if (user != null)
			{
				auditLogService.LogLogout(user.Id);
			}
			return RedirectToPage("Login");
		}
		public async Task<IActionResult> OnPostDontLogoutAsync()
		{
			return RedirectToPage("Index");
		}
	}
}
