using System.Linq;
using ShadowMaiden.Models;
using ShadowMaiden.ViewModels;

namespace ShadowMaiden.Services;

public static class CellRenderer
{
    public static void Render(GameField field, CellInfo cell)
    {
        cell.Background = "#1a1a2e";
        cell.Text = "";
        cell.Foreground = "#ffffff";

        switch (field[cell.X, cell.Y])
        {
            case Wall:
                cell.Background = "#252535";
                var flyingEnemy =
                    field.Enemies.FirstOrDefault(e => e.CanFly && e.X == cell.X && e.Y == cell.Y && e.IsAlive);
                if (flyingEnemy != null)
                    (cell.Text, cell.Foreground) = flyingEnemy.Type switch
                    {
                        ElementType.Bat => ("▼", "#9370DB"),
                        _ => ("?", "#ffffff")
                    };
                return;
            case Door { IsOpen: false }:
                cell.Background = "#5C2D00";
                cell.Text = "▬";
                cell.Foreground = "#CD853F";
                return;
            case Door { IsOpen: true }:
                return;
            case Exit:
                cell.Background = "#003322";
                cell.Text = "✦";
                cell.Foreground = "#00FF88";
                break;
        }

        var item = field.Items.FirstOrDefault(i => i.X == cell.X && i.Y == cell.Y);
        if (item != null)
            (cell.Text, cell.Foreground) = item.Type switch
            {
                ElementType.Sword => ("⚔", "#00BFFF"),
                ElementType.Potion => ("♥", "#FF69B4"),
                ElementType.Key => ("⚷", "#FFD700"),
                _ => ("?", "#ffffff")
            };

        var enemy = field.Enemies.FirstOrDefault(e => e.X == cell.X && e.Y == cell.Y && e.IsAlive);
        if (enemy != null)
            (cell.Text, cell.Foreground) = enemy.Type switch
            {
                ElementType.Skeleton => ("☠", "#C0C0C0"),
                ElementType.Bat => ("▼", "#9370DB"),
                ElementType.Boss => ("◆", "#FF3333"),
                _ => ("?", "#ffffff")
            };

        if (field.Player.X == cell.X && field.Player.Y == cell.Y)
        {
            cell.Text = "♀";
            cell.Foreground = "#FFD700";
        }
    }
}