namespace TurnBasedBattleGame;

public class Party
{
    public List<Character> Characters { get; }
    public List<IItem> Items { get; }
    public List<IGear> Gear { get; }

    public Party(IEnumerable<Character> startingCharacters, IEnumerable<IItem>? startingItems = null, IEnumerable<IGear>? startingGear = null)
    {
        Characters = startingCharacters.ToList();
        Items = startingItems?.ToList() ?? new List<IItem>();
        Gear = startingGear?.ToList() ?? new List<IGear>();
    }
}
