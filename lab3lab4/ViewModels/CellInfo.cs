namespace ShadowMaiden.ViewModels;

public class CellInfo(int x, int y) : BaseViewModel
{
    private string _background = "#1a1a2e";
    private string _text = "";
    private string _foreground = "#ffffff";

    public int X { get; } = x;
    public int Y { get; } = y;

    public string Background
    {
        get => _background;
        set => SetField(ref _background, value);
    }

    public string Text
    {
        get => _text;
        set => SetField(ref _text, value);
    }

    public string Foreground
    {
        get => _foreground;
        set => SetField(ref _foreground, value);
    }
}