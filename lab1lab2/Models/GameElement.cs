namespace ShadowMaiden.Models;

public abstract class GameElement(int x, int y, ElementType type)
{
    public int X { get; set; } = x;
    public int Y { get; set; } = y;
    public ElementType Type { get; } = type;
    public abstract bool IsPassable { get; }
}
