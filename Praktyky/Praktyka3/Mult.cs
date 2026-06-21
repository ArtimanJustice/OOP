namespace Praktyka3;

/// <summary>
/// Завдання 3.3. Клас з індексатором за двома індексами,
/// що повертає значення на перетині рядка і стовпця таблиці множення.
/// </summary>
public class Mult
{
    /// <summary>Значення таблиці множення на перетині рядка i та стовпця j.</summary>
    public int this[int i, int j] => i * j;
}
