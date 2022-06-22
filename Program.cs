using System.Text;

namespace ReminderApp
{
    class ReminderApp
    {
        private static readonly List<string> CategoryOptions = new List<string> { "Select Category", "Add Category", "Remove Category", "Rename Category", "Quit" };
        private static readonly List<string> TaskOptions = new List<string> { "Add Task", "Remove Task", "Change Priority", "Change Due Date", "Move Task to a Different Category", "Go Back" };
        private static readonly List<Category> Categories = new();
        private static int _tableWidth = 150;

        static void Main()
        {
            Console.Title = "Tasker";
            Console.CursorVisible = false;
            Console.SetWindowSize(180, 40);

            bool exit = false;

            while (!exit)
            {
                exit = PrintMenu();
            }
        }

        /// <summary>
        /// The main menu to be displayed on the console.
        /// </summary>
        static bool PrintMenu()
        {
            Menu mainMenu;
            // First, display the categories to the console if any exist.
            if (DisplayCategories())
            {
                mainMenu = new Menu("Welcome", CategoryOptions);
                mainMenu.SetCursorPosition(0, 10);
                var menuOption = mainMenu.Run();

                // Now we switch the users choice based on the valid cases.
                switch (menuOption)
                {
                    // Case to select a category to whose tasks are to be displayed. 
                    case 0:
                        Console.Clear();
                        mainMenu = new Menu(@"
 __          ___     _      _                 _                                                            
 \ \        / / |   (_)    | |               | |                                                           
  \ \  /\  / /| |__  _  ___| |__     ___ __ _| |_ ___  __ _  ___  _ __ _   _                               
   \ \/  \/ / | '_ \| |/ __| '_ \   / __/ _` | __/ _ \/ _` |/ _ \| '__| | | |                              
    \  /\  /  | | | | | (__| | | | | (_| (_| | ||  __/ (_| | (_) | |  | |_| |                              
     \/  \/   |_| |_|_|\___|_| |_|  \___\__,_|\__\___|\__, |\___/|_|   \__, |                              
                                                       __/ |            __/ |                              
                      _     _                       _ |___/        _   |___/           _           _  ___  
                     | |   | |                     | (_) |        | |                 | |         | ||__ \ 
 __      _____  _   _| | __| |  _   _  ___  _   _  | |_| | _____  | |_ ___    ___  ___| | ___  ___| |_  ) |
 \ \ /\ / / _ \| | | | |/ _` | | | | |/ _ \| | | | | | | |/ / _ \ | __/ _ \  / __|/ _ \ |/ _ \/ __| __|/ / 
  \ V  V / (_) | |_| | | (_| | | |_| | (_) | |_| | | | |   <  __/ | || (_) | \__ \  __/ |  __/ (__| |_|_|  
   \_/\_/ \___/ \__,_|_|\__,_|  \__, |\___/ \__,_| |_|_|_|\_\___|  \__\___/  |___/\___|_|\___|\___|\__(_)  
                                 __/ |                                                                     
                                |___/                                                                      
", Categories.Select(category => category.ToString()).ToList());
                        Category category = Categories[mainMenu.Run()];
                        // Display the inner menu for that specific category.
                        DisplayTaskMenuOptions(category);
                        return false;
                    // Case to add a category
                    case 1:
                        var categoryDescription = UserInputUtilities.ReadString("What would you like to call the new category?");
                        Console.CursorVisible = true;

                        Categories.Add(new Category(categoryDescription));
                        Console.Clear();
                        return false;
                    // Case to remove a specific category
                    case 2:
                        mainMenu = new Menu("Which category would you like to remove?", Categories.Select(category => category.ToString()).ToList());
                        Console.Clear();
                        Categories.RemoveAt(mainMenu.Run());
                        return false;
                    // Case to rename a specific category
                    case 3:
                        mainMenu = new Menu("Which category do you want to rename?", Categories.Select(category => category.ToString()).ToList());
                        Console.Clear();
                        Categories[mainMenu.Run()].RenameTask(UserInputUtilities.ReadString("What would you like to rename the task to?"));
                        return false;
                    case 4:
                        Console.WriteLine("\nQuitting...\n");
                        return true;
                }
            }
            // If there are no categories to display, a minimised menu is displayed with only 2 options
            var miniOptions = new List<string>();
            miniOptions.Add(CategoryOptions[1]);
            miniOptions.Add(CategoryOptions[4]);
            mainMenu = new Menu("Add some categories to get started...", miniOptions);
            mainMenu.SetCursorPosition(0, 10);
            var userOption = mainMenu.Run();

            switch (userOption)
            {
                case 0:
                    var categoryDescription = UserInputUtilities.ReadString("What would you like to call the new category?");
                    Categories.Add(new Category(categoryDescription));
                    return false;
                case 1:
                    Console.WriteLine("\nQuitting...\n");
                    return true;
                default:
                    return true;
            }
        }

        /// <summary>
        /// Displays all the categories in the Categories List. 
        /// </summary>
        static bool DisplayCategories()
        {
            // If there are no categories there is nothing to display
            if (Categories.Count == 0)
            {
                Console.WriteLine(@"
  _   _                   _                        _             _              _ _           _             _ 
 | \ | |                 | |                      (_)           | |            | (_)         | |           | |
 |  \| | ___     ___ __ _| |_ ___  __ _  ___  _ __ _  ___  ___  | |_ ___     __| |_ ___ _ __ | | __ _ _   _| |
 | . ` |/ _ \   / __/ _` | __/ _ \/ _` |/ _ \| '__| |/ _ \/ __| | __/ _ \   / _` | / __| '_ \| |/ _` | | | | |
 | |\  | (_) | | (_| (_| | ||  __/ (_| | (_) | |  | |  __/\__ \ | || (_) | | (_| | \__ \ |_) | | (_| | |_| |_|
 |_| \_|\___/   \___\__,_|\__\___|\__, |\___/|_|  |_|\___||___/  \__\___/   \__,_|_|___/ .__/|_|\__,_|\__, (_)
                                   __/ |                                               | |             __/ |  
                                  |___/                                                |_|            |___/   
");
                return false;
            }

            var row = new StringBuilder();
            // Check if table width needs to grow
            if (Categories.Count > 3)
            {
                _tableWidth = 250;
                Console.SetWindowSize(260, 40);
            }
            // Set the console color to blue and display all the categories.   
            Console.Clear();
            PrintToConsoleInColor("CATEGORIES", ConsoleColor.Blue);
            PrintLine(_tableWidth);

            // Print the category...
            PrintCategory();
            // Then print a line 
            PrintLine(_tableWidth);

            int width = (_tableWidth - Categories.Count) / Categories.Count;
            row.Append('|');

            for (int i = 0; i < Categories.Count; i++)
            {
                var taskCount = Categories[i].Tasks.Count;
                if (taskCount == 1)
                {
                    row.Append(AlignToCentre($"{taskCount} task in category", width) + '|');
                }
                else
                {
                    row.Append(AlignToCentre($"{taskCount} tasks in category", width) + '|');
                }
            }

            Console.WriteLine(row);

            PrintLine(_tableWidth);

            row.Clear();
            row.Append('|');

            int dueSoon = 0;
            int overDue = 0;

            // Go into the list of categories...
            for (int i = 0; i < Categories.Count; i++)
            {
                // Get all the tasks for that respective category...
                var tasks = Categories[i].Tasks;

                // Iterate through the task list and see which are due soon and which are overdue
                for (int j = 0; j < tasks.Count; j++)
                {
                    if (tasks[j].TimeSpan.Days <= 3 && tasks[j].TimeSpan.Days >= 0 && tasks[j].TimeSpan.Hours <= 24 && tasks[j].TimeSpan.Hours >= 0 && tasks[j].TimeSpan.Minutes <= 60 && tasks[j].TimeSpan.Minutes >= 0)
                    {
                        dueSoon++;
                    }
                    if (tasks[j].TimeSpan.Days <= 0 && tasks[j].TimeSpan.Hours <= 0 && tasks[j].TimeSpan.Minutes <= 0)
                    {
                        overDue++;
                    }
                }

                var built = new StringBuilder();

                // If there is only one task due soon append "task" otherwise append "tasks"
                built.Append((dueSoon == 1) ? $"{dueSoon} task is due soon. " : $"{dueSoon} tasks are due soon. ");

                // If there is only one task over due append "task" otherwise append "tasks"
                built.Append((overDue == 1) ? $"{overDue} task is over due. " : $"{overDue} tasks are over due. ");

                // Append the final string in the centre of the table
                row.Append(AlignToCentre(built.ToString(), width) + '|');

                // Reset the due soon and over due variables for the next category 
                dueSoon = 0;
                overDue = 0;
            }

            Console.WriteLine(row);
            Console.ResetColor();
            return true;
        }

        // The code below was found on stack overflow and adapted to fit this project. Credit goes to "Patrick McDonald"
        // https://stackoverflow.com/questions/856845/how-to-best-way-to-draw-table-in-console-app-c
        #region Table Printing

        /// <summary>
        /// Prints a line to the console, the lines length is based on the table width.
        /// </summary>
        static void PrintLine(int width)
        {
            Console.WriteLine(new string('-', width));
        }

        /// <summary>
        /// Responsible for printing the categories in the Categories List. 
        /// </summary>
        static void PrintCategory()
        {
            int width = (_tableWidth - Categories.Count) / Categories.Count;
            var row = new StringBuilder();
            row.Append('|');

            for (int i = 0; i < Categories.Count; i++)
            {
                row.Append(AlignToCentre(Categories[i].Title, width) + "|");
            }

            Console.WriteLine(row);
        }

        /// <summary>
        /// Responsible for aligning the text into the center of the “Box”. If the text is too long the end of the text is replaced with three dots.
        /// </summary>
        /// <param name="text"> The text to be aligned. </param>
        /// <param name="width"> The width of the “Box” that the text is to be placed in. </param>
        /// <returns> A string that has been concatenated if it's too long and centered based on the width </returns>
        static string AlignToCentre(string text, int width)
        {
            // The text to be centered into the "Box"
            text = text.Length > width ? string.Concat(text.AsSpan(0, width - 3), "...") : text;

            return (string.IsNullOrEmpty(text)) ? new string(' ', width) : text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
        }

        #endregion

        /// <summary>
        /// Displays the Inner Menu options to manage tasks.
        /// </summary>
        /// <param name="category"> The specific category with the tasks to manage and display. </param>
        static void DisplayTaskMenuOptions(Category category)
        {
            Menu taskMenu;
            if (category.Tasks.Count == 0)
            {
                Console.Clear();
                PrintToConsoleInColor(@"
  _______ _           _               _                                _                               _            _        
 |__   __| |         | |             | |                              | |                             | |          | |       
    | |  | |__   __ _| |_    ___ __ _| |_ ___  __ _  ___  _ __ _   _  | |__   __ _ ___   _ __   ___   | |_ __ _ ___| | _____ 
    | |  | '_ \ / _` | __|  / __/ _` | __/ _ \/ _` |/ _ \| '__| | | | | '_ \ / _` / __| | '_ \ / _ \  | __/ _` / __| |/ / __|
    | |  | | | | (_| | |_  | (_| (_| | ||  __/ (_| | (_) | |  | |_| | | | | | (_| \__ \ | | | | (_) | | || (_| \__ \   <\__ \
    |_|  |_| |_|\__,_|\__|  \___\__,_|\__\___|\__, |\___/|_|   \__, | |_| |_|\__,_|___/ |_| |_|\___/   \__\__,_|___/_|\_\___/
                                               __/ |            __/ |                                                        
                                              |___/            |___/                                                         
", ConsoleColor.Red);

                var miniTaskOptions = new List<string>();
                miniTaskOptions.Add(TaskOptions[0]);
                miniTaskOptions.Add(TaskOptions[5]);
                taskMenu = new Menu("Tasks", miniTaskOptions);
                taskMenu.SetCursorPosition(0, 10);
                var userchoice = taskMenu.Run();

                switch (userchoice)
                {
                    case 0:
                        AddTask(category);
                        break;
                    case 1:
                        Console.WriteLine("\nQuitting...\n");
                        break;
                }
            }
            else
            {
                PrintTasks(category);

                taskMenu = new Menu("Tasks", TaskOptions);
                taskMenu.SetCursorPosition(0, (category.Tasks.Count + 2) * 2);
                var userChoice = taskMenu.Run();

                switch (userChoice)
                {
                    //Add a new task
                    case 0:
                        AddTask(category);
                        DisplayTaskMenuOptions(category);
                        break;
                    //Remove a task
                    case 1:
                        Console.Clear();
                        var removeTask = new Menu("Which task would you like to remove?", category.Tasks.Select(task => task.ToString()).ToList());
                        var taskToRemove = category.GetSingleTask(removeTask.Run());
                        category.RemoveTask(taskToRemove);
                        break;
                    //Change the priority of a task
                    case 2:
                        Console.Clear();
                        var updateTaskPriority = new Menu("Which task would you like to change the priority for?", category.Tasks.Select(task => task.ToString()).ToList());
                        var taskToUpdate = category.GetSingleTask(updateTaskPriority.Run());
                        Console.Clear();
                        updateTaskPriority = new Menu("Select the new priority", Enum.GetNames(typeof(PriorityLevelType)).ToList());
                        updateTaskPriority.SetCursorPosition(0, 8);
                        int priority = updateTaskPriority.Run();
                        var type = (PriorityLevelType)priority;
                        taskToUpdate.Priority = type;
                        break;
                    //Change the due date of a task
                    case 3:
                        Console.Clear();
                        var updateTaskDate = new Menu("Which task would you like to change the due date for?", category.Tasks.Select(task => task.ToString()).ToList());
                        var task = category.GetSingleTask(updateTaskDate.Run());
                        task.ChangeDueDate(UserInputUtilities.GetDate());
                        PrintToConsoleInColor($"Date has been changed", ConsoleColor.Green);
                        Thread.Sleep(1500);
                        break;
                    //Move a task
                    case 4:
                        if (Categories.Count == 1)
                        {
                            PrintToConsoleInColor("You only have one category", ConsoleColor.Red);
                            Thread.Sleep(1500);
                            break;
                        }
                        // Get the task to move
                        Console.Clear();
                        var moveTask = new Menu("Which task would you like to move?", category.Tasks.Select(task => task.ToString()).ToList());
                        task = category.GetSingleTask(moveTask.Run());

                        Console.Clear();
                        // Get the index of category
                        moveTask = new Menu("Which category would you like to move the task to?", Categories.Select(category => category.ToString()).ToList());

                        // Loop if the user selects the same category
                        var categoryIndex = moveTask.Run();
                        while (Categories[categoryIndex].Equals(category))
                        {
                            PrintToConsoleInColor("You can't select the same category.", ConsoleColor.Red);
                            categoryIndex = moveTask.Run();
                        }

                        // Get the respective category from the category collection
                        var moveIntoCategory = Categories[categoryIndex];
                        moveIntoCategory.AddTask(task);
                        category.RemoveTask(task);

                        PrintToConsoleInColor($"Task was successfully moved into {moveIntoCategory}", ConsoleColor.Green);
                        Thread.Sleep(2500);
                        break;
                    case 5:
                        break;
                }
            }
        }

        private static void AddTask(Category category)
        {
            var description = UserInputUtilities.ReadString("Enter the description of the task.");
            PriorityLevel.PrintPriorityLevels();
            int priority = UserInputUtilities.ReadInt("Enter the number associated with the priority.");
            var type = (PriorityLevelType)priority;
            var userDate = UserInputUtilities.GetDate();

            var taskToAdd = new CategoryTask(description, userDate, type);

            category.AddTask(taskToAdd);
        }

        private static void PrintTasks(Category category)
        {
            Console.Clear();
            var tasks = category.Tasks;

            // Get's the largest string length in the task list to draw out the appropriate length of lines
            var largestString = tasks.Max(len => len.ToString().Length) + 3;

            Console.WriteLine("Tasks");
            for (int i = 0; i < tasks.Count; i++)
            {
                PrintLine(largestString + 10);

                if (tasks[i].Priority == PriorityLevelType.High)
                {
                    PrintToConsoleInColor($"  {i + 1}) | " + tasks[i], ConsoleColor.Red);
                }
                else if (tasks[i].Priority == PriorityLevelType.Medium)
                {
                    PrintToConsoleInColor($"  {i + 1}) | " + tasks[i], ConsoleColor.Yellow);
                }
                else
                {
                    PrintToConsoleInColor($"  {i + 1}) | " + tasks[i], ConsoleColor.Green);
                }
            }
            PrintLine(largestString + 10);
        }

        private static void PrintToConsoleInColor(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    
    }
}