using System.Globalization;

namespace Praktyka12;

/// <summary>Завдання 2. Одновимірні масиви (2.1 … 2.14).</summary>
public static class Zavdannya2
{
    public static void Run()
    {
        while (true)
        {
            if (!Console.IsOutputRedirected) Console.Clear();
            Console.WriteLine("=== Завдання 2. Одновимірні масиви ===");
            Console.WriteLine(" 1 — друк у зворотному порядку");
            Console.WriteLine(" 2 — розділити на додатні/нульові та від'ємні (2 колонки)");
            Console.WriteLine(" 3 — реверс без додаткового масиву");
            Console.WriteLine(" 4 — середнє арифметичне");
            Console.WriteLine(" 5 — парні, потім нулі, потім непарні");
            Console.WriteLine(" 6 — заповнити 0,2,4,..,n (n парне)");
            Console.WriteLine(" 7 — скільки разів зустрічається число k");
            Console.WriteLine(" 8 — лише унікальні елементи");
            Console.WriteLine(" 9 — заповнити n,n-1,..,0");
            Console.WriteLine("10 — заповнити n,n-2,..,1 (n непарне)");
            Console.WriteLine("11 — заповнити n,n+1,..,n+n");
            Console.WriteLine("12 — заповнити n+k,..,n");
            Console.WriteLine("13 — метод: видалити парні значення (void)");
            Console.WriteLine("14 — метод: відсортувати (void)");
            Console.WriteLine(" 0 — назад");
            Console.Write("Вибір: ");

            string? c = Console.ReadLine()?.Trim();
            if (c == "0") return;
            Console.WriteLine();
            switch (c)
            {
                case "1": T2_1(); break;
                case "2": T2_2(); break;
                case "3": T2_3(); break;
                case "4": T2_4(); break;
                case "5": T2_5(); break;
                case "6": T2_6(); break;
                case "7": T2_7(); break;
                case "8": T2_8(); break;
                case "9": T2_9(); break;
                case "10": T2_10(); break;
                case "11": T2_11(); break;
                case "12": T2_12(); break;
                case "13": T2_13(); break;
                case "14": T2_14(); break;
                default: continue;
            }
            Pause();
        }
    }

    // 2.1 — друк у зворотному порядку
    private static void T2_1()
    {
        int[] a = ReadIntArray("Введіть елементи масиву через пробіл: ");
        Console.Write("У зворотному порядку: ");
        for (int i = a.Length - 1; i >= 0; i--) Console.Write($"{a[i]} ");
        Console.WriteLine();
    }

    // 2.2 — y: додатні і нульові, w: від'ємні; друк у 2 колонки
    private static void T2_2()
    {
        double[] x = ReadDoubleArray("Введіть дійсні числа через пробіл: ");
        double[] y = Array.FindAll(x, e => e >= 0);
        double[] w = Array.FindAll(x, e => e < 0);

        Console.WriteLine($"{"y (>=0)",-15}{"w (<0)",-15}");
        int rows = Math.Max(y.Length, w.Length);
        for (int i = 0; i < rows; i++)
        {
            string ys = i < y.Length ? y[i].ToString(CultureInfo.InvariantCulture) : "";
            string ws = i < w.Length ? w[i].ToString(CultureInfo.InvariantCulture) : "";
            Console.WriteLine($"{ys,-15}{ws,-15}");
        }
    }

    // 2.3 — реверс на місці без іншого масиву
    private static void T2_3()
    {
        int[] a = ReadIntArray("Введіть цілі числа через пробіл: ");
        for (int i = 0, j = a.Length - 1; i < j; i++, j--)
            (a[i], a[j]) = (a[j], a[i]);
        Console.WriteLine("Після реверсу: " + string.Join(" ", a));
    }

    // 2.4 — середнє арифметичне
    private static void T2_4()
    {
        double[] a = ReadDoubleArray("Введіть числа через пробіл: ");
        double sum = 0;
        foreach (double e in a) sum += e;
        double avg = a.Length > 0 ? sum / a.Length : 0;
        Console.WriteLine($"Середнє арифметичне = {avg.ToString(CultureInfo.InvariantCulture)}");
    }

    // 2.5 — спочатку парні, потім нулі, потім непарні
    private static void T2_5()
    {
        int[] a = ReadIntArray("Введіть цілі числа через пробіл: ");
        int[] b = new int[a.Length];
        int idx = 0;
        foreach (int e in a) if (e != 0 && e % 2 == 0) b[idx++] = e;
        foreach (int e in a) if (e == 0) b[idx++] = e;
        foreach (int e in a) if (e % 2 != 0) b[idx++] = e;
        Console.WriteLine("Результат: " + string.Join(" ", b));
    }

