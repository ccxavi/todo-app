namespace ToDoApplication;

public partial class AddToDoPage : ContentPage
{
    public Func<string, string, Task>? OnSaveAction { get; set; }
    public Func<Task>? OnDeleteAction { get; set; }
    
    private bool _isProcessing;

    // Constructor for Add mode
    public AddToDoPage()
    {
        InitializeComponent();
    }

    // Constructor for Edit mode
    public AddToDoPage(string title, string details)
    {
        InitializeComponent();
        PageTitle.Text = "Edit";
        SaveButton.Text = "Save";
        TitleEntry.Text = title;
        DetailsEditor.Text = details;
        DeleteButton.IsVisible = true;
    }

    private async void OnBackClicked(object? sender, EventArgs e)
    {
        if (_isProcessing) return;
        await Navigation.PopModalAsync();
    }

    private void SetLoading(bool isLoading)
    {
        _isProcessing = isLoading;
        LoadingIndicator.IsVisible = isLoading;
        LoadingIndicator.IsRunning = isLoading;
        
        TitleEntry.IsEnabled = !isLoading;
        DetailsEditor.IsEnabled = !isLoading;
        SaveButton.IsEnabled = !isLoading;
        DeleteButton.IsEnabled = !isLoading;
    }

    private async void OnAddClicked(object? sender, EventArgs e)
    {
        if (_isProcessing) return;

        string title = TitleEntry.Text ?? string.Empty;
        string details = DetailsEditor.Text ?? string.Empty;

        if (string.IsNullOrWhiteSpace(title))
        {
            await DisplayAlert("Error", "Please enter a title.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(details))
        {
            await DisplayAlert("Error", "Please enter some details.", "OK");
            return;
        }

        bool success = false;
        try
        {
            SetLoading(true);
            if (OnSaveAction != null)
            {
                await OnSaveAction.Invoke(title, details);
            }
            success = true;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An unexpected error occurred: {ex.Message}", "OK");
        }
        finally
        {
            SetLoading(false);
        }

        if (success)
        {
            await Navigation.PopModalAsync();
        }
    }

    private async void OnDeleteClicked(object? sender, EventArgs e)
    {
        if (_isProcessing) return;

        try
        {
            SetLoading(true);
            if (OnDeleteAction != null)
            {
                await OnDeleteAction.Invoke();
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An unexpected error occurred: {ex.Message}", "OK");
        }
        finally
        {
            SetLoading(false);
        }
    }
}
