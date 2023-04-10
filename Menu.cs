namespace ReminderApp;

public class Menu
{
    private int SelectedIndex { get; set; }
    private string Prompt { get; }
    private List<string> Options { get; }
    private int CursorTopOffset { get; }

    public Menu(string prompt, List<string> options, int cursorTopOffset = 6)
    {
        Prompt = prompt;
        Options = options;
        SelectedIndex = 0;
        CursorTopOffset = cursorTopOffset;
    }
    
    public int Run()
    {
        ConsoleKey keyPressed;
        do
        {
            DisplayOptions(CursorTopOffset);
            var keyInfo = Console.ReadKey(intercept: true);
            keyPressed = keyInfo.Key;

            SelectedIndex = keyPressed switch
            {
                ConsoleKey.UpArrow => (SelectedIndex == 0) ? Options.Count - 1 : SelectedIndex - 1,
                ConsoleKey.DownArrow => (SelectedIndex == Options.Count - 1) ? 0 : SelectedIndex + 1,
                _ => SelectedIndex
            };
        } while (keyPressed != ConsoleKey.Enter);
        return SelectedIndex;
    }
    
    private void DisplayOptions(int cursorTopOffset = 6)
    {
        Console.SetCursorPosition(0, cursorTopOffset);
        Console.WriteLine(Prompt);

        for (var i = 0; i < Options.Count; i++)
        {
            var currentlySelected = Options[i];
            char prefix;
            if (i == SelectedIndex)
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.White;
                prefix = '>';
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
                prefix = ' ';
            }
            Console.WriteLine($"{prefix} {currentlySelected}");
        }
        Console.ResetColor();
    }
}