using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.DevTools.ViewModels;
using Avalonia.DevTools.Views;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace Avalonia.DevTools
{
    public static class NewDevTools
    {
        private static Dictionary<TopLevel, Window> s_open = new Dictionary<TopLevel, Window>();
        //private static IDisposable _keySubscription;

        public static IDisposable Attach(TopLevel control)
        {
            return control.AddHandler(
                InputElement.KeyDownEvent,
                WindowPreviewKeyDown,
                RoutingStrategies.Tunnel);
        }

        private static void WindowPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F12)
            {
                var control = (TopLevel)sender;
                var devToolsWindow = default(Window);

                if (s_open.TryGetValue(control, out devToolsWindow))
                {
                    devToolsWindow.Activate();
                }
                else
                {
                    var devTools = new MainView();
                    devTools.DataContext = new MainViewModel(control);

                    devToolsWindow = new MainWindow
                    {
                        Width = 1024,
                        Height = 512,
                        Content = devTools,
                        DataTemplates =
                        {
                            new ViewLocator<ViewModelBase>(),
                        }
                    };

                    devToolsWindow.Closed += DevToolsClosed;
                    s_open.Add(control, devToolsWindow);
                    devToolsWindow.Show();
                }
            }
        }

        private static void DevToolsClosed(object sender, EventArgs e)
        {
            var devToolsWindow = (Window)sender;
            var devTools = (MainView)devToolsWindow.Content;
            //s_open.Remove((TopLevel)devTools.Root);
            //_keySubscription.Dispose();
            devToolsWindow.Closed -= DevToolsClosed;
        }
    }
}
