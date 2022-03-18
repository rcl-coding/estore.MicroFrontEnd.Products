#nullable disable
using estore.MicroFrontEnd.Products.Models;
using estore.MicroFrontEnd.Products.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RCL.Azure.Storage.Core;

namespace estore.MicroFrontEnd.Products.Areas.Admin.Pages.Products
{
    [Authorize(Policy = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly IAzureBlobStorageService _blob;

        [BindProperty]
        public Product Product { get; set; }

        public DeleteModel(IProductService productService,
            IAzureBlobStorageService blob)
        {
            _productService = productService;
            _blob = blob;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product = await _productService.GetProductByIdAsync(id);

            if (Product == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(Product?.ImageName))
            {
                Product.ImageName = _blob.GetBlobSasUri(Helpers.Constants.AzureBlobStrorageContainer, Product.ImageName);
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product = await _productService.GetProductByIdAsync(id);

            if (Product != null)
            {
                string imageName = Product.ImageName;

                await _productService.DeleteProductAsync(id);

                await _blob.DeleteBlobAsync(Helpers.Constants.AzureBlobStrorageContainer, imageName);
            }

            return RedirectToPage("./Index");
        }
    }
}
