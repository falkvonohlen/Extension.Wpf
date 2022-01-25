using Extension.Wpf.MVVM;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Text;
using System.Threading.Tasks;

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

        public RelayCommand TestRelay { get; set; }

        public MainWindowViewModel(ILogger<MainWindowViewModel> logger)
        {
            Logger = logger;
            TestRelay = new RelayCommand(Test_Clicked);
        }

        private void Test_Clicked()
        {
            UserActionAsync(async () => 
            {
                Logger.LogInformation("Started test user action");
                await Task.Delay(1500);
            },
            () => 
            {
                Logger.LogInformation("Completed user action");
            },
            true);
        }
    }
}
