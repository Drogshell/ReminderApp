using System.Text;

namespace ReminderApp;

public class CategoryTask
{
    private DateTime _dueDate;
    private string TaskDescription { get; }
    private DateTime DueDate
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
    public TimeSpan TimeSpan { get; private set; }

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

    private void UpdateTimeSpan()
    {
        TimeSpan = _dueDate - DateTime.Now;
    }

    public override string ToString()
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append($"{TaskDescription} | {DueDate}");
        UpdateTimeSpan();

        switch (TimeSpan.Days)
        {
            case <= 0 when TimeSpan.Hours == 0:
                stringBuilder.Append(TimeSpan.Minutes == 1 ? $" | Due in {TimeSpan.Minutes} minute" : $" | Due in {TimeSpan.Minutes} minutes");
                break;
            case <= 0 when TimeSpan.Hours == 1:
                stringBuilder.Append(TimeSpan.Minutes == 1 ? $" | Due in {TimeSpan.Hours} hour and {TimeSpan.Minutes} minute" : $" | Due in {TimeSpan.Hours} hour and {TimeSpan.Minutes} minutes");
                break;
            case <= 0:
                stringBuilder.Append($" | Due in {TimeSpan.Hours} hours and {TimeSpan.Minutes} minutes");
                break;
            case 1:
                stringBuilder.Append($" | Due in {TimeSpan.Days} day and {TimeSpan.Hours} hours");
                break;
            default:
                stringBuilder.Append($" | Due in {TimeSpan.Days} days and {TimeSpan.Hours} hours");
                break;
        }
        return stringBuilder.ToString();
    }
}