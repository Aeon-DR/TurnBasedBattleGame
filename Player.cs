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

    private Character SelectRandomTarget(Battle battle, Character character)
    {
        Random random = new Random();
        Party enemyParty = battle.GetEnemyPartyFor(character);
        return enemyParty.Characters[random.Next(enemyParty.Characters.Count)];
    }
}