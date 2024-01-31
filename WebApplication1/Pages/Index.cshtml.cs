using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Model;
using Microsoft.AspNetCore.Authorization;

[Authorize]
public class IndexModel : PageModel
{

    private readonly ILogger<IndexModel> _logger;
    private readonly UserManager<ApplicationUser> _userManager;

    public IndexModel(ILogger<IndexModel> logger, UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
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
                //encryption
                var dataProtectionProvider = DataProtectionProvider.Create("EncryptData");
                var protector = dataProtectionProvider.CreateProtector("MySecretKey");

                // You might want to decrypt sensitive data here before displaying it
                // For simplicity, assuming the CreditCardNo property is decrypted already
                CurrentUser = user;
                CurrentUser.CreditCardNo = protector.Unprotect(user.CreditCardNo);
            }
        }

        return Page();
    }
}
