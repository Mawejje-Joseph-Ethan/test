namespace EmbraceEastAfrica.Models
{
    /// <summary>
    /// Represents a registered user in the system.
    /// </summary>
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // In a real app, store a hashed password — never plain text.
        // Here we store a BCrypt hash (handled by UserService).
        public string PasswordHash { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
