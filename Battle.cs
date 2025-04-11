namespace TurnBasedBattleGame;

public class Battle
{
    public IPlayer HeroesPlayer { get; }
    public IPlayer MonstersPlayer { get; }

    public Party Heroes { get; }
    public Party Monsters { get; }

    public Battle(IPlayer heroesPlayer, IPlayer monstersPlayer, Party heroes, Party monsters)
    {
        HeroesPlayer = heroesPlayer;
        MonstersPlayer = monstersPlayer;
        Heroes = heroes;
        Monsters = monsters;
    }

    public void Run()
    {
        while (true)
        {
            HandlePartyTurn(HeroesPlayer, Heroes);
            HandlePartyTurn(MonstersPlayer, Monsters);
        }
    }

    private void HandlePartyTurn(IPlayer player, Party party)
    {
        foreach(Character character in party.Characters)
        {
            Console.WriteLine();

            Console.WriteLine($"It is {character.Name}'s turn.");
            IAction chosenAction = player.ChooseAction(this, character);
            chosenAction.Perform();
        }
    }

    public Party GetPartyFor(Character character) => Heroes.Characters.Contains(character) ? Heroes : Monsters;
    public Party GetEnemyPartyFor(Character character) => Heroes.Characters.Contains(character) ? Monsters : Heroes;
}
