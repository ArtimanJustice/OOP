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

    private const int PresetLevels = 3;

    public ObservableCollection<CellInfo> Cells { get; } = [];

    public int FieldWidth => _field?.Width ?? 0;
    public int FieldHeight => _field?.Height ?? 0;

    public int PlayerHp => _field?.Player?.Hp ?? 0;
    public int PlayerMaxHp => Player.MaxHp;
    public int PlayerAttack => _field?.Player?.Attack ?? 0;
    public int PlayerKeys => _field?.Player?.Keys ?? 0;
    public int CurrentLevel => _currentLevel;
    public string LevelDisplay => $"{_currentLevel}";

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

    public ICommand MoveCommand { get; }
    public ICommand RestartCommand { get; }

    public GameViewModel()
    {
        MoveCommand = new RelayCommand(Move, _ => !_isGameOver);
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
        _field = level <= PresetLevels
            ? LevelLoader.Load(level)
            : LevelGenerator.Generate(level);
        OnPropertyChanged(nameof(FieldWidth));
        OnPropertyChanged(nameof(FieldHeight));
        InitializeCells();
        Refresh();
        GameMessage = $"Level {level} — Find the exit!";
    }

    private void InitializeCells()
    {
        Cells.Clear();
        for (var y = 0; y < _field.Height; y++)
        for (var x = 0; x < _field.Width; x++)
            Cells.Add(new CellInfo(x, y));
    }

    private void Move(object parameter)
    {
        var (dx, dy) = parameter switch
        {
            Direction.Up => (0, -1),
            Direction.Down => (0, 1),
            Direction.Left => (-1, 0),
            Direction.Right => (1, 0),
            _ => (0, 0)
        };

        if (dx != 0 || dy != 0)
            TakeTurn(dx, dy);
    }

    private void TakeTurn(int dx, int dy)
    {
        if (_isGameOver || _field == null) return;

        ResolveMove(dx, dy);
        Refresh();
    }

    private void ResolveMove(int dx, int dy)
    {
        int newX = _field.Player.X + dx;
        int newY = _field.Player.Y + dy;
        var target = _field[newX, newY];

        if (!TryEnterTile(target)) return;
        if (TryFightEnemyAt(newX, newY)) return;
        PickUpItemAt(newX, newY);
        if (TryTakeExit(target)) return;

        StepPlayer(newX, newY);
        MoveEnemies();
    }

    private bool TryEnterTile(GameElement target)
    {
        if (target is Door { IsOpen: false } door)
        {
            if (!door.TryOpen(_field.Player))
            {
                GameMessage = "You need a key!";
                return false;
            }

            GameMessage = "Door unlocked!";
            return true;
        }

        return target is { IsPassable: true };
    }

    private bool TryFightEnemyAt(int x, int y)
    {
        var enemy = _field.Enemies.FirstOrDefault(e => e.X == x && e.Y == y && e.IsAlive);
        if (enemy == null) return false;

        int selfDamage = _field.Player.Hp / 2;
        _field.Player.TakeDamage(selfDamage);
        enemy.TakeDamage(_field.Player.Attack);
        GameMessage = $"Hit {enemy.Name}! (−{selfDamage} HP to you)";

        if (!enemy.IsAlive)
        {
            _field.RemoveEnemy(enemy);
            GameMessage = $"{enemy.Name} defeated! (−{selfDamage} HP to you)";
        }

        if (!PlayerIsDead())
            MoveEnemies();

        return true;
    }

    private void PickUpItemAt(int x, int y)
    {
        var item = _field.Items.FirstOrDefault(i => i.X == x && i.Y == y);
        if (item == null) return;

        item.Apply(_field.Player);
        _field.RemoveItem(item);
        GameMessage = $"Picked up {item.Name}!";
    }

    private bool TryTakeExit(GameElement target)
    {
        if (target is not Exit) return false;

        int hp = _field.Player.Hp;
        int attack = _field.Player.Attack;
        int keys = _field.Player.Keys;
        _currentLevel++;
        LoadLevel(_currentLevel);
        _field.Player.Hp = hp;
        _field.Player.Attack = attack;
        _field.Player.Keys = keys;
        return true;
    }

    private void StepPlayer(int newX, int newY)
    {
        _field[_field.Player.X, _field.Player.Y] = new Floor(_field.Player.X, _field.Player.Y);
        _field.Player.X = newX;
        _field.Player.Y = newY;
    }

    private void MoveEnemies()
    {
        foreach (var enemy in _field.Enemies.ToList())
        {
            if (!enemy.IsAlive)
                continue;

            foreach (var (dx, dy) in enemy.GetMoveCandidates(_field.Player))
            {
                int nx = enemy.X + dx;
                int ny = enemy.Y + dy;

                if (nx == _field.Player.X && ny == _field.Player.Y)
                {
                    EnemyAttackPlayer(enemy);
                    break;
                }

                if (enemy.CanMoveTo(_field, nx, ny))
                {
                    enemy.X = nx;
                    enemy.Y = ny;
                    break;
                }
            }

            if (_isGameOver)
                return;
        }
    }

    private void EnemyAttackPlayer(Enemy enemy)
    {
        _field.Player.TakeDamage(enemy.Attack);
        GameMessage = $"{enemy.Name} attacks! (−{enemy.Attack} HP)";
        PlayerIsDead();
    }

    private bool PlayerIsDead()
    {
        if (_field.Player.Hp > 0) return false;

        IsGameOver = true;
        GameMessage = "You have fallen in battle...";
        return true;
    }

    private void Refresh()
    {
        Render();
        UpdateStats();
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
