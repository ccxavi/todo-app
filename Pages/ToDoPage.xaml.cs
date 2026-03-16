using System.Collections.ObjectModel;
using ToDoApplication.Models;
using ToDoApplication.Services;

namespace ToDoApplication;

public partial class ToDoPage : ContentPage
{
    public ObservableCollection<ToDoClass> ToDoItems { get; set; } = new();

    public ToDoPage()
    {
        InitializeComponent();
        BindingContext = ToDoItems;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        RefreshList();
        
        // Subscribe to changes in the master list
        TaskDataStore.AllItems.CollectionChanged += AllItems_CollectionChanged;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        TaskDataStore.AllItems.CollectionChanged -= AllItems_CollectionChanged;
        
        // Unsubscribe from existing items to prevent leaks
        foreach (var item in ToDoItems)
        {
            item.PropertyChanged -= Item_PropertyChanged;
        }
    }

    private void AllItems_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        RefreshList();
    }

    private void RefreshList()
    {
        // Unsubscribe from existing items
        foreach (var item in ToDoItems)
        {
            item.PropertyChanged -= Item_PropertyChanged;
        }

        ToDoItems.Clear();
        foreach (var item in TaskDataStore.AllItems)
        {
            if (item.status == "Pending")
            {
                item.PropertyChanged += Item_PropertyChanged;
                ToDoItems.Add(item);
            }
        }
    }

    private void Item_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ToDoClass.status) || e.PropertyName == nameof(ToDoClass.IsCompleted))
        {
            // If an item is now completed, it should move to the completed list
            RefreshList();
        }
    }

    private async void OnAddClicked(object? sender, EventArgs e)
    {
        var addPage = new AddToDoPage();
        addPage.OnSaveAction = (title, description) =>
        {
            TaskDataStore.AddTask(new ToDoClass
            {
                item_name = title,
                item_description = description ?? string.Empty,
                status = "Pending"
            });
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
            editPage.OnSaveAction = (newTitle, newDescription) =>
            {
                item.item_name = newTitle;
                item.item_description = newDescription ?? string.Empty;
            };
            editPage.OnDeleteAction = () =>
            {
                TaskDataStore.RemoveTask(item);
            };

            await Shell.Current.Navigation.PushModalAsync(editPage);
        }
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
