using Extension.Wpf.Dialogs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

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

        /// <summary>
        /// The logger for this class
        /// In order to use it the Microsoft logging extension has to be used as well
        /// </summary>
        private readonly ILogger _log;

        /// <summary>
        /// A dialog service implemetation in order to
        /// provide a popup on catched exceptions
        /// </summary>
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
        /// Raises the property changed event for all properties within this viewmodel
        /// and therefore triggers a rerender for all binding
        /// </summary>
        protected void RaisePropertyChangedForAll()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty));
        }

        /// <summary>
        /// Run an user action with exception handling
        /// </summary>
        /// <param name="action">The executed action</param>
        protected void UserAction(Action action)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception ex)
            {
                var msg = $"Error during {action.Method.Name}";
                _log.LogError(ex, msg);
                _dialogs.ExceptionPopup(msg, ex);
            }
        }

        /// <summary>
        /// Run an user action with exception handling
        /// </summary>
        /// <param name="action">The executed action</param>
        /// <param name="finalAction">Runs always even if an exception has been catched</param>
        /// 
        protected void UserAction(Action action, Action finalAction)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception ex)
            {
                var msg = $"Error during {action.Method.Name}";
                _log.LogError(ex, msg);
                _dialogs.ExceptionPopup(msg, ex);
            }
            finally
            {
                if (finalAction != null)
                {
                    finalAction.Invoke();
                }
            }
        }

        /// <summary>
        /// Run an user action with exception handling and sets the application in a loading state in the mean time
        /// </summary>
        /// <param name="action"></param>
        /// <param name="loading"></param>
        protected void UserAction(Action action, bool loading)
        {
            try
            {
                if (loading)
                {
                    SetLoading();
                }
                action.Invoke();
            }
            catch (Exception ex)
            {
                var msg = $"Error during {action.Method.Name}";
                _log.LogError(ex, msg);
                _dialogs.ExceptionPopup(msg, ex);
            }
            finally
            {
                if (loading)
                {
                    ResetLoading();
                }
            }
        }

        /// <summary>
        /// Run an user action with exception handling and sets the application in a loading state in the mean time
        /// </summary>
        /// <param name="action"></param>
        /// <param name="finalAction">Runs always even if an exception has been catched</param>
        /// <param name="loading"></param>
        protected void UserAction(Action action, Action finalAction, bool loading)
        {
            try
            {
                if (loading)
                {
                    SetLoading();
                }
                action.Invoke();
            }
            catch (Exception ex)
            {
                var msg = $"Error during {action.Method.Name}";
                _log.LogError(ex, msg);
                _dialogs.ExceptionPopup(msg, ex);
            }
            finally
            {
                if (finalAction != null)
                {
                    finalAction.Invoke();
                }
                if (loading)
                {
                    ResetLoading();
                }
            }
        }

        /// <summary>
        /// Run an user action with exception handling
        /// </summary>
        /// <param name="action"></param>
        protected Task UserActionAsync(Action action)
        {
            return Task.Run(() =>
            {
                UserAction(action);
            });
        }

        /// <summary>
        /// Run an user action with exception handling
        /// </summary>
        /// <param name="action"></param>
        protected Task UserActionAsync(Action action, Action finalAction)
        {
            return Task.Run(() =>
            {
                UserAction(action, finalAction);
            });
        }

        /// <summary>
        /// Run an user action with exception handling
        /// </summary>
        /// <param name="action"></param>
        protected Task UserActionAsync(Action action, bool loading)
        {
            return Task.Run(() =>
            {
                UserAction(action, loading);
            });
        }

        /// <summary>
        /// Run an user action with exception handling
        /// </summary>
        /// <param name="action"></param>
        protected Task UserActionAsync(Action action, Action finalAction, bool loading)
        {
            return Task.Run(() =>
            {
                UserAction(action, finalAction, loading);
            });
        }

        /// <summary>
        /// Set the application in a loading state
        /// Replacing the cursor and catches all mouse interactions
        /// </summary>
        protected void SetLoading()
        {
            RunInUiThreadAsync(() => 
            {
                Mouse.OverrideCursor = Cursors.Wait;
                if (Application.Current != null)
                {
                    Application.Current.MainWindow.PreviewMouseDown += MainWindow_PreviewMouseDown;
                }
            });
        }

        /// <summary>
        /// Resets the application back to its normal mode
        /// </summary>
        protected void ResetLoading()
        {
            RunInUiThreadAsync(() =>
            {
                Mouse.OverrideCursor = null;
                if (Application.Current != null)
                {
                    Application.Current.MainWindow.PreviewMouseDown -= MainWindow_PreviewMouseDown;
                }
            });
        }

        /// <summary>
        /// The to handle. This way the mouse action will be ignored
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        /// <summary>
        /// Runs a given action in the ui thread
        /// </summary>
        /// <param name="action"></param>
        public static void RunInUiThread(Action action)
        {
            _context.Send(new SendOrPostCallback((o) => action.Invoke()), null);
        }

        /// <summary>
        /// Runs a given action in the ui thread
        /// </summary>
        /// <param name="action"></param>
        public static void RunInUiThreadAsync(Action action)
        {
            _context.Post(new SendOrPostCallback((o) => action.Invoke()), null);
        }
    }
}
