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

namespace Extension.Wpf.Dialogs
{
    /// <summary>
    /// Interaction logic for ErrorPopup.xaml
    /// </summary>
    public partial class ErrorPopup : Window
    {
        public ErrorPopup(string title, Exception ex)
        {
            InitializeComponent();

            Title = title;
            Msg.Text = ex.Message;
            Trace.Text = ex.StackTrace;
        }
    }
}
