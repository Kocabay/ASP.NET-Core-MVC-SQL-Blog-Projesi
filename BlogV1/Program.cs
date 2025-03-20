using BlogV1.Context;
using BlogV1.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


//DbContext Baglantýsý
builder.Services.AddDbContext<BlogDbContext>();

//Identity Db Context Baglantýsý
builder.Services.AddDbContext<BlogIdentityDbContext>(options =>
{
    var configuration = builder.Configuration;
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);

});

//ADMÝN GÝRÝSÝ YAPMADIGINDA KULLANICILARIN OTOMATÝK BÝR SAYFA YÖNLENDÝRMESÝ

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
       options.LoginPath = "/Account/Login";
        //options.AccessDeniedPath = "/Account/AccessDenied";
    });

builder.Services.AddIdentity<BlogIdentityUser, BlogIdentityRole>()
    .AddEntityFrameworkStores<BlogIdentityDbContext>()
    .AddDefaultTokenProviders();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Blogs}/{action=Index}/{id?}");

app.Run();
