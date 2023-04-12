using Spectre.Console;

namespace ReminderApp
{
    class ReminderApp
    {
        private const int Delay = 1000;
        private static readonly List<Category> Categories = new List<Category>();
        private static readonly List<string> TaskOptions = new List<string>
            { "Add Task", "Remove Task", "Change Task Priority", "Change Task Due Date", "Rename Task" ,"Move Task", "Go Back" };
        private static readonly List<string> CategoryOptions = new List<string>
            { "Select Category", "Add Category", "Remove Category", "Rename Category", "Quit" };
        
        private static void Main(string[] args)
        {
            //------------------------Seed data-----------------------------//
            var work = new Category("Work");
            work.AddTask(new Task("Finish project proposal",
                new DateTime(2023, 4, 11), PriorityLevelType.High));
            work.AddTask(new Task("Update meeting agenda", new DateTime(2023, 4, 12), PriorityLevelType.Medium));
            work.AddTask(new Task("Email client about progress", new DateTime(2023, 4, 13),
                PriorityLevelType.Low));

            var personal = new Category("Personal");
            personal.AddTask(new Task("Schedule dentist appointment", new DateTime(2023, 4, 14),
                PriorityLevelType.Low));
            personal.AddTask(new Task("Buy birthday gift for friend", new DateTime(2023, 4, 16),
                PriorityLevelType.Medium));
            personal.AddTask(new Task("Book flight tickets for vacation", new DateTime(2023, 4, 20),
                PriorityLevelType.High));

            var fitness = new Category("Fitness");
            fitness.AddTask(new Task("Join a yoga class", new DateTime(2023, 4, 22), PriorityLevelType.Medium));
            fitness.AddTask(new Task("Buy new running shoes", new DateTime(2023, 4, 25),
                PriorityLevelType.Low));

            var empty = new Category("Empty Category");
            
            Categories.Add(work);
            Categories.Add(personal);
            Categories.Add(fitness);
            Categories.Add(empty);
            //----------------------------------------------------------------//
            
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
            var miniCategoryOptions = new List<string> { CategoryOptions[1], CategoryOptions[4] };
            var freshStartChoice =
                PromptUtilities.PromptSelection("Hello! Please add some tasks to get started", miniCategoryOptions);
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
            var userChoice = PromptUtilities.PromptSelection("Hello!", CategoryOptions);
               
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
                    AnsiConsole.MarkupLine("[bold red]NO TASKS TO DISPLAY![/]");
                    var miniTaskOptions = new List<string> { TaskOptions[0], TaskOptions[6] };
                    var userChoice =
                        PromptUtilities.PromptSelection("Get Started by adding some tasks", miniTaskOptions);
                    
                    switch (userChoice)
                    {
                        case "Add Task":
                            AddTask(category);
                            break;
                        case "Go Back":
                            Console.WriteLine("< < < Going back");
                            Thread.Sleep(Delay);
                            break;
                    }
                }
                else
                {
                    PrintTasks(category);
                    var userChoice = PromptUtilities.PromptSelection("Select a task", TaskOptions);

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

                            var userRenameChoice =
                                PromptUtilities.PromptSelection("Which task would you like to re-name?", category.GetAllTasks());
                            var newDescription = AnsiConsole.Ask<string>("Enter new description");
                            RenameTask(newDescription, userRenameChoice);
                            break;
                        //Move a task
                        case "Move Task":
                            MoveTask(category);
                            break;
                        case "Go Back":
                            AnsiConsole.MarkupLine("[green]< < < Going back[/]");
                            Thread.Sleep(Delay);
                            break;
                    }
                }
                break;
            }
        }

        private static void RenameTask(string readString, Task task)
        {
            if (!task.RenameTask(readString)) return;
            AnsiConsole.MarkupLine("[green]Success![/]");
            Thread.Sleep(Delay);
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