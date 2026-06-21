using System;
using System.Collections.Generic;
using System.Linq;

namespace ShadowMaiden.Models;

public abstract class Enemy(int x, int y, ElementType type, int hp, int attack, string name)
    : GameElement(x, y, type)
{
    private int Hp { get; set; } = hp;
    public int Attack { get; } = attack;
    public string Name { get; } = name;
    public bool IsAlive => Hp > 0;
    public virtual bool CanFly => false;
    public override bool IsPassable => false;

    public void TakeDamage(int damage)
    {
        Hp = Math.Max(0, Hp - damage);
    }

    public abstract IEnumerable<(int dx, int dy)> GetMoveCandidates(Player player);

    public bool CanMoveTo(GameField field, int nx, int ny)
    {
        if (nx < 0 || nx >= field.Width || ny < 0 || ny >= field.Height) return false;
        if (field.Enemies.Exists(e => e != this && e.X == nx && e.Y == ny)) return false;
        if (CanFly) return true;
        return field[nx, ny] is { IsPassable: true };
    }

    protected IEnumerable<(int dx, int dy)> ChaseCandidates(Player player)
    {
        (int dx, int dy)[] directions = [(1, 0), (-1, 0), (0, 1), (0, -1)];
        return directions.OrderBy(d =>
            Math.Abs(player.X - (X + d.dx)) + Math.Abs(player.Y - (Y + d.dy)));
    }
}
