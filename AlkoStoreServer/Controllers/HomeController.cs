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

        private readonly IDbRepository<Models.User> _userRepository;

        //private readonly FirebaseAuthClient _firebaseAuth;

        public HomeController(
            IDbRepository<Models.User> userRepository
        )
        {
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet("dashboard")]
        [Authorize]
        public async Task<IActionResult> Dashboard()
        {
            int userCount = ((List<Models.User>)await _userRepository.GetWithInclude()).Count();
            var dashboardData = new
            {
                userCount = userCount
            };

            return View(dashboardData);
        }
    }
}