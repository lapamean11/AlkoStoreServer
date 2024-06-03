using AlkoStoreServer.Base;
using AlkoStoreServer.Data;
using AlkoStoreServer.Models;
using AlkoStoreServer.Repositories;
using AlkoStoreServer.Repositories.Interfaces;
using AlkoStoreServer.ViewHelpers.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlkoStoreServer.Controllers
{
    [Route("CategoryAttribute")]
    public class CategoryAttributeController : BaseController
    {

        private readonly IDbRepository<CategoryAttribute> _categoryAttributeRepository;

        private readonly IDbRepository<Category> _categoryRepository;

        private readonly IDbRepository<CategoryAttributeCategory> _categoryAttributeCategoryRepository;

        private readonly IHtmlRenderer _htmlRenderer;

        public CategoryAttributeController(
            IDbRepository<CategoryAttribute> categoryAttributeRepository,
            IDbRepository<Category> categoryRepository,
            IDbRepository<CategoryAttributeCategory> categoryAttributeCategoryRepository,
            IHtmlRenderer htmlRenderer
        )
        {
            _categoryAttributeRepository = categoryAttributeRepository;
            _categoryRepository = categoryRepository;
            _categoryAttributeCategoryRepository = categoryAttributeCategoryRepository;
            _htmlRenderer = htmlRenderer;
        }

        [HttpGet("list")]
        [Authorize]
        public async Task<IActionResult> CategoryAttributeList()
        {
            List<CategoryAttribute> attributes = (List<CategoryAttribute>)await _categoryAttributeRepository.GetWithInclude();

            return View("Views/Layouts/ListLayout.cshtml", attributes);
        }

        [HttpGet("edit/{id}")]
        [Authorize]
        public async Task<IActionResult> CategoryAttributeEdit(string id)
        {
            CategoryAttribute attribute = await _categoryAttributeRepository.GetById(int.Parse(id),
                a => a.Include(e => e.AttributeType)
            );

            IHtmlContent htmlResult = _htmlRenderer.RenderEditForm(attribute);
            ViewBag.Model = attribute;

            return View("Views/Layouts/EditLayout.cshtml", htmlResult);
        }

        [HttpPost("delete/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            try
            {
                await _categoryAttributeRepository.DeleteAsync(Int32.Parse(id));

                return RedirectToAction("CategoryAttributeList");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("create")]
        [Authorize]
        public async Task<IActionResult> CreateNewAttribute()
        {
            IHtmlContent htmlResult = _htmlRenderer.RenderCreateForm(new CategoryAttribute());

            return View("Views/Layouts/CreateLayout.cshtml", htmlResult);
        }

        [HttpPost("edit/save/{id}")]
        [Authorize]
        public async Task<IActionResult> EditCategoryAttributeSave(int id, CategoryAttribute attribute)
        {
            AppDbContext context = await _categoryAttributeRepository.GetContext();

            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    CategoryAttribute attributeToUpdate = await _categoryAttributeRepository.GetById(id,
                        a => a.Include(e => e.AttributeType)
                    );

                    attributeToUpdate.Name = attribute.Name;
                    attributeToUpdate.Identifier = attribute.Identifier;
                    attributeToUpdate.TypeId = attribute.AttributeType.ID;


                    await _categoryAttributeRepository.Update(attributeToUpdate);
                    await transaction.CommitAsync();

                    return RedirectToAction("CategoryAttributeList");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();

                    return StatusCode(500, "An error occurred while saving the store.");
                }
            }
        }

        [HttpPost("create/save")]
        [Authorize]
        public async Task<IActionResult> SaveNewAttribute(CategoryAttribute attribute)
        {
            using (var transaction = await (
                await _categoryAttributeRepository.GetContext()
            ).Database.BeginTransactionAsync())
            {
                try
                {
                    attribute.TypeId = attribute.AttributeType.ID;
                    attribute.AttributeType = null;

                    int newAttributeId = await _categoryAttributeRepository.CreateEntity(attribute);

                    List<Category> categories = (List<Category>) await _categoryRepository.GetWithInclude();
                    List<CategoryAttributeCategory> attributes = new List<CategoryAttributeCategory>();

                    foreach (Category category in categories)
                    {
                        var categoryAttribute = new CategoryAttributeCategory
                        {
                            CategoryId = category.ID,
                            AttributeId = newAttributeId,
                            Value = attribute.DefaultValue
                        };

                        attributes.Add(categoryAttribute);
                    }

                    await _categoryAttributeCategoryRepository.AddRange(attributes);

                    await transaction.CommitAsync();

                    return RedirectToAction("CategoryAttributeList");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();

                    return StatusCode(500, "An error occurred while saving the attribute.");
                }
            }
        }
    }
}
