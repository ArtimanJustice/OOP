namespace ShadowMaiden.Models;

public class Sword(int x, int y) : Item(x, y, ElementType.Sword, "Sword")
{
    private static int AttackBoost => 5;

    public override void Apply(Player player)
    {
        player.Attack += AttackBoost;
    }
}
