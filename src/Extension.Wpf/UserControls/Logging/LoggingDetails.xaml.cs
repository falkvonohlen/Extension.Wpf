using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Extension.Wpf.UserControls.Logging
{
    /// <summary>
    /// Interaction logic for LoggingDetails.xaml
    /// </summary>
    public partial class LoggingDetails : Window
    {
        public LogEvent Event { get; set; }

        public LoggingDetails(LogEvent logEvent)
        {
            Event = logEvent;
            InitializeComponent();
            (this.Content as FrameworkElement).DataContext = this;
        }
    }
}
