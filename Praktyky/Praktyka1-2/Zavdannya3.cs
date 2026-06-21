namespace Praktyka12;

/// <summary>Завдання 3. Двовимірні масиви (3.1 … 3.4*).</summary>
public static class Zavdannya3
{
    private static readonly Random Rnd = new();

    public static void Run()
    {
        while (true)
        {
            if (!Console.IsOutputRedirected) Console.Clear();
            Console.WriteLine("=== Завдання 3. Двовимірні масиви ===");
            Console.WriteLine(" 1 — друк масиву таблицею (3.1)");
            Console.WriteLine(" 2 — сума першого стовпця (3.2)");
            Console.WriteLine(" 3 — сума заданого стовпця (3.3)");
            Console.WriteLine(" 4 — сума заданого стовпця АБО рядка (3.3*)");
            Console.WriteLine(" 5 — заповнення випадковими [a..b] (3.4)");
            Console.WriteLine(" 6 — заповнення випадковими [a..b] без повторів (3.4*)");
            Console.WriteLine(" 0 — назад");
            Console.Write("Вибір: ");

            string? c = Console.ReadLine()?.Trim();
            if (c == "0") return;
            Console.WriteLine();
            switch (c)
            {
                case "1": Demo3_1(); break;
                case "2": Demo3_2(); break;
                case "3": Demo3_3(); break;
                case "4": Demo3_3Star(); break;
                case "5": Demo3_4(); break;
                case "6": Demo3_4Star(); break;
                default: continue;
            }
            Pause();
        }
    }

    // 3.1 — метод друку двовимірного масиву по рядках
    public static void PrintMatrix(int[,] m)
    {
        int rows = m.GetLength(0), cols = m.GetLength(1);
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
                Console.Write($"{m[i, j],5}");
            Console.WriteLine();
        }
    }

    private static void Demo3_1()
    {
        int[,] m = CreateRandom(4, 5, 0, 99);
        Console.WriteLine("Двовимірний масив:");
        PrintMatrix(m);
    }

    // 3.2 — сума елементів першого стовпця (індекс 0)
    private static void Demo3_2()
    {
        int[,] m = CreateRandom(4, 5, 0, 99);
        PrintMatrix(m);
        int sum = 0;
        for (int i = 0; i < m.GetLength(0); i++) sum += m[i, 0];
        Console.WriteLine($"\nСума першого стовпця (індекс 0) = {sum}");
    }

    // 3.3 — сума заданого користувачем стовпця
    private static void Demo3_3()
    {
        int[,] m = CreateRandom(4, 5, 0, 99);
        PrintMatrix(m);
        int col = ReadInt($"\nНомер стовпця (0..{m.GetLength(1) - 1}): ");
        if (col < 0 || col >= m.GetLength(1)) { Console.WriteLine("Невірний номер."); return; }
        int sum = 0;
        for (int i = 0; i < m.GetLength(0); i++) sum += m[i, col];
        Console.WriteLine($"Сума стовпця {col} = {sum}");
    }

    // 3.3* — сума стовпця АБО рядка на вибір
    private static void Demo3_3Star()
    {
        int[,] m = CreateRandom(4, 5, 0, 99);
        PrintMatrix(m);
        Console.Write("\nЩо підрахувати — (с)товпець чи (р)ядок? ");
        string? mode = Console.ReadLine()?.Trim().ToLower();

        if (mode == "с" || mode == "c")
        {
            int col = ReadInt($"Номер стовпця (0..{m.GetLength(1) - 1}): ");
            if (col < 0 || col >= m.GetLength(1)) { Console.WriteLine("Невірний номер."); return; }
            int sum = 0;
            for (int i = 0; i < m.GetLength(0); i++) sum += m[i, col];
            Console.WriteLine($"Сума стовпця {col} = {sum}");
        }
        else if (mode == "р" || mode == "r")
        {
            int row = ReadInt($"Номер рядка (0..{m.GetLength(0) - 1}): ");
            if (row < 0 || row >= m.GetLength(0)) { Console.WriteLine("Невірний номер."); return; }
            int sum = 0;
            for (int j = 0; j < m.GetLength(1); j++) sum += m[row, j];
            Console.WriteLine($"Сума рядка {row} = {sum}");
        }
        else Console.WriteLine("Невідомий вибір.");
    }

    // 3.4 — метод заповнення випадковими числами [a..b]
    public static void FillRandom(int[,] m, int a, int b)
    {
        if (a > b) (a, b) = (b, a);
        for (int i = 0; i < m.GetLength(0); i++)
            for (int j = 0; j < m.GetLength(1); j++)
                m[i, j] = Rnd.Next(a, b + 1);
    }

    private static void Demo3_4()
    {
        int[,] m = new int[4, 5];
        FillRandom(m, 10, 50);
        Console.WriteLine("Заповнено випадковими [10..50]:");
        PrintMatrix(m);
    }

    // 3.4* — заповнення випадковими [a..b] без повторів
    public static void FillRandomUnique(int[,] m, int a, int b)
    {
        if (a > b) (a, b) = (b, a);
        int need = m.GetLength(0) * m.GetLength(1);
        int rangeCount = b - a + 1;
        if (need > rangeCount)
            throw new ArgumentException("Діапазон замалий для заповнення без повторів.");

        var pool = new List<int>(rangeCount);
        for (int v = a; v <= b; v++) pool.Add(v);
        for (int i = pool.Count - 1; i > 0; i--)
        {
            int j = Rnd.Next(i + 1);
            (pool[i], pool[j]) = (pool[j], pool[i]);
        }

        int idx = 0;
        for (int i = 0; i < m.GetLength(0); i++)
            for (int j = 0; j < m.GetLength(1); j++)
                m[i, j] = pool[idx++];
    }

    private static void Demo3_4Star()
    {
        int[,] m = new int[4, 5];
        FillRandomUnique(m, 1, 40);
        Console.WriteLine("Заповнено випадковими [1..40] без повторів:");
        PrintMatrix(m);
    }

    private static int[,] CreateRandom(int rows, int cols, int a, int b)
    {
        int[,] m = new int[rows, cols];
        FillRandom(m, a, b);
        return m;
    }

    private static int ReadInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out int v)) return v;
            Console.WriteLine("Некоректне число.");
        }
    }

    private static void Pause()
    {
        Console.WriteLine("\n(натисніть Enter)");
        Console.ReadLine();
    }
}
