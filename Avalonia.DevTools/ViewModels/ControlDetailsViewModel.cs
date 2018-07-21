// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Collections;
using Avalonia.VisualTree;

namespace Avalonia.DevTools.ViewModels
{
    internal class ControlDetailsViewModel : ViewModelBase
    {
        private static readonly PriorityComparer s_priorityComparer;
        private string _propertyFilter;

        public ControlDetailsViewModel(IVisual control)
        {
            if (control is AvaloniaObject avaloniaObject)
            {
                Properties = AvaloniaPropertyRegistry.Instance.GetRegistered(avaloniaObject)
                    .Concat(AvaloniaPropertyRegistry.Instance.GetRegisteredAttached(avaloniaObject.GetType()))
                    .Select(x => new PropertyDetails(avaloniaObject, x))
                    .OrderBy(x => x.Priority, s_priorityComparer)
                    .ThenBy(x => x.IsAttached)
                    .ThenBy(x => x.Name);

                var view = new CollectionViewBase(Properties);
                view.GroupDescriptions.Add(new PathGroupDescription(nameof(PropertyDetails.Group)));
                view.Filter = FilterProperty;
                PropertiesView = view;
            }
        }

        public IEnumerable<string> Classes
        {
            get;
            private set;
        }

        public IEnumerable<PropertyDetails> Properties
        {
            get;
            private set;
        }

        public ICollectionView PropertiesView { get; }

        public string PropertyFilter
        {
            get => _propertyFilter;
            set
            {
                if (RaiseAndSetIfChanged(ref _propertyFilter, value))
                {
                    PropertiesView.Refresh();
                }
            }
        }

        private bool FilterProperty(object arg)
        {
            if (!string.IsNullOrWhiteSpace(PropertyFilter) && arg is PropertyDetails property)
            {
                return property.Name.IndexOf(PropertyFilter, StringComparison.OrdinalIgnoreCase) != -1;
            }

            return true;
        }

        private class PriorityComparer : IComparer<string>
        {
            public int Compare(string x, string y)
            {
                if (x == y)
                {
                    return 0;
                }
                else if (x == "Unset")
                {
                    return -1;
                }
                else if (y == "Unset")
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}
