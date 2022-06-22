using System.Globalization;

namespace ReminderApp;

public class UserInputUtilities
{
    public static string ReadString(string prompt)
    {
        if (prompt == null)
        {
            throw new ArgumentNullException(null, "A prompt must be passed!");
        }

        Console.WriteLine(prompt);
        bool isValid = false;
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
        if (prompt == null)
        {
            throw new ArgumentNullException(null, "A prompt must be passed!");
        }

        bool isValid = false;
        var buffer = ReadString(prompt);
        int result = 0;
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
        DateTime userDate;
        var AUCulture = new CultureInfo("en-AU");

        string dateString = UserInputUtilities.ReadString("\nWhen is this task due?\nSpecify a date with the format: "
                                                          + AUCulture.DateTimeFormat.ShortDatePattern
                                                          + " h:mm AM/PM");

        while (true)
        {
            if (DateTime.TryParseExact(dateString, "g", AUCulture, DateTimeStyles.AllowWhiteSpaces, out userDate))
            {
                if (userDate < DateTime.Now)
                {
                    Console.WriteLine("You can't set a due date in the past!");
                    dateString = UserInputUtilities.ReadString("\nTry again. Specify a date with the format: "
                                                               + AUCulture.DateTimeFormat.ShortDatePattern
                                                               + " h:mm AM/PM");
                    continue;
                }
                return userDate;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Not a valid date");
                Console.ResetColor();
                dateString = UserInputUtilities.ReadString("Please enter the date in the exact format: "
                                                           + AUCulture.DateTimeFormat.ShortDatePattern
                                                           + " h:mm AM/PM");
            }
        }
    }
}