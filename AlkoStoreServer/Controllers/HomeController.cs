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

        public IActionResult Index()
        {
            var products = _productRepository.GetWithInclude(
                p => p.Include(e => e.ProductStore)
                        .ThenInclude(s => s.Store)
            );

            var lolkek = products;

            var products2 = _productRepository2.GetProductsWithStores();
            var lol3 = products2;
            /*var categories = _categoryRepository.GetWithInclude(
                    c => c.Include(e => e.CategoryAttributes)
                            .ThenInclude(ca => ca.Attribute)
                );

            var lolkek = categories;*/

            /*IEnumerable<Product> product = _productRepository.GetWithInclude(
                c => c.Include(e => e.Categories)
            );*/

            IEnumerable<CategoryProjection> category = _categoryRepository2.GetCategories();

            // var lols2 = product;
            var lola = category;

            var categories = _categoryRepository.GetWithInclude(
                    c => c.Include(e => e.CategoryAttributes)
                            .ThenInclude(ca => ca.Attribute)
                                .ThenInclude(a => a.AttributeType)
                );

            var kek = categories;

            var lol = _dbContext.Category.ToList();
            var lol2 = lol;

            return View();
        }

        public async Task<IActionResult> Test()
        {
            var uid = "48nc1LIX8jgdvNk7SFsJN5chH873";
            //var uid = "5453gdd";

            //string customToken = FirebaseAuth.DefaultInstance.CreateCustomTokenAsync(uid).Result;


            //var lol = customToken;
            UserRecord userRecord = await FirebaseAuth.DefaultInstance.GetUserAsync(uid);

            //var lol = userRecord;

            string email = "user223232@example2233.com";
            string password = "user_password222";

            UserRecordArgs args = new UserRecordArgs { Email = email, Password = password };
            args.Email = email;
            args.Password = password;
            // var user = await FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(email, password);
            // var user = await FirebaseAuth.DefaultInstance.CreateUserAsync(args);

            //var token = await FirebaseAuth.DefaultInstance.CreateCustomTokenAsync(user.Uid);
            //var token = user.ToJson();
            //var lola = token;

            // var authProv = new FirebaseAuthProvider();

            var userCreds = await _firebaseAuth.SignInWithEmailAndPasswordAsync(email, password);

            var kek = userCreds is null ? null : await userCreds.User.GetIdTokenAsync();
            var kek2 = kek;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}