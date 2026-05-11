using System;

namespace ShadowMaiden.Models;

public class Player(int x, int y) : GameElement(x, y, ElementType.Player)
{
    public int Hp { get; set; } = 100;
    public static int MaxHp => 100;
    public int Attack { get; set; } = 10;
    public int Keys { get; set; }
    public override bool IsPassable => false;

    public void TakeDamage(int damage)
    {
        Hp = Math.Max(0, Hp - damage);
    }

    public void Heal(int amount)
    {
        Hp = Math.Min(MaxHp, Hp + amount);
    }
}
