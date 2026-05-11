namespace ShadowMaiden.Models;

public class Key(int x, int y) : Item(x, y, ElementType.Key, "Key")
{
    public override void Apply(Player player)
    {
        player.Keys++;
    }
}