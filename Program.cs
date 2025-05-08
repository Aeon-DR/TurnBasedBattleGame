using TurnBasedBattleGame;

string protagonistName = InputHelper.ChooseHeroName("What is the protagonist's name?");

Party heroes = new Party(new List<Character> { new Protagonist(protagonistName) });
List<Party> monsters = new List<Party> 
{ 
    new Party(new List<Character> { new Skeleton() }),
    new Party(new List<Character> { new Skeleton(), new Skeleton() }),
    new Party(new List<Character> { new Antagonist() })
};

Battle battle = new Battle(new ComputerPlayer(), new ComputerPlayer(), heroes, monsters);
battle.Run();