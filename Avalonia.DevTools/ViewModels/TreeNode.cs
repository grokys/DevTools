// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;
using System.Collections.Specialized;
using System.Reactive;
using System.Reactive.Linq;
using Avalonia.Collections;
using Avalonia.Styling;
using Avalonia.VisualTree;

namespace Avalonia.DevTools.ViewModels
{
    internal class TreeNode : ViewModelBase
    {
        private string _classes;
        private bool _isExpanded;

        public TreeNode(IVisual visual, TreeNode parent)
        {
            Parent = parent;
            Type = visual.GetType().Name;
            Visual = visual;

            if (visual is IStyleable styleable)
            {
                var classesChanged = Observable.FromEventPattern<
                        NotifyCollectionChangedEventHandler,
                        NotifyCollectionChangedEventArgs>(
                    x => styleable.Classes.CollectionChanged += x,
                    x => styleable.Classes.CollectionChanged -= x)
                    .TakeUntil(((IStyleable)styleable).StyleDetach);

                classesChanged.Select(_ => Unit.Default)
                    .StartWith(Unit.Default)
                    .Subscribe(_ =>
                    {
                        if (styleable.Classes.Count > 0)
                        {
                            Classes = "(" + string.Join(" ", styleable.Classes) + ")";
                        }
                        else
                        {
                            Classes = string.Empty;
                        }
                    });
            }
        }

        public IAvaloniaReadOnlyList<TreeNode> Children
        {
            get;
            protected set;
        }

        public string Classes
        {
            get { return _classes; }
            private set { RaiseAndSetIfChanged(ref _classes, value); }
        }

        public IVisual Visual
        {
            get;
        }

        public bool IsExpanded
        {
            get { return _isExpanded; }
            set { RaiseAndSetIfChanged(ref _isExpanded, value); }
        }

        public TreeNode Parent
        {
            get;
        }

        public string Type
        {
            get;
            private set;
        }
    }
}
