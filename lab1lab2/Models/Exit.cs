namespace ShadowMaiden.Models;

public class Exit(int x, int y) : GameElement(x, y, ElementType.Exit)
{
    public override bool IsPassable => true;
}
