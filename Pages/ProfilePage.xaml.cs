using ToDoApplication.Services;

namespace ToDoApplication;

public partial class ProfilePage : ContentPage
{
    private readonly ISessionService _sessionService;

    public ProfilePage(ISessionService sessionService)
    {
        InitializeComponent();
        _sessionService = sessionService;
        
        // Commented out for testing purposes:
        // BindingContext = _sessionService.CurrentUser;
    }

    private async void OnSignOutClicked(object? sender, EventArgs e)
    {
        // Clear the session on sign out
        _sessionService.CurrentUser = null;
        await Shell.Current.GoToAsync(AppRoutes.SignInAbsolute);
    }
}
