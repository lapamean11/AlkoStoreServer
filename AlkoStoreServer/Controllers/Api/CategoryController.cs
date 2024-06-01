using AlkoStoreServer.Base;
using AlkoStoreServer.Models;
using AlkoStoreServer.Repositories;
using AlkoStoreServer.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlkoStoreServer.Controllers.Api
{
    [Route("api")]
    [ApiController]
    public class CategoryController : BaseController
    {
        private readonly IDbRepository<Category> _categoryRepository;

        public CategoryController(
            IDbRepository<Category> categoryRepository
        )
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet("get/categories")]
        public async Task<IActionResult> GetCategories()
        {
            List<Category> categories = (List<Category>) await _categoryRepository.GetWithInclude(
                 c => c.Include(e => e.ChildCategories).Where(ca => ca.ID != 1 && ca.CategoryLevel == 1)
            );

            var json = BaseRepository.SerializeToJson(categories);

            return Ok(json);
        }
    }
}
