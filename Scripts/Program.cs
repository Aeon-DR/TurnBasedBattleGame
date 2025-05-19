using TurnBasedBattleGame;

string protagonistName = ConsoleHelper.ChooseHeroName("What is the protagonist's name?");

int gameMode = ConsoleHelper.PromptWithMenu(
    "What game mode do you want to play?",
    ["Human vs Human", "Human vs Computer", "Computer vs Computer"]);

IPlayer p1 = (gameMode == 1 || gameMode == 2) ? new HumanPlayer() : new ComputerPlayer();
IPlayer p2 = (gameMode == 1) ? new HumanPlayer() : new ComputerPlayer();

Party heroes = new Party(
    new List<Character> { new Protagonist(protagonistName) },
    new List<IItem> { new HealthPotion(), new HealthPotion(), new HealthPotion() });

List<Party> monsters = new List<Party>
{
    new Party(
        new List<Character> { new Skeleton() },
        new List<IItem> { new HealthPotion() }
    ),
    new Party(
        new List<Character> { new Skeleton(), new Skeleton() },
        new List<IItem> { new HealthPotion() }
    ),
    new Party(
        new List<Character> { new Antagonist() },
        new List<IItem> { new HealthPotion() }
    )
};

Battle battle = new Battle(p1, p2, heroes, monsters);
battle.Run();