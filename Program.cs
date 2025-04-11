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
        return new AttackAction(actor, target, actor.StandardAttack);
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
    public string Name { get; }
    public IAttack StandardAttack { get; }
    public int MaxHealth { get; }

    private int _currentHealth;
    public int CurrentHealth
    {
        get => _currentHealth;
        set => _currentHealth = Math.Clamp(value, 0, MaxHealth); // Prevent from reducing HP below 0 and healing above max HP
    }

    protected Character(string name, IAttack standardAttack, int maxHealth)
    {
        Name = name;
        StandardAttack = standardAttack;
        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
    }
}

public class Protagonist : Character
{
    public Protagonist(string name) : base(name, new Punch(), 25) { }
}

public class Skeleton : Character
{
    public Skeleton() : base("SKELETON", new BoneCrunch(), 5) { }
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
    private readonly IAttack _attack;

    public AttackAction(Character actor, Character target, IAttack attack)
    {
        _actor = actor;
        _target = target;
        _attack = attack;
    }

    public void Perform()
    {
        Console.WriteLine($"{_actor.Name} used {_attack.Name} on {_target.Name}.");

        AttackData attackData = _attack.CreateAttackData();
        _target.CurrentHealth -= attackData.Damage;
        Console.WriteLine($"{_attack.Name} dealt {attackData.Damage} damage to {_target.Name}.");
        
        Console.WriteLine($"{_target.Name} is now at {_target.CurrentHealth}/{_target.MaxHealth} HP.");
    }
}

public interface IAttack
{
    string Name { get; }
    AttackData CreateAttackData();
}

public class Punch : IAttack
{
    public string Name => "PUNCH";
    public AttackData CreateAttackData() => new AttackData(1);
}

public class BoneCrunch : IAttack
{
    private static readonly Random _random = new Random();

    public string Name => "BONE CRUNCH";
    public AttackData CreateAttackData() => new AttackData(_random.Next(2));
}

public record AttackData(int Damage);

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