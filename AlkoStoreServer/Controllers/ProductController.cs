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

        private readonly IHtmlRenderer _htmlRenderer;

        public ProductController(
            IDbRepository<Product> productRepository,
            IDbRepository<ProductAttribute> productAttributeRepository,
            IDbRepository<ProductAttributeProduct> productAttributeProductRepository,
            IDbRepository<ProductStore> productStoreRepository,
            IDbRepository<ProductCategory> productCategoryRepository,
            IHtmlRenderer htmlRenderer
        ) {
            _productRepository = productRepository;
            _productAttributeRepository = productAttributeRepository;
            _productAttributeProductRepository = productAttributeProductRepository;
            _productStoreRepository = productStoreRepository;
            _productCategoryRepository = productCategoryRepository;
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

                    List<ProductCategory> categories = new List<ProductCategory>();
                    foreach (int categoryId in categoryIds)
                    {
                        categories.Add(new ProductCategory { CategoryId = categoryId, ProductId = newProductId });
                    }
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
    }
}
