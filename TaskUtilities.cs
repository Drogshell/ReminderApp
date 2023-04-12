using System.Globalization;
using Spectre.Console;

namespace ReminderApp
{
    public static class TaskUtilities
    {
        private const int Delay = 1000;
        public static void AddTask(Category category)
        {
            var description = AnsiConsole.Ask<string>("Enter the description of the task: ");
            TableUtilities.PrintTasks(category);
            var priorityLevel = PriorityLevel.GetPriorityLevel();
            var userDate = GetDate();
            category.AddTask(new Task(description, userDate, priorityLevel));
        }
        
        public static void RemoveTask(Category category)
        {
            Console.Clear();
            var tasks = PromptUtilities.PromptMultiSelection("Choose which tasks you want to be [red]deleted.[/]",
                category.Tasks.Select(task => task.ToString()).ToList(), false);
            
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
            var selectedTask = PromptUtilities.PromptSelection("Which task would you like to change the priority for?", taskStrings);
            var selectedIndex = taskStrings.IndexOf(selectedTask);
            var taskToUpdate = category.GetSingleTask(selectedIndex);
            
            Console.Clear();
            var newPriority = PromptUtilities.PromptSelection("Select the new priority",
                Enum.GetNames(typeof(PriorityLevelType)).ToList());
            
            var type = Enum.Parse<PriorityLevelType>(newPriority);
            taskToUpdate.Priority = type;
        }
        
        public static void UpdateTaskDueDate(Category category)
        {
            Console.Clear();
            var taskStrings = category.Tasks.Select(task => task.ToString()).ToList();
            var selectedTask = PromptUtilities.PromptSelection("Which task would you like to change the due date for?", taskStrings);
            
            var selectedIndex = taskStrings.IndexOf(selectedTask);
            var taskToUpdate = category.GetSingleTask(selectedIndex);
            var newDueDate = GetDate();
            taskToUpdate.ChangeDueDate(newDueDate);

            AnsiConsole.MarkupLine("[green]Date has been changed[/]");
            Thread.Sleep(Delay);
        }
        
        public static void MoveTask(Category category, List<Category> categories)
        {
            Console.Clear();
            if (categories.Count == 1)
            {
                AnsiConsole.MarkupLine("[bold red]You only have one category[/]");
                Thread.Sleep(Delay);
                return;
            }

            // Get the task to move
            Console.Clear();
            var taskStrings = category.Tasks.Select(task => task.ToString()).ToList();
            var selectedTask = PromptUtilities.PromptSelection("Which task would you like to move?", taskStrings);
            var selectedIndex = taskStrings.IndexOf(selectedTask);
            var taskToMove = category.GetSingleTask(selectedIndex);

            Console.Clear();
            // Get the index of category
            var categoryStrings = categories.Select(cat => cat.ToString()).ToList();
            var selectedCategory = PromptUtilities.PromptSelection("Which category would you like to move the task to?",
                categoryStrings);
            var categoryIndex = categoryStrings.IndexOf(selectedCategory);

            // Loop if the user selects the same category
            while (categories[categoryIndex].Equals(category))
            {
                AnsiConsole.MarkupLine("[red]You can't select the same category.[/]");
                selectedCategory = PromptUtilities.PromptSelection("Which category would you like to move the task to?",
                    categoryStrings);
                categoryIndex = categoryStrings.IndexOf(selectedCategory);
            }

            // Get the respective category from the category collection
            var moveIntoCategory = categories[categoryIndex];
            moveIntoCategory.AddTask(taskToMove);
            category.RemoveTask(taskToMove);

            AnsiConsole.MarkupLine($"[green]Task was successfully moved into {moveIntoCategory.Title}[/]");
            Thread.Sleep(Delay);
        }
        private static DateTime GetDate()
        {
            var promptTitle = new Rule("[bold blue]Task Due Date[/]").LeftJustified();
            
            var promptSubtitle = new Markup($"[grey62]Please enter a date and time in the format: [/][bold cyan]dd/MM/yyyy hh:mm am/pm[/][grey62].[/]").LeftJustified();

            AnsiConsole.Write(promptTitle);
            AnsiConsole.Write(promptSubtitle);
        
            var dateString = AnsiConsole.Ask<string>($"[bold green]\nEnter Date and Time[/]");
        
            while (true)
            {
                if (DateTime.TryParse(dateString, out var userDate))
                {
                    if (userDate >= DateTime.Now)
                    {
                        return userDate;
                    }
        
                    AnsiConsole.MarkupLine("[bold red]You can't set a due date in the past![/]");
                }
                else
                {
                    AnsiConsole.MarkupLine("[bold red]Not a valid date and time format! Please try again.[/]");
                }
        
                dateString = AnsiConsole.Ask<string>($"[bold green]Enter Date and Time[/]");
            }
        }
    }
}
