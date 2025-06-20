﻿namespace TurnBasedBattleGame;

public interface IItem
{
    string Name { get; }
    int Power { get; }
    bool Used { get; set; }
}

public class HealthPotion : IItem
{
    public string Name => "HEALTH POTION";
    public int Power => 10;
    public bool Used { get; set; }
}
