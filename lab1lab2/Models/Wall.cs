namespace ShadowMaiden.Models;

public class Wall(int x, int y) : GameElement(x, y, ElementType.Wall)
{
    public override bool IsPassable => false;
}
