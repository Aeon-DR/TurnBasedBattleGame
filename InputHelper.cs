namespace TurnBasedBattleGame;

public static class InputHelper
{
    public static string ChooseHeroName(string prompt)
    {
        while (true)
        {
            Console.WriteLine(prompt);
            string? name = Console.ReadLine()?.Trim().ToUpper();

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("The name cannot be empty.\n");
                continue;
            }

            Console.WriteLine();
            return name;
        }
    }

    public static int PromptWithMenu(string prompt, List<string> options)
    {
        while (true)
        {
            Console.WriteLine(prompt);
            for (int i = 0; i < options.Count; i++)
            {
                Console.WriteLine($"{i + 1} - {options[i]}");
            }

            string? input = Console.ReadLine();
            if (int.TryParse(input, out int choice) && choice >= 1 && choice <= options.Count)
            {
                return choice;
            }

            Console.WriteLine("Invalid option, please try again.\n");
        }
    }
}