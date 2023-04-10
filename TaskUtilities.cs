using System.Globalization;
using Spectre.Console;

namespace ReminderApp
{
    public static class TaskUtilities
    {
        public static void AddTask(Category category)
        {
            var description = AnsiConsole.Ask<string>("Enter the description of the task: ");
            var priorityLevel = PriorityLevel.GetPriorityLevel();
            var userDate = GetDate();
            category.AddTask(new CategoryTask(description, userDate, priorityLevel));
        }
        
        public static void RemoveTask(Category category)
        {
            Console.Clear();
            var tasks = AnsiConsole.Prompt(
                new MultiSelectionPrompt<string>()
                    .Title("Choose which tasks you want to be [red]deleted.[/]")
                    .NotRequired() // Not required to have a favorite fruit
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more tasks)[/]")
                    .InstructionsText(
                        "[grey](Press [blue]<space>[/] to toggle a task, " + 
                        "[green]<enter>[/] to accept)[/]")!
                    .AddChoices(category.Tasks.Select(task => task.ToString()).ToList()));
            
            var indexesToRemove = tasks
                .Select(selectedTask => category.Tasks.FindIndex(task => task.ToString() == selectedTask))
                .Where(index => index >= 0)
                .OrderByDescending(index => index)
                .ToList();

            foreach (var task in indexesToRemove.Select(category.GetSingleTask))
            {
                category.RemoveTask(task);
            }
        }
        
        public static void UpdateTaskPriority(Category category)
        {
            Console.Clear();
            // var updateTaskPriority = new Menu("Which task would you like to change the priority for?", category.Tasks.Select(task => task.ToString()).ToList());
            // var taskToUpdate = category.GetSingleTask(updateTaskPriority.Run());
            // Console.Clear();
            // updateTaskPriority = new Menu("Select the new priority", Enum.GetNames(typeof(PriorityLevelType)).ToList());
            // var priority = updateTaskPriority.Run();
            // var type = (PriorityLevelType)priority;
            //taskToUpdate.Priority = type;
        }
        
        public static void UpdateTaskDueDate(Category category)
        {
            Console.Clear();
            // var updateTaskDate = new Menu("Which task would you like to change the due date for?", category.Tasks.Select(task => task.ToString()).ToList());
            // var task = category.GetSingleTask(updateTaskDate.Run());
            // task.ChangeDueDate(UserInputUtilities.GetDate());
            // PrintToConsoleInColor($"Date has been changed", ConsoleColor.Green);
            Thread.Sleep(1500);
        }
        
        public static void MoveTask(Category category, List<Category> categories)
        {
            // if (categories.Count == 1)
            // {
            //     PrintToConsoleInColor("You only have one category", ConsoleColor.Red);
            //     Thread.Sleep(1500);
            //     return;
            // }
            //
            // // Get the task to move
            // Console.Clear();
            // var moveTask = new Menu("Which task would you like to move?", category.Tasks.Select(task => task.ToString()).ToList());
            // var taskToMove = category.GetSingleTask(moveTask.Run());
            //
            // Console.Clear();
            // // Get the index of category
            // moveTask = new Menu("Which category would you like to move the task to?", categories.Select(cat => cat.ToString()).ToList());
            //
            // // Loop if the user selects the same category
            // var categoryIndex = moveTask.Run();
            // while (categories[categoryIndex].Equals(category))
            // {
            //     PrintToConsoleInColor("You can't select the same category.", ConsoleColor.Red);
            //     categoryIndex = moveTask.Run();
            // }
            //
            // // Get the respective category from the category collection
            // var moveIntoCategory = categories[categoryIndex];
            // moveIntoCategory.AddTask(taskToMove);
            // category.RemoveTask(taskToMove);
            //
            // PrintToConsoleInColor($"Task was successfully moved into {moveIntoCategory.Title}", ConsoleColor.Green);
            // Thread.Sleep(2500);
        }
        public static DateTime GetDate()
        {
            DateTime date;
            while (true)
            {
                var input = AnsiConsole.Prompt(
                    new TextPrompt<string>("Enter a date (e.g., DD-MM-YYYY)")
                        .Validate(value => DateTime.TryParseExact(value, "dd-mm-yyyy", CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal, out _) ?
                            ValidationResult.Success() : ValidationResult.Error("[red]Invalid date format[/]")));

                if (DateTime.TryParseExact(input, "dd-mm-yyyy", CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal, out date))
                {
                    break;
                }
            }

            return date;
        }
    }
}
