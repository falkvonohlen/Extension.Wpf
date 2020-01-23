using Extension.Wpf.Dialogs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Extension.Wpf.MVVM
{
    /// <summary>
    /// The viewmodel base provides notification utilities for
    /// properties used in the gui by implementing the INotifyPropertyChanged interface
    /// and provide a RaisePropertyChanged method that gets the property name via reflection
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged
    {
        private static SynchronizationContext _context;
        private readonly ILogger _log;
        protected IDialogService _dialogs;

        /// <summary>
        /// The first time a viewmodel base is called the synchronization context is set
        /// </summary>
        public ViewModelBase(ILogger logger, IDialogService dialogs)
        {
            _log = logger;
            _dialogs = dialogs;

            if (_context == null)
            {
                _context = SynchronizationContext.Current;
            }
        }

        /// <summary>
        /// Event fired, if a property changed. Gui properties listen to this
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the PropertyChanged event with the name of the property this function is called from
        /// </summary>
        /// <param name="name"></param>
        protected void RaisePropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        /// <summary>
        /// Run an user action with exception handling
        /// </summary>
        /// <param name="action"></param>
        protected void UserAction(Action action, Action final = null)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error during a user triggered action");
                _dialogs.ExceptionPopup($"Error during {action.Method.Name}", ex);
            }
            finally
            {
                if (final != null)
                {
                    final.Invoke();
                }
            }
        }

        /// <summary>
        /// Run an user action with exception handling
        /// </summary>
        /// <param name="action"></param>
        protected Task UserActionAsync(Action action, Action final = null)
        {
            return Task.Run(() =>
            {
                UserAction(action, final);
            });
        }

        /// <summary>
        /// Runs a given action in the ui thread
        /// </summary>
        /// <param name="action"></param>
        public static void RunInUiThread(Action action)
        {
            _context.Post(new SendOrPostCallback((o) => action.Invoke()), null);
        }
    }
}
