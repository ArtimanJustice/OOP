using Praktyka3;

// ─────────────────────────────────────────────────────────────────────────────
//  Практика 3. Класи
//  3.1 — Stack, 3.2 — Complex, 3.3 — Mult (таблиця множення)
// ─────────────────────────────────────────────────────────────────────────────

while (true)
{
    if (!Console.IsOutputRedirected) Console.Clear();
    Console.WriteLine("=== Практика 3. Класи ===");
    Console.WriteLine("1 — Завдання 3.1 (Stack)");
    Console.WriteLine("2 — Завдання 3.2 (Complex)");
    Console.WriteLine("3 — Завдання 3.3 (таблиця множення через індексатор)");
    Console.WriteLine("0 — Вихід");
    Console.Write("Вибір: ");

    switch (Console.ReadLine()?.Trim())
    {
        case "1": DemoStack(); break;
        case "2": DemoComplex(); break;
        case "3": DemoMult(); break;
        case "0": return;
        default: continue;
    }
}

static void DemoStack()
{
    if (!Console.IsOutputRedirected) Console.Clear();
    Console.WriteLine("--- Завдання 3.1. Stack ---\n");

    var st = new Stack(3);
    Console.WriteLine($"Створено стек. Length = {st.Length}, Top = {st.Top}");

    int[] values = [10, 20, 30, 40, 50];
    foreach (int v in values)
    {
        st.Push(v);
        Console.WriteLine($"Push({v}) -> {st},  Top = {st.Top}, Length = {st.Length}");
    }

    Console.WriteLine($"\nДоступ через індексатор: st[0] = {st[0]}, st[2] = {st[2]}");

    Console.WriteLine($"\nPop() = {st.Pop()}");
    Console.WriteLine($"Pop() = {st.Pop()}");
    Console.WriteLine($"Стан: {st}, Top = {st.Top}");

    st.Length = 10;
    Console.WriteLine($"\nЗмінили Length = 10. Тепер Length = {st.Length}, Top = {st.Top}");

    Pause();
}

static void DemoComplex()
{
    if (!Console.IsOutputRedirected) Console.Clear();
    Console.WriteLine("--- Завдання 3.2. Complex ---\n");

    var a = new Complex(3, 4);
    var b = new Complex(1, -2);

    Console.Write("a = "); a.Print();
    Console.Write("b = "); b.Print();

    Console.WriteLine($"\na + 5 (ціле)   = {a.Add(5)}");
    Console.WriteLine($"a.Add(b)       = {a.Add(b)}");
    Console.WriteLine($"a + b (оператор) = {a + b}");
    Console.WriteLine($"a * b          = {a.Multiply(b)}");

    Pause();
}

static void DemoMult()
{
    if (!Console.IsOutputRedirected) Console.Clear();
    Console.WriteLine("--- Завдання 3.3. Таблиця множення 10x10 ---\n");

    var table = new Mult();
    Console.Write("    ");
    for (int j = 1; j <= 10; j++) Console.Write($"{j,5}");
    Console.WriteLine("\n" + new string('-', 4 + 5 * 10));

    for (int i = 1; i <= 10; i++)
    {
        Console.Write($"{i,3}|");
        for (int j = 1; j <= 10; j++)
            Console.Write($"{table[i, j],5}");
        Console.WriteLine();
    }

    Pause();
}

static void Pause()
{
    Console.WriteLine("\n(натисніть Enter)");
    Console.ReadLine();
}
