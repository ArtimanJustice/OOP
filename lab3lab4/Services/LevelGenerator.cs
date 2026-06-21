using System;
using System.Collections.Generic;
using ShadowMaiden.Models;

namespace ShadowMaiden.Services;

public static class LevelGenerator
{
    private static readonly Random Rng = new();

    public static GameField Generate(int level)
    {
        int difficulty = Math.Min(6, Math.Max(1, level - 3));

        int width = Rng.Next(18, 26);
        int height = Rng.Next(11, 16);

        var grid = BuildLayout(width, height);
        var reachable = FloodFill(grid);
        Populate(grid, reachable, difficulty, spawnBoss: level % 3 == 0);

        return LevelLoader.Build(ToLines(grid));
    }

    private static char[,] BuildLayout(int width, int height)
    {
        var grid = new char[height, width];

        for (var y = 0; y < height; y++)
        for (var x = 0; x < width; x++)
            grid[y, x] = x == 0 || x == width - 1 || y == 0 || y == height - 1 ? '#' : '.';

        int segments = width * height / 12;
        for (var i = 0; i < segments; i++)
        {
            int sx = Rng.Next(2, width - 2);
            int sy = Rng.Next(2, height - 2);
            int length = Rng.Next(1, 5);
            bool horizontal = Rng.Next(2) == 0;
            for (var j = 0; j < length; j++)
            {
                int x = horizontal ? sx + j : sx;
                int y = horizontal ? sy : sy + j;
                if (x > 0 && x < width - 1 && y > 0 && y < height - 1)
                    grid[y, x] = '#';
            }
        }

        int exitRow = Rng.Next(1, height - 1);
        for (var y = 1; y < height - 1; y++)
            grid[y, 1] = '.';
        for (var x = 1; x < width - 2; x++)
            grid[exitRow, x] = '.';

        grid[1, 1] = '@';
        grid[exitRow, width - 2] = 'D';
        grid[exitRow, width - 1] = 'E';

        return grid;
    }

    private static bool[,] FloodFill(char[,] grid)
    {
        int height = grid.GetLength(0);
        int width = grid.GetLength(1);
        var visited = new bool[height, width];

        var queue = new Queue<(int x, int y)>();
        queue.Enqueue((1, 1));
        visited[1, 1] = true;

        int[] dx = [0, 0, 1, -1];
        int[] dy = [1, -1, 0, 0];

        while (queue.Count > 0)
        {
            var (x, y) = queue.Dequeue();
            for (var d = 0; d < 4; d++)
            {
                int nx = x + dx[d];
                int ny = y + dy[d];
                if (nx < 0 || nx >= width || ny < 0 || ny >= height) continue;
                if (visited[ny, nx]) continue;
                if (grid[ny, nx] != '.' && grid[ny, nx] != '@') continue;
                visited[ny, nx] = true;
                queue.Enqueue((nx, ny));
            }
        }

        return visited;
    }

    private static void Populate(char[,] grid, bool[,] reachable, int difficulty, bool spawnBoss)
    {
        int height = grid.GetLength(0);
        int width = grid.GetLength(1);

        var free = new List<(int x, int y)>();
        for (var y = 0; y < height; y++)
        for (var x = 0; x < width; x++)
            if (reachable[y, x] && grid[y, x] == '.')
                free.Add((x, y));

        Shuffle(free);

        Place(grid, free, 'k', _ => true);

        bool FarFromStart((int x, int y) c) => Math.Abs(c.x - 1) + Math.Abs(c.y - 1) >= 3;

        int skeletons = 1 + difficulty;
        int bats = difficulty;
        int potions = 1 + difficulty / 2;

        for (var i = 0; i < skeletons; i++) Place(grid, free, 'S', FarFromStart);
        for (var i = 0; i < bats; i++) Place(grid, free, 'B', FarFromStart);
        if (spawnBoss) Place(grid, free, 'X', FarFromStart);

        Place(grid, free, 's', _ => true);
        for (var i = 0; i < potions; i++) Place(grid, free, 'p', _ => true);
    }

    private static void Place(char[,] grid, List<(int x, int y)> free, char spawn,
        Func<(int x, int y), bool> accept)
    {
        for (var i = 0; i < free.Count; i++)
        {
            if (!accept(free[i])) continue;
            var (x, y) = free[i];
            grid[y, x] = spawn;
            free.RemoveAt(i);
            return;
        }
    }

    private static void Shuffle<T>(IList<T> list)
    {
        for (var i = list.Count - 1; i > 0; i--)
        {
            int j = Rng.Next(i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

    private static string[] ToLines(char[,] grid)
    {
        int height = grid.GetLength(0);
        int width = grid.GetLength(1);
        var lines = new string[height];
        for (var y = 0; y < height; y++)
        {
            var chars = new char[width];
            for (var x = 0; x < width; x++)
                chars[x] = grid[y, x];
            lines[y] = new string(chars);
        }

        return lines;
    }
}
