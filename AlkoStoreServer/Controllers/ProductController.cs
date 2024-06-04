using AlkoStoreServer.Base;
using AlkoStoreServer.Data;
using AlkoStoreServer.Models;
using AlkoStoreServer.Repositories;
using AlkoStoreServer.Repositories.Interfaces;
using AlkoStoreServer.Services.Interfaces;
using AlkoStoreServer.ViewHelpers.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace AlkoStoreServer.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("product")]
    public class ProductController : BaseController
    {
        private readonly IDbRepository<Product> _productRepository;

        private readonly IDbRepository<ProductAttribute> _productAttributeRepository;

        private readonly IDbRepository<ProductAttributeProduct> _productAttributeProductRepository;

        private readonly IDbRepository<ProductStore> _productStoreRepository;

        private readonly IDbRepository<ProductCategory> _productCategoryRepository;

        private readonly IDbRepository<Category> _categoryRepository;

        private readonly IHtmlRenderer _htmlRenderer;

        public ProductController(
            IDbRepository<Product> productRepository,
            IDbRepository<ProductAttribute> productAttributeRepository,
            IDbRepository<ProductAttributeProduct> productAttributeProductRepository,
            IDbRepository<ProductStore> productStoreRepository,
            IDbRepository<ProductCategory> productCategoryRepository,
            IDbRepository<Category> categoryRepository,
            IHtmlRenderer htmlRenderer
        ) {
            _productRepository = productRepository;
            _productAttributeRepository = productAttributeRepository;
            _productAttributeProductRepository = productAttributeProductRepository;
            _productStoreRepository = productStoreRepository;
            _productCategoryRepository = productCategoryRepository;
            _categoryRepository = categoryRepository;
            _htmlRenderer = htmlRenderer;
        }

        [HttpGet("list")]
        [Authorize]
        public async Task<IActionResult> ProductList() 
        {
            List<Product> products = (List<Product>) await _productRepository.GetWithInclude();

            return View("Views/Layouts/ListLayout.cshtml", products);
        }

        [HttpGet("edit/{id}")]
        [Authorize]
        public async Task<IActionResult> ProductEdit(string id)
        {
            Product product = await _productRepository.GetById(int.Parse(id),
                p => p.Include(e => e.Categories)
                        .Include(e => e.ProductStore)
                            .ThenInclude(e => e.Store)
                          .Include(e => e.ProductAttributes)
                            .ThenInclude(e => e.Attribute)
                              .ThenInclude(e => e.AttributeType)
            );

            IHtmlContent htmlResult = _htmlRenderer.RenderEditForm(product);
            ViewBag.Model = product;

            return View("Views/Layouts/EditLayout.cshtml", htmlResult);
        }

        [HttpPost("delete/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            try
            {
                await _productRepository.DeleteAsync(Int32.Parse(id));

                return RedirectToAction("ProductList");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("create")]
        [Authorize]
        public async Task<IActionResult> CreateNewProduct()
        { 
            Product product = new Product();

            List<ProductAttribute> attributes = 
                (List<ProductAttribute>) await _productAttributeRepository.GetWithInclude(
                        a => a.Include(a => a.AttributeType)
                    );

            foreach (var attribute in attributes)
            {
                var productAttr = new ProductAttributeProduct
                {
                    Attribute = attribute,
                };
                product.ProductAttributes.Add(productAttr);
            }

            IHtmlContent htmlResult = _htmlRenderer.RenderCreateForm(product);

            return View("Views/Layouts/CreateLayout.cshtml", htmlResult);
        }

        [HttpPost("edit/save/{id}")]
        [Authorize]
        public async Task<IActionResult> EditProductSave(int id, Product product)
        {
            AppDbContext context = await _productRepository.GetContext();

            using (var transaction = await context.Database.BeginTransactionAsync()) 
            {
                try
                {
                    Product productToUpdate = await _productRepository.GetById(id,
                            p => p.Include(e => e.Categories)
                                    .Include(e => e.ProductStore)
                                        .ThenInclude(e => e.Store)
                                      .Include(e => e.ProductAttributes)
                                        .ThenInclude(e => e.Attribute)
                                          .ThenInclude(e => e.AttributeType)
                    );

                    if (productToUpdate == null)
                        throw new Exception();

                    productToUpdate.Name = product.Name;
                    productToUpdate.ImgUrl = product.ImgUrl;

                    List<int> newCategoryIds = Request.Form["Categories"].Select(int.Parse).ToList();
                    HashSet<int> allCategoryIds = new HashSet<int>();

                    foreach (int categoryId in newCategoryIds)
                    {
                        await AddCategoryAndRelations(categoryId, allCategoryIds);
                    }

                    List<int> existingCategoryIds = productToUpdate.Categories.Select(c => c.CategoryId).ToList();

                    List<ProductCategory> categoriesToRemove = productToUpdate.Categories
                        .Where(c => !allCategoryIds.Contains(c.CategoryId))
                        .ToList();

                    foreach (var category in categoriesToRemove)
                    {
                        productToUpdate.Categories.Remove(category);
                    }

                    var categoriesToAdd = allCategoryIds
                        .Where(id => !existingCategoryIds.Contains(id))
                        .Select(id => new ProductCategory { ProductId = id, CategoryId = id })
                        .ToList();

                    foreach (var category in categoriesToAdd)
                    {
                        productToUpdate.Categories.Add(category);
                    }


                    List<ProductStore> newStores = product.ProductStore.Where(e => e.StoreId != 0).ToList();
                    List<ProductStore> existingStores = productToUpdate.ProductStore;

                    List<ProductStore> storesToRemove = existingStores
                        .Where(es => !newStores.Any(ns => ns.StoreId == es.StoreId))
                        .ToList();

                    foreach (var store in storesToRemove)
                    {
                        productToUpdate.ProductStore.Remove(store);
                    }

                    foreach (var newStore in newStores)
                    {
                        var existingStore = existingStores.FirstOrDefault(es => es.StoreId == newStore.StoreId);
                        if (existingStore == null)
                        {
                            productToUpdate.ProductStore.Add(new ProductStore
                            {
                                StoreId = newStore.StoreId,
                                ProductId = id,
                                Price = newStore.Price,
                                Barcode = newStore.Barcode,
                                Qty = newStore.Qty
                            });
                        }
                        else
                        {
                            existingStore.Price = newStore.Price;
                            existingStore.Barcode = newStore.Barcode;
                            existingStore.Qty = newStore.Qty;
                        }
                    }

                    List<ProductAttributeProduct> newAttributes = product.ProductAttributes;
                    List<ProductAttributeProduct> existingAttributes = productToUpdate.ProductAttributes;

                    List<ProductAttributeProduct> attributesToRemove = existingAttributes
                        .Where(ea => !newAttributes.Any(na => na.AttributeId == ea.AttributeId))
                        .ToList();

                    foreach (var attribute in attributesToRemove)
                    {
                        productToUpdate.ProductAttributes.Remove(attribute);
                    }

                    foreach (var newAttribute in newAttributes)
                    {
                        var existingAttribute = existingAttributes.FirstOrDefault(ea => ea.AttributeId == newAttribute.AttributeId);
                        if (existingAttribute == null)
                        {
                            productToUpdate.ProductAttributes.Add(new ProductAttributeProduct
                            {
                                ProductId = id,
                                AttributeId = newAttribute.AttributeId,
                                Value = newAttribute.Value
                            });
                        }
                        else
                        {
                            existingAttribute.Value = newAttribute.Value ?? "0";
                        }
                    }

                    await _productRepository.Update(productToUpdate);
                    await transaction.CommitAsync();

                    return RedirectToAction("ProductList");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();

                    return StatusCode(500, "An error occurred while saving the product.");
                }
            }
        }

        [HttpPost("create/save")]
        [Authorize]
        public async Task<IActionResult> SaveNewProduct(Product product)
        {
            AppDbContext context = await _productRepository.GetContext(); 

            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    List<int> categoryIds = Request.Form["Categories"].Select(int.Parse).ToList();
                    List<ProductStore> stores = product.ProductStore.Where(e => e.StoreId != 0).ToList();
                    List<ProductAttributeProduct> attributes = product.ProductAttributes;

                    product.ProductAttributes = null;
                    product.ProductStore = null;

                    int newProductId = await _productRepository.CreateEntity(product);

                    HashSet<int> allCategoryIds = new HashSet<int>();

                    foreach (int categoryId in categoryIds)
                    {
                        await AddCategoryAndRelations(categoryId, allCategoryIds);
                    }

                    List<ProductCategory> categories = allCategoryIds.Select(
                        categoryId => new ProductCategory 
                        { 
                            CategoryId = categoryId,
                            ProductId = newProductId 
                        }
                    ).ToList();

                    await _productCategoryRepository.AddRange(categories);

                    if (stores.Any())
                    {
                        foreach (ProductStore store in stores)
                        {
                            store.ProductId = newProductId;
                        }
                        await _productStoreRepository.AddRange(stores);
                    }

                    if (attributes.Any())
                    {
                        foreach (ProductAttributeProduct attribute in attributes)
                        {
                            attribute.ProductId = newProductId;
                            attribute.Value = attribute.Value ?? "0";
                        }
                        await _productAttributeProductRepository.AddRange(attributes);
                    }

                    await transaction.CommitAsync();

                    return RedirectToAction("ProductList");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();

                    return StatusCode(500, "An error occurred while saving the product.");
                }
            }
        }

        private async Task AddCategoryAndRelations(int categoryId, HashSet<int> allCategoryIds)
        {
            if (!allCategoryIds.Contains(categoryId))
            {
                allCategoryIds.Add(categoryId);

                Category category = await _categoryRepository.GetById(categoryId,
                    c => c.Include(e => e.ChildCategories).Where(ca => ca.ID != 1 && ca.CategoryLevel > 0)
               );

                if (category != null)
                {
                    if (category.ParentCategoryId.HasValue && category.ParentCategoryId != 1)
                    {
                        await AddCategoryAndRelations(category.ParentCategoryId.Value, allCategoryIds);
                    }

                    foreach (var subCategory in category.ChildCategories)
                    {
                        await AddCategoryAndRelations(subCategory.ID, allCategoryIds);
                    }
                }
            }
        }
    }
}
