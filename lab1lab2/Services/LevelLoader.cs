using System;
using System.IO;
using System.Linq;
using ShadowMaiden.Models;

namespace ShadowMaiden.Services;

public static class LevelLoader
{
    public static GameField Load(int levelNumber)
    {
        var path = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, "Levels", $"level{levelNumber}.txt");
        var lines = File.ReadAllLines(path);

        var height = lines.Length;
        var width = lines.Max(l => l.Length);
        var field = new GameField(width, height);

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                var c = x < lines[y].Length ? lines[y][x] : '#';

                switch (c)
                {
                    case '#':
                        field[x, y] = new Wall(x, y);
                        break;
                    case '@':
                        field[x, y] = new Floor(x, y);
                        field.Player = new Player(x, y);
                        break;
                    case 'S':
                        field[x, y] = new Floor(x, y);
                        field.Enemies.Add(new Skeleton(x, y));
                        break;
                    case 'B':
                        field[x, y] = new Floor(x, y);
                        field.Enemies.Add(new Bat(x, y));
                        break;
                    case 'X':
                        field[x, y] = new Floor(x, y);
                        field.Enemies.Add(new Boss(x, y));
                        break;
                    case 's':
                        field[x, y] = new Floor(x, y);
                        field.Items.Add(new Sword(x, y));
                        break;
                    case 'p':
                        field[x, y] = new Floor(x, y);
                        field.Items.Add(new Potion(x, y));
                        break;
                    case 'k':
                        field[x, y] = new Floor(x, y);
                        field.Items.Add(new Key(x, y));
                        break;
                    case 'D':
                        field[x, y] = new Door(x, y);
                        break;
                    case 'E':
                        field[x, y] = new Exit(x, y);
                        break;
                    default:
                        field[x, y] = new Floor(x, y);
                        break;
                }
            }
        }

        return field;
    }
}
