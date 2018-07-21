// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Globalization;
using System.Reactive.Linq;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Diagnostics;
using Avalonia.Markup.Xaml;

namespace Avalonia.DevTools.ViewModels
{
    internal class PropertyDetails : ViewModelBase
    {
        private AvaloniaObject _target;
        private AvaloniaProperty _property;
        private object _value;
        private string _priority;
        private string _diagnostic;
        private TypeConverter _converter;
        private string _group;

        public PropertyDetails(AvaloniaObject o, AvaloniaProperty property)
        {
            _target = o;
            _property = property;

            Name = property.IsAttached ?
                $"[{property.OwnerType.Name}.{property.Name}]" :
                property.Name;
            UpdateGroup();

            // TODO: Unsubscribe when view model is deactivated.
            o.GetObservable(property).Subscribe(x =>
            {
                var diagnostic = o.GetDiagnostic(property);
                RaiseAndSetIfChanged(ref _value, diagnostic.Value);
                Priority = (diagnostic.Priority != BindingPriority.Unset) ?
                    diagnostic.Priority.ToString() :
                    diagnostic.Property.Inherits ? "Inherited" : "Unset";
                Diagnostic = diagnostic.Diagnostic;
            });
        }

        public string Name { get; }

        public bool IsAttached => _property.IsAttached;

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

        public string Value
        {
            get
            {
                if (_value == null)
                {
                    return "(null)";
                }

                return Converter?.CanConvertTo(typeof(string)) == true ?
                    Converter.ConvertToString(_value) :
                    _value.ToString();
            }
            set
            {
                try
                {
                    var convertedValue = Converter?.CanConvertFrom(typeof(string)) == true ?
                        Converter.ConvertFromString(value) :
                        DefaultValueConverter.Instance.ConvertBack(value, _property.PropertyType, null, CultureInfo.CurrentCulture);
                    _target.SetValue(_property, convertedValue);
                }
                catch { }
            }
        }

        public string Group
        {
            get { return _group; }
            private set { RaiseAndSetIfChanged(ref _group, value); }
        }

        private TypeConverter Converter
        {
            get
            {
                if (_converter == null)
                {
                    var converterType = AvaloniaTypeConverters.GetTypeConverter(_value.GetType());

                    if (converterType != null)
                    {
                        _converter = (TypeConverter)Activator.CreateInstance(converterType);
                    }
                }

                return _converter;
            }
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
