#nullable disable
using estore.MicroFrontEnd.Products.Models;
using estore.MicroFrontEnd.Products.Options;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace estore.MicroFrontEnd.Products.Services
{
    public class ProductService : IProductService
    {
        private static readonly HttpClient _httpClient;
        private readonly IOptions<ApiOptions> _apiOptions;

        public ProductService(IOptions<ApiOptions> apiOptions)
        {
            _apiOptions = apiOptions;
        }

        static ProductService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Add(_apiOptions.Value.UserName, _apiOptions.Value.Password);
                var response = await _httpClient.GetAsync($"{_apiOptions.Value.Endpoint}/v1/product/all");
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Product>>(json);
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return new List<Product>();
            }
        }

        public async Task<Product> GetProductByIdAsync(int? id)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Add(_apiOptions.Value.UserName, _apiOptions.Value.Password);
                var response = await _httpClient.GetAsync($"{_apiOptions.Value.Endpoint}/v1/product/id/{id}");
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Product>(json);
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return new Product();
            }
        }

        public async Task<Product> CreateOrUpdateProductAsync(Product product)
        {
            try
            {
                
                _httpClient.DefaultRequestHeaders.Add(_apiOptions.Value.UserName, _apiOptions.Value.Password);

                var response = await _httpClient.PutAsync($"{_apiOptions.Value.Endpoint}/v1/product",
                    new StringContent(JsonSerializer.Serialize(product), Encoding.UTF8, "application/json"));
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Product>(json);
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return new Product();
            }
        }

        public async Task DeleteProductAsync(int? id)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Add(_apiOptions.Value.UserName, _apiOptions.Value.Password);
                var response = await _httpClient.DeleteAsync($"{_apiOptions.Value.Endpoint}/v1/product/id/{id}");
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                string err = ex.Message;
            }
        }
    }
}
