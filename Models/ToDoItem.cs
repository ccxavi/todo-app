using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ToDoApplication.Models;

public class ToDoClass : INotifyPropertyChanged
{
    private int _item_id;
    private string? _item_name;
    private string? _item_description;
    private string _status = "Pending";
    private int _user_id;
    private string _timemodified = string.Empty;

    public int item_id
    {
        get => _item_id;
        set 
        { 
            if (_item_id == value) return;
            _item_id = value; 
            OnPropertyChanged(); 
        }
    }

    public string item_name
    {
        get => _item_name ?? string.Empty;
        set 
        { 
            if (_item_name == value) return;
            _item_name = value; 
            OnPropertyChanged(); 
        }
    }

    public string item_description
    {
        get => _item_description ?? string.Empty;
        set 
        { 
            if (_item_description == value) return;
            _item_description = value; 
            OnPropertyChanged(); 
        }
    }

    public string status
    {
        get => _status;
        set 
        { 
            if (_status == value) return;
            _status = value; 
            OnPropertyChanged(); 
            OnPropertyChanged(nameof(IsCompleted));
        }
    }

    public bool IsCompleted
    {
        get => _status == "Completed";
        set 
        { 
            string newStatus = value ? "Completed" : "Pending";
            if (_status == newStatus) return;
            _status = newStatus; 
            OnPropertyChanged(); 
            OnPropertyChanged(nameof(status)); 
        }
    }

    public int user_id
    {
        get => _user_id;
        set 
        { 
            if (_user_id == value) return;
            _user_id = value; 
            OnPropertyChanged(); 
        }
    }

    public string timemodified
    {
        get => _timemodified;
        set
        {
            if (_timemodified == value) return;
            _timemodified = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public class AddItemResponseClass
{
    public int status { get; set; }
    public ToDoClass? data { get; set; }
    public string message { get; set; } = "";
}
