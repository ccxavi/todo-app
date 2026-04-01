using System.Net.Http.Json;
using ToDoApplication.Models;

namespace ToDoApplication.Services;

public interface IToDoService
{
    Task<ToDoClass> AddItemAsync(string name, string description, int userId);
    Task<IEnumerable<ToDoClass>> GetItemsAsync(string status, int userId);
    Task UpdateItemAsync(int itemId, string name, string description);
    Task UpdateStatusAsync(int itemId, string status);
}

public class ToDoService : IToDoService
{
    private readonly HttpClient _httpClient;

    public ToDoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ToDoClass> AddItemAsync(string name, string description, int userId)
    {
        var requestBody = new
        {
            item_name = name,
            item_description = description,
            user_id = userId
        };

        var response = await _httpClient.PostAsJsonAsync("/addItem_action.php", requestBody);

        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            throw new Exception(string.IsNullOrWhiteSpace(errorMessage) ? "Failed to add item." : errorMessage);
        }

        var result = await response.Content.ReadFromJsonAsync<AddItemResponseClass>();

        if (result == null)
            throw new Exception("Invalid server response.");

        if (result.status != 200 || result.data == null)
            throw new Exception(result.message ?? "Failed to add item.");

        return result.data;
    }

    public async Task<IEnumerable<ToDoClass>> GetItemsAsync(string status, int userId)
    {
        var response = await _httpClient.GetAsync($"/getItems_action.php?status={status}&user_id={userId}");

        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            throw new Exception(string.IsNullOrWhiteSpace(errorMessage) ? "Failed to fetch items." : errorMessage);
        }

        var result = await response.Content.ReadFromJsonAsync<GetItemsResponseClass>();

        if (result == null)
            throw new Exception("Invalid server response.");

        if (result.status != 200)
            throw new Exception("Server returned error status.");

        if (result.data == null)
            return Enumerable.Empty<ToDoClass>();

        return result.data.Values;
    }

    public async Task UpdateItemAsync(int itemId, string name, string description)
    {
        var requestBody = new
        {
            item_name = name,
            item_description = description,
            item_id = itemId
        };

        var response = await _httpClient.PutAsJsonAsync("/editItem_action.php", requestBody);

        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            throw new Exception(string.IsNullOrWhiteSpace(errorMessage) ? "Failed to update item." : errorMessage);
        }

        var result = await response.Content.ReadFromJsonAsync<UpdateItemResponseClass>();

        if (result == null)
            throw new Exception("Invalid server response.");

        if (result.status != 200)
            throw new Exception(result.message ?? "Failed to update item.");
    }

    public async Task UpdateStatusAsync(int itemId, string status)
    {
        var requestBody = new
        {
            status = status,
            item_id = itemId
        };

        var response = await _httpClient.PutAsJsonAsync("/statusItem_action.php", requestBody);

        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            throw new Exception(string.IsNullOrWhiteSpace(errorMessage) ? "Failed to update status." : errorMessage);
        }

        var result = await response.Content.ReadFromJsonAsync<StatusItemResponseClass>();

        if (result == null)
            throw new Exception("Invalid server response.");

        if (result.status != 200)
            throw new Exception(result.message ?? "Failed to update status.");
    }
}
