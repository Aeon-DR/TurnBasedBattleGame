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

    private void HandlePartyTurn(IPlayer player, Party party)
    {
        foreach(Character character in party.Characters)
        {
            Console.WriteLine();

            Console.WriteLine($"It is {character.Name}'s turn.");
            IAction chosenAction = player.ChooseAction(this, character);
            chosenAction.Perform();
        }
    }

    public Party GetPartyFor(Character character) => Heroes.Characters.Contains(character) ? Heroes : Monsters;
    public Party GetEnemyPartyFor(Character character) => Heroes.Characters.Contains(character) ? Monsters : Heroes;}

public interface IPlayer
{
    IAction ChooseAction(Battle battle, Character actor);
}

public class ComputerPlayer : IPlayer
{
    public IAction ChooseAction(Battle battle, Character actor)
    {
        // A brief delay to simulate hesitation
        Thread.Sleep(1000);

        Character target = SelectRandomTarget(battle, actor);
        return new AttackAction(actor, target);
    }

    private Character SelectRandomTarget(Battle battle, Character character)
    {
        Random random = new Random();
        Party enemyParty = battle.GetEnemyPartyFor(character);
        return enemyParty.Characters[random.Next(enemyParty.Characters.Count)];
    }
}

public abstract class Character
{
    public abstract string Name { get; }
    public abstract StandardAttack StandardAttack { get; }
}

public class Protagonist : Character
{
    public override string Name { get; }
    public override StandardAttack StandardAttack { get; } = new StandardAttack("PUNCH");

    public Protagonist(string name)
    {
        Name = name;
    }
}

public class Skeleton : Character
{
    public override string Name => "SKELETON";
    public override StandardAttack StandardAttack { get; } = new StandardAttack("BONE CRUNCH");
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
    private readonly Character _actor;

    public SkipTurnAction(Character actor)
    {
        _actor = actor;
    }

    public void Perform()
    {
        Console.WriteLine($"{_actor.Name} SKIPPED the turn.");
    }
}

public class AttackAction : IAction
{
    private readonly Character _actor;
    private readonly Character _target;

    public AttackAction(Character actor, Character target)
    {
        _actor = actor;
        _target = target;
    }

    public void Perform()
    {
        Console.WriteLine($"{_actor.Name} used {_actor.StandardAttack.Name} on {_target.Name}.");
    }
}

public interface IAttack
{
    string Name { get; }
}

public class StandardAttack : IAttack
{
    public string Name { get; }

    public StandardAttack(string name)
    {
        Name = name;
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