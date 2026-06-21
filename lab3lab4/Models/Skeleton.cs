using System.Collections.Generic;

namespace ShadowMaiden.Models;

public class Skeleton(int x, int y) : Enemy(x, y, ElementType.Skeleton, 30, 8, "Skeleton")
{
    public override IEnumerable<(int dx, int dy)> GetMoveCandidates(Player player) =>
        ChaseCandidates(player);
}
