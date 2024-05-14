using AlkoStoreServer.Data;
using AlkoStoreServer.Middleware;
using AlkoStoreServer.Repositories;
using AlkoStoreServer.Repositories.Interfaces;
using Firebase.Auth;
using Firebase.Auth.Providers;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile(
        "Config\\firebase.json"
        ),
    ProjectId = "testproj-6693e"
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.Authority = "https://securetoken.google.com/testproj-6693e";
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = "https://securetoken.google.com/testproj-6693e",
                ValidateAudience = true,
                ValidAudience = "testproj-6693e",
                ValidateLifetime = true
            };
        });

builder.Services.AddAuthorization();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped(typeof(IDbRepository<>), typeof(DbRepository<>));
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddSingleton(new FirebaseAuthClient(new FirebaseAuthConfig
{
    ApiKey = "AIzaSyCbTg4ZTgKQ20oZXGu5nhPJDfQYv71JwSg",
    AuthDomain = "testproj-6693e.firebaseapp.com",
    Providers = new FirebaseAuthProvider[]
    {
        new EmailProvider(),
        new GoogleProvider()
    }
}));

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

app.UseWhen(context => context.Request.Path.StartsWithSegments("/api"), builder => 
{ 
    builder.UseFirebaseJwtMiddleware();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "Test",
    pattern: "/test",
    defaults: new { controller = "Home", action = "Test" });

/*app.MapControllerRoute(
    name: "Api_Get_Products",
    pattern: "/api/get/products",
    defaults: new { controller = "Api/ProductController", action = "GetProducts" });*/

app.Run();
