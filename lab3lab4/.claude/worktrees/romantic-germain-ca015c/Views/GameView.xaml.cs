using System;
using System.Windows.Input;
using ShadowMaiden.ViewModels;
using static System.Windows.Input.Key;

namespace ShadowMaiden.Views;

public partial class GameView
{
    public GameView()
    {
        InitializeComponent();
    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        if (DataContext is not GameViewModel vm)
            return;

        switch (e.Key)
        {
            case W:
            case Up:
                vm.MoveUpCommand.Execute(null);
                break;
            case S:
            case Down:
                vm.MoveDownCommand.Execute(null);
                break;
            case A:
            case Key.Left:
                vm.MoveLeftCommand.Execute(null);
                break;
            case D:
            case Right:
                vm.MoveRightCommand.Execute(null);
                break;
            case R:
                vm.RestartCommand.Execute(null);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}