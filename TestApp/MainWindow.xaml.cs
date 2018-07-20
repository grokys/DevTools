using Avalonia;
using Avalonia.Controls;
using Avalonia.DevTools;
using Avalonia.Markup.Xaml;

namespace TestApp
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            NewDevTools.Attach(this);
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
