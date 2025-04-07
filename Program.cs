string protagonistName = InputHelper.ChooseHeroName("What is the protagonist's name?");

Party heroes = new Party(new List<Character> { new Protagonist(protagonistName) });
Party monsters = new Party(new List<Character> { new Skeleton() });

Battle battle = new Battle(new ComputerPlayer(), new ComputerPlayer(), heroes, monsters);
battle.Run();

public class Battle
{
    public IPlayer HeroesPlayer { get; }
    public IPlayer MonstersPlayer { get; }

    public Party Heroes { get; }
    public Party Monsters { get; }

    public Battle(IPlayer heroesPlayer, IPlayer monstersPlayer, Party heroes, Party monsters)
    {
        HeroesPlayer = heroesPlayer;
        MonstersPlayer = monstersPlayer;
        Heroes = heroes;
        Monsters = monsters;
    }

    public void Run()
    {
        while (true)
        {
            HandlePartyTurn(HeroesPlayer, Heroes);
            HandlePartyTurn(MonstersPlayer, Monsters);
        }
    }

    private static void HandlePartyTurn(IPlayer player, Party party)
    {
        foreach(Character character in party.Characters)
        {
            Console.WriteLine($"It is {character.Name}'s turn.");

            IAction chosenAction = player.ChooseAction(character);
            chosenAction.Perform();

            Console.WriteLine();
            Thread.Sleep(1000);
        }
    }
}

public interface IPlayer
{
    IAction ChooseAction(Character character);
}

public class ComputerPlayer : IPlayer
{
    public IAction ChooseAction(Character character)
    {
        return new SkipTurnAction(character);
    }
}

public abstract class Character
{
    public abstract string Name { get; }
}

public class Protagonist : Character
{
    public override string Name { get; }

    public Protagonist(string name)
    {
        Name = name;
    }
}

public class Skeleton : Character
{
    public override string Name => "SKELETON";
}

public class Party
{
    public List<Character> Characters { get; }

    public Party(IEnumerable<Character> startingCharacters)
    {
        Characters = startingCharacters.ToList();
    }
}

public interface IAction
{
    void Perform();
}

public class SkipTurnAction : IAction
{
    private Character _actor;

    public SkipTurnAction(Character actor)
    {
        _actor = actor;
    }

    public void Perform()
    {
        Console.WriteLine($"{_actor.Name} SKIPPED the turn.");
    }
}

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