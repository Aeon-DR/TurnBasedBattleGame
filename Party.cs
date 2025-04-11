namespace TurnBasedBattleGame;

public class Party
{
    public List<Character> Characters { get; }

    public Party(IEnumerable<Character> startingCharacters)
    {
        Characters = startingCharacters.ToList();
    }
}
