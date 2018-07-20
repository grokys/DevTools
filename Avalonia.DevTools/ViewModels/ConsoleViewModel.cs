using System;
using System.Reflection;
using System.Threading.Tasks;
using Avalonia.Collections;
using Avalonia.DevTools.Models;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace Avalonia.DevTools.ViewModels
{
    internal class ConsoleViewModel : ViewModelBase
    {
        readonly ConsoleContext _context = new ConsoleContext();
        readonly Action<ConsoleContext> _updateContext;
        ScriptState<object> _state;
        string _input;

        public ConsoleViewModel(Action<ConsoleContext> updateContext)
        {
            _updateContext = updateContext;
        }

        public string Input
        {
            get => _input;
            set => RaiseAndSetIfChanged(ref _input, value);
        }

        public AvaloniaList<string> Output { get; } = new AvaloniaList<string>();

        public async Task Execute()
        {
            try
            {
                var options = ScriptOptions.Default
                    .AddReferences(Assembly.GetAssembly(typeof(Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo)));

                _updateContext(_context);

                if (_state == null)
                {
                    _state = await CSharpScript.RunAsync(Input, options: options, globals: _context);
                }
                else
                {
                    _state = await _state.ContinueWithAsync(Input);
                }

                Output.Add(_state.ReturnValue?.ToString() ?? "No output");
            }
            catch (Exception ex)
            {
                Output.Add(ex.Message);
            }

            Input = string.Empty;
        }
    }
}
