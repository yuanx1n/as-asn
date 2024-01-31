using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

[Authorize]
public class IndexModel : PageModel
{
	public bool ChangePasswordPrompt { get; set; }
	private readonly ILogger<IndexModel> _logger;
    private readonly IOptions<SessionOptions> sessionOptions;
    private readonly UserManager<ApplicationUser> _userManager;

    public IndexModel(ILogger<IndexModel> logger,IOptions<SessionOptions> sessionOptions ,UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        this.sessionOptions = sessionOptions;
        _userManager = userManager;
    }

    public ApplicationUser CurrentUser { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        // Check if User property is not null
        if (User != null)
        {
            // Retrieve the current user
            var user = await _userManager.GetUserAsync(User);


            if (user != null)
            {
                // Store user information in the session
                HttpContext.Session.SetString("UserId", user.Id);
                HttpContext.Session.SetString("FirstName", user.FirstName);
                HttpContext.Session.SetString("LastName", user.LastName);

                //encryption
                var dataProtectionProvider = DataProtectionProvider.Create("EncryptData");
                var protector = dataProtectionProvider.CreateProtector("MySecretKey");

                // You might want to decrypt sensitive data here before displaying it
                // For simplicity, assuming the CreditCardNo property is decrypted already
                CurrentUser = user;
                CurrentUser.CreditCardNo = protector.Unprotect(user.CreditCardNo);
				var maxPasswordAge = TimeSpan.FromMinutes(2); // Set your desired maximum password age

				if (user.LastPasswordChange != null)
				{
					var timeSinceLastChange = DateTime.UtcNow - user.LastPasswordChange;

					if (timeSinceLastChange > maxPasswordAge)
					{
						ChangePasswordPrompt = true;
					}
				}

			}
		}

        return Page();
    }
}
