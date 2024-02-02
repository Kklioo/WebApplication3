using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication3.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.DataProtection;
using Assignment2.Model;

namespace WebApplication3.Pages
/*{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(ILogger<IndexModel> logger, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            this.signInManager = signInManager;
            _userManager = userManager;
        }

        public void OnGet()
        {
        }


        public async Task<IActionResult> OnGetDecryptCreditCardAsync()
        {
            try
            {
                var userEmail = User.Identity.Name;

                // Retrieve the user after a successful sign-in
                var user = await _userManager.FindByEmailAsync(userEmail);

                if (user != null)
                {
                    var dataProtectionProvider = DataProtectionProvider.Create("EncryptData");
                    var protector = dataProtectionProvider.CreateProtector("MySecretKey");

                    // Decrypt the credit card information
                    var decryptedCreditCard = protector.Unprotect(user.CreditCardNo);

                    ViewData["DecryptedCreditCard"] = decryptedCreditCard;

                    return Page();
                }
                else
                {
                    _logger.LogWarning($"User not found during credit card decryption.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception during credit card decryption: {ex}");
            }

            return RedirectToPage("/Index"); // Handle error by redirecting or displaying an error message
        }


    }
}*/

{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}