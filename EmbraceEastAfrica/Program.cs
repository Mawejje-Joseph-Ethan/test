using EmbraceEastAfrica.Services;

var builder = WebApplication.CreateBuilder(args);

// ── Register services ──────────────────────────────────────────────────────
builder.Services.AddRazorPages();

// Register UserService as a Singleton so the in-memory user list persists
// across requests during the app's lifetime.
builder.Services.AddSingleton<UserService>();

// Add session support (used to keep the user logged in across pages)
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;   // Prevent JavaScript access to session cookie
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
});

// ── Build the app ──────────────────────────────────────────────────────────
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// Serve static files (HTML pages, CSS, images)
app.UseStaticFiles();

app.UseRouting();

// Enable session middleware
app.UseSession();

app.UseAuthorization();

app.MapRazorPages();

// Also serve the existing static HTML pages (country pages, gallery, etc.)
app.MapFallbackToFile("index.html");

app.Run();
