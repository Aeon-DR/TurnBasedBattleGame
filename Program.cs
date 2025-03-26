Party heroes = new Party(new List<Character> { new Skeleton() });
Party monsters = new Party(new List<Character> { new Skeleton() });

Battle battle = new Battle(heroes, monsters);
battle.Run();

public class Battle
{
    public Party Heroes { get; }
    public Party Monsters { get; }

    public Battle(Party heroes, Party monsters)
    {
        Heroes = heroes;
        Monsters = monsters;
    }

    public void Run()
    {
        while (true)
        {
            HandlePartyTurn(Heroes);
            HandlePartyTurn(Monsters);
        }
    }

    private static void HandlePartyTurn(Party party)
    {
        foreach(Character character in party.Characters)
        {
            Console.WriteLine($"It is {character.Name}'s turn.");
            IAction chosenAction = character.TakeTurn();
            chosenAction.Perform();

            Console.WriteLine();
            Thread.Sleep(1000);
        }
    }
}

public abstract class Character
{
    public abstract string Name { get; }

    public IAction TakeTurn()
    {
        return new SkipTurnAction(this);
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