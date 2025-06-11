namespace TurnBasedBattleGame;

public abstract class Character
{
    public string Name { get; }
    public IAttack StandardAttack { get; }
    public IGear? Gear { get; set; }
    public IAttackModifier? DefensiveAttackModifier { get; set; }
    public int MaxHealth { get; }

    private int _currentHealth;
    public int CurrentHealth
    {
        get => _currentHealth;
        set => _currentHealth = Math.Clamp(value, 0, MaxHealth); // Prevent from reducing HP below 0 and healing above max HP
    }

    public bool IsAlive => CurrentHealth > 0;

    public string GetCharacterInfo()
    {
        string gearInfo = Gear != null ? $" | {Gear.Name}" : string.Empty;
        return $"{Name} ({CurrentHealth}/{MaxHealth}){gearInfo}";
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
    public Protagonist(string name) : base(name, new Punch(), 25) 
    {
        Gear = new Sword();
    }
}

public class VinFletcher : Character
{
    public VinFletcher() : base("VIN FLETCHER", new Punch(), 15)
    {
        Gear = new VinsBow();
    }
}

public class Antagonist : Character
{
    public Antagonist() : base("THE UNCODED ONE", new Unraveling(), 15) { }
}

public class Skeleton : Character
{
    public Skeleton() : base("SKELETON", new BoneCrunch(), 5) { }
}

public class StoneGargoyle : Character
{
    public StoneGargoyle() : base("STONE GARGOYLE", new Bite(), 4) 
    {
        DefensiveAttackModifier = new StoneArmor();
    }
}