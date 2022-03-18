using estore.MicroFrontEnd.Products.Helpers;
using estore.MicroFrontEnd.Products.Models;
using estore.MicroFrontEnd.Products.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RCL.Azure.Storage.Core;

namespace estore.MicroFrontEnd.Products.Areas.Admin.Pages.Products
{
    [Authorize(Policy = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly IAzureBlobStorageService _blob;

        public IList<Product> Products { get; set; } = new List<Product>();

        public IndexModel(IProductService productService,
            IAzureBlobStorageService blob)
        {
            _productService = productService;
            _blob = blob;
        }

        public async Task OnGetAsync()
        {
            List<Product> _products = await _productService.GetAllProductsAsync();

            if (_products?.Count > 0)
            {
                foreach (var product in _products)
                {
                    if (!string.IsNullOrEmpty(product.ImageName))
                    {
                        product.ImageName = _blob.GetBlobSasUri(Constants.AzureBlobStrorageContainer, product.ImageName);
                    }
                }

                Products = _products.OrderBy(o => o.SortCode).ToList();
            }
        }
    }
}
