using System.Collections.ObjectModel;
using ToDoApplication.Models;
using ToDoApplication.Services;

namespace ToDoApplication;

public partial class CompletedPage : ContentPage
{
    public ObservableCollection<ToDoClass> CompletedItems { get; set; } = new();

    private readonly IToDoService _toDoService;
    private readonly ISessionService _sessionService;

    public CompletedPage(IToDoService toDoService, ISessionService sessionService)
    {
        InitializeComponent();
        _toDoService = toDoService;
        _sessionService = sessionService;
        BindingContext = CompletedItems;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadCompletedTasksAsync();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        
        // Unsubscribe from existing items to prevent leaks
        foreach (var item in CompletedItems)
        {
            item.PropertyChanged -= Item_PropertyChanged;
        }
    }

    private async Task LoadCompletedTasksAsync()
    {
        if (_sessionService.CurrentUser == null) return;

        try
        {
            var items = await _toDoService.GetItemsAsync("inactive", _sessionService.CurrentUser.id);

            // Unsubscribe from old items
            foreach (var item in CompletedItems)
            {
                item.PropertyChanged -= Item_PropertyChanged;
            }

            CompletedItems.Clear();
            foreach (var item in items)
            {
                item.PropertyChanged += Item_PropertyChanged;
                CompletedItems.Add(item);
            }

            if (CompletedItems.Count == 0)
            {
                // Optional: Show empty state message
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load completed tasks: {ex.Message}", "OK");
        }
    }

    private void Item_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ToDoClass.status) || e.PropertyName == nameof(ToDoClass.IsCompleted))
        {
            // If an item is now pending (unchecked), it should move to the pending list
            _ = LoadCompletedTasksAsync();
        }
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
                CompletedItems.Remove(item);
            };

            await Shell.Current.Navigation.PushModalAsync(editPage);
        }
    }

}
