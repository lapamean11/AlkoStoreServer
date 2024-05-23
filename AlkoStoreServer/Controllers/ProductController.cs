using AlkoStoreServer.Base;
using AlkoStoreServer.Models;
using AlkoStoreServer.Repositories.Interfaces;
using AlkoStoreServer.Services.Interfaces;
using AlkoStoreServer.ViewHelpers.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlkoStoreServer.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("product")]
    public class ProductController : BaseController
    {
        private readonly IDbRepository<Product> _productRepository;

        private readonly IAttributeService _attributeService;

        private readonly IHtmlRenderer _htmlRenderer;

        public ProductController(
            IDbRepository<Product> productRepository,
            IAttributeService attributeService,
            IHtmlRenderer htmlRenderer
        ) {
            _productRepository = productRepository;
            _attributeService = attributeService;
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
            var lol = id;
            Product product = await _productRepository.GetById(int.Parse(id),
                p => p.Include(e => e.Categories)
                        .Include(e => e.ProductStore)
                            .ThenInclude(e => e.Store)
                          .Include(e => e.ProductAttributes)
                            .ThenInclude(e => e.Attribute)
                              .ThenInclude(e => e.AttributeType)
            );

            IHtmlContent htmlResult = _htmlRenderer.RenderEditForm(product);

            return View("Views/Layouts/EditLayout.cshtml", htmlResult);
        }
    }
}
