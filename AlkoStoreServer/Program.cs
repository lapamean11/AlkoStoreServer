using AlkoStoreServer.Auth;
using AlkoStoreServer.Data;
using AlkoStoreServer.Middleware;
using AlkoStoreServer.Repositories;
using AlkoStoreServer.Repositories.Interfaces;
using AlkoStoreServer.Services.Interfaces;
using AlkoStoreServer.Services;
using Firebase.Auth;
using Firebase.Auth.Providers;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using AlkoStoreServer.ViewHelpers;
using AlkoStoreServer.ViewHelpers.Interfaces;
using FirebaseAdmin.Auth;
using Google.Api;
using Google.Cloud.Firestore;
using static Google.Cloud.Firestore.V1.StructuredQuery.Types;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews()
    .AddJsonOptions(options => 
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    });

builder.Services.AddHttpContextAccessor();

var configuration = builder.Configuration;

var firebaseConfig = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("Config/Firebase/firebase.json", optional: true, reloadOnChange: true)
    .Build();

var dbConfig = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("Config/Database/db.json", optional: true, reloadOnChange: true)
    .Build();

var firestoreConfig = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("Config/GoogleCloud/google.json", optional: true, reloadOnChange: true)
    .Build();


var firebaseCredentialPath = Path.Combine(builder.Environment.ContentRootPath, "Config", "Firebase", "firebase.json");
var firebaseCredential = GoogleCredential.FromFile(firebaseCredentialPath);
Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", firebaseCredentialPath);

builder.Services.AddSingleton(FirestoreDb.Create(firestoreConfig["GoogleCloud:ProjectId"]));

var dbPath = Path.Combine(builder.Environment.ContentRootPath, dbConfig["DbName"]);

FirebaseApp.Create(new AppOptions()
{
    Credential = firebaseCredential,
    ProjectId = firebaseConfig["project_id"]
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.Authority = "https://securetoken.google.com/" + firebaseConfig["project_id"];
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = "https://securetoken.google.com/" + firebaseConfig["project_id"],
                ValidateAudience = true,
                ValidAudience = firebaseConfig["project_id"],
                ValidateLifetime = true
            };
        });

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.Cookie.HttpOnly = true;
            options.Cookie.Name = "LoginCookie";
            options.LoginPath = "/Account/Login";
            options.LogoutPath = "/Account/Logout";
            options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminAccess", policy =>
         policy.Requirements.Add(new AdminRequirement()));
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source="+dbPath));

builder.Services.AddScoped(typeof(IDbRepository<>), typeof(DbRepository<>));
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddSingleton(FirebaseAuth.DefaultInstance);

builder.Services.AddScoped<IAuthorizationHandler, AdminRequirementHandler>();
builder.Services.AddSingleton<IInstanceResolver, InstanceResolver>();

builder.Services.AddScoped<IAttributeService, AttributeService>();
builder.Services.AddScoped<IAttributeRepository, AttributeRepository>();

builder.Services.AddScoped<IHtmlRenderer, HtmlRenderer>();
builder.Services.AddScoped<IUserService, UserService>();

//builder.WebHost.UseUrls("http://0.0.0.0:5000");

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

app.Run();
