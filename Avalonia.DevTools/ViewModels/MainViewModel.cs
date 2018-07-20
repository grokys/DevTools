using System;
using System.Collections.Generic;
using System.Text;
using Avalonia.Controls;
using Avalonia.DevTools.Models;
using Avalonia.Input;

namespace Avalonia.DevTools.ViewModels
{
    internal class MainViewModel : ViewModelBase
    {
        private TreePageViewModel _content;
        private int _selectedTab;
        private TreePageViewModel _logicalTree;
        private TreePageViewModel _visualTree;
        private string _focusedControl;
        private string _pointerOverElement;

        public MainViewModel(IControl root)
        {
            _logicalTree = new TreePageViewModel(LogicalTreeNode.Create(root));
            _visualTree = new TreePageViewModel(VisualTreeNode.Create(root));

            UpdateFocusedControl();
            KeyboardDevice.Instance.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(KeyboardDevice.Instance.FocusedElement))
                {
                    UpdateFocusedControl();
                }
            };

            SelectedTab = 0;
            root.GetObservable(TopLevel.PointerOverElementProperty)
                .Subscribe(x => PointerOverElement = x?.GetType().Name);
            Console = new ConsoleViewModel(UpdateConsoleContext);
        }

        public ConsoleViewModel Console { get; }

        public TreePageViewModel Content
        {
            get { return _content; }
            private set { RaiseAndSetIfChanged(ref _content, value); }
        }

        public int SelectedTab
        {
            get { return _selectedTab; }
            set
            {
                _selectedTab = value;

                switch (value)
                {
                    case 0:
                        Content = _logicalTree;
                        break;
                    case 1:
                        Content = _visualTree;
                        break;
                }

                RaisePropertyChanged();
            }
        }

        public string FocusedControl
        {
            get { return _focusedControl; }
            private set { RaiseAndSetIfChanged(ref _focusedControl, value); }
        }

        public string PointerOverElement
        {
            get { return _pointerOverElement; }
            private set { RaiseAndSetIfChanged(ref _pointerOverElement, value); }
        }

        private void UpdateConsoleContext(ConsoleContext context)
        {
            context.e = Content.SelectedNode?.Visual;
        }

        public void SelectControl(IControl control)
        {
            var tree = Content as TreePageViewModel;

            if (tree != null)
            {
                tree.SelectControl(control);
            }
        }

        private void UpdateFocusedControl()
        {
            FocusedControl = KeyboardDevice.Instance.FocusedElement?.GetType().Name;
        }
    }
}
