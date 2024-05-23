using AlkoStoreServer.Base;
using AlkoStoreServer.Models;
using AlkoStoreServer.Repositories;
using AlkoStoreServer.Repositories.Interfaces;
using AlkoStoreServer.Services.Interfaces;
using AlkoStoreServer.ViewHelpers.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlkoStoreServer.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("category")]
    public class CategoryController : BaseController
    {
        private readonly IDbRepository<Category> _categoryRepository;

        private readonly IHtmlRenderer _htmlRenderer;

        public CategoryController(
            IDbRepository<Category> categoryRepository,
            IHtmlRenderer htmlRenderer
        ) {
            _categoryRepository = categoryRepository;
            _htmlRenderer = htmlRenderer;
        }

        [HttpGet("list")]
        [Authorize]
        public async Task<IActionResult> ProductList()
        {
            List<Category> categories = (List<Category>) await _categoryRepository.GetWithInclude();

            return View("Views/Layouts/ListLayout.cshtml", categories);
        }

        [HttpGet("edit/{id}")]
        [Authorize]
        public async Task<IActionResult> ProductEdit(string id)
        {
            Category category = await _categoryRepository.GetById(int.Parse(id),
                c => c.Include(e => e.CategoryAttributes)
                        .ThenInclude(e => e.Attribute)
                          .ThenInclude(e => e.AttributeType)
           );

            IHtmlContent htmlResult = _htmlRenderer.RenderEditForm(category);

            return View("Views/Layouts/EditLayout.cshtml", htmlResult);
        }
    }
}
