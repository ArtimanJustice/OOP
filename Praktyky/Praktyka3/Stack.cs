namespace Praktyka3;

/// <summary>
/// Завдання 3.1. Стек на масиві.
/// Top — кількість елементів (тільки читання);
/// Length — розмір виділеної пам'яті (читання/запис);
/// індексатор — доступ до елемента за номером.
/// </summary>
public class Stack
{
    private int[] _items;
    private int _count;

    public Stack() : this(10) { }

    public Stack(int size)
    {
        if (size < 1) size = 1;
        _items = new int[size];
        _count = 0;
    }

    /// <summary>Кількість елементів у стеку (тільки для читання).</summary>
    public int Top => _count;

    /// <summary>Розмір виділеної пам'яті під стек (читання і запис).</summary>
    public int Length
    {
        get => _items.Length;
        set
        {
            if (value < _count)
                throw new ArgumentException("Новий розмір менший за кількість елементів.");
            Array.Resize(ref _items, value < 1 ? 1 : value);
        }
    }

    /// <summary>Додати елемент у стек.</summary>
    public void Push(int value)
    {
        if (_count == _items.Length)
            Array.Resize(ref _items, _items.Length * 2);
        _items[_count++] = value;
    }

    /// <summary>Зняти елемент зі стека.</summary>
    public int Pop()
    {
        if (_count == 0)
            throw new InvalidOperationException("Стек порожній.");
        return _items[--_count];
    }

    /// <summary>Доступ до елемента за номером (у звичайному стеку не дозволяється).</summary>
    public int this[int index]
    {
        get
        {
            if (index < 0 || index >= _count)
                throw new IndexOutOfRangeException();
            return _items[index];
        }
        set
        {
            if (index < 0 || index >= _count)
                throw new IndexOutOfRangeException();
            _items[index] = value;
        }
    }

    public override string ToString() =>
        "[" + string.Join(", ", _items[.._count]) + "]";
}
