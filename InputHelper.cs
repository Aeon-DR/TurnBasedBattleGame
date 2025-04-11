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
                Console.WriteLine("The name cannot be empty.");
                continue;
            }

            return name;
        }
    }
}