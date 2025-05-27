namespace TurnBasedBattleGame;

public interface IPlayer
{
    IAction ChooseAction(Battle battle, Character actor);
}

public class ComputerPlayer : IPlayer
{
    private readonly Random _random = new Random();

    public IAction ChooseAction(Battle battle, Character actor)
    {
        // A brief delay to simulate hesitation
        Thread.Sleep(1000);

        // Use a health potion at random if HP is at or under 50% and a potion is available
        List<IItem> potions = battle.GetPartyFor(actor).Items.Where(i => i is HealthPotion && !i.Used).ToList();
        if (potions.Count > 0 && actor.CurrentHealth <= actor.MaxHealth / 2 && _random.NextDouble() < 0.5f)
        {
            return new UseItemAction(actor, actor, potions.First());
        }

        // Equip gear at random if no gear is equipped and available
        List<IGear> partyGear = battle.GetPartyGear(actor);
        if (actor.Gear == null && partyGear.Count > 0 && _random.NextDouble() < 0.5f)
        {
            return new EquipGearAction(battle, actor, partyGear.First());
        }

        // Attack a random target with gear attack if available, otherwise do the standard attack
        Character target = SelectRandomTarget(battle, actor);
        if (actor.Gear != null)
        {
            return new AttackAction(battle, actor, target, actor.Gear.Attack);
        }
        return new AttackAction(battle, actor, target, actor.StandardAttack);
    }

    private Character SelectRandomTarget(Battle battle, Character character)
    {
        var targets = battle.GetAliveEnemies(character);
        return targets[_random.Next(targets.Count)];
    }
}

public class HumanPlayer : IPlayer
{
    public IAction ChooseAction(Battle battle, Character actor)
    {
        var actions = new Dictionary<string, Func<IAction>>();

        List<IItem> items = battle.GetPartyFor(actor).Items.Where(i => !i.Used).ToList();
        List<IGear> partyGear = battle.GetPartyGear(actor);
        
        actions.Add($"Standard Attack ({actor.StandardAttack.Name})", () => PerformAttack(battle, actor, actor.StandardAttack));

        if (actor.Gear != null)
            actions.Add($"Special Attack ({actor.Gear.Attack.Name})", () => PerformAttack(battle, actor, actor.Gear.Attack));

        if (partyGear.Count > 0)
            actions.Add("Equip Gear", () => EquipGear(battle, actor, partyGear));

        if (items.Count > 0)
            actions.Add("Use Item", () => UseItem(battle, actor, items));

        actions.Add("Skip Turn", () => new SkipTurnAction(actor));

        int choice = ConsoleHelper.PromptWithMenu($"What do you want {actor.Name} to do?", actions.Keys.ToList());

        return actions.Values.ElementAt(choice - 1)();
    }

    private static AttackAction PerformAttack(Battle battle, Character actor, IAttack attack)
    {
        var targets = battle.GetAliveEnemies(actor);
        if (targets.Count == 1) return new AttackAction(battle, actor, targets[0], attack);

        int targetChoice = ConsoleHelper.PromptWithMenu(
            $"What target do you want {actor.Name} to attack?",
           targets.Select(t => $"{t.Name} ({t.CurrentHealth}/{t.MaxHealth} HP)").ToList());

        return new AttackAction(battle, actor, targets[targetChoice - 1], attack);
    }

    private static UseItemAction UseItem(Battle battle, Character actor, List<IItem> items)
    {
        int itemChoice = ConsoleHelper.PromptWithMenu(
                $"What item do you want to use?",
               items.Select(i => $"{i.Name} ({i.Power} HP)").ToList());

        IItem item = items[itemChoice - 1];

        if (item is HealthPotion)
        {
            var targets = battle.GetAliveAllies(actor);
            if (targets.Count == 1) return new UseItemAction(actor, targets[0], item);

            int targetChoice = ConsoleHelper.PromptWithMenu(
            $"What target do you want to heal?",
           targets.Select(t => $"{t.Name} ({t.CurrentHealth}/{t.MaxHealth} HP)").ToList());

            return new UseItemAction(actor, targets[targetChoice - 1], item);
        }

        throw new InvalidOperationException("Unsupported item type.");
    }

    private static EquipGearAction EquipGear(Battle battle, Character actor, List<IGear> partyGear)
    {
        if (partyGear.Count == 1) return new EquipGearAction(battle, actor, partyGear[0]);

        int gearChoice = ConsoleHelper.PromptWithMenu(
                $"What gear do you want to equip?",
               partyGear.Select(g => $"{g.Name}").ToList());

        return new EquipGearAction(battle, actor, partyGear[gearChoice - 1]);
    }
}