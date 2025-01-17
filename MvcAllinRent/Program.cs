using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.FileProviders;
using MvcAllinRent.Interfaces;
using MvcAllinRent.Models;
using MvcAllinRent.Repositories;
using MvcAllinRent.Services;
using MvcAllinRent.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.Configure<EmailConfigs>(builder.Configuration.GetSection("EmailConfigs"));
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddScoped<ItemTypeRepository>();
builder.Services.AddScoped<ItemRepository>();
builder.Services.AddScoped<RentalRepository>();
builder.Services.AddScoped<AuthUserRepository>();
builder.Services.AddScoped<AuthConfirmService>();

// Add authentication cookie
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
        {
            options.Cookie.Name = "AuthCookie";
            options.LoginPath = "/Auth/Login";
            options.LogoutPath = "/Auth/Logout";
            options.ExpireTimeSpan = TimeSpan.FromDays(2);
        });

builder.Services.AddHttpContextAccessor();

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
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "..", "SharedImages")),
    RequestPath = "/shared-images"
});

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
