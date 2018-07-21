using Avalonia;
using Avalonia.Controls;
using Avalonia.DevTools.ViewModels;
using Avalonia.Input;
using Avalonia.Markup.Xaml;

namespace Avalonia.DevTools.Views
{
    public class ConsoleView : UserControl
    {
        private readonly TextBox _input;

        public ConsoleView()
        {
            this.InitializeComponent();
            _input = this.FindControl<TextBox>("input");
            _input.KeyDown += InputKeyDown;
        }

        public void FocusInput() => _input.Focus();

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void InputKeyDown(object sender, KeyEventArgs e)
        {
            var vm = (ConsoleViewModel)DataContext;

            switch (e.Key)
            {
                case Key.Enter:
                    vm.Execute();
                    e.Handled = true;
                    break;
                case Key.Up:
                    vm.HistoryUp();
                    _input.CaretIndex = _input.Text.Length;
                    e.Handled = true;
                    break;
                case Key.Down:
                    vm.HistoryDown();
                    _input.CaretIndex = _input.Text.Length;
                    e.Handled = true;
                    break;
            }
        }
    }
}
