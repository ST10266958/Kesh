using Cloud_Storage.Services;
using Gary.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gary.Controllers
{
    public class StoreController : Controller
    {
        private readonly BlobService _blobService;
        private readonly TableStorageService _tableStorageService;

        public StoreController(BlobService blobService, TableStorageService tableStorageService)
        {
            _blobService = blobService;
            _tableStorageService = tableStorageService;
        }

        [HttpGet]
        public IActionResult AddStore()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddStore(Store store, IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var imageUrl = await _blobService.UploadAsync(stream, file.FileName);
                store.ImageUrl = imageUrl;
            }

            if (ModelState.IsValid)
            {
                store.PartitionKey = "StoresPartition";
                store.RowKey = Guid.NewGuid().ToString();
                await _tableStorageService.AddStoreAsync(store);
                return RedirectToAction("Index");
            }
            return View(store);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStore(string partitionKey, string rowKey)
        {
            // Fetch the store to get the ImageUrl
            var store = await _tableStorageService.GetStoreAsync(partitionKey, rowKey);

            if (store != null && !string.IsNullOrEmpty(store.ImageUrl))
            {
                // Delete the associated blob image
                await _blobService.DeleteBlobAsync(store.ImageUrl);
            }

            // Delete Table entity
            await _tableStorageService.DeleteStoreAsync(partitionKey, rowKey);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Index()
        {
            var stores = await _tableStorageService.GetAllStoreAsync();
            return View(stores);
        }
        
    }
}


