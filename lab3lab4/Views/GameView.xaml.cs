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

        switch (e.Key) {
            case W:
            case Up:
                vm.MoveCommand.Execute(Direction.Up);
                break;
            case S:
            case Down:
                vm.MoveCommand.Execute(Direction.Down);
                break;
            case A:
            case Key.Left:
                vm.MoveCommand.Execute(Direction.Left);
                break;
            case D:
            case Right:
                vm.MoveCommand.Execute(Direction.Right);
                break;
            case R:
                vm.RestartCommand.Execute(null);
                break;
        }
    }
}