using System;

namespace ShadowMaiden.Models;

public abstract class Enemy(int x, int y, ElementType type, int hp, int attack, string name)
    : GameElement(x, y, type)
{
    private int Hp { get; set; } = hp;
    public int Attack { get; } = attack;
    public string Name { get; } = name;
    public bool IsAlive => Hp > 0;
    public override bool IsPassable => false;

    public void TakeDamage(int damage)
    {
        Hp = Math.Max(0, Hp - damage);
    }

    public virtual (int dx, int dy) GetMove(Player player)
    {
        int dx = 0, dy = 0;
        if (Math.Abs(player.X - X) > Math.Abs(player.Y - Y))
            dx = player.X > X ? 1 : -1;
        else
            dy = player.Y > Y ? 1 : -1;
        return (dx, dy);
    }
}