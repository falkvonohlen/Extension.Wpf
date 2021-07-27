using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
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
    /// Interaction logic for FileControl.xaml
    /// </summary>
    public partial class FileControl : UserControl
    {
        /// <summary>
        /// The dependency property used to set the filepath from the parent control
        /// </summary>
        public static readonly DependencyProperty FilePathProperty = DependencyProperty.Register(
            "FilePath", typeof(string), typeof(FileControl),
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// The property used in this control
        /// </summary>
        public string FilePath
        {
            get { return (string)GetValue(FilePathProperty); }
            set { SetValue(FilePathProperty, value); }
        }

        /// <summary>
        /// The dependency property used to set the buttontext from the parent control
        /// </summary>
        public static readonly DependencyProperty ButtonTextProperty = DependencyProperty.Register(
            "ButtonText", typeof(string), typeof(FileControl),
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
        /// The dependency property used to set the default extension from the parent control
        /// </summary>
        public static readonly DependencyProperty DefaultExtProperty = DependencyProperty.Register(
            "DefaultExt", typeof(string), typeof(FileControl),
            new FrameworkPropertyMetadata(null));

        /// <summary>
        /// The property used in this control
        /// </summary>
        public string DefaultExt
        {
            get { return (string)GetValue(DefaultExtProperty); }
            set { SetValue(DefaultExtProperty, value); }
        }

        /// <summary>
        /// The dependency property used to set the Filter from the parent control
        /// </summary>
        public static readonly DependencyProperty FilterProperty = DependencyProperty.Register(
            "Filter", typeof(string), typeof(FileControl),
            new FrameworkPropertyMetadata("*.* (All Files)|*.*"));

        /// <summary>
        /// The property used in this control
        /// </summary>
        public string Filter
        {
            get { return (string)GetValue(FilterProperty); }
            set { SetValue(FilterProperty, value); }
        }

        /// <summary>
        /// The dependency property used to set the Filter from the parent control
        /// </summary>
        public static readonly DependencyProperty InitialDirectoryProperty = DependencyProperty.Register(
            "InitialDirectory", typeof(string), typeof(FileControl),
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
            "ButtonMargin", typeof(Thickness), typeof(FileControl),
            new FrameworkPropertyMetadata(new Thickness(5,0,0,0)));

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
            "TextBoxMargin", typeof(Thickness), typeof(FileControl),
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
            "ButtonColumnWidth", typeof(double), typeof(FileControl),
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
            "IsReadOnly", typeof(bool), typeof(FileControl),
            new FrameworkPropertyMetadata(false));

        /// <summary>
        /// The property used in this control
        /// </summary>
        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        /// <summary>
        /// Instanciates a new file control
        /// </summary>
        public FileControl()
        {
            InitializeComponent();

            //The bindings need to be mapped on the code behind
            (this.Content as FrameworkElement).DataContext = this;
        }

        /// <summary>
        /// Opens the default file dialog and sets the value return to this control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenFileBrowser_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();

            if (DefaultExt != null)
            {
                dialog.DefaultExt = DefaultExt;
            }
            if (Filter != null)
            {
                dialog.Filter = Filter;
            }

            dialog.InitialDirectory = !string.IsNullOrEmpty(FilePath) && Directory.Exists(FilePath) ?
                System.IO.Path.GetDirectoryName(FilePath) : !string.IsNullOrEmpty(InitialDirectory) && Directory.Exists(InitialDirectory) ?
                InitialDirectory : Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            dialog.CheckFileExists = true;

            bool? returnValue = dialog.ShowDialog();

            if (returnValue.HasValue && returnValue.Value)
            {
                FilePath = dialog.FileName;
            }
        }
        
        /// <summary>
        /// If a file is selected and exists it will be opened with the default program
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(FilePath) && File.Exists(FilePath))
            {
                var psi = new ProcessStartInfo(FilePath)
                {
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
        }

        /// <summary>
        /// If a file is selected and exists the explorer will be opened at the location it is stored
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenExplorer_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(FilePath) && File.Exists(FilePath))
            {
                var psi = new ProcessStartInfo("explorer.exe")
                {
                    Arguments = $"/separate /select, {System.IO.Path.GetDirectoryName(FilePath)}",
                    UseShellExecute = false,
                };
                Process.Start(psi);
            }
        }
    }
}
