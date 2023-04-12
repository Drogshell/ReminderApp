using Spectre.Console;

namespace ReminderApp;

public static class PromptUtilities
{
    private const int PageSize = 10;
    public static T PromptSelection<T>(string prompt, List<T> choices) where T : notnull
    {
        AnsiConsole.Write(new Rule("[grey]─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─[/]").RuleStyle("grey"));

        var selection = AnsiConsole.Prompt(
            new SelectionPrompt<T>()
                .Title(prompt)
                .PageSize(PageSize)
                .AddChoices(choices)
        );
        
        return selection;
    }
    
    public static List<T> PromptMultiSelection<T>(string prompt, List<T> choices, bool required) where T : notnull
    {
        AnsiConsole.Write(new Rule("[grey]─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─[/]").RuleStyle("grey"));
        List<T> selection;
        if (required)
        {
            selection = AnsiConsole.Prompt(
                new MultiSelectionPrompt<T>()
                    .Title(prompt)
                    .PageSize(PageSize)
                    .MoreChoicesText("[grey](Move up and down to reveal more tasks)[/]")
                    .InstructionsText(
                        "[grey](Press [blue]<space>[/] to toggle a task, " + 
                        "[green]<enter>[/] to accept)[/]")!
                    .AddChoices(choices));
        }
        else
        {
            selection = AnsiConsole.Prompt(
                new MultiSelectionPrompt<T>()
                    .Title(prompt)
                    .NotRequired()
                    .PageSize(PageSize)
                    .MoreChoicesText("[grey](Move up and down to reveal more tasks)[/]")
                    .InstructionsText(
                        "[grey](Press [blue]<space>[/] to toggle a task, " + 
                        "[green]<enter>[/] to accept)[/]")!
                    .AddChoices(choices));
        }
        return selection;
    }
}