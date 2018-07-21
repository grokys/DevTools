using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace Avalonia.DevTools.Views
{
    public class MainWindow : Window, IStyleHost
    {
        public MainWindow()
        {
            this.InitializeComponent();
            Avalonia.Diagnostics.DevTools.Attach(this);
        }

        IStyleHost IStyleHost.StylingParent => null;

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
