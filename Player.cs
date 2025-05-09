namespace TurnBasedBattleGame;

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

    private static Character SelectRandomTarget(Battle battle, Character character)
    {
        Random random = new Random();
        var targets = battle.GetAliveEnemies(character);
        return targets[random.Next(targets.Count)];
    }
}

public class HumanPlayer : IPlayer
{
    public IAction ChooseAction(Battle battle, Character actor)
    {
        int choice = InputHelper.PromptWithMenu(
            $"What do you want {actor.Name} to do?", 
            [$"Standard Attack ({actor.StandardAttack.Name})", "Skip Turn"]);

        if (choice == 1)
        {
            var targets = battle.GetAliveEnemies(actor);
            if (targets.Count == 1) return new AttackAction(actor, targets[0], actor.StandardAttack);

            int targetChoice = InputHelper.PromptWithMenu(
                $"What target do you want {actor.Name} to attack?",
               targets.Select(t => $"{t.Name} ({t.CurrentHealth}/{t.MaxHealth} HP)").ToList());

            return new AttackAction(actor, targets[targetChoice - 1], actor.StandardAttack);
        }

        return new SkipTurnAction(actor);
    }
}