using System;
using ShadowMaiden.Models;

namespace ShadowMaiden.Services;

public static class ConsoleRenderer
{
    public static void Render(GameField field, string message, int currentLevel, int totalLevels,
        bool isGameOver, bool isVictory)
    {
        Console.SetCursorPosition(0, 0);

        for (var y = 0; y < field.Height; y++)
        {
            for (var x = 0; x < field.Width; x++)
            {
                var (symbol, fg) = GetCell(field, x, y);
                Console.ForegroundColor = fg;
                Console.Write(symbol);
            }
            Console.WriteLine();
        }

        Console.ResetColor();
        Console.WriteLine();

        Console.Write("HP: ");
        Console.ForegroundColor = field.Player.Hp > 50
            ? ConsoleColor.Green
            : field.Player.Hp > 25
                ? ConsoleColor.Yellow
                : ConsoleColor.Red;
        Console.Write($"{field.Player.Hp}/{Player.MaxHp}");
        Console.ResetColor();
        Console.WriteLine($"  ATK: {field.Player.Attack}  Keys: {field.Player.Keys}  Level: {currentLevel}/{totalLevels}    ");

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"{message,-50}");
        Console.ResetColor();

        if (isGameOver)
        {
            Console.ForegroundColor = isVictory ? ConsoleColor.Green : ConsoleColor.Red;
            Console.WriteLine(isVictory ? "=== VICTORY! ===" : "=== GAME OVER ===");
            Console.ResetColor();
            Console.WriteLine("[R] Restart   [Esc] Quit");
        }
        else
        {
            Console.WriteLine("[↑↓←→ / WASD] Move   [Esc] Quit");
        }
    }

    private static (char symbol, ConsoleColor fg) GetCell(GameField field, int x, int y)
    {
        if (field.Player.X == x && field.Player.Y == y)
            return ('♀', ConsoleColor.Yellow);

        var enemy = field.Enemies.Find(e => e.X == x && e.Y == y && e.IsAlive);
        if (enemy != null)
            return enemy.Type switch
            {
                ElementType.Skeleton => ('☠', ConsoleColor.Gray),
                ElementType.Bat      => ('▼', ConsoleColor.Magenta),
                ElementType.Boss     => ('◆', ConsoleColor.Red),
                _                    => ('?', ConsoleColor.White)
            };

        var item = field.Items.Find(i => i.X == x && i.Y == y);
        if (item != null)
            return item.Type switch
            {
                ElementType.Sword   => ('⚔', ConsoleColor.Cyan),
                ElementType.Potion  => ('♥', ConsoleColor.Magenta),
                ElementType.Key     => ('⚷', ConsoleColor.DarkYellow),
                _                   => ('?', ConsoleColor.White)
            };

        return field[x, y] switch
        {
            Wall                    => ('█', ConsoleColor.DarkGray),
            Door { IsOpen: false }  => ('▬', ConsoleColor.DarkYellow),
            Door { IsOpen: true }   => (' ', ConsoleColor.Black),
            Exit                    => ('✦', ConsoleColor.Green),
            _                       => (' ', ConsoleColor.DarkGray)
        };
    }
}
