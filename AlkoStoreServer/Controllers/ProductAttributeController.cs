using AlkoStoreServer.Base;
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
    [Route("ProductAttribute")]
    public class ProductAttributeController : BaseController
    {
        private readonly IDbRepository<ProductAttribute> _productAttributeRepository;

        private readonly IDbRepository<ProductAttributeProduct> _productAttributeProductRepository;

        private readonly IDbRepository<Product> _productRepository;

        private readonly IHtmlRenderer _htmlRenderer;

        public ProductAttributeController(
            IDbRepository<ProductAttribute> productAttributeRepository,
            IDbRepository<ProductAttributeProduct> productAttributeProductRepository,
            IDbRepository<Product> productRepository,
            IHtmlRenderer htmlRenderer
        )
        {
            _productAttributeRepository = productAttributeRepository;
            _productAttributeProductRepository = productAttributeProductRepository;
            _productRepository = productRepository;
            _htmlRenderer = htmlRenderer;
        }

        [HttpGet("list")]
        [Authorize]
        public async Task<IActionResult> ProductAttributeList()
        {
            List<ProductAttribute> attributes = (List<ProductAttribute>)await _productAttributeRepository.GetWithInclude();

            return View("Views/Layouts/ListLayout.cshtml", attributes);
        }

        [HttpGet("edit/{id}")]
        [Authorize]
        public async Task<IActionResult> ProductAttributeEdit(string id)
        {
            ProductAttribute attribute = await _productAttributeRepository.GetById(int.Parse(id),
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
                await _productAttributeRepository.DeleteAsync(Int32.Parse(id));

                return RedirectToAction("ProductAttributeList");
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
            IHtmlContent htmlResult = _htmlRenderer.RenderCreateForm(new ProductAttribute());

            return View("Views/Layouts/CreateLayout.cshtml", htmlResult);
        }

        [HttpPost("create/save")]
        [Authorize]
        public async Task<IActionResult> SaveNewAttribute(ProductAttribute attribute)
        {
            using (var transaction = await (
                await _productRepository.GetContext()
            ).Database.BeginTransactionAsync())
            {
                try
                {
                    attribute.TypeId = attribute.AttributeType.ID;
                    attribute.AttributeType = null;

                    int newAttributeId = await _productAttributeRepository.CreateEntity(attribute);

                    List<Product> products = (List<Product>)await _productRepository.GetWithInclude();
                    List<ProductAttributeProduct> productAttributeProducts = new List<ProductAttributeProduct>();

                    foreach (Product product in products)
                    {
                        var productAttributeProduct = new ProductAttributeProduct
                        {
                            ProductId = product.ID,
                            AttributeId = newAttributeId,
                            Value = attribute.DefaultValue
                        };

                        productAttributeProducts.Add(productAttributeProduct);
                    }

                    await _productAttributeProductRepository.AddRange(productAttributeProducts);

                    await transaction.CommitAsync();

                    return RedirectToAction("ProductAttributeList");
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
