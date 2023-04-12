using System.Text;

namespace ReminderApp
{
    public class Task
    {
        public string TaskDescription { get; private set; }
        public DateTime DueDate { get; private set; }
        public PriorityLevelType Priority { get; set; }
        
        public TimeSpan TimeRemainingTimeSpan { get; private set; }
        
        public string TimeRemainingString
        {
            get
            {
                var remainingTime = DueDate - DateTime.Now;
                return remainingTime < TimeSpan.Zero ? "Overdue" : $"{remainingTime.Days} days, {remainingTime.Hours} hours";
            }
        }

        public Task(string description, DateTime dueDate, PriorityLevelType priorityLevel)
        {
            TaskDescription = description;
            DueDate = dueDate;
            Priority = priorityLevel;
            UpdateTimeRemaining();
        }

        public void ChangeDueDate(DateTime date)
        {
            DueDate = date;
            UpdateTimeRemaining();
        }

        public bool RenameTask(string newDescription)
        {
            TaskDescription = newDescription;
            return true;
        }

        public bool IsDueSoon()
        {
            return TimeRemainingTimeSpan.TotalDays <= 3 && TimeRemainingTimeSpan.TotalDays > 0;
        }

        public bool IsOverdue()
        {
            return TimeRemainingTimeSpan <= TimeSpan.Zero;
        }

        private void UpdateTimeRemaining()
        {
            TimeRemainingTimeSpan = DueDate - DateTime.Now;
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            if (IsOverdue())
            {
                var overdueTime = DateTime.Now - DueDate;

                if (overdueTime.TotalDays >= 1)
                {
                    stringBuilder.Append($"Overdue by {overdueTime.Days} day{(overdueTime.Days == 1 ? "" : "s")}");
                    if (overdueTime.Hours > 0 || overdueTime.Minutes > 0)
                    {
                        stringBuilder.Append($" {overdueTime.Hours} hour{(overdueTime.Hours == 1 ? "" : "s")}");
                    }
                    if (overdueTime.Minutes > 0 || overdueTime.Seconds > 0)
                    {
                        stringBuilder.Append($" {overdueTime.Minutes} minute{(overdueTime.Minutes == 1 ? "" : "s")}");
                    }
                }
                else if (overdueTime.Hours >= 1)
                {
                    stringBuilder.Append($"Overdue by {overdueTime.Hours} hour{(overdueTime.Hours == 1 ? "" : "s")}");
                    if (overdueTime.Minutes > 0 || overdueTime.Seconds > 0)
                    {
                        stringBuilder.Append($" {overdueTime.Minutes} minute{(overdueTime.Minutes == 1 ? "" : "s")}");
                    }
                }
                else
                {
                    stringBuilder.Append($"Overdue by {overdueTime.Minutes} minute{(overdueTime.Minutes == 1 ? "" : "s")}");
                    if (overdueTime.Seconds > 0)
                    {
                        stringBuilder.Append($" {overdueTime.Seconds} second{(overdueTime.Seconds == 1 ? "" : "s")}");
                    }
                }
            }
            else
            {
                UpdateTimeRemaining();

                if (TimeRemainingTimeSpan <= TimeSpan.FromMinutes(1))
                {
                    stringBuilder.Append("Due in less than a minute");
                }
                else if (TimeRemainingTimeSpan <= TimeSpan.FromHours(1))
                {
                    stringBuilder.Append(TimeRemainingTimeSpan.Minutes == 1 ? $"Due in {TimeRemainingTimeSpan.Minutes} minute" : $"Due in {TimeRemainingTimeSpan.Minutes} minutes");
                }
                else if (TimeRemainingTimeSpan <= TimeSpan.FromDays(1))
                {
                    stringBuilder.Append(TimeRemainingTimeSpan.Hours == 1 ? $"Due in {TimeRemainingTimeSpan.Hours} hour and {TimeRemainingTimeSpan.Minutes} minute" : $"Due in {TimeRemainingTimeSpan.Hours} hours and {TimeRemainingTimeSpan.Minutes} minutes");
                }
                else if (TimeRemainingTimeSpan <= TimeSpan.FromDays(2))
                {
                    stringBuilder.Append($"Due in {TimeRemainingTimeSpan.Days} day and {TimeRemainingTimeSpan.Hours} hours");
                }
                else
                {
                    stringBuilder.Append($"Due in {TimeRemainingTimeSpan.Days} days and {TimeRemainingTimeSpan.Hours} hours");
                }
            }

            return stringBuilder.ToString();
        }

    }
}