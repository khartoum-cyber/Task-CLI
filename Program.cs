using Microsoft.Extensions.DependencyInjection;
using Task_CLI.Helpers;
using Task_CLI.Interfaces;
using Task_CLI.Services;

var serviceProvider = new ServiceCollection().AddSingleton<ITaskService, TaskService>().BuildServiceProvider();
var _taskService = serviceProvider.GetService<ITaskService>();

List<string> commands;

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

    commands = Helper.InputParser(input);

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

        case "add":
            AddNewTask();
            break;

        default:
            break;
    }

    if (exit)
    {
        break;
    }
}
void AddNewTask()
{
    if (!IsUserInputValid(commands, 2))
        return;

    var taskAdded = _taskService?.AddNewTask(commands[1]);

    if (taskAdded != null && taskAdded.Result != 0)
        Helper.PrintInfoMessage($"Task added successfully with Id : {taskAdded.Result}");
    else
        Helper.PrintInfoMessage("Task not saved!");
}

bool IsUserInputValid(List<string> commands, int requiredParameter)
{
    bool validInput = true;

    if (requiredParameter == 1)
    {
        if (commands.Count != requiredParameter)
        {
            validInput = false;
        }
    }

    if (requiredParameter == 2)
    {
        if (commands.Count != requiredParameter || string.IsNullOrEmpty(commands[1]))
        {
            validInput = false;
        }
    }

    if (requiredParameter == 3)
    {
        if (commands.Count != requiredParameter || string.IsNullOrEmpty(commands[1]) || string.IsNullOrEmpty(commands[2]))
        {
            validInput = false;
        }
    }

    if (!validInput)
    {

        Helper.PrintErrorMessage("Wrong command! Try again.");
        Helper.PrintInfoMessage("Type \"help\" to know the set of commands");
        return false;
    }

    return true;
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
