using System;
using System.Collections.Generic;
using System.Linq;

namespace ReminderApp
{
    public static class CategoryUtilities
    {
        public static void AddCategory(List<Category> categories)
        {
            Console.Clear();
            var categoryName = UserInputUtilities.ReadString("Enter a name for the new category:");
            categories.Add(new Category(categoryName));
        }

        public static Category SelectCategory(List<Category> categories)
        {
            Console.Clear();
            var selectCategoryMenu = new Menu("Select a category to manage tasks",
                categories.Select(cat => cat.Title).ToList());
            var selectedIndex = selectCategoryMenu.Run();
            return categories[selectedIndex];
        }

        public static void RemoveCategory(List<Category> categories)
        {
            Console.Clear();
            var removeCategoryMenu =
                new Menu("Select a category to remove", categories.Select(cat => cat.Title).ToList());
            var selectedIndex = removeCategoryMenu.Run();
            categories.RemoveAt(selectedIndex);

            Console.WriteLine("Category removed successfully.");
            Thread.Sleep(1500);
        }

        public static void RenameCategory(List<Category> categories)
        {
            Console.Clear();
            var renameCategoryMenu =
                new Menu("Select a category to rename", categories.Select(cat => cat.Title).ToList());
            var selectedIndex = renameCategoryMenu.Run();

            var newName = UserInputUtilities.ReadString("Enter the new name for the category:");
            categories[selectedIndex].Title = newName;

            Console.WriteLine("Category renamed successfully.");
            Thread.Sleep(1500);
        }
    }
}