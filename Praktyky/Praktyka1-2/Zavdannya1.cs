using System.Text;

namespace Praktyka12;

/// <summary>Завдання 1. Друк патернів за допомогою циклів (1.1 … 1.19).</summary>
public static class Zavdannya1
{
    public static void Run()
    {
        if (!Console.IsOutputRedirected) Console.Clear();
        P_1_1();  P_1_2();  P_1_3();  P_1_4();  P_1_5();
        P_1_6();  P_1_7();  P_1_8();  P_1_9();  P_1_10();
        P_1_11(); P_1_12(); P_1_13(); P_1_14(); P_1_15();
        P_1_16(); P_1_17(); P_1_18(); P_1_19();

        Console.WriteLine("\n--- Кінець Завдання 1. Натисніть Enter ---");
        Console.ReadLine();
    }

    private static void Title(string t) => Console.WriteLine($"\n{t}");

    // 41..80 по 10 у рядку
    private static void P_1_1()
    {
        Title("1.1)");
        for (int n = 41; n <= 80; n++)
        {
            Console.Write($"{n} ");
            if (n % 10 == 0) Console.WriteLine();
        }
    }

    // 1 / 2 2 / 3 3 3 / … / 7 7 7 7 7 7 7
    private static void P_1_2()
    {
        Title("1.2)");
        for (int i = 1; i <= 7; i++)
        {
            for (int j = 0; j < i; j++) Console.Write($"{i} ");
            Console.WriteLine();
        }
    }

    // 10 9 8 7 / 10 9 8 / 10 9 / 10
    private static void P_1_3()
    {
        Title("1.3)");
        for (int k = 0; k <= 3; k++)
        {
            for (int n = 10; n >= 7 + k; n--) Console.Write($"{n} ");
            Console.WriteLine();
        }
    }

    // 0 / 1 0 / 2 1 0 / 3 2 1 0 / 4 3 2 1 0
    private static void P_1_4()
    {
        Title("1.4)");
        for (int i = 0; i <= 4; i++)
        {
            for (int n = i; n >= 0; n--) Console.Write($"{n} ");
            Console.WriteLine();
        }
    }

    // 7 6 5 4 3 2 / 6 5 4 3 2 / … / 2
    private static void P_1_5()
    {
        Title("1.5)");
        for (int s = 7; s >= 2; s--)
        {
            for (int n = s; n >= 2; n--) Console.Write($"{n} ");
            Console.WriteLine();
        }
    }

    // 5 5 5 5 5 / 6 6 6 6 / 7 7 7 / 8 8 / 9
    private static void P_1_6()
    {
        Title("1.6)");
        for (int i = 0; i <= 4; i++)
        {
            int value = 5 + i, count = 5 - i;
            for (int j = 0; j < count; j++) Console.Write($"{value} ");
            Console.WriteLine();
        }
    }

    // піраміда 1 / 1 2 / 1 2 3 / … / 1 2 3 4 5 6 7 (вирівняно праворуч)
    private static void P_1_7()
    {
        Title("1.7)");
        for (int i = 1; i <= 7; i++)
        {
            var sb = new StringBuilder();
            for (int n = 1; n <= i; n++) sb.Append($"{n} ");
            PrintRight(sb.ToString().TrimEnd(), 7 * 2);
        }
    }

    // 0 / 1 0 / 2 1 0 / … / 10 9 … 0 (вирівняно праворуч)
    private static void P_1_8()
    {
        Title("1.8)");
        for (int i = 0; i <= 10; i++)
        {
            var sb = new StringBuilder();
            for (int n = i; n >= 0; n--) sb.Append($"{n} ");
            PrintRight(sb.ToString().TrimEnd(), 11 * 3);
        }
    }

    // 1 / 0 / 2 2 / 0 0 / 3 3 3 / 0 0 0 / 4 4 4 4 / 0 0 0 0
    private static void P_1_9()
    {
        Title("1.9)");
        for (int i = 1; i <= 4; i++)
        {
            for (int j = 0; j < i; j++) Console.Write($"{i} ");
            Console.WriteLine();
            for (int j = 0; j < i; j++) Console.Write("0 ");
            Console.WriteLine();
        }
    }

