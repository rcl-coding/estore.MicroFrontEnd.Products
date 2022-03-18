#nullable disable
using estore.MicroFrontEnd.Products.Helpers;
using estore.MicroFrontEnd.Products.Models;
using estore.MicroFrontEnd.Products.Services;
using Microsoft.AspNetCore.Mvc;
using RCL.Azure.Storage.Core;

namespace estore.MicroFrontEnd.Products.Pages.Shared.Components.ProductsList
{
    public class ProductsListViewComponent : ViewComponent
    {
        private readonly IProductService _productService;
        private readonly IAzureBlobStorageService _blob;

        public ProductsListViewComponent(IProductService productService,
            IAzureBlobStorageService blob)
        {
            _productService = productService;
            _blob = blob;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Product> Products = await _productService.GetAllProductsAsync();

            if (Products?.Count > 0)
            {
                foreach (var item in Products)
                {
                    item.ImageName = _blob.GetBlobSasUri(Constants.AzureBlobStrorageContainer, item.ImageName);
                }
            }

            return View(Products.OrderBy(o => o.SortCode).ToList());
        }
    }
}
