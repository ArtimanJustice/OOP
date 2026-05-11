namespace ShadowMaiden.Models;

public class Potion(int x, int y) : Item(x, y, ElementType.Potion, "Potion")
{
    private static int HealAmount => 25;

    public override void Apply(Player player)
    {
        player.Heal(HealAmount);
    }
}
