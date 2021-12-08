using FileManager.Core.Annotations;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace FileManager
{
    public class ActionCommand<TParameter> : ActionCommand
    {
        private new readonly Action<TParameter> _execute;

        public ActionCommand(Predicate<object> canExecute, Action<TParameter> execute)
        {
            this._execute = execute;
            base._execute = delegate (object o)
            {
                execute((TParameter)o);
            };

            this._canExecute = canExecute;
        }

        public ActionCommand(Action<TParameter> execute)
        {
            this._execute = execute;
            base._execute = delegate (object o) { execute((TParameter)o); };

            this._canExecute = o => true;
        }

        public void Execute(TParameter parameter)
        {
            this._execute(parameter);
        }
    }

    public class ActionCommand : ICommand, INotifyPropertyChanged
    {
        protected Predicate<object> _canExecute;
        protected Action<object> _execute;

        public ActionCommand(Predicate<object> canExecute, Action<object> execute)
        {
            this._canExecute = canExecute;
            this._execute = execute;
        }

        public ActionCommand(Action<object> execute) : this()
        {
            this._execute = execute;
        }

        protected ActionCommand()
        {
            this._canExecute = x => true;
        }

        public void Execute(object parameter)
        {
            this._execute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public virtual bool CanExecute(object parameter)
        {
            return this._canExecute(parameter);
        }

        public virtual void Execute()
        {
            this._execute(null);
        }

        public static ActionCommand Create(Action<object> action)
        {
            return new ActionCommand(action);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}