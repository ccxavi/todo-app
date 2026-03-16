using System.Collections.ObjectModel;
using ToDoApplication.Models;
using ToDoApplication.Services;

namespace ToDoApplication;

public partial class CompletedPage : ContentPage
{
    public ObservableCollection<ToDoClass> CompletedItems { get; set; } = new();

    public CompletedPage()
    {
        InitializeComponent();
        BindingContext = CompletedItems;
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
        foreach (var item in CompletedItems)
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
        foreach (var item in CompletedItems)
        {
            item.PropertyChanged -= Item_PropertyChanged;
        }

        CompletedItems.Clear();
        foreach (var item in TaskDataStore.AllItems)
        {
            if (item.status == "Completed")
            {
                item.PropertyChanged += Item_PropertyChanged;
                CompletedItems.Add(item);
            }
        }
    }

    private void Item_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ToDoClass.status) || e.PropertyName == nameof(ToDoClass.IsCompleted))
        {
            // If an item is now pending (unchecked), it should move to the pending list
            RefreshList();
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

}
