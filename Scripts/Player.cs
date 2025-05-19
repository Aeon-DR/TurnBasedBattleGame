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

        // Use a health potion at random if HP is under 50% and a potion is available
        List<IItem> potions = battle.GetPartyFor(actor).Items.Where(i => i is HealthPotion && !i.Used).ToList();
        if (potions.Count > 0 && actor.CurrentHealth < actor.MaxHealth / 2 && _random.NextDouble() < 0.3f)
        {
            return new UseItemAction(actor, actor, potions.First());
        }

        // Attack a random target
        Character target = SelectRandomTarget(battle, actor);
        return new AttackAction(actor, target, actor.StandardAttack);
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
        List<string> options = new List<string>();
        List<IItem> items = battle.GetPartyFor(actor).Items.Where(i => !i.Used).ToList();

        options.Add($"Standard Attack ({actor.StandardAttack.Name})");
        if (items.Count > 0) options.Add("Use Item");
        options.Add("Skip Turn");

        int choice = ConsoleHelper.PromptWithMenu($"What do you want {actor.Name} to do?", options);

        if (choice == 1)
        {
            return PerformStandardAttack(battle, actor);
        }

        if (choice == 2 && items.Count > 0)
        {
            return UseItem(battle, actor, items);
        }

        return new SkipTurnAction(actor);
    }

    private static AttackAction PerformStandardAttack(Battle battle, Character actor)
    {
        var targets = battle.GetAliveEnemies(actor);
        if (targets.Count == 1) return new AttackAction(actor, targets[0], actor.StandardAttack);

        int targetChoice = ConsoleHelper.PromptWithMenu(
            $"What target do you want {actor.Name} to attack?",
           targets.Select(t => $"{t.Name} ({t.CurrentHealth}/{t.MaxHealth} HP)").ToList());

        return new AttackAction(actor, targets[targetChoice - 1], actor.StandardAttack);
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
}