    // 2.6 — заповнити 0,2,4,..,n (n парне)
    private static void T2_6()
    {
        int n = ReadInt("Введіть n (парне): ");
        if (n < 0 || n % 2 != 0) { Console.WriteLine("n має бути невід'ємним парним."); return; }
        int[] a = new int[n / 2 + 1];
        for (int i = 0; i < a.Length; i++) a[i] = i * 2;
        Console.WriteLine(string.Join(" ", a));
    }

    // 2.7 — скільки разів зустрічається k
    private static void T2_7()
    {
        int[] a = ReadIntArray("Введіть масив через пробіл: ");
        int k = ReadInt("Введіть k: ");
        int count = 0;
        foreach (int e in a) if (e == k) count++;
        Console.WriteLine($"Число {k} зустрічається {count} раз(ів).");
    }

    // 2.8 — лише унікальні елементи зі збереженням порядку
    private static void T2_8()
    {
        int[] a = ReadIntArray("Введіть масив через пробіл: ");
        var seen = new List<int>();
        foreach (int e in a) if (!seen.Contains(e)) seen.Add(e);
        Console.WriteLine("Унікальні: " + string.Join(" ", seen));
    }

    // 2.9 — заповнити n,n-1,..,0
    private static void T2_9()
    {
        int n = ReadInt("Введіть n: ");
        if (n < 0) { Console.WriteLine("n має бути невід'ємним."); return; }
        int[] a = new int[n + 1];
        for (int i = 0; i <= n; i++) a[i] = n - i;
        Console.WriteLine(string.Join(" ", a));
    }

    // 2.10 — заповнити n,n-2,..,1 (n непарне)
    private static void T2_10()
    {
        int n = ReadInt("Введіть n (непарне): ");
        if (n < 1 || n % 2 == 0) { Console.WriteLine("n має бути додатним непарним."); return; }
        int[] a = new int[n / 2 + 1];
        for (int i = 0, v = n; i < a.Length; i++, v -= 2) a[i] = v;
        Console.WriteLine(string.Join(" ", a));
    }

    // 2.11 — заповнити n,n+1,..,n+n
    private static void T2_11()
    {
        int n = ReadInt("Введіть n: ");
        int[] a = new int[n + 1];
        for (int i = 0; i <= n; i++) a[i] = n + i;
        Console.WriteLine(string.Join(" ", a));
    }

    // 2.12 — заповнити n+k,n+k-1,..,n
    private static void T2_12()
    {
        int n = ReadInt("Введіть n: ");
        int k = ReadInt("Введіть k (>=0): ");
        if (k < 0) { Console.WriteLine("k має бути невід'ємним."); return; }
        int[] a = new int[k + 1];
        for (int i = 0; i <= k; i++) a[i] = n + k - i;
        Console.WriteLine(string.Join(" ", a));
    }

    // 2.13 — метод (void), що видаляє з масиву парні значення
    private static void T2_13()
    {
        int[] a = ReadIntArray("Введіть масив через пробіл: ");
        Console.WriteLine("До:    " + string.Join(" ", a));
        RemoveEven(ref a);
        Console.WriteLine("Після: " + string.Join(" ", a));
    }

    private static void RemoveEven(ref int[] a)
    {
        var kept = new List<int>();
        foreach (int e in a) if (e % 2 != 0) kept.Add(e);
        a = kept.ToArray();
    }

    // 2.14 — метод (void), що сортує масив
    private static void T2_14()
    {
        int[] a = ReadIntArray("Введіть масив через пробіл: ");
        Console.WriteLine("До:    " + string.Join(" ", a));
        SortArray(a);
        Console.WriteLine("Після: " + string.Join(" ", a));
    }

    // сортування бульбашкою
    private static void SortArray(int[] a)
    {
        for (int i = 0; i < a.Length - 1; i++)
            for (int j = 0; j < a.Length - 1 - i; j++)
                if (a[j] > a[j + 1])
                    (a[j], a[j + 1]) = (a[j + 1], a[j]);
    }

    private static int[] ReadIntArray(string prompt)
    {
        Console.Write(prompt);
        string line = Console.ReadLine() ?? "";
        return ParseInts(line);
    }

    private static double[] ReadDoubleArray(string prompt)
    {
        Console.Write(prompt);
        string line = Console.ReadLine() ?? "";
        var parts = line.Split([' ', '\t', ','], StringSplitOptions.RemoveEmptyEntries);
        var list = new List<double>();
        foreach (var p in parts)
            if (double.TryParse(p.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out double d))
                list.Add(d);
        return list.ToArray();
    }

    private static int[] ParseInts(string line)
    {
        var parts = line.Split([' ', '\t', ','], StringSplitOptions.RemoveEmptyEntries);
        var list = new List<int>();
        foreach (var p in parts) if (int.TryParse(p, out int v)) list.Add(v);
        return list.ToArray();
    }

    private static int ReadInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out int v)) return v;
            Console.WriteLine("Некоректне число, спробуйте ще раз.");
        }
    }

    private static void Pause()
    {
        Console.WriteLine("\n(натисніть Enter)");
        Console.ReadLine();
    }
}
