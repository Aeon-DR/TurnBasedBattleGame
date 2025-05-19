namespace TurnBasedBattleGame;

public class Party
{
    public List<Character> Characters { get; }
    public List<IItem> Items { get; }

    public Party(IEnumerable<Character> startingCharacters, IEnumerable<IItem> startingItems)
    {
        Characters = startingCharacters.ToList();
        Items = startingItems.ToList();
    }
}
