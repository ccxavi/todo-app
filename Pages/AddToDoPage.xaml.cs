namespace ToDoApplication;

public partial class AddToDoPage : ContentPage
{
    public Func<string, string, Task>? OnSaveAction { get; set; }
    public Func<Task>? OnDeleteAction { get; set; }

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
        await Shell.Current.Navigation.PopModalAsync();
    }

    private async void OnAddClicked(object? sender, EventArgs e)
    {
        string title = TitleEntry.Text ?? string.Empty;
        string details = DetailsEditor.Text ?? string.Empty;

        if (string.IsNullOrWhiteSpace(title))
        {
            await DisplayAlert("Error", "Please enter a title.", "OK");
            return;
        }

        if (OnSaveAction != null)
        {
            await OnSaveAction.Invoke(title, details);
        }

        await Shell.Current.Navigation.PopModalAsync();
    }

    private async void OnDeleteClicked(object? sender, EventArgs e)
    {
        if (OnDeleteAction != null)
        {
            await OnDeleteAction.Invoke();
        }
    }
}
