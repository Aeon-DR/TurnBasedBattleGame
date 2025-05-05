namespace TurnBasedBattleGame;

public class Battle
{
    public IPlayer HeroesPlayer { get; }
    public IPlayer MonstersPlayer { get; }

    public Party Heroes { get; }
    public Party Monsters => _monsterParties[_battleNumber];

    private readonly List<Party> _monsterParties;
    private int _battleNumber;

    private bool WinnerDetermined => HeroPartyEliminated || AllMonsterPartiesEliminated;
    private bool HeroPartyEliminated => !Heroes.Characters.Any(c => c.IsAlive);
    private bool MonsterPartyEliminated => !Monsters.Characters.Any(c => c.IsAlive);
    private bool AllMonsterPartiesEliminated => _battleNumber == _monsterParties.Count - 1 && MonsterPartyEliminated;

    public Battle(IPlayer heroesPlayer, IPlayer monstersPlayer, Party heroes, List<Party> monsterParties)
    {
        HeroesPlayer = heroesPlayer;
        MonstersPlayer = monstersPlayer;
        Heroes = heroes;
        _monsterParties = monsterParties;
        _battleNumber = 0;
    }

    public void Run()
    {
        while (true)
        {
            // Run the hero party
            HandlePartyTurn(HeroesPlayer, Heroes);
            if (WinnerDetermined) break;
            if (MonsterPartyEliminated)
            {
                Console.WriteLine("\nAdvancing to the next battle now!");
                _battleNumber++;
                continue;
            }

            // Run the monster party
            HandlePartyTurn(MonstersPlayer, Monsters);
            if (WinnerDetermined) break;
            if (MonsterPartyEliminated)
            {
                Console.WriteLine("\nAdvancing to the next battle now!");
                _battleNumber++;
                continue;
            }
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

        if (HeroPartyEliminated)
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
