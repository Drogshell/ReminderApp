using System.Globalization;
using Spectre.Console;

namespace ReminderApp
{ 
    public static class TableUtilities
    {
        public static void PrintCategory(List<Category> categories)
        {
            Console.Clear();
            AnsiConsole.Write(new Rule("[grey]─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─[/]").RuleStyle("grey"));
            var table = new Table()
                .Border(TableBorder.Rounded)
                .AddColumn(new TableColumn("[underline]Category[/]").Centered())
                .AddColumn(new TableColumn("[underline]Task Count[/]").Centered())
                .AddColumn(new TableColumn("[underline]Tasks Due Soon[/]").Centered())
                .AddColumn(new TableColumn("[underline]Tasks Overdue[/]").Centered());

            foreach (var category in categories)
            {
                var taskCount = category.Tasks.Count;
                var tasksDueSoon = category.Tasks.Count(task => task.IsDueSoon());
                var tasksOverdue = category.Tasks.Count(task => task.IsOverdue());

                table.AddRow(new Markup($"[bold]{category.Title}[/]"), new Text(taskCount.ToString()),
                    tasksDueSoon > 0 ? new Markup($"[yellow]{tasksDueSoon}[/]") : new Text("0"),
                    tasksOverdue > 0 ? new Markup($"[red]{tasksOverdue}[/]") : new Text("0"));

            }

            AnsiConsole.Write(table.RoundedBorder().Centered());
        }
        
        public static void PrintTasks(Category category)
        {
            Console.Clear();
            AnsiConsole.Write(new Rule("[grey]─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─[/]").RuleStyle("grey"));
            var table = new Table();
            table.AddColumn(new TableColumn("[underline]Description[/]").Centered());
            table.AddColumn(new TableColumn("[underline]Due Date[/]").Centered());
            table.AddColumn(new TableColumn("[underline]Priority Level[/]").Centered());
            table.AddColumn(new TableColumn("[underline]Time remaining[/]").Centered());
        
            foreach (var tasks in category.GetAllTasks())
            {
                string taskDescription;
                var taskDueDate = tasks.DueDate;
                var taskPriorityLevel = tasks.Priority;
                
                string colouredPriority;
                string colouredDate;
                string colouredTimeRemaining;
        
                switch (taskPriorityLevel)
                {
                    case PriorityLevelType.High:
                        taskDescription = $"[red]{tasks.TaskDescription}[/]";
                        colouredDate = $"[red]{taskDueDate.ToString(CultureInfo.CurrentCulture)}[/]";
                        colouredPriority = $"[red]{taskPriorityLevel}[/]";
                        colouredTimeRemaining = $"[bold underline red]{tasks}[/]";
                        break;
                    case PriorityLevelType.Medium:
                        taskDescription = $"[yellow]{tasks.TaskDescription}[/]";
                        colouredDate = $"[yellow]{taskDueDate.ToString(CultureInfo.CurrentCulture)}[/]";
                        colouredPriority = $"[yellow]{taskPriorityLevel}[/]";
                        colouredTimeRemaining = $"[bold yellow]{tasks}[/]";
        
                        break;
                    case PriorityLevelType.Low:
                        taskDescription = $"[green]{tasks.TaskDescription}[/]";
                        colouredDate = $"[green]{taskDueDate.ToString(CultureInfo.CurrentCulture)}[/]";
                        colouredPriority = $"[green]{taskPriorityLevel}[/]";
                        colouredTimeRemaining = $"[green]{tasks}[/]";
        
                        break;
                    default:
                        taskDescription = "[red]ERROR[/]";
                        colouredDate = "[red]ERROR[/]";
                        colouredPriority = taskPriorityLevel.ToString();
                        colouredTimeRemaining = $"{tasks}";
                        break;
                }
                table.AddRow(taskDescription, colouredDate, colouredPriority, colouredTimeRemaining);
            }
            AnsiConsole.Write(table.Centered());
        }
    }
}