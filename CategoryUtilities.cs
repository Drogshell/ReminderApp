using Spectre.Console;

namespace ReminderApp
{
    public static class CategoryUtilities
    {
        private const int Delay = 1000;

        public static void AddCategory(List<Category> categories)
        {
            Console.Clear();
            var categoryName = AnsiConsole.Ask<string>("Enter a name for the new category:");
            categories.Add(new Category(categoryName));
        }

        public static Category SelectCategory(List<Category> categories)
        {
            Console.Clear();
            AnsiConsole.Write(new Rule("[grey]─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─[/]").RuleStyle("grey"));
            var selectedIndex = AnsiConsole.Prompt(
                    new SelectionPrompt<(string, int)>()
                        .Title("Select a category")
                        .PageSize(10)
                        .AddChoices(categories.Select((cat, index) => (cat.Title, index)))
                        .UseConverter(t => t.Item1)).Item2;
            return categories[selectedIndex];
        }

        public static void RemoveCategory(List<Category> categories)
        {
            Console.Clear();
            AnsiConsole.Write(new Rule("[grey]─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─[/]").RuleStyle("grey"));
            var selectedIndex = AnsiConsole.Prompt(
                    new SelectionPrompt<(string, int)>()
                        .Title("Select a category to remove")
                        .PageSize(10)
                        .AddChoices(categories.Select((cat, index) => (cat.Title, index)))
                        .UseConverter(t => t.Item1)).Item2;
            
            AnsiConsole.MarkupLine($"[red]Removing \"{categories[selectedIndex]}\"[/]");
            Thread.Sleep(Delay);
            categories.RemoveAt(selectedIndex);
        }

        public static void RenameCategory(List<Category> categories)
        {
            Console.Clear();
            AnsiConsole.Write(new Rule("[grey]─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─[/]").RuleStyle("grey"));
            var selectedIndex = AnsiConsole.Prompt(
                    new SelectionPrompt<(string, int)>()
                        .Title("Select a category to rename")
                        .PageSize(10)
                        .AddChoices(categories.Select((cat, index) => (cat.Title, index)))
                        .UseConverter(t => t.Item1)).Item2;
            
            var newCategoryName = AnsiConsole.Ask<string>("Enter a name for the new category:");
            categories[selectedIndex].Title = newCategoryName;
            AnsiConsole.MarkupLine($"[green]Success! Renamed {categories[selectedIndex]}[/]");
            Thread.Sleep(Delay);
        }
    }
}