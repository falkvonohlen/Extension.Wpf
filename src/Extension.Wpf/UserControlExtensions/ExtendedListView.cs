using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Extension.Wpf.UserControlExtensions
{
    public class ExtendedListView : ListView
    {
        /// <summary>
        /// The dependency property for autoscroll behaviore
        /// </summary>
        public static readonly DependencyProperty AutoScrollProperty =
            DependencyProperty.RegisterAttached("AutoScroll", typeof(bool), typeof(ExtendedListView), new PropertyMetadata(false, AutoScrollPropertyChanged));

        /// <summary>
        /// The actual property
        /// </summary>
        public bool AutoScroll
        {
            get { return (bool)GetValue(AutoScrollProperty); }
            set { SetValue(AutoScrollProperty, value); }
        }

        /// <summary>
        /// Attach the event handler to the collection changed event
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="args"></param>
        private static void AutoScrollPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            ExtendedListView listBox = obj as ExtendedListView;

            if (listBox != null && (bool)args.NewValue)
            {
                ((INotifyCollectionChanged)listBox.Items).CollectionChanged += listBox.CollectionChangedEventHandler;
            }
            else
            {
                ((INotifyCollectionChanged)listBox.Items).CollectionChanged -= listBox.CollectionChangedEventHandler;
            }
        }

        /// <summary>
        /// Event handler to scroll the newest item into the view, if the collection changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void CollectionChangedEventHandler(object sender, NotifyCollectionChangedEventArgs args)
        {
            var coll = sender as ItemCollection;
            if (coll.Count > 0)
            {
                var lastItemAdded = coll[coll.Count - 1];
                ScrollIntoView(lastItemAdded);
            }
        }
    }
}
