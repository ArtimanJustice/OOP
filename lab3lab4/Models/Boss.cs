using System.Collections.Generic;

namespace ShadowMaiden.Models;

public class Boss(int x, int y) : Enemy(x, y, ElementType.Boss, 80, 15, "Shadow Lord")
{
    public override IEnumerable<(int dx, int dy)> GetMoveCandidates(Player player) =>
        ChaseCandidates(player);
}
