namespace ShadowMaiden.Models;

public abstract class Item(int x, int y, ElementType type, string name)
    : GameElement(x, y, type)
{
    public string Name { get; } = name;
    public override bool IsPassable => true;

    public abstract void Apply(Player player);
}