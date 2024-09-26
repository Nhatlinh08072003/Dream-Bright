
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllers();
builder.Services.AddSingleton<EmailService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();
app.UseAuthorization();

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
    name: "LienHe",
    pattern: "/lienhe",
    defaults: new { controller = "Home", action = "LienHe" }
);
app.MapControllerRoute(
    name: "DichVu",
    pattern: "/dichvu",
    defaults: new { controller = "Home", action = "DichVu" }
);
app.MapControllerRoute(
    name: "TimTruong",
    pattern: "/timtruong",
    defaults: new { controller = "Home", action = "TimTruong" }
);
app.MapControllerRoute(
    name: "Uc",
    pattern: "/uc",
    defaults: new { controller = "Home", action = "Uc" }
);
app.MapControllerRoute(
    name: "My",
    pattern: "/my",
    defaults: new { controller = "Home", action = "My" }
);
app.MapControllerRoute(
    name: "Singapore",
    pattern: "/singapore",
    defaults: new { controller = "Home", action = "Singapore" }
); app.MapControllerRoute(
    name: "ThuySy",
    pattern: "/thuysy",
    defaults: new { controller = "Home", action = "ThuySy" }
);
app.Run();
