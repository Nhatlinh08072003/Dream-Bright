using Microsoft.EntityFrameworkCore;
using Dream_Bridge.Models.Main;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Dream_Bridge.Hubs;
using Dream_Bridge.Services.Factories;
using Dream_Bridge.Services.Adapters;
using Dream_Bridge.Services.Logging;
using Dream_Bridge.Services.Repositories;
using Dream_Bridge.Services.Notifications;
using Dream_Bridge.Model.Main;
using DreamBright.Services;
using DreamBright.Services.Factory;
using DreamBright.Services.US;
using DreamBright.Services.UK;
using Dream_Bridge.Models.Observer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();
builder.Services.AddSingleton<LoggerSingleton>();
builder.Services.AddSingleton<NotificationService>();

// Add ChatPermissionService
builder.Services.AddScoped<IChatPermissionService, ChatPermissionService>();

// Add HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Add event notification services
builder.Services.AddSingleton<EventNotifier>();

// Database configuration
builder.Services.AddDbContext<StudyAbroadDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.EnableSensitiveDataLogging(false);
});

// Register services
builder.Services.AddScoped<IApiAdapter, ApiAdapter>();
builder.Services.AddScoped<ILoggingService, LoggingService>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IServiceFactory, ServiceFactory>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddHttpClient<IApiAdapter, ApiAdapter>();

// Register study abroad services
builder.Services.AddScoped<USStudyAbroadFactory>();
builder.Services.AddScoped<UKStudyAbroadFactory>();
builder.Services.AddScoped<IStudyAbroadFactorySelector, StudyAbroadFactorySelector>();
builder.Services.AddScoped<IScholarshipService, USScholarshipService>();
builder.Services.AddScoped<IScholarshipService, UKScholarshipService>();
builder.Services.AddScoped<IVisaService, USVisaService>();
builder.Services.AddScoped<IVisaService, UKVisaService>();
builder.Services.AddScoped<ISchoolSelectionService, USSchoolSelectionService>();
builder.Services.AddScoped<ISchoolSelectionService, UKSchoolSelectionService>();

// Authentication & Authorization
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
});

// Session configuration
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

// Configure SignalR hubs
app.MapHub<ChatHub>("/chatHub");
app.MapHub<LogHub>("/logHub");
app.MapHub<Dream_Bridge.Hubs.NotificationHub>("/notificationHub");

// Configure routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

var routes = new Dictionary<string, (string Controller, string Action)>
{
    { "/home", ("Home", "Home") },
    { "/vechungtoi", ("Home", "VeChungToi") },
    { "/duhoc", ("Home", "DuHoc") },
    { "/chat", ("Home", "Chat") },
    { "/dichvu", ("Home", "DichVu") },
    { "/chatbot", ("Home", "ChatBot") },
    { "/tintuc", ("Home", "TinTuc") },
    { "/notification", ("Home", "Notification") },
    { "/timtruong", ("Home", "TimTruong") },
    
    // Du học routes
    { "/uc", ("DuHoc", "Uc") },
    { "/my", ("DuHoc", "My") },
    { "/singapore", ("DuHoc", "Singapore") },
    { "/thuysy", ("DuHoc", "ThuySy") },
    
    // Dịch vụ routes
    { "/visauc", ("DichVu", "VisaUc") },
    { "/visamy", ("DichVu", "VisaMy") },
    { "/visasingapore", ("DichVu", "VisaSingapore") },
    { "/visathuysy", ("DichVu", "VisaThuySy") },
    { "/chitiet", ("DichVu", "ChiTiet") },
    { "/chitietruong", ("DichVu", "ChiTietTruong") },
    
    // Admin routes
    { "/qltruong", ("Admin", "QLTruong") },
    { "/qltintuc", ("Admin", "QLTintuc") },
    { "/qltuvan", ("Admin", "QLTuvan") },
    { "/qltaikhoan", ("Admin", "QLTaikhoan") },
    { "/tongquan", ("Admin", "TQuan") },
    { "/qlchat", ("Admin", "QLChat") },
    { "/qlduhoc", ("Admin", "QLDanhMuc") },
    { "/qltruycap", ("Admin", "QuanLyTruyCap") },
    
    // Account routes
    { "/login", ("Account", "Login") },
    { "/profile", ("Account", "Profile") },
    { "/register", ("Account", "Register") },
    { "/historyorder", ("Account", "HistoryOrder") },
    { "/resetpassword", ("Account", "Resetpassword") },
    { "/updateprofile", ("Account", "UpdateProfile") },
    { "/pageacc", ("Account", "PageAcc") },
    
    // Log and Report routes
    { "/log", ("Log", "Index") },
    { "/report", ("Report", "Index") },

    // Consulting route
    { "/consulting", ("Consulting", "GetServices") }
};

foreach (var route in routes)
{
    app.MapControllerRoute(
        name: route.Key,
        pattern: route.Key,
        defaults: new { controller = route.Value.Controller, action = route.Value.Action }
    );
}

app.Run();
