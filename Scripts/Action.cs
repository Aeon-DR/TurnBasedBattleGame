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
    private static readonly Random _random = new Random();

    private readonly Battle _battle;
    private readonly Character _actor;
    private readonly Character _target;
    private readonly IAttack _attack;

    public AttackAction(Battle battle, Character actor, Character target, IAttack attack)
    {
        _battle = battle;
        _actor = actor;
        _target = target;
        _attack = attack;
    }

    public void Perform()
    {
        Console.WriteLine($"{_actor.Name} used {_attack.Name} on {_target.Name}.");

        AttackData attackData = _attack.CreateAttackData();

        if (_random.NextDouble() <= attackData.ProbabilityOfSuccess)
        {
            if (_target.DefensiveAttackModifier != null)
            {
                int initialDamage = attackData.Damage;
                attackData = _target.DefensiveAttackModifier.ModifyAttack(attackData);
                ConsoleHelper.WriteColoredLine($"{_target.DefensiveAttackModifier.Name} reduced the attack by {initialDamage - attackData.Damage} point(s).", ConsoleColor.Magenta);
            }

            _target.CurrentHealth -= attackData.Damage;
            ConsoleHelper.WriteColoredLine($"{_attack.Name} dealt {attackData.Damage} damage to {_target.Name}.", ConsoleColor.DarkRed);
            Console.WriteLine($"{_target.Name} is now at {_target.CurrentHealth}/{_target.MaxHealth} HP.");

            if (!_target.IsAlive)
            {
                ConsoleHelper.WriteColoredLine($"{_target.Name} has been defeated!", ConsoleColor.Blue);
                if (_target.Gear != null)
                {
                    _battle.GetPartyFor(_actor).Gear.Add(_target.Gear);
                    ConsoleHelper.WriteColoredLine($"{_actor.Name}'s party acquired {_target.Gear.Name}!", ConsoleColor.DarkGreen);
                    _target.Gear = null;
                }
            }
        }
        else
        {
            ConsoleHelper.WriteColoredLine($"{_actor.Name} missed!", ConsoleColor.DarkRed);
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
        Console.WriteLine($"{_actor.Name} used {_item.Name} on {_target.Name}.");

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

public class EquipGearAction : IAction
{
    private readonly Battle _battle;
    private readonly Character _target;
    private readonly IGear _gear;

    public EquipGearAction(Battle battle, Character target, IGear gear)
    {
        _battle = battle;
        _target = target;
        _gear = gear;
    }

    public void Perform()
    {
        IGear? previousGear = _target.Gear;
        List<IGear> partyGear = _battle.GetPartyGear(_target);

        _target.Gear = _gear;
        partyGear.Remove(_gear);

        ConsoleHelper.WriteColoredLine($"{_target.Name} equipped {_gear.Name}.", ConsoleColor.DarkGreen);

        if (previousGear != null)
        {
            partyGear.Add(previousGear);
            ConsoleHelper.WriteColoredLine($"Previously equipped {previousGear.Name} has been returned to the party's inventory.", ConsoleColor.DarkRed);
        }
    }
}