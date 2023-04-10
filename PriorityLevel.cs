using Spectre.Console;

namespace ReminderApp;

public enum PriorityLevelType
{
    High,
    Medium,
    Low
}

public class PriorityLevel
{
    public static PriorityLevelType GetPriorityLevel()
    {
        var enumNames = Enum.GetNames(typeof(PriorityLevelType));
        var priorityType = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Priority Level?")
                .PageSize(10)
                .AddChoices(enumNames));
        
        return (PriorityLevelType)Enum.Parse(typeof(PriorityLevelType), priorityType);
    }
}