using Avalonia;
using Avalonia.Controls;
using Avalonia.DevTools.ViewModels;
using Avalonia.Input;
using Avalonia.Markup.Xaml;

namespace Avalonia.DevTools.Views
{
    public class ConsoleView : UserControl
    {
        public ConsoleView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void InputKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ((ConsoleViewModel)DataContext).Execute();
            }
        }
    }
}
