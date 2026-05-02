using EmbraceEastAfrica.Models;

namespace EmbraceEastAfrica.Services
{
    /// <summary>
    /// Handles user registration and authentication.
    /// Uses an in-memory store for demonstration.
    /// In production, replace with a real database (e.g., Entity Framework + SQL Server).
    /// </summary>
    public class UserService
    {
        // In-memory user store — simulates a database table
        private static readonly List<User> _users = new();
        private static int _nextId = 1;

        /// <summary>
        /// Registers a new user. Returns null on success, or an error message string.
        /// </summary>
        public string? Register(SignUpModel model)
        {
            // Check for duplicate email
            if (_users.Any(u => u.Email.Equals(model.Email, StringComparison.OrdinalIgnoreCase)))
                return "An account with this email already exists.";

            // Check for duplicate username
            if (_users.Any(u => u.Username.Equals(model.Username, StringComparison.OrdinalIgnoreCase)))
                return "This username is already taken.";

            // Hash the password using BCrypt-style simple hash
            // (In production use BCrypt.Net or ASP.NET Identity)
            string passwordHash = HashPassword(model.Password);

            var user = new User
            {
                Id = _nextId++,
                FullName = model.FullName.Trim(),
                Username = model.Username.Trim(),
                Email = model.Email.Trim().ToLower(),
                PasswordHash = passwordHash,
                CreatedAt = DateTime.UtcNow
            };

            _users.Add(user);
            return null; // null = success
        }

        /// <summary>
        /// Authenticates a user. Returns the User on success, null on failure.
        /// </summary>
        public User? Authenticate(SignInModel model)
        {
            var user = _users.FirstOrDefault(u =>
                u.Email.Equals(model.Email.Trim(), StringComparison.OrdinalIgnoreCase));

            if (user == null) return null;

            // Verify password hash
            if (!VerifyPassword(model.Password, user.PasswordHash))
                return null;

            return user;
        }

        /// <summary>
        /// Simple SHA-256 password hashing.
        /// In production, use BCrypt.Net-Next or ASP.NET Core Identity.
        /// </summary>
        private static string HashPassword(string password)
        {
            using var sha = System.Security.Cryptography.SHA256.Create();
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(password + "EmbraceEA_Salt_2025");
            byte[] hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        private static bool VerifyPassword(string password, string storedHash)
        {
            return HashPassword(password) == storedHash;
        }

        /// <summary>
        /// Returns all registered users (for admin/demo purposes).
        /// </summary>
        public IReadOnlyList<User> GetAllUsers() => _users.AsReadOnly();
    }
}
