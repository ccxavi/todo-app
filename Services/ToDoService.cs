using System.Net.Http.Json;
using ToDoApplication.Models;

namespace ToDoApplication.Services;

public interface IToDoService
{
    Task<ToDoClass> AddItemAsync(string name, string description, int userId);
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
}
