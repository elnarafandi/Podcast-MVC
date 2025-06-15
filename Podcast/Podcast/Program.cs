using Domain.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Podcast.Middlewares;
using Repository;
using Repository.Data;
using Serilog;
using Service;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Load Kestrel limits from appsettings.json
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize =
        builder.Configuration.GetValue<long>("Kestrel:Limits:MaxRequestBodySize");
});

// Configure form options for file uploads (50MB)
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 50 * 1024 * 1024; // 50MB
});

// Add services to the container.
builder.Services.AddControllersWithViews();

var conString = builder.Configuration.GetConnectionString("Default") ??
     throw new InvalidOperationException("Connection string 'Default'" +
    " not found.");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(conString));

builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>()
                                                         .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie (çerez) parametrl?ri
    options.ExpireTimeSpan = TimeSpan.FromDays(3);
    options.Cookie.HttpOnly = true;  
    options.Cookie.SecurePolicy = CookieSecurePolicy.None;  
});

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    options.SignIn.RequireConfirmedEmail = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;

});

Log.Logger = new LoggerConfiguration()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)  // Write logs to file (daily rolling)
    .Enrich.FromLogContext()
    .MinimumLevel.Information()
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddFluentValidationAutoValidation();
var assemblies = AppDomain.CurrentDomain.GetAssemblies();
foreach (var assembly in assemblies)
{
    builder.Services.AddValidatorsFromAssembly(assembly);
}

builder.Services.AddHttpContextAccessor();

builder.Services.AddRepositoryLayer();
builder.Services.AddServiceLayer();

// Register the global exception handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();


StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();

app.UseAuthentication();
app.UseAuthorization();

// Use the global exception handler
app.UseExceptionHandler();

app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
          );

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=About}/{action=Index}/{id?}");

app.Run();
