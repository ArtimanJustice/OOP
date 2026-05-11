using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShadowMaiden.Models;
using ShadowMaiden.Services;

namespace ShadowMaiden.Engine;

public class GameEngine
{
    private GameField _field;
    private int _currentLevel;
    private string _gameMessage = "";
    private bool _isGameOver;
    private bool _isVictory;

    public const int TotalLevels = 3;

    public void Run()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.CursorVisible = false;
        Console.Clear();

        StartGame();

        while (true)
        {
            ConsoleRenderer.Render(_field, _gameMessage, _currentLevel, TotalLevels, _isGameOver, _isVictory);

            var input = Console.ReadKey(true);

            if (_isGameOver)
            {
                if (input.Key == ConsoleKey.R)
                {
                    Console.Clear();
                    StartGame();
                }
                else if (input.Key == ConsoleKey.Escape)
                    break;
                continue;
            }

            switch (input.Key)
            {
                case ConsoleKey.UpArrow or ConsoleKey.W:
                    MovePlayer(0, -1);
                    break;
                case ConsoleKey.DownArrow or ConsoleKey.S:
                    MovePlayer(0, 1);
                    break;
                case ConsoleKey.LeftArrow or ConsoleKey.A:
                    MovePlayer(-1, 0);
                    break;
                case ConsoleKey.RightArrow or ConsoleKey.D:
                    MovePlayer(1, 0);
                    break;
                case ConsoleKey.Escape:
                    return;
            }
        }
    }

    private void StartGame()
    {
        _currentLevel = 1;
        _isGameOver = false;
        _isVictory = false;
        LoadLevel(_currentLevel);
    }

    private void LoadLevel(int level)
    {
        _field = LevelLoader.Load(level);
        _gameMessage = $"Level {level} — Find the exit!";
    }

    private void MovePlayer(int dx, int dy)
    {
        if (_isGameOver || _field == null) return;

        int newX = _field.Player.X + dx;
        int newY = _field.Player.Y + dy;

        var target = _field[newX, newY];
        switch (target)
        {
            case null:
            case Wall:
                return;
            case Door { IsOpen: false } door:
                if (_field.Player.Keys > 0)
                {
                    _field.Player.Keys--;
                    door.IsOpen = true;
                    _gameMessage = "Door unlocked!";
                }
                else
                {
                    _gameMessage = "You need a key!";
                    return;
                }
                break;
        }

        var enemy = _field.Enemies.FirstOrDefault(e => e.X == newX && e.Y == newY && e.IsAlive);
        if (enemy != null)
        {
            int selfDamage = _field.Player.Hp / 2;
            _field.Player.TakeDamage(selfDamage);
            enemy.TakeDamage(_field.Player.Attack);

            _gameMessage = $"Hit {enemy.Name}! (−{selfDamage} HP to you)";

            if (!enemy.IsAlive)
            {
                _field.RemoveEnemy(enemy);
                _gameMessage = $"{enemy.Name} defeated! (−{selfDamage} HP to you)";
            }

            if (_field.Player.Hp <= 0)
            {
                _isGameOver = true;
                _gameMessage = "You have fallen in battle...";
                return;
            }

            MoveEnemies();
            return;
        }

        var item = _field.Items.FirstOrDefault(i => i.X == newX && i.Y == newY);
        if (item != null)
        {
            item.Apply(_field.Player);
            _field.RemoveItem(item);
            _gameMessage = $"Picked up {item.Name}!";
        }

        if (target is Exit)
        {
            if (_currentLevel >= TotalLevels)
            {
                _isVictory = true;
                _isGameOver = true;
                _gameMessage = "Victory! The Shadow Maiden conquers the dungeon!";
                return;
            }

            int hp = _field.Player.Hp;
            int attack = _field.Player.Attack;
            int keys = _field.Player.Keys;
            _currentLevel++;
            LoadLevel(_currentLevel);
            _field.Player.Hp = hp;
            _field.Player.Attack = attack;
            _field.Player.Keys = keys;
            return;
        }

        _field[_field.Player.X, _field.Player.Y] = new Floor(_field.Player.X, _field.Player.Y);
        _field.Player.X = newX;
        _field.Player.Y = newY;

        MoveEnemies();
    }

    private void MoveEnemies()
    {
        foreach (var enemy in _field.Enemies.ToList())
        {
            if (!enemy.IsAlive) continue;

            var (pdx, pdy) = enemy.GetMove(_field.Player);
            var dirs = new List<(int, int)> { (pdx, pdy) };
            if (pdx != 0) { dirs.Add((0, 1)); dirs.Add((0, -1)); }
            else if (pdy != 0) { dirs.Add((1, 0)); dirs.Add((-1, 0)); }

            foreach (var (edx, edy) in dirs)
            {
                int nx = enemy.X + edx;
                int ny = enemy.Y + edy;

                if (nx == _field.Player.X && ny == _field.Player.Y)
                {
                    _field.Player.TakeDamage(enemy.Attack);
                    _gameMessage = $"{enemy.Name} attacks! (−{enemy.Attack} HP)";
                    if (_field.Player.Hp <= 0)
                    {
                        _isGameOver = true;
                        _gameMessage = "You have fallen in battle...";
                        return;
                    }
                    break;
                }

                if (CanEnemyMoveTo(enemy, nx, ny))
                {
                    enemy.X = nx;
                    enemy.Y = ny;
                    break;
                }
            }
        }
    }

    private bool CanEnemyMoveTo(Enemy enemy, int nx, int ny)
    {
        if (nx < 0 || nx >= _field.Width || ny < 0 || ny >= _field.Height) return false;
        if (_field.Enemies.Exists(e => e != enemy && e.X == nx && e.Y == ny)) return false;
        if (enemy.CanFly) return true;
        return _field[nx, ny] is { IsPassable: true };
    }
}
