using System.Collections.Generic;

namespace ShadowMaiden.Models;

public class GameField(int width, int height)
{
    private readonly GameElement[,] _cells = new GameElement[width, height];
    public int Width { get; } = width;
    public int Height { get; } = height;
    public Player Player { get; set; }
    public List<Enemy> Enemies { get; } = [];
    public List<Item> Items { get; } = [];

    public GameElement this[int x, int y]
    {
        get
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
                return null;
            return _cells[x, y];
        }
        set
        {
            if (x >= 0 && x < Width && y >= 0 && y < Height)
                _cells[x, y] = value;
        }
    }

    public void RemoveEnemy(Enemy enemy)
    {
        Enemies.Remove(enemy);
    }

    public void RemoveItem(Item item)
    {
        Items.Remove(item);
    }
}