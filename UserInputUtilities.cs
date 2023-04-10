using System.Globalization;

namespace ReminderApp;

public class UserInputUtilities
{
    public static string ReadString(string prompt)
    {
        CheckPromptForNull(prompt);
        Console.WriteLine(prompt);
        var isValid = false;
        var buffer = Console.ReadLine();
        do
        {
            if (string.IsNullOrWhiteSpace(buffer))
            {
                Console.WriteLine("Error: White spaces or empty words are not allowed");
                buffer = Console.ReadLine();
                continue;
            }

            isValid = true;

        } while (!isValid);

        return buffer.Trim();
    }

    public static int ReadInt(string prompt)
    {
        CheckPromptForNull(prompt);
        var isValid = false;
        var buffer = ReadString(prompt);
        var result = 0;
        do
        {
            try
            {
                result = Convert.ToInt32(buffer);
                isValid = true;
            }
            catch (Exception)
            {
                Console.WriteLine("Error: Not a number");
                buffer = ReadString("Try again: ");
            }

        } while (!isValid);

        return result;
    }

    public static DateTime GetDate()
    {
        var auCulture = new CultureInfo("en-AU");

        var dateString = ReadString("\nWhen is this task due?\nSpecify a date with the format: "
                                                       + auCulture.DateTimeFormat.ShortDatePattern
                                                       + " h:mm AM/PM");

        while (true)
        {
            if (DateTime.TryParseExact(dateString, "g", auCulture, DateTimeStyles.AllowWhiteSpaces, out var userDate))
            {
                if (userDate >= DateTime.Now) return userDate;
                
                Console.WriteLine("You can't set a due date in the past!");
                dateString = ReadString("\nTry again. Specify a date with the format: "
                                                           + auCulture.DateTimeFormat.ShortDatePattern
                                                           + " h:mm AM/PM");
                continue;
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Not a valid date");
            Console.ResetColor();
            dateString = ReadString("Please enter the date in the exact format: "
                                    + auCulture.DateTimeFormat.ShortDatePattern
                                    + " h:mm AM/PM");
        }
    }
    
    private static void CheckPromptForNull(string prompt)
    {
        if (prompt == null)
        {
            throw new ArgumentNullException(null, "A prompt must be passed!");
        }
    }
}