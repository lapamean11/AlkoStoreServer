using AlkoStoreServer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using FirebaseAdmin;
using FireSharp.Response;
using FireSharp.Config;
using FireSharp.Interfaces;
using Microsoft.EntityFrameworkCore;
using AlkoStoreServer.Data;
using Microsoft.Extensions.Hosting;
using AlkoStoreServer.Models.Projections;
using AlkoStoreServer.Repositories.Interfaces;
using FirebaseAdmin.Auth;
using Firebase.Auth;
using FireSharp.Extensions;
using Firebase.Auth.Providers;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authorization;
using static System.Net.Mime.MediaTypeNames;

namespace AlkoStoreServer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly AppDbContext _dbContext;

        private readonly IDbRepository<Category> _categoryRepository;

        private readonly IDbRepository<Product> _productRepository;

        private readonly ICategoryRepository _categoryRepository2;

        private readonly IProductRepository _productRepository2;

        private readonly FirebaseAuthClient _firebaseAuth;

        public HomeController(
            ILogger<HomeController> logger, 
            AppDbContext dbContext,
            IDbRepository<Category> categoryRepository,
            IDbRepository<Product> productRepository,
            ICategoryRepository categoryRepository2,
            IProductRepository productRepository2,
            FirebaseAuthClient firebaseAuth
        )
        {
            _logger = logger;
            _dbContext = dbContext;
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            _categoryRepository2 = categoryRepository2;
            _productRepository2 = productRepository2;
            _firebaseAuth = firebaseAuth;

        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet("test")]
        [Authorize(Policy = "AdminAccess")]
        public async Task<IActionResult> Test()
        {
            string email = "test@test.com";
            string password = "123456";

            UserRecordArgs args = new UserRecordArgs
            { 
                Email = "test@test.com",
                Password = "123456"
            };
            args.Email = email;
            args.Password = password;

            var userCreds = await _firebaseAuth.SignInWithEmailAndPasswordAsync(email, password);

            var kek = userCreds is null ? null : await userCreds.User.GetIdTokenAsync();
            var kek2 = kek;
            return View();
        }

        [HttpGet("dashboard")]
        [Authorize] // (Policy = "AdminAccess")
        public async Task<IActionResult> Dashboard()
        {
            return View();
        }
    }
}