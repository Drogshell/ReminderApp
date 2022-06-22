namespace ReminderApp;

public class Menu
{
    public int SelectedIndex { get; set; }
    public string Prompt { get; }
    public List<string> Options { get; }
    public int CursorLeft { get; set; }
    public int CursorTop { get; set; }

    public Menu(string prompt, List<string> options)
    {
        Prompt = prompt;
        Options = options;
        SelectedIndex = 0;
    }

    public void SetCursorPosition(int left, int top)
    {
        CursorLeft = left;
        CursorTop = top;
    }

    public int Run()
    {
        ConsoleKey keyPressed;

        do
        {
            Console.SetCursorPosition(CursorLeft, CursorTop);
            DisplayOptions();
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            keyPressed = keyInfo.Key;

            if (keyPressed == ConsoleKey.UpArrow)
            {
                SelectedIndex--;
                if (SelectedIndex == -1)
                {
                    SelectedIndex = Options.Count - 1;
                }
            }
            else if (keyPressed == ConsoleKey.DownArrow)
            {
                SelectedIndex++;
                if (SelectedIndex == Options.Count)
                {
                    SelectedIndex = 0;
                }
            }

        } while (keyPressed != ConsoleKey.Enter);

        return SelectedIndex;
    }

    private void DisplayOptions()
    {
        Console.WriteLine(Prompt);
        char prefix; 

        for (int i = 0; i < Options.Count; i++)
        {
            var currentlySelected = Options[i];
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