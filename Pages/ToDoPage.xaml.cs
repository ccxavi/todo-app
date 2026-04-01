using System.Collections.ObjectModel;
using ToDoApplication.Models;
using ToDoApplication.Services;

namespace ToDoApplication;

public partial class ToDoPage : ContentPage
{
    public ObservableCollection<ToDoClass> ToDoItems { get; set; } = new();

    private readonly IToDoService _toDoService;
    private readonly ISessionService _sessionService;

    public ToDoPage(IToDoService toDoService, ISessionService sessionService)
    {
        InitializeComponent();
        _toDoService = toDoService;
        _sessionService = sessionService;
        BindingContext = ToDoItems;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadTasksAsync();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        
        // Unsubscribe from existing items to prevent leaks
        foreach (var item in ToDoItems)
        {
            item.PropertyChanged -= Item_PropertyChanged;
        }
    }

    private async Task LoadTasksAsync()
    {
        if (_sessionService.CurrentUser == null) return;

        try
        {
            var items = await _toDoService.GetItemsAsync("active", _sessionService.CurrentUser.id);
            
            // Unsubscribe from old items
            foreach (var item in ToDoItems)
            {
                item.PropertyChanged -= Item_PropertyChanged;
            }

            ToDoItems.Clear();
            foreach (var item in items)
            {
                item.PropertyChanged += Item_PropertyChanged;
                ToDoItems.Add(item);
            }

            if (ToDoItems.Count == 0)
            {
                // Optional: Show empty state message
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load tasks: {ex.Message}", "OK");
        }
    }

    private async void OnAddClicked(object? sender, EventArgs e)
    {
        var addPage = new AddToDoPage();
        addPage.OnSaveAction = async (title, description) =>
        {
            if (_sessionService.CurrentUser == null)
            {
                await DisplayAlert("Error", "User not logged in.", "OK");
                return;
            }

            try
            {
                var newItem = await _toDoService.AddItemAsync(title, description, _sessionService.CurrentUser.id);
                newItem.PropertyChanged += Item_PropertyChanged;
                ToDoItems.Add(newItem);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to add task: {ex.Message}", "OK");
            }
        };

        await Shell.Current.Navigation.PushModalAsync(addPage);
    }

    private async void OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is ToDoClass item)
        {
            // Clear selection
            if (sender is CollectionView collectionView)
            {
                collectionView.SelectedItem = null;
            }

            var editPage = new AddToDoPage(item.item_name, item.item_description);
            editPage.OnSaveAction = async (newTitle, newDescription) =>
            {
                try
                {
                    await _toDoService.UpdateItemAsync(item.item_id, newTitle, newDescription ?? string.Empty);
                    item.item_name = newTitle;
                    item.item_description = newDescription ?? string.Empty;
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Failed to update task: {ex.Message}", "OK");
                }
            };
            editPage.OnDeleteAction = () =>
            {
                ToDoItems.Remove(item);
            };

            await Shell.Current.Navigation.PushModalAsync(editPage);
        }
    }

}
