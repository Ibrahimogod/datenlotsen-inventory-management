using System.Net.Http;
using System.Net.Http.Json;
using Datenlotsen.WPF.ViewModels;

namespace Datenlotsen.WPF.Services
{
    public class InventoryApiService
    {
        private readonly HttpClient _httpClient;
        public InventoryApiService()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:5155/api/") };
        }

        public async Task<List<InventoryItemModel>> GetInventoryItemsAsync(string? name = null,
            StockStatus? stockStatus = null, Guid? categoryId = null, CancellationToken cancellationToken = default)
        {
            var url = $"inventory?name={name}&stockStatus={stockStatus}&categoryId={categoryId}";
            return await _httpClient.GetFromJsonAsync<List<InventoryItemModel>>(url, cancellationToken) ??
                   new List<InventoryItemModel>();
        }

        public async Task<List<CategoryModel>> GetCategoriesAsync(CancellationToken cancellationToken = default)
        {
            return await _httpClient.GetFromJsonAsync<List<CategoryModel>>("categories", cancellationToken) ?? new List<CategoryModel>();
        }

        public async Task<InventoryItemModel?> GetInventoryItemAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _httpClient.GetFromJsonAsync<InventoryItemModel>($"inventory/{id}", cancellationToken);
        }

        public async Task<Guid?> CreateInventoryItemAsync(string name, decimal stockQuantity, Guid categoryId, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.PostAsJsonAsync("inventory", new { Name = name, StockQuantity = stockQuantity, CategoryId = categoryId }, cancellationToken);
            if (!response.IsSuccessStatusCode) return null;
            var result = await response.Content.ReadFromJsonAsync<InventoryItemCreatedResult>(cancellationToken: cancellationToken);
            return result?.Id;
        }

        public async Task<bool> UpdateInventoryItemAsync(Guid id, string name, decimal stockQuantity, Guid categoryId, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.PutAsJsonAsync($"inventory/{id}", new { Name = name, StockQuantity = stockQuantity, CategoryId = categoryId }, cancellationToken);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteInventoryItemAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.DeleteAsync($"inventory/{id}", cancellationToken);
            return response.IsSuccessStatusCode;
        }

        private class InventoryItemCreatedResult
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public decimal StockQuantity { get; set; }
        }
    }
}