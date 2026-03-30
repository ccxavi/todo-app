using System.Net.Http.Json;
using ToDoApplication.Models;

namespace ToDoApplication.Services;

public interface IAuthService
{
    Task<AccountClass?> SignInAsync(SignInRequestClass request);
    Task<string> SignUpAsync(SignUpRequestClass request);
    Task SignOutAsync();
}

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;

    public AuthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<AccountClass?> SignInAsync(SignInRequestClass request)
    {
        var url = $"/signin_action.php?email={Uri.EscapeDataString(request.email)}&password={Uri.EscapeDataString(request.password)}";

        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            var error_message = await response.Content.ReadAsStringAsync();
            throw new Exception(string.IsNullOrWhiteSpace(error_message) ? "Sign in failed." : error_message);
        }

        var result = await response.Content.ReadFromJsonAsync<SignInResponseClass>();

        if (result == null)
            throw new Exception("Invalid server response.");

        if (result.status != 200)
            throw new Exception(result.message);

        return result.data;
    }

    public async Task<string> SignUpAsync(SignUpRequestClass request)
    {
        var response = await _httpClient.PostAsJsonAsync("/signup_action.php", request);

        if (!response.IsSuccessStatusCode)
        {
            var error_message = await response.Content.ReadAsStringAsync();
            throw new Exception(string.IsNullOrWhiteSpace(error_message) ? "Sign up failed." : error_message);
        }

        var result = await response.Content.ReadFromJsonAsync<SignUpResponseClass>();

        if (result == null)
            throw new Exception("Invalid server response.");

        return result.message;
    }

    public Task SignOutAsync()
    {
        return Task.CompletedTask;
    }
}
