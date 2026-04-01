using System.Net.Http.Json;
using ToDoApplication.Models;

namespace ToDoApplication.Services;

public interface IToDoService
{
    Task<ToDoClass> AddItemAsync(string name, string description, int userId);
    Task<IEnumerable<ToDoClass>> GetItemsAsync(string status, int userId);
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
}
