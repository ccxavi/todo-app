namespace ToDoApplication;

public partial class SignInPage : ContentPage
{
    public SignInPage()
    {
        InitializeComponent();
    }

    private async void OnSignInClicked(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(EmailEntry.Text) || string.IsNullOrWhiteSpace(PasswordEntry.Text))
        {
            await DisplayAlertAsync("Sign In", "Enter your email and password first.", "OK");
            return;
        }

        await Shell.Current.GoToAsync(AppRoutes.ToDoAbsolute);
    }

    private async void OnSignUpClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(AppRoutes.SignUpPage);
    }
}
