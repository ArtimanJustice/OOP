using System.Text;

namespace Praktyka12;

/// <summary>Завдання 4. Робота з рядками (4.1 … 4.2).</summary>
public static class Zavdannya4
{
    public static void Run()
    {
        while (true)
        {
            if (!Console.IsOutputRedirected) Console.Clear();
            Console.WriteLine("=== Завдання 4. Рядки ===");
            Console.WriteLine(" 1 — кількість заданого символу в рядку (4.1)");
            Console.WriteLine(" 2 — порівняти довжини двох рядків (1/0/-1)");
            Console.WriteLine(" 3 — позиція символу в рядку (або 0)");
            Console.WriteLine(" 4 — заповнити рядок символом з позиції до кінця");
            Console.WriteLine(" 5 — кількість слів");
            Console.WriteLine(" 6 — кількість букв в останньому слові");
            Console.WriteLine(" 7 — перше входження будь-якого символу з 2-го рядка");
            Console.WriteLine(" 8 — *копіювати з 2-го рядка до заданого символу");
            Console.WriteLine(" 9 — *замінити символ на '*', якщо поруч немає іншого символу");
            Console.WriteLine("10 — *видалити з 1-го рядка символи, що є в 2-му");
            Console.WriteLine(" 0 — назад");
            Console.Write("Вибір: ");

            string? c = Console.ReadLine()?.Trim();
            if (c == "0") return;
            Console.WriteLine();
            switch (c)
            {
                case "1": Demo4_1(); break;
                case "2": Demo4_2(); break;
                case "3": Demo4_3(); break;
                case "4": Demo4_4(); break;
                case "5": Demo4_5(); break;
                case "6": Demo4_6(); break;
                case "7": Demo4_7(); break;
                case "8": Demo4_8(); break;
                case "9": Demo4_9(); break;
                case "10": Demo4_10(); break;
                default: continue;
            }
            Pause();
        }
    }

    // 4.1 — кількість символів c у рядку s
    public static int CountChar(string s, char c)
    {
        int count = 0;
        for (int i = 0; i < s.Length; i++) if (s[i] == c) count++;
        return count;
    }

    private static void Demo4_1()
    {
        string s = Read("Введіть рядок: ");
        char c = ReadChar("Введіть символ: ");
        Console.WriteLine($"Символ '{c}' зустрічається {CountChar(s, c)} раз(ів).");
    }

    // 1 якщо перший довший, 0 рівні, -1 якщо другий довший
    public static int CompareLength(string a, string b) =>
        a.Length > b.Length ? 1 : a.Length < b.Length ? -1 : 0;

    private static void Demo4_2()
    {
        string a = Read("Перший рядок: ");
        string b = Read("Другий рядок: ");
        Console.WriteLine($"Результат: {CompareLength(a, b)}");
    }

    // позиція символу c у рядку (1-based) або 0
    public static int PositionOf(string s, char c)
    {
        for (int i = 0; i < s.Length; i++) if (s[i] == c) return i + 1;
        return 0;
    }

    private static void Demo4_3()
    {
        string s = Read("Введіть рядок: ");
        char c = ReadChar("Введіть символ: ");
        int pos = PositionOf(s, c);
        Console.WriteLine(pos == 0 ? "Символу немає (0)." : $"Позиція: {pos}");
    }

    // заповнити рядок символом c, починаючи з позиції index (0-based) до кінця
    public static string FillFrom(string s, char c, int index)
    {
        if (index < 0) index = 0;
        var chars = s.ToCharArray();
        for (int i = index; i < chars.Length; i++) chars[i] = c;
        return new string(chars);
    }

    private static void Demo4_4()
    {
        string s = Read("Введіть рядок: ");
        char c = ReadChar("Символ-заповнювач: ");
        int idx = ReadInt("Початкова позиція (з 0): ");
        Console.WriteLine($"Результат: \"{FillFrom(s, c, idx)}\"");
    }

    // кількість слів (групи символів, розділені пробілами)
    public static int WordCount(string s)
    {
        int count = 0;
        bool inWord = false;
        foreach (char ch in s)
        {
            if (ch != ' ' && ch != '\t')
            {
                if (!inWord) { count++; inWord = true; }
            }
            else inWord = false;
        }
        return count;
    }

    private static void Demo4_5()
    {
        string s = Read("Введіть рядок: ");
        Console.WriteLine($"Кількість слів: {WordCount(s)}");
    }

    // кількість букв в останньому слові
    public static int LastWordLength(string s)
    {
        int i = s.Length - 1;
        while (i >= 0 && (s[i] == ' ' || s[i] == '\t')) i--;
        int end = i;
        while (i >= 0 && s[i] != ' ' && s[i] != '\t') i--;
        return end - i;
    }

    private static void Demo4_6()
    {
        string s = Read("Введіть рядок: ");
        Console.WriteLine($"Букв в останньому слові: {LastWordLength(s)}");
    }

    // перше входження в a будь-якого символу з b, або -1 (0-based)
    public static int FirstCommon(string a, string b)
    {
        for (int i = 0; i < a.Length; i++)
            if (b.IndexOf(a[i]) >= 0) return i;
        return -1;
    }

    private static void Demo4_7()
    {
        string a = Read("Перший рядок: ");
        string b = Read("Другий рядок: ");
        int pos = FirstCommon(a, b);
        Console.WriteLine(pos == -1 ? "Спільних символів немає (-1)." : $"Позиція (з 0): {pos}");
    }

    // *скопіювати в перший рядок символи з другого від початку до символу stop
    public static string CopyUntil(string second, char stop)
    {
        var sb = new StringBuilder();
        foreach (char ch in second)
        {
            if (ch == stop) break;
            sb.Append(ch);
        }
        return sb.ToString();
    }

    private static void Demo4_8()
    {
        string b = Read("Рядок-джерело: ");
        char stop = ReadChar("Символ зупинки: ");
        Console.WriteLine($"Перший рядок став: \"{CopyUntil(b, stop)}\"");
    }

    // *замінити входження символу target на '*', якщо поруч (зліва/справа) немає neighbor
    public static string ReplaceIfNoNeighbor(string s, char target, char neighbor)
    {
        var chars = s.ToCharArray();
        for (int i = 0; i < chars.Length; i++)
        {
            if (chars[i] != target) continue;
            bool hasNeighbor = (i > 0 && s[i - 1] == neighbor) ||
                               (i < s.Length - 1 && s[i + 1] == neighbor);
            if (!hasNeighbor) chars[i] = '*';
        }
        return new string(chars);
    }

    private static void Demo4_9()
    {
        string s = Read("Введіть рядок: ");
        char target = ReadChar("Символ для заміни: ");
        char neighbor = ReadChar("Символ-сусід: ");
        Console.WriteLine($"Результат: \"{ReplaceIfNoNeighbor(s, target, neighbor)}\"");
    }

    // *видалити з a кожен символ, що є в b
    public static string RemoveCommon(string a, string b)
    {
        var sb = new StringBuilder();
        foreach (char ch in a)
            if (b.IndexOf(ch) < 0) sb.Append(ch);
        return sb.ToString();
    }

    private static void Demo4_10()
    {
        string a = Read("Перший рядок: ");
        string b = Read("Другий рядок: ");
        Console.WriteLine($"Результат: \"{RemoveCommon(a, b)}\"");
    }

    private static string Read(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine() ?? "";
    }

    private static char ReadChar(string prompt)
    {
        Console.Write(prompt);
        string s = Console.ReadLine() ?? " ";
        return s.Length > 0 ? s[0] : ' ';
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
