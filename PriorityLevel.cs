namespace ReminderApp;

public enum PriorityLevelType
{
    High,
    Medium,
    Low
}
class PriorityLevel
{
    public static void PrintPriorityLevels()
    {
        Console.WriteLine("\nWhat level of priority is this task?\n");
        var priorityTypes = Enum.GetNames(typeof(PriorityLevelType));
        for (int i = 0; i < priorityTypes.Length; i++)
        {
            Console.WriteLine($"{i + 1}) " + priorityTypes[i]);
        }
    }
}