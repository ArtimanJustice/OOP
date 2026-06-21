using Praktyka12;

// ─────────────────────────────────────────────────────────────────────────────
//  Практика 1-2. Основи C#
//  Завдання 1 — цикли (1.1 … 1.19)
//  Завдання 2 — одновимірні масиви (2.1 … 2.14)
//  Завдання 3 — двовимірні масиви (3.1 … 3.4)
//  Завдання 4 — рядки (4.1 … 4.2)
// ─────────────────────────────────────────────────────────────────────────────

while (true)
{
    if (!Console.IsOutputRedirected) Console.Clear();
    Console.WriteLine("=== Практика 1-2. Основи C# ===");
    Console.WriteLine("1 — Завдання 1 (цикли, патерни 1.1-1.19)");
    Console.WriteLine("2 — Завдання 2 (одновимірні масиви 2.1-2.14)");
    Console.WriteLine("3 — Завдання 3 (двовимірні масиви 3.1-3.4)");
    Console.WriteLine("4 — Завдання 4 (рядки 4.1-4.2)");
    Console.WriteLine("0 — Вихід");
    Console.Write("Ваш вибір: ");

    switch (Console.ReadLine()?.Trim())
    {
        case "1": Zavdannya1.Run(); break;
        case "2": Zavdannya2.Run(); break;
        case "3": Zavdannya3.Run(); break;
        case "4": Zavdannya4.Run(); break;
        case "0": return;
        default: continue;
    }
}
