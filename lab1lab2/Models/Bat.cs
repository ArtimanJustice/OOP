using System;

namespace ShadowMaiden.Models;

public class Bat(int x, int y) : Enemy(x, y, ElementType.Bat, 15, 5, "Bat")
{
    private static readonly Random Rng = new();
    public override bool CanFly => true;

    public override (int dx, int dy) GetMove(Player player)
    {
        return Rng.Next(4) switch
        {
            0 => (0, -1),
            1 => (0, 1),
            2 => (-1, 0),
            _ => (1, 0)
        };
    }
}
