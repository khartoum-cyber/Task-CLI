using Microsoft.Extensions.DependencyInjection;
using Task_CLI.Helpers;
using Task_CLI.Interfaces;
using Task_CLI.Services;

var serviceProvider = new ServiceCollection().AddSingleton<ITaskService, TaskService>().BuildServiceProvider();
var _taskService = serviceProvider.GetService<ITaskService>();

WelcomeMessage();

while (true)
{
    Helper.PrintCommandMessage("Enter command : ");

    var input = Console.ReadLine() ?? string.Empty;

    if (string.IsNullOrEmpty(input))
    {
        Helper.PrintInfoMessage("No input detected, try again !");
        continue;
    }

    var commands = Helper.InputParser(input);

    var command = commands[0].ToLower();

    var exit = false;

    switch (command)
    {
        case "help":
            HelpMessage();
            break;

        case "exit":
            exit = true;
            break;

        default:
            break;
    }

    if (exit)
    {
        break;
    }
}

void HelpMessage()
{
    var helpCommands = _taskService?.GetAllHelpCommands();

    int count = 1;
    if (helpCommands != null)
    {
        foreach (var item in helpCommands)
        {
            Helper.PrintHelpMessage(count + ". " + item);
            count++;
        }
    }
}
static void WelcomeMessage()
{
    Helper.PrintInfoMessage("Hello, Welcome to Task Tracker!");
    Helper.PrintInfoMessage("Type \"help\" to know the set of commands");
}
