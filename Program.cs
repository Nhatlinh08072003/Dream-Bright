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
using Dream_Bridge.Models.Composite;
using Dream_Bridge.Models.Flyweight;
using Dream_Bright.Services.Logging;
using Dream_Bright.Services.PaymentGateways;

var builder = WebApplication.CreateBuilder(args);

// Adapter
// Đăng ký các service thanh toán
builder.Services.AddSingleton<ITransactionLogger, ConsoleLogger>();
builder.Services.AddTransient<PayPalService>();
builder.Services.AddTransient<VNPayService>();

// Bridge
var onlineBasic = new OnlineConsulting(new BasicService());
var directPremium = new DirectConsulting(new PremiumService());

onlineBasic.Consult();
directPremium.Consult();

// Composite
var visaConsulting = new SingleService("Tư vấn Visa");
var scholarshipSupport = new SingleService("Hỗ trợ Học bổng");

var studyAbroadServices = new ServiceGroup("Dịch vụ Du học");
studyAbroadServices.AddService(visaConsulting);
studyAbroadServices.AddService(scholarshipSupport);

studyAbroadServices.ShowInfo();

// Flyweight
var harvard1 = UniversityFactory.GetUniversity("Harvard", "USA");
var harvard2 = UniversityFactory.GetUniversity("Harvard", "USA");

Console.WriteLine(ReferenceEquals(harvard1, harvard2)); // True
///////

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


// Cấu hình JsonConvert
JsonConvert.DefaultSettings = () => new JsonSerializerSettings
{
    Formatting = Formatting.Indented,
    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
};

// Register the original service as a concrete type
builder.Services.AddScoped<DichvuService>();

// Manually register the decorator
builder.Services.AddScoped<IDichvuService>(provider =>
{
    var originalService = provider.GetRequiredService<DichvuService>();
    var logger = provider.GetRequiredService<ILogger<LoggingDecorator>>();
    return new LoggingDecorator(originalService, logger);
});

builder.Services.AddScoped<IChatPermissionService, ChatPermissionService>();


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


// Add a route for the DemoBuilderPattern action
app.MapControllerRoute(
    name: "demoBuilderPattern",
    pattern: "Home/DemoBuilderPattern",
    defaults: new { controller = "Home", action = "DemoBuilderPattern" });

//Adapter
app.MapControllerRoute(
    name: "Adapter",
    pattern: "/Adapter/Index",
    defaults: new { controller = "Adapter", action = "Index" }
);
//Bridge
app.MapControllerRoute(
    name: "Bridge",
    pattern: "/Bridge/Index",
    defaults: new { controller = "Bridge", action = "Index" }
);
//Composite
app.MapControllerRoute(
    name: "Composite",
    pattern: "/Composite/Index",
    defaults: new { controller = "Composite", action = "Index" }
);
//Flyweight
app.MapControllerRoute(
    name: "Flyweight",
    pattern: "/Flyweight/Index",
    defaults: new { controller = "Flyweight", action = "Index" }
);
//////////
app.MapControllerRoute(
    name: "Notification",
    pattern: "/Notification/TestNotification",
    defaults: new { controller = "Notification", action = "TestNotification" }
);
app.MapControllerRoute(
    name: "Home",
    pattern: "/home",
    defaults: new { controller = "Home", action = "Home" }
);
app.MapControllerRoute(
    name: "Log",
    pattern: "/log",
    defaults: new { controller = "Log", action = "Index" }
);
app.MapControllerRoute(
    name: "Report",
    pattern: "/report",
    defaults: new { controller = "Report", action = "Index" }
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

// Thêm route cho Consulting
app.MapControllerRoute(
    name: "consulting",
    pattern: "Consulting/{action=GetServices}/{country?}",
    defaults: new { controller = "Consulting" }
);

app.MapControllerRoute(
    name: "demoDecoratorPattern",
    pattern: "DichVu/DemoDecoratorPattern",
    defaults: new { controller = "DichVu", action = "DemoDecoratorPattern" }
);


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
