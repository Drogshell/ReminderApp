using System.Text;

namespace ReminderApp
{
    internal static class ReminderApp
    {
        private static readonly List<Category> Categories = new List<Category>();
        private static readonly List<string> TaskOptions = new List<string>
            { "Add Task", "Remove Task", "Change Task Priority", "Change Task Due Date", "Move Task", "Go Back" };
        private static readonly List<string> CategoryOptions = new List<string>
            { "Select Category", "Add Category", "Remove Category", "Rename Category", "Quit" };
        private static int _tableWidth;
        
        static void Main(string[] args)
        {
            while (true)
            {
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
            var freshStartOptions = new List<string> { "Add Category", "Quit" };
            var freshStartMenu = new Menu("Welcome to Task Manager! Please add a category to get started.",
                freshStartOptions);
            var freshStartChoice = freshStartMenu.Run();

            switch (freshStartChoice)
            {
                case 0:
                    AddCategory();
                    break;
                case 1:
                    ExitApplication();
                    break;
            }
        }

        private static void PrintCategories()
        {
            Console.Clear();
            _tableWidth = TableUtilities.GetConsoleTableWidth(Categories);

            TableUtilities.PrintLine(_tableWidth);
            TableUtilities.PrintCategory(Categories, _tableWidth);
            TableUtilities.PrintLine(_tableWidth);

            var categoryMenu = new Menu("Categories", CategoryOptions);
            var userChoice = categoryMenu.Run();

            switch (userChoice)
            {
                case 0:
                    SelectCategory();
                    break;
                case 1:
                    AddCategory();
                    break;
                case 2:
                    RemoveCategory();
                    break;
                case 3:
                    RenameCategory();
                    break;
                case 4:
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
                Menu taskMenu;
                if (category.Tasks.Count == 0)
                {
                    Console.Clear();
                    TaskUtilities.PrintToConsoleInColor(@"That category has no tasks", ConsoleColor.Red);

                    var miniTaskOptions = new List<string> { TaskOptions[0], TaskOptions[5] };

                    var cursorTopOffset = (category.Tasks.Count + 2) * 2;
                    taskMenu = new Menu("Tasks", miniTaskOptions, cursorTopOffset);
                    var userChoice = taskMenu.Run();
                    switch (userChoice)
                    {
                        case 0:
                            AddTask(category);
                            break;
                        case 1:
                            Console.WriteLine("< < < Going back");
                            Thread.Sleep(1500);
                            break;
                    }
                }
                else
                {
                    PrintTasks(category);
                    var cursorTopOffset = (category.Tasks.Count + 2) * 2;
                    taskMenu = new Menu("Tasks", TaskOptions, cursorTopOffset);
                    var userChoice = taskMenu.Run();
                    switch (userChoice)
                    {
                        //Add a new task
                        case 0:
                            AddTask(category);
                            break;
                        //Remove a task
                        case 1:
                            RemoveTask(category);
                            break;
                        //Change the priority of a task
                        case 2:
                            ChangeTaskPriority(category);
                            break;
                        //Change the due date of a task
                        case 3:
                            ChangeTaskDueDate(category);
                            break;
                        //Move a task
                        case 4:
                            MoveTask(category);
                            break;
                    }
                }

                break;
            }
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
            TaskUtilities.PrintTasks(category);
        }
        
        private static void ExitApplication()
        {
            Console.WriteLine("\nQuitting...\n");
            Environment.Exit(0);
        }
    }
}
