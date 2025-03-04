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
using Dream_Bridge.Data;
using Dream_Bridge.Models.Observer;

var builder = WebApplication.CreateBuilder(args);

// Cấu hình Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Đăng ký dịch vụ
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<StudyAbroadDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("localDB"));
    options.EnableSensitiveDataLogging(false);
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Đăng ký các dịch vụ vào DI container
builder.Services.AddScoped<IApiAdapter, ApiAdapter>();
builder.Services.AddScoped<ILoggingService, LoggingService>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IServiceFactory, ServiceFactory>();
builder.Services.AddScoped<Func<string, IStudyAbroadFactory>>(serviceProvider => country =>
{
    return country switch
    {
        "US" => new USStudyAbroadFactory(),
        "UK" => new UKStudyAbroadFactory(),
        _ => throw new Exception("Không hỗ trợ quốc gia này")
    };
});

// Cấu hình Observer: tạo instance, attach observer, và đăng ký vào DI
var notifier = new EventNotifier();
notifier.Attach(new AdminObserver());
notifier.Attach(new UserObserver("Người dùng A"));
builder.Services.AddSingleton(notifier);

// Đăng ký SignalR
builder.Services.AddSignalR();

// Đăng ký EmailService
builder.Services.AddScoped<EmailService>();

// Đăng ký HttpClient và ApiAdapter (chỉ cần đăng ký một lần)
builder.Services.AddHttpClient<IApiAdapter, ApiAdapter>();

// Cấu hình xác thực
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

// Cấu hình phân quyền
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
});

// Cấu hình session
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Cấu hình JsonConvert
JsonConvert.DefaultSettings = () => new JsonSerializerSettings
{
    Formatting = Formatting.Indented,
    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
};

var app = builder.Build();

// Cấu hình Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

// Cấu hình SignalR
app.MapHub<ChatHub>("/chatHub");

// Định tuyến Controller
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

var routes = new Dictionary<string, (string Controller, string Action)>
{
    { "/home", ("Home", "Home") },
    { "/vechungtoi", ("Home", "VeChungToi") },
    { "/duhoc", ("Home", "DuHoc") },
    { "/chat", ("Home", "Chat") },
    { "/dichvu", ("Home", "DichVu") },
    { "/tintuc", ("Home", "TinTuc") },
    { "/notification", ("Home", "Notification") },
    { "/timtruong", ("Home", "TimTruong") },
    { "/uc", ("DuHoc", "Uc") },
    { "/my", ("DuHoc", "My") },
    { "/singapore", ("DuHoc", "Singapore") },
    { "/thuysy", ("DuHoc", "ThuySy") },
    { "/visauc", ("DichVu", "VisaUc") },
    { "/visamy", ("DichVu", "VisaMy") },
    { "/visasingapore", ("DichVu", "VisaSingapore") },
    { "/visathuysy", ("DichVu", "VisaThuySy") },
    { "/chitiet", ("DichVu", "ChiTiet") },
    { "/chitietruong", ("DichVu", "ChiTietTruong") },
    { "/qltruong", ("Admin", "QLTruong") },
    { "/qltintuc", ("Admin", "QLTintuc") },
    { "/qltuvan", ("Admin", "QLTuvan") },
    { "/qltaikhoan", ("Admin", "QLTaikhoan") },
    { "/tongquan", ("Admin", "TQuan") },
    { "/qlchat", ("Admin", "QLChat") },
    { "/qlduhoc", ("Admin", "QLDanhMuc") },
    { "/login", ("Account", "Login") },
    { "/profile", ("Account", "Profile") },
    { "/register", ("Account", "Register") },
    { "/historyorder", ("Account", "HistoryOrder") },
    { "/resetpassword", ("Account", "Resetpassword") },
    { "/updateprofile", ("Account", "UpdateProfile") },
    { "/pageacc", ("Account", "PageAcc") },
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
