using System.Collections.ObjectModel;
using ToDoApplication.Models;

namespace ToDoApplication.Services;

public static class TaskDataStore
{
    private static readonly ObservableCollection<ToDoClass> _allItems = new();

    public static ObservableCollection<ToDoClass> AllItems => _allItems;

    static TaskDataStore()
    {
        // Add some dummy data for initial testing if needed, or keep empty
    }

    public static void AddTask(ToDoClass item)
    {
        _allItems.Add(item);
    }

    public static void RemoveTask(ToDoClass item)
    {
        _allItems.Remove(item);
    }
}
