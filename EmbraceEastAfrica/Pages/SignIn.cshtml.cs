using EmbraceEastAfrica.Models;
using EmbraceEastAfrica.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmbraceEastAfrica.Pages
{
    /// <summary>
    /// C# code-behind for the Sign In page.
    /// Handles GET (display form) and POST (process login).
    /// </summary>
    public class SignInPageModel : PageModel
    {
        private readonly UserService _userService;
        private readonly ILogger<SignInPageModel> _logger;

        // Bound to the form inputs via [BindProperty]
        [BindProperty]
        public SignInModel Input { get; set; } = new();

        // Messages shown to the user after form submission
        public string? SuccessMessage { get; set; }
        public string? ErrorMessage { get; set; }

        public SignInPageModel(UserService userService, ILogger<SignInPageModel> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        // GET: /SignIn — just render the form
        public void OnGet()
        {
            // Check if redirected here after successful registration
            if (TempData["SignUpSuccess"] is string msg)
                SuccessMessage = msg;
        }

        // POST: /SignIn — process the login attempt
        public IActionResult OnPost()
        {
            // Server-side validation (C# DataAnnotations)
            if (!ModelState.IsValid)
                return Page();

            // Attempt authentication via UserService
            var user = _userService.Authenticate(Input);

            if (user == null)
            {
                // Generic error — don't reveal whether email or password was wrong (security best practice)
                ErrorMessage = "Invalid email or password. Please try again.";
                _logger.LogWarning("Failed login attempt for email: {Email}", Input.Email);
                return Page();
            }

            // Store user info in session
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserName", user.FullName);
            HttpContext.Session.SetInt32("UserId", user.Id);

            _logger.LogInformation("User {Email} signed in successfully.", user.Email);

            // Redirect to homepage after successful login
            TempData["WelcomeMessage"] = $"Welcome back, {user.FullName}!";
            return RedirectToPage("/Index");
        }
    }
}
