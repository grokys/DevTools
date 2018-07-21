using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.DevTools.ViewModels;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;

namespace Avalonia.DevTools.Views
{
    public class MainView : UserControl
    {
        private const int ConsoleRow = 5;
        private readonly ConsoleView _console;
        private readonly Grid _rootGrid;
        private double _consoleHeight = 1;

        public MainView()
        {
            this.InitializeComponent();
            this.AddHandler(KeyDownEvent, PreviewKeyDown, RoutingStrategies.Tunnel);
            _console = this.FindControl<ConsoleView>("console");
            _rootGrid = this.FindControl<Grid>("rootGrid");
        }

        public void ToggleConsole()
        {
            var vm = (MainViewModel)DataContext;

            vm.Console.ToggleVisibility();

            if (vm.Console.IsVisible)
            {
                _rootGrid.RowDefinitions[ConsoleRow].Height = new GridLength(_consoleHeight, GridUnitType.Star);
                Dispatcher.UIThread.Post(() => _console.FocusInput(), DispatcherPriority.Background);
            }
            else
            {
                _consoleHeight = _rootGrid.RowDefinitions[ConsoleRow].Height.Value;
                _rootGrid.RowDefinitions[ConsoleRow].Height = GridLength.Auto;
            }
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                ToggleConsole();
                e.Handled = true;
            }
        }
    }
}
