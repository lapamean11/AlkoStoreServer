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
    [Route("store")]
    public class StoreController : BaseController
    {
        private readonly IDbRepository<Store> _storeRepository;

        private readonly IHtmlRenderer _htmlRenderer;

        public StoreController(
            IDbRepository<Store> storeRepository,
            IHtmlRenderer htmlRenderer
        ) 
        { 
            _storeRepository = storeRepository;
            _htmlRenderer = htmlRenderer;
        }

        [HttpGet("list")]
        [Authorize]
        public async Task<IActionResult> StoreList()
        {
            List<Store> stores = (List<Store>) await _storeRepository.GetWithInclude();

            return View("Views/Layouts/ListLayout.cshtml", stores);
        }

        [HttpGet("edit/{id}")]
        [Authorize]
        public async Task<IActionResult> StoreEdit(int id)
        {
            Store store = await _storeRepository.GetById(id,
                s => s.Include(e => e.ProductStore)
                        .ThenInclude(e => e.Product)
            );

            //IHtmlContent htmlResult = _htmlRenderer.RenderEditForm(store);
            IHtmlContent htmlResult = _htmlRenderer.RenderForm(store);
            ViewBag.Model = store;

            return View("Views/Layouts/EditLayout.cshtml", htmlResult);
        }

        [HttpPost("delete/{id}")]
        [Authorize]
        [Authorize(Policy = "AdminAccess")]
        public async Task<IActionResult> DeleteStore(string id)
        {
            try
            {
                await _storeRepository.DeleteAsync(Int32.Parse(id));

                return RedirectToAction("StoreList");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("create")]
        [Authorize]
        public async Task<IActionResult> CreateNewStore()
        {
            Store store = new Store();

            IHtmlContent htmlResult = _htmlRenderer.RenderForm(store);
            ViewBag.Model = store;

            return View("Views/Layouts/CreateLayout.cshtml", htmlResult);
        }

        [HttpPost("edit/save/{id}")]
        [Authorize]
        [Authorize(Policy = "AdminAccess")]
        public async Task<IActionResult> EditStoreSave(int id, Store store)
        {
            AppDbContext context = await _storeRepository.GetContext();

            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                { 
                    Store storeToUpdate = await _storeRepository.GetById(id);

                    storeToUpdate.Name = store.Name;
                    storeToUpdate.StoreLink = store.StoreLink;
                    storeToUpdate.Country = store.Country;
                    storeToUpdate.ImgUrl = store.ImgUrl;

                    await _storeRepository.Update(storeToUpdate);
                    await transaction.CommitAsync();

                    return RedirectToAction("StoreList");
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
        public async Task<IActionResult> SaveNewStore(Store store)
        {
            store.ProductStore = null;

            AppDbContext context = await _storeRepository.GetContext();

            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    store.ProductStore = null;

                    int newStoreId = await _storeRepository.CreateEntity(store);

                    await transaction.CommitAsync();

                    return RedirectToAction("StoreList");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();

                    return StatusCode(500, "An error occurred while saving the store.");
                }
            }
        }
    }
}
