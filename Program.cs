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
using Dream_Bridge.Model.Main;

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

builder.Services.AddScoped<IChatPermissionService, ChatPermissionService>();

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



// Thêm endpoint cho SignalR
app.MapHub<Dream_Bridge.Hubs.ChatHub>("/chatHub");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "Home",
    pattern: "/home",
    defaults: new { controller = "Home", action = "Home" }
);
app.MapControllerRoute(
    name: "VeChungToi",
    pattern: "/vechungtoi",
    defaults: new { controller = "Home", action = "VeChungToi" }
);
app.MapControllerRoute(
    name: "DuHoc",
    pattern: "/duhoc",
    defaults: new { controller = "Home", action = "DuHoc" }
);
app.MapControllerRoute(
    name: "Chat",
    pattern: "/chat",
    defaults: new { controller = "Home", action = "Chat" }
);
app.MapControllerRoute(
    name: "DichVu",
    pattern: "/dichvu",
    defaults: new { controller = "Home", action = "DichVu" }
);
app.MapControllerRoute(
    name: "ChatBot",
    pattern: "/chatbot",
    defaults: new { controller = "Home", action = "ChatBot" }
);
app.MapControllerRoute(
    name: "TinTuc",
    pattern: "/tintuc",
    defaults: new { controller = "Home", action = "TinTuc" }
);
app.MapControllerRoute(
    name: "Notification",
    pattern: "/notification",
    defaults: new { controller = "Home", action = "Notification" }
);
app.MapControllerRoute(
    name: "TimTruong",
    pattern: "/timtruong",
    defaults: new { controller = "Home", action = "TimTruong" }
);
app.MapControllerRoute(
    name: "Uc",
    pattern: "/uc",
    defaults: new { controller = "DuHoc", action = "Uc" }
);
app.MapControllerRoute(
    name: "My",
    pattern: "/my",
    defaults: new { controller = "DuHoc", action = "My" }
);
app.MapControllerRoute(
    name: "Singapore",
    pattern: "/singapore",
    defaults: new { controller = "DuHoc", action = "Singapore" }
);
app.MapControllerRoute(
    name: "ThuySy",
    pattern: "/thuysy",
    defaults: new { controller = "DuHoc", action = "ThuySy" });

// dich vu
app.MapControllerRoute(
    name: "VisaUc",
    pattern: "/visauc",
    defaults: new { controller = "DichVu", action = "VisaUc" }
);
app.MapControllerRoute(
    name: "VisaMy",
    pattern: "/visamy",
    defaults: new { controller = "DichVu", action = "VisaMy" }
);
app.MapControllerRoute(
    name: "VisaSingapore",
    pattern: "/visasingapore",
    defaults: new { controller = "DichVu", action = "VisaSingapore" }
);
app.MapControllerRoute(
    name: "VisaThuySy",
    pattern: "/viasathusy",
    defaults: new { controller = "DichVu", action = "VisaThuySy" }
);
app.MapControllerRoute(
    name: "ChiTiet",
    pattern: "/chitiet",
    defaults: new { controller = "DichVu", action = "ChiTiet" }
);
app.MapControllerRoute(
    name: "ChiTietTruong",
    pattern: "/chitietruong",
    defaults: new { controller = "DichVu", action = "ChiTietTruong" }
);
app.MapControllerRoute(
   name: "QLTruong",
   pattern: "/qltruong",
   defaults: new { controller = "Admin", action = "QLTruong" }
);
app.MapControllerRoute(
    name: "QLTintuc",
    pattern: "/qltintuc",
    defaults: new { controller = "Admin", action = "QLTintuc" }
);
app.MapControllerRoute(
    name: "QLTuvan",
    pattern: "/qltuvan",
    defaults: new { controller = "Admin", action = "QLTuvan" }
);
app.MapControllerRoute(
    name: "QLTaikhoan",
    pattern: "/qltaikhoan",
    defaults: new { controller = "Admin", action = "QLTaikhoan" }
);
app.MapControllerRoute(
    name: "TQuan",
    pattern: "/tongquan",
    defaults: new { controller = "Admin", action = "TQuan" }
);
app.MapControllerRoute(
    name: "QLTaikhoan",
    pattern: "/qltaikhoan",
    defaults: new { controller = "Admin", action = "QLTaikhoan" }
);
app.MapControllerRoute(
    name: "QLTruyCap",
    pattern: "/qltruycap",
    defaults: new { controller = "Admin", action = "QuanLyTruyCap" }
);
app.MapControllerRoute(
    name: "QLChat",
    pattern: "/qlchat",
    defaults: new { controller = "Admin", action = "QLChat" }
);
app.MapControllerRoute(
    name: "QLDanhMuc",
    pattern: "/qlduhoc",
    defaults: new { controller = "Admin", action = "QLDanhMuc" }
);
app.MapControllerRoute(
    name: "Login",
    pattern: "/login",
    defaults: new { controller = "Account", action = "Login" }
);
app.MapControllerRoute(
    name: "Profile",
    pattern: "/profile",
    defaults: new { controller = "Account", action = "Profile" }
);
app.MapControllerRoute(
    name: "Register",
    pattern: "/register",
    defaults: new { controller = "Account", action = "Register" }
);
app.MapControllerRoute(
    name: "HistoryOrder",
    pattern: "/historyorder",
    defaults: new { controller = "Account", action = "HistoryOrder" }
);
app.MapControllerRoute(
    name: "Resetpassword",
    pattern: "/resetpassword",
    defaults: new { controller = "Account", action = "Resetpassword" }
);
app.MapControllerRoute(
    name: "UpdateProfile",
    pattern: "/updateprofile",
    defaults: new { controller = "Account", action = "UpdateProfile" }
);
app.MapControllerRoute(
    name: "PageAcc",
    pattern: "/pageacc",
    defaults: new { controller = "Account", action = "PageAcc" }
);

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
