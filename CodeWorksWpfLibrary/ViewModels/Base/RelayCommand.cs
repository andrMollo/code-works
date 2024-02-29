using System;
using System.Windows.Input;

namespace CodeWorksWpfLibrary.ViewModels.Base
{
    /// <summary>
    /// A basic command that runs an Action
    /// </summary>
    internal class RelayCommand : ICommand
    {
        #region Private fields

        private readonly Action _action;

        #endregion

        #region Public events

        /// <summary>
        /// The events that fires when the <see cref="CanExecute(object)"/> value has changed
        /// </summary>
        public event EventHandler CanExecuteChanged = (sender, e) => { };

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public RelayCommand(Action action)
        {
            _action = action;
        }

        #endregion

        #region Command methods

        /// <summary>
        /// A relay command can always execute
        /// </summary>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Executes the commands Action
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            _action();
        }

        #endregion
    }
}
