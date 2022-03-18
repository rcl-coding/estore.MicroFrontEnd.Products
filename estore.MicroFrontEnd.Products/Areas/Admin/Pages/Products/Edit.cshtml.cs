#nullable disable
using estore.MicroFrontEnd.Products.Models;
using estore.MicroFrontEnd.Products.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RCL.Azure.Storage.Core;

namespace estore.MicroFrontEnd.Products.Areas.Admin.Pages.Products
{
    [Authorize(Policy = "Admin")]
    public class EditModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly IAzureBlobStorageService _blob;

        public string ErrorMessage { get; set; } = string.Empty;

        [BindProperty]
        public Product Product { get; set; }

        public EditModel(IProductService productService,
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

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile file)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string oldImageName = string.Empty;
            string fileExtension = string.Empty;

            if (file != null)
            {
                if (file.Length < 1)
                {
                    ErrorMessage = "File error";
                    return Page();
                }

                if (FileHelper.IsImageFile(file.FileName) == false)
                {
                    ErrorMessage = "Only jpg, jpeg, png, gif, bmp, svg image files are allowed.";
                    return Page();
                }

                oldImageName = Product.ImageName;
                fileExtension = FileHelper.GetFileExtension(file.FileName);
                Product.ImageName = $"{Guid.NewGuid().ToString()}{fileExtension}";
            }

            try
            {
                await _productService.CreateOrUpdateProductAsync(Product);

                if (!string.IsNullOrEmpty(oldImageName))
                {
                    using (var readStream = file.OpenReadStream())
                    {
                        var blob = await _blob.UploadBlobAsync(Helpers.Constants.AzureBlobStrorageContainer, ContainerType.Public, Product.ImageName, readStream, FileHelper.GetContentType(fileExtension));
                    }

                    int del = await _blob.DeleteBlobAsync(Helpers.Constants.AzureBlobStrorageContainer, oldImageName);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
