// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Reactive.Linq;
using Avalonia.Data;
using Avalonia.Diagnostics;

namespace Avalonia.DevTools.ViewModels
{
    internal class PropertyDetails : ViewModelBase
    {
        private object _value;
        private string _priority;
        private string _diagnostic;
        private string _group;

        public PropertyDetails(AvaloniaObject o, AvaloniaProperty property)
        {
            Name = property.IsAttached ?
                $"[{property.OwnerType.Name}.{property.Name}]" :
                property.Name;
            IsAttached = property.IsAttached;
            UpdateGroup();

            // TODO: Unsubscribe when view model is deactivated.
            o.GetObservable(property).Subscribe(x =>
            {
                var diagnostic = o.GetDiagnostic(property);
                Value = diagnostic.Value ?? "(null)";
                Priority = (diagnostic.Priority != BindingPriority.Unset) ?
                    diagnostic.Priority.ToString() :
                    diagnostic.Property.Inherits ? "Inherited" : "Unset";
                Diagnostic = diagnostic.Diagnostic;
            });
        }

        public string Name { get; }

        public bool IsAttached { get; }

        public string Priority
        {
            get { return _priority; }
            private set { RaiseAndSetIfChanged(ref _priority, value); }
        }

        public string Diagnostic
        {
            get { return _diagnostic; }
            private set { RaiseAndSetIfChanged(ref _diagnostic, value); }
        }

        public object Value
        {
            get { return _value; }
            private set { RaiseAndSetIfChanged(ref _value, value); }
        }

        public string Group
        {
            get { return _group; }
            private set { RaiseAndSetIfChanged(ref _group, value); }
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            UpdateGroup();
        }

        private void UpdateGroup()
        {
            if (Priority == "Unset")
            {
                Group = Priority;
            }
            else if (IsAttached)
            {
                Group = "Attached Properties";
            }
            else
            {
                Group = "Properties";
            }
        }
    }
}
