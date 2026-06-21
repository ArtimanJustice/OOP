using System.Globalization;

namespace Praktyka4;

/// <summary>
/// Програма 1. Одновимірний масив (задачі з Практики 1-2, Завдання 2):
/// зворотний порядок, середнє арифметичне, сортування.
/// Введення і вивід — на формі.
/// </summary>
public sealed class ArrayForm : Form
{
    private readonly TextBox _input;
    private readonly TextBox _output;

    public ArrayForm()
    {
        Text = "Програма 1 — Одновимірний масив";
        ClientSize = new Size(440, 320);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        Font = new Font("Segoe UI", 10F);

        var lbl = new Label
        {
            Text = "Введіть числа через пробіл:",
            Location = new Point(15, 15),
            AutoSize = true
        };

        _input = new TextBox
        {
            Location = new Point(15, 40),
            Size = new Size(410, 25),
            Text = "5 -3 8 0 -1 4 2"
        };

        var btnReverse = new Button { Text = "Зворотний порядок", Location = new Point(15, 80), Size = new Size(150, 35) };
        var btnAvg = new Button { Text = "Середнє", Location = new Point(175, 80), Size = new Size(110, 35) };
        var btnSort = new Button { Text = "Сортувати", Location = new Point(295, 80), Size = new Size(130, 35) };

        btnReverse.Click += (_, _) => OnReverse();
        btnAvg.Click += (_, _) => OnAverage();
        btnSort.Click += (_, _) => OnSort();

        var lblOut = new Label { Text = "Результат:", Location = new Point(15, 130), AutoSize = true };

        _output = new TextBox
        {
            Location = new Point(15, 155),
            Size = new Size(410, 145),
            Multiline = true,
            ReadOnly = true,
            ScrollBars = ScrollBars.Vertical
        };

        Controls.AddRange([lbl, _input, btnReverse, btnAvg, btnSort, lblOut, _output]);
    }

    private double[]? ParseInput()
    {
        var parts = _input.Text.Split([' ', '\t', ','], StringSplitOptions.RemoveEmptyEntries);
        var list = new List<double>();
        foreach (var p in parts)
        {
            if (!double.TryParse(p.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out double d))
            {
                MessageBox.Show($"Некоректне число: \"{p}\"", "Помилка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }
            list.Add(d);
        }
        if (list.Count == 0)
        {
            MessageBox.Show("Масив порожній.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return null;
        }
        return list.ToArray();
    }

    private void OnReverse()
    {
        double[]? a = ParseInput();
        if (a == null) return;
        Array.Reverse(a);
        _output.Text = "Зворотний порядок:\r\n" + Format(a);
    }

    private void OnAverage()
    {
        double[]? a = ParseInput();
        if (a == null) return;
        double sum = 0;
        foreach (double e in a) sum += e;
        double avg = sum / a.Length;
        _output.Text = $"Сума = {sum.ToString(CultureInfo.InvariantCulture)}\r\n" +
                       $"Кількість = {a.Length}\r\n" +
                       $"Середнє арифметичне = {avg.ToString("0.###", CultureInfo.InvariantCulture)}";
    }

    private void OnSort()
    {
        double[]? a = ParseInput();
        if (a == null) return;
        Array.Sort(a);
        _output.Text = "Відсортовано за зростанням:\r\n" + Format(a);
    }

    private static string Format(double[] a) =>
        string.Join("  ", Array.ConvertAll(a, x => x.ToString(CultureInfo.InvariantCulture)));
}
