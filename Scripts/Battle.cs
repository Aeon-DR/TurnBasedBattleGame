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
                AdvanceToNextBattle();
                continue;
            }

            // Run the monster party
            HandlePartyTurn(MonstersPlayer, Monsters);
            if (WinnerDetermined) break;
            if (MonsterPartyEliminated)
            {
                AdvanceToNextBattle();
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

            DisplayBattleStatus();
            ConsoleHelper.WriteColoredLine($"It is {character.Name}'s turn.", ConsoleColor.DarkYellow);
            IAction chosenAction = player.ChooseAction(this, character);
            chosenAction.Perform();

            if (WinnerDetermined) return;
        }

        RemoveDeadCharacters();
        RemoveUsedItems();
    }

    private void RemoveDeadCharacters()
    {
        Heroes.Characters.RemoveAll(c => !c.IsAlive);
        Monsters.Characters.RemoveAll(c => !c.IsAlive);
    }

    private void RemoveUsedItems()
    {
        Heroes.Items.RemoveAll(i => i.Used);
        Monsters.Items.RemoveAll(i => i.Used);
    }

    private void DisplayBattleStatus()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;

        Console.WriteLine("\n============ BATTLE ============");
        foreach(Character character in Heroes.Characters.Where(c => c.IsAlive))
        {
            Console.WriteLine(character.GetCharacterInfo());
        }
        Console.WriteLine("------------- VS ---------------");
        foreach (Character character in Monsters.Characters.Where(c => c.IsAlive))
        {
            Console.WriteLine(character.GetCharacterInfo());
        }
        Console.WriteLine("================================\n");

        Console.ForegroundColor = ConsoleColor.Gray;
    }

    private void AnnounceWinner()
    {
        Console.WriteLine();

        if (HeroPartyEliminated)
        {
            ConsoleHelper.WriteColoredLine("The Uncoded One’s forces have prevailed, the heroes have perished...", ConsoleColor.Red);
        }
        else
        {
            ConsoleHelper.WriteColoredLine("The Uncoded One has been vanquished, the heroes can celebrate the victory!", ConsoleColor.DarkGreen);
        }
    }

    private void AdvanceToNextBattle()
    {
        ConsoleHelper.WriteColoredLine("\nThe enemy party has been defeated!", ConsoleColor.Blue);

        Console.ForegroundColor = ConsoleColor.DarkGreen;
        TransferDefeatedPartyItems();
        TransferDefeatedPartyGear();

        ConsoleHelper.WriteColoredLine("\nAdvancing to the next battle now!", ConsoleColor.Blue);
        _battleNumber++;
    }

    public Party GetPartyFor(Character character) => Heroes.Characters.Contains(character) ? Heroes : Monsters;
    public Party GetEnemyPartyFor(Character character) => Heroes.Characters.Contains(character) ? Monsters : Heroes;

    public List<Character> GetAliveEnemies(Character character)
    {
        return GetEnemyPartyFor(character).Characters.Where(c => c.IsAlive).ToList();
    }

    public List<Character> GetAliveAllies(Character character)
    {
        return GetPartyFor(character).Characters.Where(c => c.IsAlive).ToList();
    }

    public List<IGear> GetPartyGear(Character character)
    {
        return GetPartyFor(character).Gear;
    }

    private void TransferDefeatedPartyItems()
    {
        if (Monsters.Items.Count == 0) return;

        foreach (IItem item in Monsters.Items)
        {
            Heroes.Items.Add(item);
            Console.WriteLine($"Acquired {item.Name} from the defeated party!");
        }

        Monsters.Items.Clear();
    }

    private void TransferDefeatedPartyGear()
    {
        if (Monsters.Gear.Count == 0) return; 

        foreach (IGear gear in Monsters.Gear)
        {
            Heroes.Gear.Add(gear);
            Console.WriteLine($"Acquired {gear.Name} from the defeated party!");
        }

        Monsters.Gear.Clear();
    }
}
