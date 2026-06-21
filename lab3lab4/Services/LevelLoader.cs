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
        return Build(File.ReadAllLines(path));
    }

    public static GameField Build(string[] lines)
    {
        var height = lines.Length;
        var width = lines.Max(l => l.Length);
        var field = new GameField(width, height);

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                var c = x < lines[y].Length ? lines[y][x] : '#';

                field[x, y] = c switch
                {
                    '#' => new Wall(x, y),
                    'D' => new Door(x, y),
                    'E' => new Exit(x, y),
                    _ => new Floor(x, y)
                };

                switch (c)
                {
                    case '@':
                        field.Player = new Player(x, y);
                        break;
                    case 'S':
                        field.Enemies.Add(new Skeleton(x, y));
                        break;
                    case 'B':
                        field.Enemies.Add(new Bat(x, y));
                        break;
                    case 'X':
                        field.Enemies.Add(new Boss(x, y));
                        break;
                    case 's':
                        field.Items.Add(new Sword(x, y));
                        break;
                    case 'p':
                        field.Items.Add(new Potion(x, y));
                        break;
                    case 'k':
                        field.Items.Add(new Key(x, y));
                        break;
                }
            }
        }

        return field;
    }
}