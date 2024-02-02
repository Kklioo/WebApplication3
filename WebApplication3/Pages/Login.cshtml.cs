using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using WebApplication3.ViewModels;
using System.Text.Json;
using IdentityUser = Microsoft.AspNetCore.Identity.IdentityUser;
using System.Net;

namespace WebApplication3.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public Login LModel { get; set; }

        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly ILogger<LoginModel> logger;
        private readonly IHttpContextAccessor contxt;

        public LoginModel(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, ILogger<LoginModel> logger, IHttpContextAccessor contxt)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.logger = logger;
            this.contxt = contxt;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                if (ValidateCaptcha())
                {
                    Console.WriteLine("captcha successful");
                    var identityResult = await signInManager.PasswordSignInAsync(LModel.Email, LModel.Password,
                        LModel.RememberMe, false);

                    if (identityResult.Succeeded)
                    {
                        // Retrieve the user based on the provided email
                        var user = await userManager.FindByEmailAsync(LModel.Email);

                        if (user != null)
                        {

                            contxt.HttpContext.Session.SetString("Email", user.Email);

                            // Check if PhoneNumber is not null before setting it in the session
                            if (user.PhoneNumber != null)
                            {
                                contxt.HttpContext.Session.SetString("PhoneNumber", user.PhoneNumber);
                            }


                            contxt.HttpContext.Session.SetString("IsAuthenticated", "true");
                        }
                        else
                        {
                            logger.LogWarning($"User not found during secured session creation.");
                        }

                        return RedirectToPage("Index");
                    }

                    ModelState.AddModelError("", "Username or Password incorrect");

                }
            }

            return Page();
        }

        public bool ValidateCaptcha()
        {
            bool result = false;

            //When user submits the recaptcha form, the user gets a response POST parameter. 
            //captchaResponse consist of the user click pattern. Behaviour analytics! AI :) 
            string captchaResponse = Request.Form["g-recaptcha-response"];

            //To send a GET request to Google along with the response and Secret key.
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://www.google.com/recaptcha/api/siteverify?secret=6Lc0BmApAAAAAIBNE7ndvN7Oz-jBzCLlpM1cHiNi&response=" + captchaResponse);


            try
            {

                //Codes to receive the Response in JSON format from Google Server
                using (WebResponse wResponse = req.GetResponse())
                {
                    using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
                    {
                        //The response in JSON format
                        string jsonResponse = readStream.ReadToEnd();

                        //Create jsonObject to handle the response e.g success or Error
                        //Deserialize Json
                        var jsonObject = JsonSerializer.Deserialize<JsonDocument>(jsonResponse);

                        //Convert the string "False" to bool false or "True" to bool true
                        result = jsonObject.RootElement.GetProperty("success").GetBoolean();

                    }
                }

                return result;
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }
    }
}
