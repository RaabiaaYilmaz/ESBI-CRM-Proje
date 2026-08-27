using CRMProject.Data.Context;
using CRMProject.Data.Entities;
using CRMProject.Services.Implementations;
using CRMProject.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// MVC servislerini ekle
builder.Services.AddControllersWithViews();

// Veritabaný baðlantýsý
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Identity — AppUser ve rol yönetimi
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Service katmaný baðýmlýlýklarý
builder.Services.AddScoped<ICariService, CariService>();
builder.Services.AddScoped<IMalzemeService, MalzemeService>();
builder.Services.AddScoped<IKullaniciService, KullaniciService>();

// Giriþ yapmamýþ kullanýcýyý /Account/Login sayfasýna yönlendir
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Authorization'dan ÖNCE olmalý — sýra kritik
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");
// Uygulama baþlarken rolleri ve admin hesabýný oluþtur
using (var scope = app.Services.CreateScope())
{
    await CRMProject.Data.Seed.DbInitializer.SeedAsync(scope.ServiceProvider);
}
app.Run();