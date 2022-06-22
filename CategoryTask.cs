using System.Text;

namespace ReminderApp;

public class CategoryTask
{
    private TimeSpan _timeSpan;
    private DateTime _dueDate;

    public string TaskDescription { get; set; }
    public DateTime DueDate
    {
        get
        {
            UpdateTimeSpan();
            return _dueDate;
        }

        set
        {
            _dueDate = value;
            UpdateTimeSpan();
        }
    }
    public PriorityLevelType Priority { get; set; }
    public TimeSpan TimeSpan { get { return _timeSpan; } set { _timeSpan = value; } }

    public CategoryTask(string description, DateTime dueDate, PriorityLevelType priorityLevel)
    {
        TaskDescription = description;
        DueDate = dueDate;
        Priority = priorityLevel;
    }

    public void ChangeDueDate(DateTime date)
    {
        DueDate = date;
    }

    public void UpdateTimeSpan()
    {
        TimeSpan = _dueDate - DateTime.Now;
    }

    public override string ToString()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append($"{TaskDescription} | {DueDate}");
        UpdateTimeSpan();

        if (TimeSpan.Days <= 0)
        {
            if (TimeSpan.Hours == 0)
            {
                stringBuilder.Append((TimeSpan.Minutes == 1) ? $" | Due in {TimeSpan.Minutes} minute" : $" | Due in {TimeSpan.Minutes} minutes");
            }
            else if (TimeSpan.Hours == 1)
            {
                stringBuilder.Append((TimeSpan.Minutes == 1) ? $" | Due in {TimeSpan.Hours} hour and {TimeSpan.Minutes} minute" : $" | Due in {TimeSpan.Hours} hour and {TimeSpan.Minutes} minutes");
            }
            else
            {
                stringBuilder.Append($" | Due in {TimeSpan.Hours} hours and {TimeSpan.Minutes} minutes");
            }
        }
        else if (TimeSpan.Days == 1)
        {
            stringBuilder.Append($" | Due in {TimeSpan.Days} day and {TimeSpan.Hours} hours");
        }
        else
        {
            stringBuilder.Append($" | Due in {TimeSpan.Days} days and {TimeSpan.Hours} hours");
        }
        return stringBuilder.ToString();
    }
}