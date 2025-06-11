namespace TurnBasedBattleGame;

public interface IAttackModifier
{
    string Name { get; }
    AttackData ModifyAttack(AttackData attackData);
}

public class StoneArmor : IAttackModifier
{
    public string Name => "STONE ARMOR";
    public AttackData ModifyAttack(AttackData attackData)
    {
        return attackData with { Damage = Math.Max(0, attackData.Damage - 1) };
    }     
}