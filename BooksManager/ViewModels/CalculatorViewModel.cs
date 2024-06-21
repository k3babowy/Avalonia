using System;
using System.ComponentModel;
using System.Reactive;
using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ReactiveUI;

namespace BooksManager.ViewModels
{
    public class CalculatorViewModel : ViewModelBase
    {
        private string _display;
        private string _currentInput = "";
        private string _operator = "";
        private double _firstNumber = 0;
        private double _secondNumber = 0;

        public string Display
        {
            get => _display;
            set => this.RaiseAndSetIfChanged(ref _display, value);
        }

        public CalculatorViewModel()
        {
            ClearCommand = ReactiveCommand.Create(Clear);
            EnterDigitCommand = ReactiveCommand.Create<string>(EnterDigit);
            EnterOperatorCommand = ReactiveCommand.Create<string>(EnterOperator);
            CalculateCommand = ReactiveCommand.Create(Calculate);
        }

        public ReactiveCommand<Unit, Unit> ClearCommand { get; }
        public ReactiveCommand<string, Unit> EnterDigitCommand { get; }
        public ReactiveCommand<string, Unit> EnterOperatorCommand { get; }
        public ReactiveCommand<Unit, Unit> CalculateCommand { get; }

        private void Clear()
        {
            _currentInput = "";
            _firstNumber = 0;
            _secondNumber = 0;
            _operator = "";
            Display = "";
        }

        private void EnterDigit(string digit)
        {
            _currentInput += digit;
            Display = _currentInput;
        }

        private void EnterOperator(string op)
        {
            if (!string.IsNullOrEmpty(_currentInput))
            {
                _firstNumber = double.Parse(_currentInput);
                _currentInput = "";
            }
            _operator = op;
        }

        private void Calculate()
        {
            _secondNumber = double.Parse(_currentInput);
            switch (_operator)
            {
                case "+":
                    Display = (_firstNumber + _secondNumber).ToString();
                    break;
                case "-":
                    Display = (_firstNumber - _secondNumber).ToString();
                    break;
                case "*":
                    Display = (_firstNumber * _secondNumber).ToString();
                    break;
                case "/":
                    Display = (_firstNumber / _secondNumber).ToString();
                    break;
            }
            _currentInput = Display;
            _operator = "";
        }
    }
}
