using TurnBasedBattleGame;

string protagonistName = InputHelper.ChooseHeroName("What is the protagonist's name?");

Party heroes = new Party(new List<Character> { new Protagonist(protagonistName) });
Party monsters = new Party(new List<Character> { new Skeleton() });

Battle battle = new Battle(new ComputerPlayer(), new ComputerPlayer(), heroes, monsters);
battle.Run();