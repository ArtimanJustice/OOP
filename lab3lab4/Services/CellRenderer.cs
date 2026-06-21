using System.Linq;
using ShadowMaiden.Models;
using ShadowMaiden.ViewModels;

namespace ShadowMaiden.Services;

public static class CellRenderer
{
    private const string DefaultBg = "#1a1a2e";
    private const string DefaultFg = "#ffffff";
    private const string WallBg = "#252535";
    private const string DoorBg = "#5C2D00";
    private const string DoorFg = "#CD853F";
    private const string ExitBg = "#003322";
    private const string ExitFg = "#00FF88";
    private const string SwordFg = "#00BFFF";
    private const string PotionFg = "#FF69B4";
    private const string KeyFg = "#FFD700";
    private const string SkeletonFg = "#C0C0C0";
    private const string BatFg = "#9370DB";
    private const string BossFg = "#FF3333";
    private const string PlayerFg = "#FFD700";

    private const string Empty = "";
    private const string Unknown = "?";
    private const string DoorGlyph = "▬";
    private const string ExitGlyph = "✦";
    private const string SwordGlyph = "⚔";
    private const string PotionGlyph = "♥";
    private const string KeyGlyph = "⚷";
    private const string SkeletonGlyph = "☠";
    private const string BatGlyph = "▼";
    private const string BossGlyph = "◆";
    private const string PlayerGlyph = "♀";

    public static void Render(GameField field, CellInfo cell)
    {
        cell.Background = DefaultBg;
        cell.Text = Empty;
        cell.Foreground = DefaultFg;

        switch (field[cell.X, cell.Y])
        {
            case Wall:
                cell.Background = WallBg;
                var flyingEnemy =
                    field.Enemies.FirstOrDefault(e => e.CanFly && e.X == cell.X && e.Y == cell.Y && e.IsAlive);
                if (flyingEnemy != null)
                    (cell.Text, cell.Foreground) = flyingEnemy.Type switch
                    {
                        ElementType.Bat => (BatGlyph, BatFg),
                        _ => (Unknown, DefaultFg)
                    };
                return;
            case Door { IsOpen: false }:
                cell.Background = DoorBg;
                cell.Text = DoorGlyph;
                cell.Foreground = DoorFg;
                return;
            case Door { IsOpen: true }:
                return;
            case Exit:
                cell.Background = ExitBg;
                cell.Text = ExitGlyph;
                cell.Foreground = ExitFg;
                break;
        }

        var item = field.Items.FirstOrDefault(i => i.X == cell.X && i.Y == cell.Y);
        if (item != null)
            (cell.Text, cell.Foreground) = item.Type switch
            {
                ElementType.Sword => (SwordGlyph, SwordFg),
                ElementType.Potion => (PotionGlyph, PotionFg),
                ElementType.Key => (KeyGlyph, KeyFg),
                _ => (Unknown, DefaultFg)
            };

        var enemy = field.Enemies.FirstOrDefault(e => e.X == cell.X && e.Y == cell.Y && e.IsAlive);
        if (enemy != null)
            (cell.Text, cell.Foreground) = enemy.Type switch
            {
                ElementType.Skeleton => (SkeletonGlyph, SkeletonFg),
                ElementType.Bat => (BatGlyph, BatFg),
                ElementType.Boss => (BossGlyph, BossFg),
                _ => (Unknown, DefaultFg)
            };

        if (field.Player.X == cell.X && field.Player.Y == cell.Y)
        {
            cell.Text = PlayerGlyph;
            cell.Foreground = PlayerFg;
        }
    }
}
