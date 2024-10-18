using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.DependencyInjection;
using Task_CLI.Helpers;
using Task_CLI.Interfaces;
using Task_CLI.Models;
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

        case "update":
            UpdateTask();
            break;

        case "delete":
            DeleteTask();
            break;

        case "list":
            ListAllTasks();
            break;

        case "mark-in-progress":
            SetTaskStatus();
            break;

        case "mark-todo":
            SetTaskStatus();
            break;

        case "mark-done":
            SetTaskStatus();
            break;

            //default:
            //    break;
    }

    if (exit)
    {
        break;
    }
}

void SetTaskStatus()
{
    if (!IsUserInputValid(commands, 2))
        return;

    var id = IsValidIdProvided(commands, 0).Item2;

    if (id == 0)
    {
        return;
    }

    var statusChanged = _taskService?.SetTaskStatus(commands[0], id).Result;

    if (statusChanged != null && statusChanged.Value)
    {
        Helper.PrintInfoMessage($"Status for task Id : {id} changed.");
    }
    else
    {
        Helper.PrintInfoMessage($"Task with Id : {id}, does not exist!");
    }
}

void ListAllTasks()
{
    if (commands.Count > 2)
    {
        Helper.PrintErrorMessage("Wrong command! Try again.");
        Helper.PrintInfoMessage("Type \"help\" to know the set of commands");
        return;
    }

    var tasks = new List<CliTask>();

    if (commands.Count == 1)
    {
        tasks = _taskService?.ListAllTasks().Result.OrderBy(x => x.Id).ToList() ?? tasks;
    }
    else
    {
        if (!commands[1].ToLower().Equals("in-progress") && !commands[1].ToLower().Equals("done") && !commands[1].ToLower().Equals("todo"))
        {
            Helper.PrintErrorMessage("Wrong command! Try again.");
            Helper.PrintInfoMessage("Type \"help\" to know the set of commands");
            return;
        }
        tasks = _taskService?.GetTaskByStatus(commands[1]).Result.OrderBy(x => x.Id).ToList() ?? tasks;
    }

    CreateTaskTable(tasks);
}

void DeleteTask()
{
    if (!IsUserInputValid(commands, 2))
        return;

    var id = IsValidIdProvided(commands, 0).Item2;

    if (id == 0)
    {
        return;
    }

    var taskDeleted = _taskService?.DeleteTask(id).Result;

    if (taskDeleted != null && taskDeleted.Value)
    {
        Helper.PrintInfoMessage($"Task deleted successfully with Id : {id}");
    }
    else
    {
        Helper.PrintInfoMessage($"Task with Id : {id}, does not exist!");
    }
}

void UpdateTask()
{
    if (!IsUserInputValid(commands, 3))
        return;

    var id = IsValidIdProvided(commands, 0).Item2;

    if (id == 0)
    {
        return;
    }

    var result = _taskService?.UpdateTask(id, commands[2]).Result;

    if (result != null && result.Value)
    {
        Helper.PrintInfoMessage($"Task updated successfully with Id : {id}");
    }
    else
    {
        Helper.PrintInfoMessage($"Task with Id : {id}, does not exist!");
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

static Tuple<bool, int> IsValidIdProvided(List<string> commands, int id)
{
    int.TryParse(commands[1], out id);

    if (id == 0)
    {
        Helper.PrintErrorMessage("Wrong command! Try again.");
        Helper.PrintInfoMessage("Type \"help\" to know the set of commands");
        return new Tuple<bool, int>(false, id);
    }

    return new Tuple<bool, int>(true, id);
}

static void CreateTaskTable(List<CliTask> tasks)
{
    int colWidth1 = 15, colWidth2 = 35, colWidth3 = 15, colWidth4 = 15;
    if (tasks != null && tasks.Count > 0)
    {
        Console.WriteLine("\n{0,-" + colWidth1 + "} {1,-" + colWidth2 + "} {2,-" + colWidth3 + "} {3,-" + colWidth4 + "}",
            "Task Id", "Description", "Status", "Created Date" + "\n");

        foreach (var task in tasks)
        {
            SetConsoleTextColor(task);
            Console.WriteLine("{0,-" + colWidth1 + "} {1,-" + colWidth2 + "} {2,-" + colWidth3 + "} {3,-" + colWidth4 + "}"
                , task.Id, task.Description, task.TaskStatus, task.CreatedAt.Date.ToString("dd-MM-yyyy"));
            Console.ResetColor();
        }
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("\n No Task exists! \n");
        Console.ResetColor();

        Console.WriteLine("{0,-" + colWidth1 + "} {1,-" + colWidth2 + "} {2,-" + colWidth3 + "} {3,-" + colWidth4 + "}",
            "Task Id", "Description", "Status", "CreatedDate");
    }
}

static void SetConsoleTextColor(CliTask task)
{
    Console.ForegroundColor = task.TaskStatus switch
    {
        Task_CLI.Enums.Status.ToDo => ConsoleColor.Magenta,
        Task_CLI.Enums.Status.Done => ConsoleColor.Green,
        _ => ConsoleColor.Yellow
    };
}