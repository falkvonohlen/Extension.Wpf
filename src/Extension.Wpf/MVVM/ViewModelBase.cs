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
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// The synchronisation context for this viewmodel
        /// </summary>
        public static SynchronizationContext SynchronizationContext;

        /// <summary>
        /// If set to true and the logger and dioalog service are defined
        /// exceptions are catched 
        /// </summary>
        public bool CatchExceptions = true;

        /// <summary>
        /// The logger for this class
        /// </summary>
        public ILogger Logger { get; protected set; }

        /// <summary>
        /// The dialogservice for this class
        /// </summary>
        public IDialogService DialogService { get; protected set; }

        /// <summary>
        /// The first time a viewmodel base is called the synchronization context is set
        /// </summary>
        public ViewModelBase()
        {
            if (SynchronizationContext == null)
            {
                SynchronizationContext = SynchronizationContext.Current;
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
        protected void RaisePropertyChangedAsync([CallerMemberName] string name = null)
        {
            UICallbackAsync(() => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)));
        }

        /// <summary>
        /// Raises the PropertyChanged event with the name of the property this function is called from
        /// </summary>
        /// <param name="name"></param>
        protected void RaisePropertyChanged([CallerMemberName] string name = null)
        {
            UICallback(() => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)));
        }

        /// <summary>
        /// Raises the property changed event for all properties within this viewmodel
        /// and therefore triggers a rerender for all binding
        /// </summary>
        protected void RaisePropertyChangedForAll()
        {
            UICallback(() => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty)));
        }

        /// <summary>
        /// Raises the property changed event for all properties within this viewmodel
        /// and therefore triggers a rerender for all binding
        /// </summary>
        protected void RaisePropertyChangedForAllAsync()
        {
            UICallbackAsync(() => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty)));
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
            catch (Exception ex) when (CatchExceptions)
            {
                var msg = $"Error during {action.Method.Name}";
                HandleException(ex, msg);
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
            catch (Exception ex) when (CatchExceptions)
            {
                var msg = $"Error during {action.Method.Name}";
                HandleException(ex, msg);
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
            catch (Exception ex) when (CatchExceptions)
            {
                var msg = $"Error during {action.Method.Name}";
                HandleException(ex, msg);
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
            catch (Exception ex) when (CatchExceptions)
            {
                var msg = $"Error during {action.Method.Name}";
                HandleException(ex, msg);
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
            UICallbackAsync(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
                if (Application.Current?.MainWindow != null)
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
            UICallbackAsync(() =>
            {
                Mouse.OverrideCursor = null;
                if (Application.Current?.MainWindow != null)
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
        public static void UICallback(Action action)
        {
            if (SynchronizationContext != null)
            {
                SynchronizationContext.Send(new SendOrPostCallback((o) => action.Invoke()), null);
            }
            else
            {
                throw new InvalidOperationException("The SynchronizationContext of the viewmodel bas has not been set");
            }
        }

        /// <summary>
        /// Runs a given action in the ui thread
        /// </summary>
        /// <param name="action"></param>
        public static void UICallbackAsync(Action action)
        {
            if (SynchronizationContext != null)
            {
                SynchronizationContext.Post(new SendOrPostCallback((o) => action.Invoke()), null);
            }
            else
            {
                throw new InvalidOperationException("The SynchronizationContext of the viewmodel bas has not been set");
            }
        }

        /// <summary>
        /// Handles excpetions
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="msg"></param>
        private void HandleException(Exception ex, string msg)
        {
            if (Logger != null)
            {
                Logger.LogError(ex, msg);
            }
            if (DialogService != null)
            {
                DialogService.ExceptionPopup(msg, ex);
            }
        }
    }
}
