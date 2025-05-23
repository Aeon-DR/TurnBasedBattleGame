namespace TurnBasedBattleGame;

public class Party
{
    public List<Character> Characters { get; }
    public List<IItem> Items { get; }
    public List<IGear>? Gear { get; set; }

    public Party(IEnumerable<Character> startingCharacters, IEnumerable<IItem> startingItems, IEnumerable<IGear>? startingGear = null)
    {
        Characters = startingCharacters.ToList();
        Items = startingItems.ToList();
        Gear = startingGear?.ToList();
    }
}
