namespace Praktyka4;

/// <summary>
/// Практика 4. Форми. Головне вікно — запуск двох програм.
/// Програма 1 — задача на одновимірний масив (Практика 1-2, Завдання 2).
/// Програма 2 — задача на рядки (Практика 1-2, Завдання 4).
/// </summary>
public sealed class MainForm : Form
{
    public MainForm()
    {
        Text = "Практика 4. Форми";
        ClientSize = new Size(360, 200);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        Font = new Font("Segoe UI", 10F);

        var lbl = new Label
        {
            Text = "Оберіть програму:",
            Location = new Point(20, 20),
            AutoSize = true
        };

        var btnArray = new Button
        {
            Text = "Програма 1 — Одновимірний масив",
            Location = new Point(20, 60),
            Size = new Size(320, 45)
        };
        btnArray.Click += (_, _) => new ArrayForm().ShowDialog();

        var btnString = new Button
        {
            Text = "Програма 2 — Робота з рядком",
            Location = new Point(20, 115),
            Size = new Size(320, 45)
        };
        btnString.Click += (_, _) => new StringForm().ShowDialog();

        Controls.AddRange([lbl, btnArray, btnString]);
    }
}
