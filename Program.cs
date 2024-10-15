using Task_CLI.Helpers;

namespace Task_CLI
{
    internal class Program
    {
        private static void Main(string[] args)
        {
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

                var exit = false;

                switch (input)
                {
                    case "help":
                        Console.WriteLine("HELP !");
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
    }
}
