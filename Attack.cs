namespace TurnBasedBattleGame;

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