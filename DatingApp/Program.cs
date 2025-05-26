using DatingApp.Components;
using DatingApp.Data;
using DatingApp.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add environment-based appsettings.json files
builder.Configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add ApplicationDbContext with SQL Server and Identity (int PKs)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options => { })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Add cookie authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/login";
        options.ExpireTimeSpan = TimeSpan.FromHours(5);
    });

// Register AuthService
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddHttpContextAccessor();

// Add Blazor authentication state provider
builder.Services.AddScoped<AuthenticationStateProvider, DatingApp.Services.ServerAuthenticationStateProvider>();
builder.Services.AddAuthorizationCore();

var baseAddress = builder.Configuration["AppBaseUrl"] ?? "https://localhost:7026/";
builder.Services.AddHttpClient("Default", client =>
{
    client.BaseAddress = new Uri(baseAddress);
});
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Default"));

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopCenter;
    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 10000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});



var app = builder.Build();






// Seed users and roles
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    UserSeeder.SeedUsersAndRolesAsync(services).GetAwaiter().GetResult();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Minimal API endpoint for login
app.MapPost("/api/login", async (HttpContext http, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, [FromBody] LoginRequest login) =>
{
    var user = await userManager.FindByNameAsync(login.Username);
    if (user == null)
        return Results.Unauthorized();
    var result = await signInManager.CheckPasswordSignInAsync(user, login.Password, false);
    if (!result.Succeeded)
        return Results.Unauthorized();
    await signInManager.SignInAsync(user, isPersistent: true);
    return Results.Ok();
})
.WithName("Login");

// Minimal API endpoint for logout
app.MapPost("/api/logout", async (SignInManager<ApplicationUser> signInManager) =>
{
    await signInManager.SignOutAsync();
    return Results.Ok();
});

app.Run();

public class LoginRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}
