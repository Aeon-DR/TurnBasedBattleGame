namespace TurnBasedBattleGame;

public class Battle
{
    public IPlayer HeroesPlayer { get; }
    public IPlayer MonstersPlayer { get; }

    public Party Heroes { get; }
    public Party Monsters { get; }

    private bool WinnerDetermined => !Heroes.Characters.Any(c => c.IsAlive) || !Monsters.Characters.Any(c => c.IsAlive);

    public Battle(IPlayer heroesPlayer, IPlayer monstersPlayer, Party heroes, Party monsters)
    {
        HeroesPlayer = heroesPlayer;
        MonstersPlayer = monstersPlayer;
        Heroes = heroes;
        Monsters = monsters;
    }

    public void Run()
    {
        while (!WinnerDetermined)
        {
            HandlePartyTurn(HeroesPlayer, Heroes);
            if (WinnerDetermined) break;
            HandlePartyTurn(MonstersPlayer, Monsters); 
        }

        AnnounceWinner();
    }

    private void HandlePartyTurn(IPlayer player, Party party)
    {
        foreach(Character character in party.Characters)
        {
            if (!character.IsAlive) continue;

            Console.WriteLine();
            Console.WriteLine($"It is {character.Name}'s turn.");
            IAction chosenAction = player.ChooseAction(this, character);
            chosenAction.Perform();

            if (WinnerDetermined) return;
        }

        RemoveDeadCharacters();
    }

    private void RemoveDeadCharacters()
    {
        Heroes.Characters.RemoveAll(c => !c.IsAlive);
        Monsters.Characters.RemoveAll(c => !c.IsAlive);
    }

    private void AnnounceWinner()
    {
        Console.WriteLine();

        if (Heroes.Characters.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("The Uncoded One’s forces have prevailed, the heroes have perished...");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("The Uncoded One has been vanquished, the heroes can celebrate the victory!");
        }

        Console.ForegroundColor = ConsoleColor.Gray;
    }

    public Party GetPartyFor(Character character) => Heroes.Characters.Contains(character) ? Heroes : Monsters;
    public Party GetEnemyPartyFor(Character character) => Heroes.Characters.Contains(character) ? Monsters : Heroes;
}
