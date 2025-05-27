namespace TurnBasedBattleGame;

public static class ConsoleHelper
{
    public static string ChooseHeroName(string prompt)
    {
        while (true)
        {
            Console.WriteLine(prompt);

            Console.ForegroundColor = ConsoleColor.Cyan;
            string? name = Console.ReadLine()?.Trim().ToUpper();
            Console.ForegroundColor = ConsoleColor.Gray;

            if (string.IsNullOrWhiteSpace(name))
            {
                WriteColoredLine("The name cannot be empty.\n", ConsoleColor.DarkRed);
                continue;
            }

            Console.WriteLine();
            return name;
        }
    }

    public static int PromptWithMenu(string prompt, List<string> options, bool backAllowed = false)
    {
        while (true)
        {
            Console.WriteLine(prompt);

            if (backAllowed)
            {
                Console.WriteLine($"0 - Back");
            }

            for (int i = 0; i < options.Count; i++)
            {
                Console.WriteLine($"{i + 1} - {options[i]}");
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            string? input = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.Gray;

            if (int.TryParse(input, out int choice))
            {
                if (choice == 0) return -1;
                if (choice >= 1 && choice <= options.Count) return choice;
            }

            WriteColoredLine("Invalid option, please try again.\n", ConsoleColor.DarkRed);
        }
    }

    public static void WriteColoredLine(string message, ConsoleColor foregroundColor)
    {
        Console.ForegroundColor = foregroundColor;
        Console.WriteLine(message);
        Console.ForegroundColor = ConsoleColor.Gray;
    }
}