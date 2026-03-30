using ToDoApplication.Models;
using ToDoApplication.Services;

namespace ToDoApplication;

public partial class SignUpPage : ContentPage
{
    private readonly IAuthService _authService;

    public SignUpPage(IAuthService authService)
    {
        InitializeComponent();
        _authService = authService;
    }

    private async void OnSignUpClicked(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(FirstNameEntry.Text) ||
            string.IsNullOrWhiteSpace(LastNameEntry.Text) ||
            string.IsNullOrWhiteSpace(EmailEntry.Text) ||
            string.IsNullOrWhiteSpace(PasswordEntry.Text) ||
            string.IsNullOrWhiteSpace(ConfirmPasswordEntry.Text))
        {
            await DisplayAlert("Sign Up", "Fill in all fields first.", "OK");
            return;
        }

        if (PasswordEntry.Text != ConfirmPasswordEntry.Text)
        {
            await DisplayAlert("Sign Up", "Passwords do not match.", "OK");
            return;
        }

        try
        {
            var request = new SignUpRequestClass
            {
                first_name = FirstNameEntry.Text.Trim(),
                last_name = LastNameEntry.Text.Trim(),
                email = EmailEntry.Text.Trim(),
                password = PasswordEntry.Text,
                confirm_password = ConfirmPasswordEntry.Text
            };

            var account = await _authService.SignUpAsync(request);

            if (account == null)
            {
                await DisplayAlert("Sign Up", "No account data returned.", "OK");
                return;
            }

            await DisplayAlert("Sign Up", "Account created successfully.", "OK");
            await Shell.Current.GoToAsync(AppRoutes.SignInAbsolute);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Sign Up Failed", ex.Message, "OK");
        }
    }

    private async void OnSignInClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(AppRoutes.SignInAbsolute);
    }
}
