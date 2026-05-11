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
    private string _gameMessage;
    private bool _isGameOver;
    private bool _isVictory;

    public const int TotalLevels = 3;

    public ObservableCollection<CellInfo> Cells { get; } = [];

    public int FieldWidth => _field?.Width ?? 0;
    public int FieldHeight => _field?.Height ?? 0;

    public int PlayerHp => _field?.Player?.Hp ?? 0;
    public int PlayerMaxHp => Player.MaxHp;
    public int PlayerAttack => _field?.Player?.Attack ?? 0;
    public int PlayerKeys => _field?.Player?.Keys ?? 0;
    public int CurrentLevel => _currentLevel;
    public string LevelDisplay => $"{_currentLevel} / {TotalLevels}";

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
        LoadLevel(_currentLevel);
    }

    private void LoadLevel(int level)
    {
        _field = LevelLoader.Load(level);
        OnPropertyChanged(nameof(FieldWidth));
        OnPropertyChanged(nameof(FieldHeight));
        InitializeCells();
        Render();
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
                    GameMessage = "Door unlocked!";
                }
                else
                {
                    GameMessage = "You need a key!";
                    Render();
                    UpdateStats();
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

            GameMessage = $"Hit {enemy.Name}! (−{selfDamage} HP to you)";

            if (!enemy.IsAlive)
            {
                _field.RemoveEnemy(enemy);
                GameMessage = $"{enemy.Name} defeated! (−{selfDamage} HP to you)";
            }

            if (_field.Player.Hp <= 0)
            {
                IsGameOver = true;
                GameMessage = "You have fallen in battle...";
                Render();
                UpdateStats();
                return;
            }

            MoveEnemies();
            Render();
            UpdateStats();
            return;
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
            if (_currentLevel >= TotalLevels)
            {
                IsVictory = true;
                IsGameOver = true;
                GameMessage = "Victory! The Shadow Maiden conquers the dungeon!";
                Render();
                UpdateStats();
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
            UpdateStats();
            return;
        }

        _field[_field.Player.X, _field.Player.Y] = new Floor(_field.Player.X, _field.Player.Y);
        _field.Player.X = newX;
        _field.Player.Y = newY;

        MoveEnemies();
        Render();
        UpdateStats();
    }

    private void MoveEnemies()
    {
        foreach (var enemy in _field.Enemies.ToList())
        {
            if (!enemy.IsAlive)
                continue;

            var (pdx, pdy) = enemy.GetMove(_field.Player);

            var dirs = new System.Collections.Generic.List<(int, int)> { (pdx, pdy) };
            if (pdx != 0)
            {
                dirs.Add((0, 1));
                dirs.Add((0, -1));
            }
            else if (pdy != 0)
            {
                dirs.Add((1, 0));
                dirs.Add((-1, 0));
            }

            foreach (var (dx, dy) in dirs)
            {
                int nx = enemy.X + dx;
                int ny = enemy.Y + dy;

                if (nx == _field.Player.X && ny == _field.Player.Y)
                {
                    _field.Player.TakeDamage(enemy.Attack);
                    GameMessage = $"{enemy.Name} attacks! (−{enemy.Attack} HP)";
                    if (_field.Player.Hp <= 0)
                    {
                        IsGameOver = true;
                        GameMessage = "You have fallen in battle...";
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

    private void Render()
    {
        if (_field == null) return;
        foreach (var cell in Cells)
            CellRenderer.Render(_field, cell);
    }

    private void UpdateStats()
    {
        if (_field?.Player == null) return;
        OnPropertyChanged(nameof(PlayerHp));
        OnPropertyChanged(nameof(PlayerAttack));
        OnPropertyChanged(nameof(PlayerKeys));
        OnPropertyChanged(nameof(CurrentLevel));
        OnPropertyChanged(nameof(LevelDisplay));
    }
}