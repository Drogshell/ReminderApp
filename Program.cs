using Spectre.Console;

namespace ReminderApp
{
    class ReminderApp
    {
        private static readonly List<Category> Categories = new List<Category>();
        private static readonly List<string> TaskOptions = new List<string>
            { "Add Task", "Remove Task", "Change Task Priority", "Change Task Due Date", "Rename Task" ,"Move Task", "Go Back" };
        private static readonly List<string> CategoryOptions = new List<string>
            { "Select Category", "Add Category", "Remove Category", "Rename Category", "Quit" };

        private static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                if (Categories.Count == 0)
                {
                    PrintFreshStartMenu();
                }
                else
                {
                    PrintCategories();
                }
            }
        }

        private static void PrintFreshStartMenu()
        {
            Console.Clear();
            var freshStartChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Hello! Please add some tasks to get started")
                    .PageSize(10)
                    .AddChoices(new[] {
                        "Add Category", "Quit",
                    }));

            switch (freshStartChoice)
            {
                case "Add Category":
                    AddCategory();
                    break;
                case "Quit":
                    ExitApplication();
                    break;
            }
        }

        private static void PrintCategories()
        {
            Console.Clear();
            TableUtilities.PrintCategory(Categories);

            var userChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Categories")
                    .PageSize(10)
                    .AddChoices(CategoryOptions));
            
            switch (userChoice)
            {
                case "Select Category":
                    SelectCategory();
                    break;
                case "Add Category":
                    AddCategory();
                    break;
                case "Remove Category":
                    RemoveCategory();
                    break;
                case "Rename Category":
                    RenameCategory();
                    break;
                case "Quit":
                    ExitApplication();
                    break;
            }
        }
        
        private static void AddCategory()
        {
            CategoryUtilities.AddCategory(Categories);
        }

        private static void SelectCategory()
        {
            var selectedCategory = CategoryUtilities.SelectCategory(Categories);
            DisplayTaskMenuOptions(selectedCategory);
        }

        private static void RemoveCategory()
        {
            CategoryUtilities.RemoveCategory(Categories);
        }

        private static void RenameCategory()
        {
            CategoryUtilities.RenameCategory(Categories);
        }

        /// <summary>
        /// Displays the Inner Menu options to manage tasks.
        /// </summary>
        /// <param name="category"> The specific category with the tasks to manage and display. </param>
        static void DisplayTaskMenuOptions(Category category)
        {
            while (true)
            {
                if (category.Tasks.Count == 0)
                {
                    Console.Clear();
                    AnsiConsole.MarkupLine("[bold yellow on black]NO TASKS TO DISPLAY![/]");
                    var miniTaskOptions = new List<string> { TaskOptions[0], TaskOptions[5] };
                    var userChoice = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("Get Started by adding some tasks")
                            .PageSize(10)
                            .AddChoices(miniTaskOptions));
                    
                    switch (userChoice)
                    {
                        case "Add Task":
                            AddTask(category);
                            break;
                        case "Go Back":
                            Console.WriteLine("< < < Going back");
                            Thread.Sleep(1500);
                            break;
                    }
                }
                else
                {
                    PrintTasks(category);
                    var userChoice = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("Current Tasks")
                            .PageSize(10)
                            .AddChoices(TaskOptions));
                    
                    switch (userChoice)
                    {
                        //Add a new task
                        case "Add Task":
                            AddTask(category);
                            break;
                        //Remove a task
                        case "Remove Task":
                            RemoveTask(category);
                            break;
                        //Change the priority of a task
                        case "Change Task Priority":
                            ChangeTaskPriority(category);
                            break;
                        //Change the due date of a task
                        case "Change Task Due Date":
                            ChangeTaskDueDate(category);
                            break;
                        //rename Task
                        case "Rename Task":
                            Console.Clear();
                            
                            var userRenameChoice = AnsiConsole.Prompt(
                                new SelectionPrompt<CategoryTask>()
                                    .Title("Which task would you like to re-name?")
                                    .PageSize(10)
                                    .AddChoices(category.GetAllTasks()));
                            
                            var newDescription = AnsiConsole.Ask<string>("Enter new description");
                            RenameTask(newDescription, userRenameChoice);
                            break;
                        //Move a task
                        case "Move Task":
                            MoveTask(category);
                            break;
                        case "Go Back":
                            AnsiConsole.MarkupLine("[green]< < < Going back[/]");
                            Thread.Sleep(1500);
                            break;
                    }
                }
                break;
            }
        }

        private static void RenameTask(string readString, CategoryTask task)
        {
            if (!task.RenameTask(readString)) return;
            AnsiConsole.MarkupLine("[green]Success![/]");
            Thread.Sleep(1500);
        }

        private static void AddTask(Category category)
        {
            TaskUtilities.AddTask(category);
        }
        private static void RemoveTask(Category category)
        {
            TaskUtilities.RemoveTask(category);
        }
        
        private static void ChangeTaskPriority(Category category)
        {
            TaskUtilities.UpdateTaskPriority(category);
        }
        
        private static void ChangeTaskDueDate(Category category)
        {
            TaskUtilities.UpdateTaskDueDate(category);
        }
        
        private static void MoveTask(Category category)
        {
            TaskUtilities.MoveTask(category, Categories);
        }
        
        private static void PrintTasks(Category category)
        {
            TableUtilities.PrintTasks(category);
        }
        
        private static void ExitApplication()
        {
            Console.WriteLine("\nQuitting...\n");
            Environment.Exit(0);
        }
    }
}