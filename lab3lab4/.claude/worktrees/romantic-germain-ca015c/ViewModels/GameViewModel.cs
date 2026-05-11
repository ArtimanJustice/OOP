using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ShadowMaiden.Models;
using ShadowMaiden.Services;

namespace ShadowMaiden.ViewModels;

public class GameViewModel : BaseViewModel
{
    private GameField _field;
    private int _currentLevel;
    private readonly int _totalLevels;
    private string _playerStats;
    private string _gameMessage;
    private bool _isGameOver;
    private bool _isVictory;

    public ObservableCollection<CellInfo> Cells { get; }
    public int FieldWidth => _field?.Width ?? 0;
    public int FieldHeight => _field?.Height ?? 0;

    public string PlayerStats
    {
        get => _playerStats;
        private set => SetField(ref _playerStats, value);
    }

    public string GameMessage
    {
        get => _gameMessage;
        private set => SetField(ref _gameMessage, value);
    }

    public bool IsGameOver
    {
        get => _isGameOver;
        set => SetField(ref _isGameOver, value);
    }

    public bool IsVictory
    {
        get => _isVictory;
        set => SetField(ref _isVictory, value);
    }

    public ICommand MoveUpCommand { get; }
    public ICommand MoveDownCommand { get; }
    public ICommand MoveLeftCommand { get; }
    public ICommand MoveRightCommand { get; }
    public ICommand RestartCommand { get; }

    public GameViewModel()
    {
        Cells = [];
        _currentLevel = 1;
        _totalLevels = 3;

        MoveUpCommand = new RelayCommand(_ => MovePlayer(0, -1), _ => !_isGameOver);
        MoveDownCommand = new RelayCommand(_ => MovePlayer(0, 1), _ => !_isGameOver);
        MoveLeftCommand = new RelayCommand(_ => MovePlayer(-1, 0), _ => !_isGameOver);
        MoveRightCommand = new RelayCommand(_ => MovePlayer(1, 0), _ => !_isGameOver);
        RestartCommand = new RelayCommand(_ => StartGame());

        StartGame();
    }

    private void StartGame()
    {
        _currentLevel = 1;
        IsGameOver = false;
        IsVictory = false;
        GameMessage = "Enter the dungeon...";
        LoadLevel(_currentLevel);
    }

    private void LoadLevel(int level)
    {
        _field = LevelLoader.Load(level);
        OnPropertyChanged(nameof(FieldWidth));
        OnPropertyChanged(nameof(FieldHeight));
        InitializeCells();
        UpdateCells();
        UpdateStats();
        GameMessage = $"Level {level} — Find the exit!";
    }

    private void InitializeCells()
    {
        Cells.Clear();
        for (var y = 0; y < _field.Height; y++)
        for (var x = 0; x < _field.Width; x++)
            Cells.Add(new CellInfo(x, y));
    }

    private void MovePlayer(int dx, int dy)
    {
        if (_isGameOver || _field == null)
            return;

        var newX = _field.Player.X + dx;
        var newY = _field.Player.Y + dy;

        var target = _field[newX, newY];
        switch (target)
        {
            case null:
            case Wall:
                return;
            case Door { IsOpen: false } door:
            {
                if (_field.Player.Keys > 0)
                {
                    _field.Player.Keys--;
                    door.IsOpen = true;
                    GameMessage = "Door opened!";
                }
                else
                {
                    GameMessage = "You need a key!";
                    UpdateCells();
                    UpdateStats();
                    return;
                }

                break;
            }
        }

        var enemy = _field.Enemies.FirstOrDefault(e => e.X == newX && e.Y == newY && e.IsAlive);
        if (enemy != null)
        {
            enemy.TakeDamage(_field.Player.Attack);
            GameMessage = $"Hit {enemy.Name}! (-{_field.Player.Attack} HP)";

            if (!enemy.IsAlive)
            {
                _field.RemoveEnemy(enemy);
                GameMessage = $"{enemy.Name} defeated!";
            }
            else
            {
                _field.Player.TakeDamage(enemy.Attack);
                GameMessage += $" | {enemy.Name} strikes back! (-{enemy.Attack} HP)";

                if (_field.Player.Hp <= 0)
                {
                    IsGameOver = true;
                    GameMessage = "You have fallen in battle...";
                    UpdateCells();
                    UpdateStats();
                    return;
                }

                MoveEnemies();
                UpdateCells();
                UpdateStats();
                return;
            }
        }

        var item = _field.Items.FirstOrDefault(i => i.X == newX && i.Y == newY);
        if (item != null)
        {
            item.Apply(_field.Player);
            _field.RemoveItem(item);
            GameMessage = $"Picked up {item.Name}!";
        }

        if (target is Exit)
        {
            if (_currentLevel >= _totalLevels)
            {
                IsVictory = true;
                IsGameOver = true;
                GameMessage = "Victory! The Shadow Maiden conquers the dungeon!";
                UpdateCells();
                UpdateStats();
                return;
            }

            var hp = _field.Player.Hp;
            var attack = _field.Player.Attack;
            var keys = _field.Player.Keys;
            _currentLevel++;
            LoadLevel(_currentLevel);
            _field.Player.Hp = hp;
            _field.Player.Attack = attack;
            _field.Player.Keys = keys;
            UpdateStats();
            return;
        }

        _field[_field.Player.X, _field.Player.Y] = new Floor(_field.Player.X, _field.Player.Y);
        _field.Player.X = newX;
        _field.Player.Y = newY;

        MoveEnemies();
        UpdateCells();
        UpdateStats();
    }