    // піраміда з 5 (вирівняно праворуч)
    private static void P_1_10()
    {
        Title("1.10)");
        for (int i = 1; i <= 5; i++)
        {
            var sb = new StringBuilder();
            for (int j = 0; j < i; j++) sb.Append("5 ");
            PrintRight(sb.ToString().TrimEnd(), 5 * 2);
        }
    }

    // 8 6 4 2 0 / 6 4 2 0 / 4 2 0 / 2 0 / 0
    private static void P_1_11()
    {
        Title("1.11)");
        for (int s = 8; s >= 0; s -= 2)
        {
            for (int n = s; n >= 0; n -= 2) Console.Write($"{n} ");
            Console.WriteLine();
        }
    }

    // 0 2 4 6 / 0 2 4 / 0 2 / 0
    private static void P_1_12()
    {
        Title("1.12)");
        for (int e = 6; e >= 0; e -= 2)
        {
            for (int n = 0; n <= e; n += 2) Console.Write($"{n} ");
            Console.WriteLine();
        }
    }

    // 9 / 9 7 / 9 7 5 / 9 7 5 3 / 9 7 5 3 1
    private static void P_1_13()
    {
        Title("1.13)");
        for (int i = 1; i <= 5; i++)
        {
            int printed = 0;
            for (int n = 9; printed < i; n -= 2, printed++) Console.Write($"{n} ");
            Console.WriteLine();
        }
    }

    // 7 6 5 4 3 / 1 2 3 4 5 / 7 6 5 4 / 1 2 3 4 / … / 7 / 1
    private static void P_1_14()
    {
        Title("1.14)");
        for (int c = 5; c >= 1; c--)
        {
            for (int n = 7; n >= 8 - c; n--) Console.Write($"{n} ");
            Console.WriteLine();
            for (int n = 1; n <= c; n++) Console.Write($"{n} ");
            Console.WriteLine();
        }
    }

    // піраміда 7 / 7 5 / 7 5 3 / 7 5 3 1 (вирівняно праворуч)
    private static void P_1_15()
    {
        Title("1.15)");
        for (int i = 1; i <= 4; i++)
        {
            var sb = new StringBuilder();
            int printed = 0;
            for (int n = 7; printed < i; n -= 2, printed++) sb.Append($"{n} ");
            PrintRight(sb.ToString().TrimEnd(), 4 * 2);
        }
    }

    // 15 16 17 18 19 / 16 17 18 19 / … / 19
    private static void P_1_16()
    {
        Title("1.16)");
        for (int s = 15; s <= 19; s++)
        {
            for (int n = s; n <= 19; n++) Console.Write($"{n} ");
            Console.WriteLine();
        }
    }

    // 1 2 3 4 5 / 6 5 4 3 / 1 2 3 4 / 6 5 4 / … / 6 / 1 (зі зсувом)
    private static void P_1_17()
    {
        Title("1.17)");
        int indent = 0;
        for (int k = 5; k >= 1; k--)
        {
            var asc = new StringBuilder();
            for (int n = 1; n <= k; n++) asc.Append($"{n} ");
            Console.WriteLine(new string(' ', indent * 2) + asc.ToString().TrimEnd());

            if (k > 1)
            {
                var desc = new StringBuilder();
                for (int n = 6; n >= 8 - k; n--) desc.Append($"{n} ");
                Console.WriteLine(new string(' ', indent * 2 + 2) + desc.ToString().TrimEnd());
            }
            indent++;
        }
    }

    // 8 6 4 2 0 / 6 4 2 0 / 4 2 0 / 2 0 / 0
    private static void P_1_18()
    {
        Title("1.18)");
        for (int s = 8; s >= 0; s -= 2)
        {
            for (int n = s; n >= 0; n -= 2) Console.Write($"{n} ");
            Console.WriteLine();
        }
    }

    // піраміда 8 / 6 6 / 4 4 4 / 2 2 2 2 / 0 0 0 0 0 (вирівняно праворуч)
    private static void P_1_19()
    {
        Title("1.19)");
        for (int i = 1; i <= 5; i++)
        {
            int value = 10 - 2 * i;
            var sb = new StringBuilder();
            for (int j = 0; j < i; j++) sb.Append($"{value} ");
            PrintRight(sb.ToString().TrimEnd(), 5 * 2);
        }
    }

    /// <summary>Друкує рядок, вирівняний праворуч до ширини <paramref name="width"/>.</summary>
    private static void PrintRight(string row, int width) =>
        Console.WriteLine(row.PadLeft(width));
}
