using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Extension.Wpf.UserControls
{
    /// <summary>
    /// Interaction logic for DirectoryControl.xaml
    /// </summary>
    public partial class DirectoryControl : System.Windows.Controls.UserControl
    {
        /// <summary>
        /// The dependency property used to set the filepath from the parent control
        /// </summary>
        public static readonly DependencyProperty DirectoryPathProperty = DependencyProperty.Register(
            "DirectoryPath", typeof(string), typeof(DirectoryControl),
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// The property used in this control
        /// </summary>
        public string DirectoryPath
        {
            get { return (string)GetValue(DirectoryPathProperty); }
            set { SetValue(DirectoryPathProperty, value); }
        }

        /// <summary>
        /// The dependency property used to set the buttontext from the parent control
        /// </summary>
        public static readonly DependencyProperty ButtonTextProperty = DependencyProperty.Register(
            "ButtonText", typeof(string), typeof(DirectoryControl),
            new FrameworkPropertyMetadata("Browse"));

        /// <summary>
        /// The property used in this control
        /// </summary>
        public string ButtonText
        {
            get { return (string)GetValue(ButtonTextProperty); }
            set { SetValue(ButtonTextProperty, value); }
        }

        /// <summary>
        /// The dependency property used to set the Filter from the parent control
        /// </summary>
        public static readonly DependencyProperty InitialDirectoryProperty = DependencyProperty.Register(
            "InitialDirectory", typeof(string), typeof(DirectoryControl),
            new FrameworkPropertyMetadata(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)));

        /// <summary>
        /// The property used in this control
        /// </summary>
        public string InitialDirectory
        {
            get { return (string)GetValue(InitialDirectoryProperty); }
            set { SetValue(InitialDirectoryProperty, value); }
        }

        /// <summary>
        /// The dependency property used to set the margin of the button from the parent control
        /// </summary>
        public static readonly DependencyProperty ButtonMarginProperty = DependencyProperty.Register(
            "ButtonMargin", typeof(Thickness), typeof(DirectoryControl),
            new FrameworkPropertyMetadata(new Thickness(5, 0, 0, 0)));

        /// <summary>
        /// The property used in this control
        /// </summary>
        public Thickness ButtonMargin
        {
            get { return (Thickness)GetValue(ButtonMarginProperty); }
            set { SetValue(ButtonMarginProperty, value); }
        }

        /// <summary>
        /// The dependency property used to set the margin of the textbox from the parent control
        /// </summary>
        public static readonly DependencyProperty TextBoxMarginProperty = DependencyProperty.Register(
            "TextBoxMargin", typeof(Thickness), typeof(DirectoryControl),
            new FrameworkPropertyMetadata(new Thickness(0, 0, 0, 0)));

        /// <summary>
        /// The property used in this control
        /// </summary>
        public Thickness TextBoxMargin
        {
            get { return (Thickness)GetValue(TextBoxMarginProperty); }
            set { SetValue(TextBoxMarginProperty, value); }
        }

        /// <summary>
        /// The dependency property used to set the width of the button column from the parent control
        /// </summary>
        public static readonly DependencyProperty ButtonColumnWidthProperty = DependencyProperty.Register(
            "ButtonColumnWidth", typeof(double), typeof(DirectoryControl),
            new FrameworkPropertyMetadata(100.0));

        /// <summary>
        /// The property used in this control
        /// </summary>
        public double ButtonColumnWidth
        {
            get { return (double)GetValue(ButtonColumnWidthProperty); }
            set { SetValue(ButtonColumnWidthProperty, value); }
        }

        /// <summary>
        /// The dependency property used to set the width of the button column from the parent control
        /// </summary>
        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register(
            "IsReadOnly", typeof(bool), typeof(DirectoryControl),
            new FrameworkPropertyMetadata(false));

        /// <summary>
        /// The property used in this control
        /// </summary>
        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        public DirectoryControl()
        {
            InitializeComponent();

            //The bindings need to be mapped on the code behind
            (this.Content as FrameworkElement).DataContext = this;
        }

        /// <summary>
        /// Opens the default directory dialog and sets the value return to this control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenDirectoryBrowser_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            dialog.ShowNewFolderButton = true;
            dialog.SelectedPath = !string.IsNullOrEmpty(DirectoryPath) && Directory.Exists(DirectoryPath) ?
                DirectoryPath : !string.IsNullOrEmpty(InitialDirectory) && Directory.Exists(InitialDirectory) ?
                InitialDirectory : Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (DialogResult.OK == dialog.ShowDialog())
            {
                DirectoryPath = dialog.SelectedPath;
            }
        }

        /// <summary>
        /// If a file is selected and exists the explorer will be opened at the location it is stored
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Open_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(DirectoryPath) && Directory.Exists(DirectoryPath))
            {
                var psi = new ProcessStartInfo("explorer.exe")
                {
                    Arguments = DirectoryPath,
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
        }
    }
}