    private void MoveEnemies()
    {
        foreach (var enemy in _field.Enemies.ToList().Where(enemy => enemy.IsAlive))
        {
            var (dx, dy) = enemy.GetMove(_field.Player);
            var newX = enemy.X + dx;
            var newY = enemy.Y + dy;

            if (newX == _field.Player.X && newY == _field.Player.Y)
            {
                _field.Player.TakeDamage(enemy.Attack);
                GameMessage = $"{enemy.Name} attacks! (-{enemy.Attack} HP)";

                if (_field.Player.Hp <= 0)
                {
                    IsGameOver = true;
                    GameMessage = "You have fallen in battle...";
                    return;
                }
                continue;
            }

            var target = _field[newX, newY];
            if (target is not { IsPassable: true } ||
                _field.Enemies.Any(e => e != enemy && e.X == newX && e.Y == newY)) continue;
            enemy.X = newX;
            enemy.Y = newY;
        }
    }

    private void UpdateCells()
    {
        if (_field == null)
            return;

        foreach (var cell in Cells)
        {
            var element = _field[cell.X, cell.Y];

            cell.Background = "#1a1a2e";
            cell.Text = "";
            cell.Foreground = "#ffffff";

            switch (element)
            {
                case Wall:
                    cell.Background = "#3a3a4e";
                    cell.Text = "";
                    break;
                case Door door:
                    cell.Background = door.IsOpen ? "#1a1a2e" : "#8B4513";
                    cell.Text = door.IsOpen ? "" : "D";
                    cell.Foreground = "#FFD700";
                    break;
                case Exit:
                    cell.Background = "#004400";
                    cell.Text = "E";
                    cell.Foreground = "#00FF00";
                    break;
            }

            var item = _field.Items.FirstOrDefault(i => i.X == cell.X && i.Y == cell.Y);
            if (item != null)
            {
                cell.Text = item.Type switch
                {
                    ElementType.Sword => "\u2694",
                    ElementType.Potion => "\u2665",
                    ElementType.Key => "\u26B7",
                    _ => "?"
                };
                cell.Foreground = item.Type switch
                {
                    ElementType.Sword => "#00BFFF",
                    ElementType.Potion => "#FF69B4",
                    ElementType.Key => "#FFD700",
                    _ => "#ffffff"
                };
            }

            var enemyOnCell = _field.Enemies.FirstOrDefault(
                e => e.X == cell.X && e.Y == cell.Y && e.IsAlive);
            if (enemyOnCell != null)
            {
                cell.Text = enemyOnCell.Type switch
                {
                    ElementType.Skeleton => "\u2620",
                    ElementType.Bat => "\u25BC",
                    ElementType.Boss => "\u25C6",
                    _ => "?"
                };
                cell.Foreground = enemyOnCell.Type switch
                {
                    ElementType.Skeleton => "#E0E0E0",
                    ElementType.Bat => "#9370DB",
                    ElementType.Boss => "#FF4444",
                    _ => "#ffffff"
                };
            }

            if (_field.Player.X != cell.X || _field.Player.Y != cell.Y) continue;
            cell.Text = "\u2640";
            cell.Foreground = "#FFD700";
        }
    }

    private void UpdateStats()
    {
        if (_field?.Player == null)
            return;
        var p = _field.Player;
        PlayerStats = $"HP: {p.Hp}/{Player.MaxHp}    ATK: {p.Attack}    Keys: {p.Keys}    Level: {_currentLevel}";
    }
}