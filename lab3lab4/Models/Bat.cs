using System;
using System.Collections.Generic;

namespace ShadowMaiden.Models;

public class Bat(int x, int y) : Enemy(x, y, ElementType.Bat, 15, 5, "Bat")
{
    private static readonly Random Rng = new();
    public override bool CanFly => true;

    public override IEnumerable<(int dx, int dy)> GetMoveCandidates(Player player)
    {
        yield return Rng.Next(4) switch
        {
            0 => (0, -1),
            1 => (0, 1),
            2 => (-1, 0),
            _ => (1, 0)
        };
    }
}
