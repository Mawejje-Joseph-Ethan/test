using EmbraceEastAfrica.Models;
using EmbraceEastAfrica.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmbraceEastAfrica.Pages
{
    /// <summary>
    /// C# code-behind for the Sign Up page.
    /// Handles GET (display form) and POST (process registration).
    /// </summary>
    public class SignUpPageModel : PageModel
    {
        private readonly UserService _userService;
        private readonly ILogger<SignUpPageModel> _logger;

        [BindProperty]
        public SignUpModel Input { get; set; } = new();

        public string? ErrorMessage { get; set; }

        public SignUpPageModel(UserService userService, ILogger<SignUpPageModel> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        // GET: /SignUp — render the registration form
        public void OnGet() { }

        // POST: /SignUp — process the registration
        public IActionResult OnPost()
        {
            // Server-side validation via C# DataAnnotations
            if (!ModelState.IsValid)
                return Page();

            // Attempt to register via UserService
            string? error = _userService.Register(Input);

            if (error != null)
            {
                // Registration failed (e.g., duplicate email/username)
                ErrorMessage = error;
                _logger.LogWarning("Registration failed for {Email}: {Error}", Input.Email, error);
                return Page();
            }

            _logger.LogInformation("New user registered: {Email}", Input.Email);

            // Pass success message to Sign In page via TempData
            TempData["SignUpSuccess"] = $"Account created successfully! Welcome, {Input.FullName}. Please sign in.";
            return RedirectToPage("/SignIn");
        }
    }
}
