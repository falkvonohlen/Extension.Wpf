using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Extension.Wpf.Utils
{
    /// <summary>
    /// A proxy to set a binding on another a predefined datacontext
    /// </summary>
    public class BindingProxy : Freezable
    {
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(object),
            typeof(BindingProxy), new UIPropertyMetadata(null));

        /// <summary>
        /// The datacontext binding
        /// </summary>
        public object Data
        {
            get { return (object)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        /// <summary>
        /// Creates an instance of the binding proxy
        /// </summary>
        /// <returns></returns>
        protected override Freezable CreateInstanceCore()
        {
            return new BindingProxy();
        }
    }
}
