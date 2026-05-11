using System;

namespace ShadowMaiden.Models;

public class Bat(int x, int y) : Enemy(x, y, ElementType.Bat, 15, 5, "Bat")
{
    public override (int dx, int dy) GetMove(Player player)
    {
        var random = new Random();
        var direction = random.Next(4);
        return direction switch
        {
            0 => (0, -1),
            1 => (0, 1),
            2 => (-1, 0),
            _ => (1, 0)
        };
    }
}