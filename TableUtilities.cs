using System.Globalization;
using Spectre.Console;

namespace ReminderApp
{ 
    public static class TableUtilities
    {
        public static void PrintCategory(List<Category> categories)
        {
            Console.Clear();
            var table = new Table();
            table.AddColumn(new TableColumn("[u]Category[/]"));
            table.AddColumn(new TableColumn("[u]Task Count[/]"));
            table.AddColumn(new TableColumn("[u]Tasks Due Soon[/]"));
            table.AddColumn(new TableColumn("[u]Tasks Overdue[/]"));

            foreach (var category in categories)
            {
                var taskCount = category.Tasks.Count;
                var tasksDueSoon = category.Tasks.Count(task => task.IsDueSoon());
                var tasksOverdue = category.Tasks.Count(task => task.IsOverdue());

                table.AddRow(category.Title, taskCount.ToString(), tasksDueSoon.ToString(), tasksOverdue.ToString());
            }

            AnsiConsole.Write(table);
        }

        public static void PrintTasks(Category category)
        {
            Console.Clear();
            var table = new Table();
            table.AddColumn(new TableColumn("[u]Description[/]"));
            table.AddColumn(new TableColumn("[u]Due Date[/]"));
            table.AddColumn(new TableColumn("[u]Priority Level[/]"));

            foreach (var tasks in category.GetAllTasks())
            {
                string taskDescription;
                var taskDueDate = tasks.DueDate;
                var taskPriorityLevel = tasks.Priority;
                
                string colouredPriority;
                string colouredDate;

                switch (taskPriorityLevel)
                {
                    case PriorityLevelType.High:
                        taskDescription = $"[red]{tasks.TaskDescription}[/]";
                        colouredDate = $"[red]{taskDueDate.ToString(CultureInfo.CurrentCulture)}[/]";
                        colouredPriority = $"[red]{taskPriorityLevel}[/]";
                        break;
                    case PriorityLevelType.Medium:
                        taskDescription = $"[yellow]{tasks.TaskDescription}[/]";
                        colouredDate = $"[yellow]{taskDueDate.ToString(CultureInfo.CurrentCulture)}[/]";
                        colouredPriority = $"[yellow]{taskPriorityLevel}[/]";
                        break;
                    case PriorityLevelType.Low:
                        taskDescription = $"[green]{tasks.TaskDescription}[/]";
                        colouredDate = $"[green]{taskDueDate.ToString(CultureInfo.CurrentCulture)}[/]";
                        colouredPriority = $"[green]{taskPriorityLevel}[/]";
                        break;
                    default:
                        taskDescription = "[red]ERROR[/]";
                        colouredDate = "[red]ERROR[/]";
                        colouredPriority = taskPriorityLevel.ToString();
                        break;
                }
                table.AddRow(taskDescription, colouredDate, colouredPriority);
            }
            AnsiConsole.Write(table);
        }
    }
}