namespace ReminderApp;

public class Category
{
    private List<CategoryTask> _tasks = new();

    public string Title { get; set; }

    public List<CategoryTask> Tasks
    {
        get
        {
            // Gets the tracked tasks ordered by its priority and then by its TimeSpan.
            _tasks = _tasks.OrderBy(task => (int)task.Priority).ThenBy(task => task.TimeSpan).ToList();
            return _tasks;
        }
    }

    public Category(string title)
    {
        Title = title;
    }

    public bool AddTask(CategoryTask task)
    {
        Tasks.Add(task);
        return true;
    }

    public bool RemoveTask(CategoryTask task)
    {
        return Tasks.Remove(task);
    }

    
    public CategoryTask GetSingleTask(int index)
    {
        return (index < 0 || index > Tasks.Count) ? throw new IndexOutOfRangeException("That index is out of range") : Tasks[index];
    }
    
    public List<CategoryTask> GetAllTasks()
    {
        return _tasks;
    }

    public override string ToString()
    {
        return $"{Title}";
    }
}