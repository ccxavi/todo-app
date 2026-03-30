using ToDoApplication.Models;
using ToDoApplication.Services;

namespace ToDoApplication;

public partial class SignInPage : ContentPage
{
    private readonly IAuthService _authService;

    public SignInPage(IAuthService authService)
    {
        InitializeComponent();
        _authService = authService;
    }

    private void SetLoadingState(bool isLoading)
    {
        LoadingIndicator.IsVisible = isLoading;
        LoadingIndicator.IsRunning = isLoading;
        EmailEntry.IsEnabled = !isLoading;
        PasswordEntry.IsEnabled = !isLoading;
        SignInButton.IsEnabled = !isLoading;
        SignUpButton.IsEnabled = !isLoading;
    }

    private async void OnSignInClicked(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(EmailEntry.Text) || string.IsNullOrWhiteSpace(PasswordEntry.Text))
        {
            await DisplayAlert("Sign In", "Enter your email and password first.", "OK");
            return;
        }

        SetLoadingState(true);

        try
        {
            var request = new SignInRequestClass
            {
                email = EmailEntry.Text.Trim(),
                password = PasswordEntry.Text
            };

            var account = await _authService.SignInAsync(request);

            if (account == null)
            {
                await DisplayAlert("Sign In", "No account data returned.", "OK");
                return;
            }

            await DisplayAlert("Sign In", $"Welcome, {account.fname}!", "OK");
            await Shell.Current.GoToAsync(AppRoutes.ToDoAbsolute);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Sign In Failed", ex.Message, "OK");
        }
        finally
        {
            SetLoadingState(false);
        }
    }

    private async void OnSignUpClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(AppRoutes.SignUpPage);
    }
}
