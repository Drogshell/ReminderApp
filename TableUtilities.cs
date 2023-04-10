using System;
using System.Collections.Generic;
using System.Text;

namespace ReminderApp
{
    // The code below was found on stack overflow and adapted to fit this project. Credit goes to "Patrick McDonald"
    // https://stackoverflow.com/questions/856845/how-to-best-way-to-draw-table-in-console-app-c
    public static class TableUtilities
    {
        /// <summary>
        /// Prints a line to the console, the lines length is based on the table width.
        /// </summary>
        /// <param name="width">The width of the table.</param>
        public static void PrintLine(int width)
        {
            Console.WriteLine(new string('-', width));
        }

        /// <summary>
        /// Responsible for printing the categories in the Categories List.
        /// </summary>
        /// <param name="categories">The list of categories to print.</param>
        /// <param name="tableWidth">The width of the table.</param>
        public static void PrintCategory(List<Category> categories, int tableWidth)
        {
            int width = (tableWidth - categories.Count) / categories.Count;
            var row = new StringBuilder();
            row.Append('|');

            foreach (var t in categories)
            {
                row.Append(AlignToCentre(t.Title, width) + "|");
            }

            Console.WriteLine(row);
        }

        /// <summary>
        /// Responsible for aligning the text into the center of the “Box”. If the text is too long the end of the text is replaced with three dots.
        /// </summary>
        /// <param name="text"> The text to be aligned. </param>
        /// <param name="width"> The width of the “Box” that the text is to be placed in. </param>
        /// <returns> A string that has been concatenated if it's too long and centered based on the width </returns>
        public static string AlignToCentre(string text, int width)
        {
            // The text to be centered into the "Box"
            text = text.Length > width ? string.Concat(text.AsSpan(0, width - 3), "...") : text;

            return (string.IsNullOrEmpty(text)) ? new string(' ', width) : text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
        }
        
        public static string Colorize(string text, ConsoleColor color)
        {
            return $"\x1b[38;5;{(int)color}m{text}\x1b[0m";
        }
        
        public static int GetConsoleTableWidth(List<Category> categories)
        {
            int totalWidth = 0;

            foreach (var category in categories)
            {
                // Add the width of each category title plus 3 (for padding and the vertical separator)
                int categoryWidth = category.Title.Length + 10;

                // Keep track of the maximum width of each column
                totalWidth = Math.Max(totalWidth, categoryWidth);
            }

            // Multiply the maximum column width by the number of categories to get the total table width
            return totalWidth * categories.Count;
        }
    }
}