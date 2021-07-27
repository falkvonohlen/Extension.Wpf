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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Extension.Wpf.UserControls
{
    /// <summary>
    /// Interaction logic for OnOffButton.xaml
    /// </summary>
    public partial class OnOffButton : UserControl
    {
        public static readonly DependencyProperty EnabledBrushProperty = DependencyProperty.Register("EnabledBrush", typeof(Brush), typeof(OnOffButton),
         new PropertyMetadata(Brushes.DarkGreen));

        public static readonly DependencyProperty DisabledBrushProperty = DependencyProperty.Register("DisabledBrush", typeof(Brush), typeof(OnOffButton),
         new PropertyMetadata(Brushes.Gray));

        public static readonly DependencyProperty StrokeBrushProperty = DependencyProperty.Register("StrokeBrush", typeof(Brush), typeof(OnOffButton),
         new PropertyMetadata(Brushes.DarkGray));

        public static readonly DependencyProperty KnobBrushProperty = DependencyProperty.Register("KnobBrush", typeof(Brush), typeof(OnOffButton),
         new PropertyMetadata(Brushes.White));

        public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register(
            "IsActive",
            typeof(bool),
            typeof(OnOffButton),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// Brush to highlight the enabled state
        /// </summary>
        public Brush EnabledBrush
        {
            get { return (Brush)GetValue(EnabledBrushProperty); }
            set { SetValue(EnabledBrushProperty, value); }
        }
        /// <summary>
        /// Brush to highlight the disabled state
        /// </summary>
        public Brush DisabledBrush
        {
            get { return (Brush)GetValue(DisabledBrushProperty); }
            set { SetValue(DisabledBrushProperty, value); }
        }
        /// <summary>
        /// Brush to display the stroke color
        /// </summary>
        public Brush StrokeBrush
        {
            get { return (Brush)GetValue(StrokeBrushProperty); }
            set { SetValue(StrokeBrushProperty, value); }
        }
        /// <summary>
        /// Brush to display the knob color
        /// </summary>
        public Brush KnobBrush
        {
            get { return (Brush)GetValue(KnobBrushProperty); }
            set { SetValue(KnobBrushProperty, value); }
        }

        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        public OnOffButton()
        {
            InitializeComponent();

            // Make our bindings being attached to this codebehind file
            (Content as FrameworkElement).DataContext = this;
        }

        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsActive = !IsActive;
        }
    }
}
