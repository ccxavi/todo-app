namespace ToDoApplication;

public partial class SignUpPage : ContentPage
{
    public SignUpPage()
    {
        InitializeComponent();
    }

    private async void OnSignInClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(AppRoutes.SignInAbsolute);
    }
}
