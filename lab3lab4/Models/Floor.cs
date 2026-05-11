namespace ShadowMaiden.Models;

public class Floor(int x, int y) : GameElement(x, y, ElementType.Floor)
{
    public override bool IsPassable => true;
}