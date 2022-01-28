using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Extension.Wpf.UserControls.Logging
{
    /// <summary>
    /// Interaction logic for Logging.xaml
    /// </summary>
    public partial class LogView : UserControl
    {
        private static SynchronizationContext _synchronizationContext;

        /// <summary>
        /// The dependency property used to set the Filter from the parent control
        /// </summary>
        public static readonly DependencyProperty IdProperty = DependencyProperty.Register(
            "Id", typeof(string), typeof(LogView),
            new FrameworkPropertyMetadata("", new PropertyChangedCallback(IdChangedCallback)));

        /// <summary>
        /// The property used in this control
        /// </summary>
        public string Id
        {
            get { return (string)GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }

        /// <summary>
        /// The dependency property used to set the Filter from the parent control
        /// </summary>
        public static readonly DependencyProperty AutoScrollProperty = DependencyProperty.Register(
            "AutoScroll", typeof(bool), typeof(LogView),
            new FrameworkPropertyMetadata(true));

        /// <summary>
        /// The property used in this control
        /// </summary>
        public bool AutoScroll
        {
            get { return (bool)GetValue(AutoScrollProperty); }
            set { SetValue(AutoScrollProperty, value); }
        }

        /// <summary>
        /// A concurrent bag with log events
        /// </summary>
        private ConcurrentBag<LogEvent> LogEventsBag = new ConcurrentBag<LogEvent>();

        /// <summary>
        /// The list of all log events for this control
        /// </summary>
        public ObservableCollection<LogEvent> LogEvents { get; set; } = new ObservableCollection<LogEvent>();

        public LogView()
        {
            if (_synchronizationContext == default)
            {
                _synchronizationContext = SynchronizationContext.Current;
            }

            InitializeComponent();

            //The bindings need to be mapped on the code behind
            (this.Content as FrameworkElement).DataContext = this;
        }

        public void AddLogEvent(LogEvent log)
        {
            _synchronizationContext.Post(new SendOrPostCallback((o) => { LogEvents.Add(log); }), null);
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                var block = (TextBlock)sender;
                var logEvent = (LogEvent)block.DataContext;
                var popup = new LoggingDetails(logEvent)
                {
                    Owner = Application.Current.MainWindow,
                };
                if (Background != default)
                {
                    popup.Background = Background;
                }
                if (Foreground != default)
                {
                    popup.Foreground = Foreground;
                }
                popup.ShowDialog();
            }
        }

        /// <summary>
        /// Called when the logger id of the log view and therefor the logging target changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void IdChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var c = (LogView)sender;
            _synchronizationContext.Post(new SendOrPostCallback((o) =>
            {
                var oldValue = (string)e.OldValue;
                var newValue = (string)e.NewValue;
                if (string.IsNullOrEmpty(newValue) && !string.IsNullOrEmpty(oldValue))
                {
                    LogViewBroker.UnregisterControl(oldValue);
                }
                
                if (!string.IsNullOrEmpty(newValue))
                {
                    LogViewBroker.RegisterControl(newValue, c);
                }
            }), null);
        }
    }
}
