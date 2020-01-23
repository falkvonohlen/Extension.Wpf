using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Extension.Wpf.MVVM
{
    /// <summary>
    /// The viewmodel base provides notification utilities for
    /// properties used in the gui by implementing the INotifyPropertyChanged interface
    /// and provide a RaisePropertyChanged method that gets the property name via reflection
    /// </summary>
    public class SimpleViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Event fired, if a property changed. Gui properties listen to this
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the PropertyChanged event with the name of the property this function is called from
        /// </summary>
        /// <param name="name"></param>
        protected void RaisePropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
