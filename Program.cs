using Microsoft.EntityFrameworkCore;
using Dream_Bridge.Models.Main;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Dream_Bridge.Hubs;
using Dream_Bridge.Model.Main;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();


var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<Program>>();

// Kiểm tra Singleton trước khi chạy ứng dụng
var logger1 = LoggerSingleton.Instance;
var logger2 = LoggerSingleton.Instance;
var logger3 = LoggerSingleton.Instance;

logger1.Log("Log từ instance 1");
logger2.Log("Log từ instance 2");

// Kiểm tra 2 instance có giống nhau không
logger.LogInformation($"🧐 Cùng instance? {ReferenceEquals(logger1, logger2)}");


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<StudyAbroadDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("localDB"));
    options.EnableSensitiveDataLogging(false);
});
// // Add singelton
// builder.Services.AddSingleton<LoggerSingleton>();

// Cấu hình SignalR
builder.Services.AddSignalR();

// Thay đổi từ AddSingleton sang AddScoped cho EmailService
builder.Services.AddScoped<EmailService>();

// Cấu hình xác thực
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Thiết lập thời gian hết hạn
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.SlidingExpiration = true; // Bật thời gian hết hạn trượt
    });

// Cấu hình phân quyền
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});

// Cấu hình session
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true; // Cần thiết cho tuân thủ GDPR
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
});
JsonConvert.DefaultSettings = () => new JsonSerializerSettings
{
    Formatting = Formatting.Indented,
    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
};

builder.Services.AddScoped<IChatPermissionService, ChatPermissionService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseRouting();

// Thêm xác thực và phân quyền
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();




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



app.Run();





