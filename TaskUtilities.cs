using System;
using System.Collections.Generic;
using System.Linq;

namespace ReminderApp
{
    public static class TaskUtilities
    {
        public static void AddTask(Category category)
        {
            var description = UserInputUtilities.ReadString("Enter the description of the task.");
            PriorityLevel.PrintPriorityLevels();
            var priority = UserInputUtilities.ReadInt("Enter the number associated with the priority.") - 1;
            var type = (PriorityLevelType)priority;
            var userDate = UserInputUtilities.GetDate();

            var taskToAdd = new CategoryTask(description, userDate, type);

            category.AddTask(taskToAdd);
        }
        
        public static void RemoveTask(Category category)
        {
            Console.Clear();
            var removeTaskMenu = new Menu("Which task would you like to remove?", category.Tasks.Select(task => task.ToString()).ToList());
            var taskToRemove = category.GetSingleTask(removeTaskMenu.Run());
            category.RemoveTask(taskToRemove);
        }
        
        public static void UpdateTaskPriority(Category category)
        {
            Console.Clear();
            var updateTaskPriority = new Menu("Which task would you like to change the priority for?", category.Tasks.Select(task => task.ToString()).ToList());
            var taskToUpdate = category.GetSingleTask(updateTaskPriority.Run());
            Console.Clear();
            updateTaskPriority = new Menu("Select the new priority", Enum.GetNames(typeof(PriorityLevelType)).ToList());
            var priority = updateTaskPriority.Run();
            var type = (PriorityLevelType)priority;
            taskToUpdate.Priority = type;
        }
        
        public static void UpdateTaskDueDate(Category category)
        {
            Console.Clear();
            var updateTaskDate = new Menu("Which task would you like to change the due date for?", category.Tasks.Select(task => task.ToString()).ToList());
            var task = category.GetSingleTask(updateTaskDate.Run());
            task.ChangeDueDate(UserInputUtilities.GetDate());
            PrintToConsoleInColor($"Date has been changed", ConsoleColor.Green);
            Thread.Sleep(1500);
        }
        
        public static void MoveTask(Category category, List<Category> categories)
        {
            if (categories.Count == 1)
            {
                PrintToConsoleInColor("You only have one category", ConsoleColor.Red);
                Thread.Sleep(1500);
                return;
            }

            // Get the task to move
            Console.Clear();
            var moveTask = new Menu("Which task would you like to move?", category.Tasks.Select(task => task.ToString()).ToList());
            var taskToMove = category.GetSingleTask(moveTask.Run());

            Console.Clear();
            // Get the index of category
            moveTask = new Menu("Which category would you like to move the task to?", categories.Select(cat => cat.ToString()).ToList());

            // Loop if the user selects the same category
            var categoryIndex = moveTask.Run();
            while (categories[categoryIndex].Equals(category))
            {
                PrintToConsoleInColor("You can't select the same category.", ConsoleColor.Red);
                categoryIndex = moveTask.Run();
            }

            // Get the respective category from the category collection
            var moveIntoCategory = categories[categoryIndex];
            moveIntoCategory.AddTask(taskToMove);
            category.RemoveTask(taskToMove);

            PrintToConsoleInColor($"Task was successfully moved into {moveIntoCategory.Title}", ConsoleColor.Green);
            Thread.Sleep(2500);
        }

        public static void PrintTasks(Category category)
        {
            Console.Clear();
            Console.WriteLine($"Tasks in {category.Title} category:");
            var tasks = category.Tasks;
            // Gets the largest string length in the task list to draw out the appropriate length of lines
            var largestString = tasks.Max(len => len.ToString().Length) + 3;
            
            Console.WriteLine("Tasks");
            for (var i = 0; i < tasks.Count; i++)
            {
                TableUtilities.PrintLine(largestString);
            
                switch (tasks[i].Priority)
                {
                    case PriorityLevelType.High:
                        PrintToConsoleInColor($"  {i + 1}) | " + tasks[i], ConsoleColor.Red);
                        break;
                    case PriorityLevelType.Medium:
                        PrintToConsoleInColor($"  {i + 1}) | " + tasks[i], ConsoleColor.Yellow);
                        break;
                    case PriorityLevelType.Low:
                    default:
                        PrintToConsoleInColor($"  {i + 1}) | " + tasks[i], ConsoleColor.Green);
                        break;
                }
            }
            TableUtilities.PrintLine(largestString);
        }

        public static void PrintToConsoleInColor(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
