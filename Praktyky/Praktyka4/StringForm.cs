namespace Praktyka4;

/// <summary>
/// Програма 2. Робота з рядком (задачі з Практики 1-2, Завдання 4):
/// кількість заданого символу, кількість слів, позиція символу.
/// Введення і вивід — на формі.
/// </summary>
public sealed class StringForm : Form
{
    private readonly TextBox _text;
    private readonly TextBox _char;
    private readonly TextBox _output;

    public StringForm()
    {
        Text = "Програма 2 — Робота з рядком";
        ClientSize = new Size(440, 320);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        Font = new Font("Segoe UI", 10F);

        var lblText = new Label { Text = "Рядок:", Location = new Point(15, 15), AutoSize = true };
        _text = new TextBox
        {
            Location = new Point(15, 40),
            Size = new Size(410, 25),
            Text = "Hello world from Csharp"
        };

        var lblChar = new Label { Text = "Символ:", Location = new Point(15, 75), AutoSize = true };
        _char = new TextBox
        {
            Location = new Point(80, 72),
            Size = new Size(40, 25),
            MaxLength = 1,
            Text = "o"
        };

        var btnCount = new Button { Text = "К-ть символу", Location = new Point(15, 110), Size = new Size(130, 35) };
        var btnWords = new Button { Text = "К-ть слів", Location = new Point(155, 110), Size = new Size(120, 35) };
        var btnPos = new Button { Text = "Позиція символу", Location = new Point(285, 110), Size = new Size(140, 35) };

        btnCount.Click += (_, _) => OnCount();
        btnWords.Click += (_, _) => OnWords();
        btnPos.Click += (_, _) => OnPosition();

        var lblOut = new Label { Text = "Результат:", Location = new Point(15, 155), AutoSize = true };
        _output = new TextBox
        {
            Location = new Point(15, 180),
            Size = new Size(410, 120),
            Multiline = true,
            ReadOnly = true,
            ScrollBars = ScrollBars.Vertical
        };

        Controls.AddRange([lblText, _text, lblChar, _char, btnCount, btnWords, btnPos, lblOut, _output]);
    }

    private char GetChar() => _char.Text.Length > 0 ? _char.Text[0] : ' ';

    /// <summary>4.1 — кількість заданого символу.</summary>
    private void OnCount()
    {
        string s = _text.Text;
        char c = GetChar();
        int count = 0;
        foreach (char ch in s) if (ch == c) count++;
        _output.Text = $"Символ '{c}' зустрічається {count} раз(ів).";
    }

    /// <summary>Кількість слів.</summary>
    private void OnWords()
    {
        string s = _text.Text;
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
        _output.Text = $"Кількість слів: {count}";
    }

    /// <summary>Позиція символу (1-based) або 0.</summary>
    private void OnPosition()
    {
        string s = _text.Text;
        char c = GetChar();
        int pos = 0;
        for (int i = 0; i < s.Length; i++)
            if (s[i] == c) { pos = i + 1; break; }
        _output.Text = pos == 0
            ? $"Символу '{c}' немає в рядку (0)."
            : $"Перша позиція символу '{c}': {pos}";
    }
}
