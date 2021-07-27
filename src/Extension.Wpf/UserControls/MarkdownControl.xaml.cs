using Markdig;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for MarkdownControl.xaml
    /// </summary>
    public partial class MarkdownControl : UserControl
    {
        /// <summary>
        /// Caption Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
               "Text",
               typeof(string),
               typeof(MarkdownControl),
               new FrameworkPropertyMetadata("# No Content Provided", new PropertyChangedCallback(TextChanged)));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public MarkdownControl()
        {
            InitializeComponent();
            _browser.Navigating += Browser_Navigating;
        }

        /// <summary>
        /// Called when the logger target changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void TextChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var mc = (MarkdownControl)sender;

            string htmlString = CreateHtml(mc, "<h1>No Content Provided</h1>");
            if (!string.IsNullOrEmpty(mc.Text))
            {
                try
                {
                    var htmlBody = Markdown.ToHtml(mc.Text);
                    htmlString = CreateHtml(mc, htmlBody);
                }
                catch (Exception ex)
                {
                    htmlString = CreateHtml(mc, $"<h1>Markdown Parsing failed!</h1><p>{ex.Message}</p>");
                }
            }

            mc._browser.NavigateToString(htmlString);
        }

        private static string CreateHtml(MarkdownControl refControl, string body)
        {
            return $"<body style=\"background-color:#333;color:#eee;overflow:auto;font-family:{refControl.FontFamily};font-size:{refControl.FontSize}\">\n" +
                        $"{body}\n" +
                   "</body>\n";
        }

        /// <summary>
        /// Redirects the navigation to the standard browser as we do not want to display it within wpf
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Browser_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            if (e.Uri != default)
            {
                Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
                e.Cancel = true;
            }
        }
    }
}
