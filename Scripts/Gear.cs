﻿namespace TurnBasedBattleGame;

public interface IGear
{
    string Name { get; }
    public IAttack Attack { get; }
}

public class Sword : IGear
{
    public string Name => "SWORD";
    public IAttack Attack => new Slash();
}

public class Dagger : IGear
{
    public string Name => "DAGGER";
    public IAttack Attack => new Stab();
}

public class VinsBow : IGear
{
    public string Name => "VIN'S BOW";
    public IAttack Attack => new QuickShot();
}