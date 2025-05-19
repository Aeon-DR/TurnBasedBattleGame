namespace TurnBasedBattleGame;

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

        ConsoleHelper.WriteColoredLine($"{_attack.Name} dealt {attackData.Damage} damage to {_target.Name}.", ConsoleColor.DarkRed);
        Console.WriteLine($"{_target.Name} is now at {_target.CurrentHealth}/{_target.MaxHealth} HP.");

        if (!_target.IsAlive)
        {
            ConsoleHelper.WriteColoredLine($"{_target.Name} has been defeated!", ConsoleColor.Blue);
        }
    }
}

public class UseItemAction : IAction 
{
    private readonly Character _actor;
    private readonly Character _target;
    private readonly IItem _item;

    public UseItemAction(Character actor, Character target, IItem item)
    {
        _actor = actor;
        _target = target;
        _item = item;
    }

    public void Perform()
    {
        Console.WriteLine($"{_actor.Name} used {_item.Name.ToUpper()} on {_target.Name}.");

        if (_item is HealthPotion)
        {
            int previousHealth = _target.CurrentHealth;

            _target.CurrentHealth += _item.Power;
            _item.Used = true;

            int restoredAmount = _target.CurrentHealth - previousHealth;

            ConsoleHelper.WriteColoredLine($"{_target.Name} restored {restoredAmount} HP.", ConsoleColor.DarkGreen);
            Console.WriteLine($"{_target.Name} is now at {_target.CurrentHealth}/{_target.MaxHealth} HP.");
        }
        else
        {
            throw new InvalidOperationException("Unsupported item type.");
        }
    }
}