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
            
            var taskStrings = category.Tasks.Select(task => task.ToString()).ToList();

            var selectedTask = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Which task would you like to change the priority for?")
                    .PageSize(10)
                    .AddChoices(taskStrings));

            int selectedIndex = taskStrings.IndexOf(selectedTask);
            var taskToUpdate = category.GetSingleTask(selectedIndex);

            Console.Clear();
    
            var newPriority = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select the new priority")
                    .PageSize(10)
                    .AddChoices(Enum.GetNames(typeof(PriorityLevelType)).ToList()));

            var type = Enum.Parse<PriorityLevelType>(newPriority);
            taskToUpdate.Priority = type;

        }
        
        public static void UpdateTaskDueDate(Category category)
        {
            Console.Clear();

            var taskStrings = category.Tasks.Select(task => task.ToString()).ToList();
            var selectedTask = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Which task would you like to change the due date for?")
                    .PageSize(10)
                    .AddChoices(taskStrings));

            int selectedIndex = taskStrings.IndexOf(selectedTask);
            var taskToUpdate = category.GetSingleTask(selectedIndex);

            var newDueDate = AnsiConsole.Ask<DateTime>("Enter the new due date (yyyy-mm-dd):");
            taskToUpdate.ChangeDueDate(newDueDate);

            AnsiConsole.MarkupLine("[green]Date has been changed[/]");
            Thread.Sleep(1500);
        }
        
        public static void MoveTask(Category category, List<Category> categories)
        {
            if (categories.Count == 1)
            {
                AnsiConsole.MarkupLine("[red]You only have one category[/]");
                Thread.Sleep(1500);
                return;
            }

            // Get the task to move
            Console.Clear();
            var taskStrings = category.Tasks.Select(task => task.ToString()).ToList();
            var selectedTask = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Which task would you like to move?")
                    .PageSize(10)
                    .AddChoices(taskStrings));

            int selectedIndex = taskStrings.IndexOf(selectedTask);
            var taskToMove = category.GetSingleTask(selectedIndex);

            Console.Clear();
            // Get the index of category
            var categoryStrings = categories.Select(cat => cat.ToString()).ToList();
            var selectedCategory = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Which category would you like to move the task to?")
                    .PageSize(10)
                    .AddChoices(categoryStrings));

            int categoryIndex = categoryStrings.IndexOf(selectedCategory);

            // Loop if the user selects the same category
            while (categories[categoryIndex].Equals(category))
            {
                AnsiConsole.MarkupLine("[red]You can't select the same category.[/]");
                selectedCategory = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Which category would you like to move the task to?")
                        .PageSize(10)
                        .AddChoices(categoryStrings));
                categoryIndex = categoryStrings.IndexOf(selectedCategory);
            }

            // Get the respective category from the category collection
            var moveIntoCategory = categories[categoryIndex];
            moveIntoCategory.AddTask(taskToMove);
            category.RemoveTask(taskToMove);

            AnsiConsole.MarkupLine($"[green]Task was successfully moved into {moveIntoCategory.Title}[/]");
            Thread.Sleep(2500);
        }

        private static DateTime GetDate()
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
