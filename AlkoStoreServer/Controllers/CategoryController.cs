using AlkoStoreServer.Base;
using AlkoStoreServer.Data;
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

        private readonly IDbRepository<CategoryAttribute> _categoryAttributeRepository;

        private readonly IDbRepository<CategoryAttributeCategory> _categoryAttributeCategoryRepository;

        private readonly IHtmlRenderer _htmlRenderer;

        public CategoryController(
            IDbRepository<Category> categoryRepository,
            IDbRepository<CategoryAttribute> categoryAttributeRepository,
            IDbRepository<CategoryAttributeCategory> categoryAttributeCategoryRepository,
            IHtmlRenderer htmlRenderer
        ) {
            _categoryRepository = categoryRepository;
            _categoryAttributeRepository = categoryAttributeRepository;
            _categoryAttributeCategoryRepository = categoryAttributeCategoryRepository;
            _htmlRenderer = htmlRenderer;
        }

        [HttpGet("list")]
        [Authorize]
        public async Task<IActionResult> CategoryList()
        {
            List<Category> categories = (List<Category>) await _categoryRepository.GetWithInclude();

            return View("Views/Layouts/ListLayout.cshtml", categories);
        }

        [HttpGet("edit/{id}")]
        [Authorize]
        public async Task<IActionResult> CategoryEdit(string id)
        {
            Category category = await _categoryRepository.GetById(int.Parse(id),
                c => c.Include(e => e.CategoryAttributes)
                        .ThenInclude(e => e.Attribute)
                          .ThenInclude(e => e.AttributeType)
                      .Include(e => e.ParentCategory)
           );

            //IHtmlContent htmlResult = _htmlRenderer.RenderEditForm(category);
            IHtmlContent htmlResult = _htmlRenderer.RenderForm(category);
            ViewBag.Model = category;

            return View("Views/Layouts/EditLayout.cshtml", htmlResult);
        }

        [HttpPost("delete/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            try
            {
                await _categoryRepository.DeleteAsync(Int32.Parse(id));

                return RedirectToAction("CategoryList");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("create")]
        [Authorize]
        public async Task<IActionResult> CreateNewCategory()
        {
            Category category = new Category();

            List<CategoryAttribute> attributes = 
                (List<CategoryAttribute>) await _categoryAttributeRepository.GetWithInclude(
                        a => a.Include(e => e.AttributeType)
                    );

            foreach (var attribute in attributes) 
            {
                var categoryAttr = new CategoryAttributeCategory
                {
                    Attribute = attribute,
                };
                category.CategoryAttributes.Add(
                        categoryAttr
                    );
            }

            //IHtmlContent htmlResult = _htmlRenderer.RenderCreateForm(category);
            IHtmlContent htmlResult = _htmlRenderer.RenderForm(category);
            ViewBag.Model = category;

            return View("Views/Layouts/CreateLayout.cshtml", htmlResult);
        }

        [HttpPost("edit/save/{id}")]
        [Authorize]
        public async Task<IActionResult> EditCategorySave(int id, Category category)
        {
            AppDbContext context = await _categoryRepository.GetContext();

            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    Category categoryToUppdate = await _categoryRepository.GetById(id,
                        c => c.Include(e => e.CategoryAttributes)
                    );

                    categoryToUppdate.Name = category.Name;
                    categoryToUppdate.ParentCategoryId = category.ParentCategory.ID;

                    List<CategoryAttributeCategory> newAttributes = category.CategoryAttributes;
                    List<CategoryAttributeCategory> existingAttributes = categoryToUppdate.CategoryAttributes;

                    List<CategoryAttributeCategory> attributesToRemove = existingAttributes
                        .Where(ea => !newAttributes.Any(na => na.AttributeId == ea.AttributeId))
                        .ToList();

                    foreach (var attribute in attributesToRemove)
                    {
                        categoryToUppdate.CategoryAttributes.Remove(attribute);
                    }

                    foreach (var newAttribute in newAttributes)
                    {
                        var existingAttribute = existingAttributes.FirstOrDefault(ea => ea.AttributeId == newAttribute.AttributeId);
                        if (existingAttribute == null)
                        {
                            categoryToUppdate.CategoryAttributes.Add(new CategoryAttributeCategory
                            {
                                CategoryId = id,
                                AttributeId = newAttribute.AttributeId,
                                Value = newAttribute.Value
                            });
                        }
                        else
                        {
                            existingAttribute.Value = newAttribute.Value ?? "0";
                        }
                    }

                    await _categoryRepository.Update(categoryToUppdate);

                    await transaction.CommitAsync();

                    return RedirectToAction("CategoryList");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();

                    return StatusCode(500, "An error occurred while saving the category.");
                }
            }
        }

        [HttpPost("create/save")]
        [Authorize]
        public async Task<IActionResult> SaveNewCategory(Category category)
        {
            using (var transaction = await (
                await _categoryRepository.GetContext()
            ).Database.BeginTransactionAsync())
            {
                try 
                {
                    category.ParentCategoryId = category.ParentCategory.ID;

                    Category parentCategory = await _categoryRepository.GetById((int)category.ParentCategoryId);
                    var categoryAttributes = category.CategoryAttributes;
                    category.CategoryLevel = parentCategory.CategoryLevel + 1;
                    category.CategoryAttributes = null;
                    category.ParentCategory = null;

                    int newCategoryId = await _categoryRepository.CreateEntity(category);

                    List<CategoryAttributeCategory> attributes = new List<CategoryAttributeCategory>();

                    foreach (CategoryAttributeCategory item in categoryAttributes)
                    {
                        var attribute = new CategoryAttributeCategory
                        {
                            CategoryId = newCategoryId,
                            AttributeId = item.AttributeId,
                            Value = item.Value ?? "0"
                        };

                        attributes.Add(attribute);
                    }

                    await _categoryAttributeCategoryRepository.AddRange(attributes);

                    await transaction.CommitAsync();

                    return RedirectToAction("CategoryList");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();

                    return StatusCode(500, "An error occurred while saving the Category.");
                }
            }
        }
    }
}
