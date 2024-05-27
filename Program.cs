using Microsoft.EntityFrameworkCore;
using Kluby.Data;
using Kluby.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// register db
builder.Services.AddDbContext<KlubyDBContext>(options => options.UseSqlite("Data Source=KlubyDBContext.db"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(20); // Set session timeout value
    options.Cookie.HttpOnly = true; // Set cookie as essential
    options.Cookie.IsEssential = true;
});

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

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope()) {
    // create new user if database is empty
    using(KlubyDBContext ?db = scope.ServiceProvider.GetService<KlubyDBContext>()) {
        if (db is null) {
            throw new Exception("UserDBContext is null");
        }

        if (db.Users.Count() == 0) {
            String password = "admin";
            String salt = Kluby.Controllers.CryptoCalculator.GenerateRandomString(128);

            db.Users.Add(new UserModel {
                Username = "admin",
                PasswordHash = Kluby.Controllers.CryptoCalculator.CreateSHA256WithSalt(password, salt),
                PasswordSalt = salt,
                IsAdmin = true,
                IsApproved = true
            });

            db.SaveChanges();
        }
    }
}

app.Run();
