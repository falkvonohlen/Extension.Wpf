using Extension.Wpf.MVVM;
using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Text;

namespace Wpf.Test
{
    public class MainWindowViewModel : ViewModelBase
    {

        /// <summary>
        /// 
        /// </summary>
        private string _filePath = default;

        /// <summary>
        /// A proxy property for the file _filePath
        /// Fires a propertychanged event, if the value changes
        /// </summary>
        public string FilePath
        {
            get => _filePath;
            set
            {
                if (value != _filePath)
                {
                    _filePath = value;
                    RaisePropertyChanged();
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private string _directoryPath = default;

        /// <summary>
        /// A proxy property for the file _directoryPath
        /// Fires a propertychanged event, if the value changes
        /// </summary>
        public string DirectoryPath
        {
            get => _directoryPath;
            set
            {
                if (value != _directoryPath)
                {
                    _directoryPath = value;
                    RaisePropertyChanged();
                }
            }
        }
    }
}
