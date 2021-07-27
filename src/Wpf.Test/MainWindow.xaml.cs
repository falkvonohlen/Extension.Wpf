using Extension.Wpf.Dialogs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Wpf.Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ILogger _logger;
        private Random _rnd = new Random();

        public MainWindow(ILogger<MainWindow> logger)
        {
            _logger = logger;
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                throw new ArgumentException("Test");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error Button clicked");
                var popup = new ErrorPopup("Test", ex);
                popup.Show();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var value = _rnd.Next(4);
            switch (value)
            {
                case 0:
                    _logger.LogTrace("A random trace log.");
                    break;
                case 1:
                    _logger.LogDebug("A random debug log.");
                    break;
                case 2:
                    _logger.LogInformation("A random information log.");
                    break;
                case 3:
                    _logger.LogWarning("A random warning log.");
                    break;
                case 4:
                    _logger.LogError("A random error log.");
                    break;
            }
        }
    }
}
