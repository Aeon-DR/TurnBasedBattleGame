namespace TurnBasedBattleGame;

public interface IAttack
{
    string Name { get; }
    AttackData CreateAttackData();
}

public static class AttackUtilities
{
    private static readonly Random _random = new Random();
    public static int GetRandomDamage(int maxExclusive) => _random.Next(maxExclusive);
}

public class Punch : IAttack
{
    public string Name => "PUNCH";
    public AttackData CreateAttackData() => new AttackData(1);
}

public class BoneCrunch : IAttack
{
    public string Name => "BONE CRUNCH";
    public AttackData CreateAttackData() => new AttackData(AttackUtilities.GetRandomDamage(2));
}

public class Unraveling : IAttack
{
    public string Name => "UNRAVELING";
    public AttackData CreateAttackData() => new AttackData(AttackUtilities.GetRandomDamage(3));
}

public class Slash : IAttack
{
    public string Name => "SLASH";
    public AttackData CreateAttackData() => new AttackData(2);
}

public class Stab : IAttack
{
    public string Name => "STAB";
    public AttackData CreateAttackData() => new AttackData(1);
}

public class QuickShot : IAttack
{
    public string Name => "QUICK SHOT";
    public AttackData CreateAttackData() => new AttackData(3, 0.5);
}