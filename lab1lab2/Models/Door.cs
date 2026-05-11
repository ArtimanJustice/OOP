namespace ShadowMaiden.Models;

public class Door(int x, int y) : GameElement(x, y, ElementType.Door)
{
    public bool IsOpen { get; set; }
    public override bool IsPassable => IsOpen;
}
