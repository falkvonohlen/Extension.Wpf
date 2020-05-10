using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Input;

namespace Extension.Wpf.MVVM
{
    /// <summary>
    /// A relay command is used to bind execution and validation functions
    /// to a command in the gui
    /// </summary>
    public class RelayCommand<T> : ICommand
    {
        private SynchronizationContext _synchronizationContext;
        readonly Action<T> _execute = null;
        readonly Predicate<T> _canExecute = null;

        /// <summary>
        /// Initializes a new instance of <see cref="RelayCommand{T}"/>.
        /// </summary>
        /// <param name="execute">Delegate to execute when Execute is called on the command.  This can be null to just hook up a CanExecute delegate.</param>
        /// <remarks><seealso cref="CanExecute"/> will always return true.</remarks>
        public RelayCommand(Action<T> execute)
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));

            _execute = execute;
            _synchronizationContext = SynchronizationContext.Current;
        }

        /// <summary>
        /// Creates a new command.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public RelayCommand(Action<T> execute, Predicate<T> canExecute) : this(execute)
        {
            if (canExecute == null)
                throw new ArgumentNullException(nameof(canExecute));

            _canExecute = canExecute;
            _synchronizationContext = SynchronizationContext.Current;
        }

        ///<summary>
        ///Defines the method that determines whether the command can execute in its current state.
        ///</summary>
        ///<param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        ///<returns>
        ///true if this command can be executed; otherwise, false.
        ///</returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute((T)parameter);
        }

        ///<summary>
        ///Defines the method to be called when the command is invoked.
        ///</summary>
        ///<param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to <see langword="null" />.</param>
        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }

        ///<summary>
        ///Occurs when changes occur that affect whether or not the command should execute.
        ///</summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Called to trigger a reevcaluation of the can Execute method
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            _synchronizationContext.Post(o => CanExecuteChanged?.Invoke(this, EventArgs.Empty), null);
        }
    }

    /// <summary>
    /// A relay command is used to bind execution and validation functions
    /// to a command in the gui
    /// </summary>
    public class RelayCommand : ICommand
    {
        private SynchronizationContext _synchronizationContext;
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        /// <summary>
        /// Creates a new command
        /// </summary>
        /// <param name="execute"></param>
        public RelayCommand(Action execute)
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));

            _execute = execute;
            _synchronizationContext = SynchronizationContext.Current;
        }



        /// <summary>
        /// Creates a new command with a can Execute method.
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        public RelayCommand(Action execute, Func<bool> canExecute) : this(execute)
        {
            if (canExecute == null)
                throw new ArgumentNullException(nameof(canExecute));

            _canExecute = canExecute;
            _synchronizationContext = SynchronizationContext.Current;
        }

        ///<summary>
        ///Defines the method that determines whether the command can execute in its current state.
        ///</summary>
        ///<returns>
        ///true if this command can be executed; otherwise, false.
        ///</returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute();
        }

        ///<summary>
        ///Defines the method to be called when the command is invoked.
        ///</summary>
        public virtual void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                _execute();
            }
        }

        ///<summary>
        ///Occurs when changes occur that affect whether or not the command should execute.
        ///</summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Called to trigger a reevcaluation of the can Execute method
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            _synchronizationContext.Post(o => CanExecuteChanged?.Invoke(this, EventArgs.Empty), null);
        }
    }
}
