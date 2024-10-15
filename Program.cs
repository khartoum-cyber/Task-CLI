using Task_CLI.Helpers;

namespace Task_CLI
{
    internal class Program
    {
        private static void Main(string[] args)
        {
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
        }

        private static void HelpMessage()
        {
            var helpCommands = _taskService?.GetAllHelpCommands();
        }

        private static void WelcomeMessage()
        {
            Helper.PrintInfoMessage("Hello, Welcome to Task Tracker!");
            Helper.PrintInfoMessage("Type \"help\" to know the set of commands");
        }
    }
}
