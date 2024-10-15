using System.Text.RegularExpressions;

namespace Task_CLI.Helpers
{
    internal static class Helper
    {
        internal static void PrintInfoMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n" + message);
            Console.ResetColor();
        }

        public static void PrintHelpMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n" + message);
            Console.ResetColor();
        }

        internal static void PrintCommandMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n" + message + "\n");
            Console.ResetColor();
        }

        internal static List<string> InputParser(string input)
        {
            var commandArgs = new List<string>();

            var regex = new Regex(@"[\""].+?[\""]|[^ ]+");
            var matches = regex.Matches(input);

            foreach (Match match in matches)
            {
                // Remove surrounding quotes if any
                var value = match.Value.Trim('"');
                commandArgs.Add(value);
            }

            return commandArgs;
        }
    }
}
