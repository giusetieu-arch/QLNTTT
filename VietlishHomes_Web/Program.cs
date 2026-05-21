using Microsoft.EntityFrameworkCore;
using VietlishHomes_Web.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Thêm các d?ch v? (Services) vào Container (B?t bu?c n?m TR??C builder.Build())
builder.Services.AddControllersWithViews();

// C?u hình k?t n?i c? s? d? li?u SQL Server thông qua DbContext c?a b?n
builder.Services.AddDbContext<QlntDoVanTieuContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// 2. C?u hình HTTP request pipeline (Middleware)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // Giá tr? HSTS m?c ??nh là 30 ngày. B?n có th? thay ??i khi tri?n khai th?c t?.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// 3. C?U HÌNH ???NG D?N M?C ??NH (ROUTE)
// ?ã chuy?n ??i t? Home/Index sang CuDan/Login ?? m? Web lên là hi?n Form ??ng Nh?p luôn
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=CuDan}/{action=Login}/{id?}");

app.Run();