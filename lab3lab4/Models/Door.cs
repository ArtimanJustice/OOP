namespace ShadowMaiden.Models;

public class Door(int x, int y) : GameElement(x, y, ElementType.Door)
{
    public bool IsOpen { get; set; }
    public override bool IsPassable => IsOpen;

    public bool TryOpen(Player player)
    {
        if (IsOpen) return true;
        if (player.Keys == 0) return false;

        player.Keys--;
        IsOpen = true;
        return true;
    }
}
