namespace ToDoApplication;

public partial class CompletedPage : ContentPage
{
    public CompletedPage()
    {
        InitializeComponent();
    }

    private async void OnToDoClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(AppRoutes.ToDoAbsolute);
    }

    private async void OnCompletedClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(AppRoutes.CompletedAbsolute);
    }

    private async void OnProfileClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(AppRoutes.ProfileAbsolute);
    }
}
