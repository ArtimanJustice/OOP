using System;
using ShadowMaiden.Models;

namespace ShadowMaiden.Services;

public static class ConsoleRenderer
{
    private const int CellWidth = 2;

    public static void Render(GameField field, string message, int currentLevel, int totalLevels,
        bool isGameOver, bool isVictory)
    {
        int frameWidth = field.Width * CellWidth;
        int statusWidth = Math.Max(frameWidth, 50);
        EnsureSize(frameWidth, field.Height);

        var line = 0;

        for (var y = 0; y < field.Height; y++)
        {
            Console.SetCursorPosition(0, line++);
            for (var x = 0; x < field.Width; x++)
            {
                var (symbol, fg) = GetCell(field, x, y);
                Console.ForegroundColor = fg;
                Console.Write(symbol);
            }

            Console.ResetColor();
        }

        WriteAt(line++, "", statusWidth);

        var hpText = $"{field.Player.Hp}/{Player.MaxHp}";
        Console.SetCursorPosition(0, line++);
        Console.Write("HP: ");
        Console.ForegroundColor = field.Player.Hp > 50
            ? ConsoleColor.Green
            : field.Player.Hp > 25
                ? ConsoleColor.Yellow
                : ConsoleColor.Red;
        Console.Write(hpText);
        Console.ResetColor();
        var stats = $"   ATK: {field.Player.Attack}   Keys: {field.Player.Keys}   Level: {currentLevel}/{totalLevels}";
        Console.Write(stats.PadRight(Math.Max(stats.Length, statusWidth - 4 - hpText.Length)));

        Console.ForegroundColor = ConsoleColor.Cyan;
        WriteAt(line++, message, statusWidth);
        Console.ResetColor();

        if (isGameOver)
        {
            Console.ForegroundColor = isVictory ? ConsoleColor.Green : ConsoleColor.Red;
            WriteAt(line++, isVictory ? "=== VICTORY! ===" : "=== GAME OVER ===", statusWidth);
            Console.ResetColor();
            WriteAt(line, "[R] Restart   [Esc] Quit", statusWidth);
        }
        else
        {
            WriteAt(line++, "[Arrows / WASD] Move   [Esc] Quit", statusWidth);
            WriteAt(line, "", statusWidth);
        }
    }

    private static void WriteAt(int y, string text, int width)
    {
        Console.SetCursorPosition(0, y);
        Console.Write(text.Length >= width ? text : text.PadRight(width));
    }

    private static void EnsureSize(int frameWidth, int fieldHeight)
    {
        if (!OperatingSystem.IsWindows()) return;

        int needWidth = frameWidth + 2;
        int needHeight = fieldHeight + 8;
        try
        {
            if (Console.BufferWidth < needWidth) Console.BufferWidth = needWidth;
            if (Console.WindowWidth < needWidth)
                Console.WindowWidth = Math.Min(needWidth, Console.LargestWindowWidth);
            if (Console.BufferHeight < needHeight) Console.BufferHeight = needHeight;
            if (Console.WindowHeight < needHeight)
                Console.WindowHeight = Math.Min(needHeight, Console.LargestWindowHeight);
        }
        catch
        {
        }
    }

    private static (string symbol, ConsoleColor fg) GetCell(GameField field, int x, int y)
    {
        if (field.Player.X == x && field.Player.Y == y)
            return ("♀ ", ConsoleColor.Yellow);

        var enemy = field.Enemies.Find(e => e.X == x && e.Y == y && e.IsAlive);
        if (enemy != null)
            return enemy.Type switch
            {
                ElementType.Skeleton => ("☠ ", ConsoleColor.Gray),
                ElementType.Bat => ("▼ ", ConsoleColor.Magenta),
                ElementType.Boss => ("◆ ", ConsoleColor.Red),
                _ => ("? ", ConsoleColor.White)
            };

        var item = field.Items.Find(i => i.X == x && i.Y == y);
        if (item != null)
            return item.Type switch
            {
                ElementType.Sword => ("⚔ ", ConsoleColor.Cyan),
                ElementType.Potion => ("♥ ", ConsoleColor.Magenta),
                ElementType.Key => ("⚷ ", ConsoleColor.DarkYellow),
                _ => ("? ", ConsoleColor.White)
            };

        return field[x, y] switch
        {
            Wall => ("██", ConsoleColor.DarkGray),
            Door { IsOpen: false } => ("▒ ", ConsoleColor.DarkYellow),
            Door { IsOpen: true } => ("  ", ConsoleColor.Black),
            Exit => ("✦ ", ConsoleColor.Green),
            _ => ("  ", ConsoleColor.DarkGray)
        };
    }
}
