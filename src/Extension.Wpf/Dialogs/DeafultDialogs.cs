using Extension.Wpf.MVVM;
using System;
using System.Collections.Generic;
using System.Text;

namespace Extension.Wpf.Dialogs
{
    /// <summary>
    /// Default dialog options
    /// </summary>
    public class DefaultDialogs : IDialogService
    {
        public void ExceptionPopup(string name, Exception ex)
        {
            ViewModelBase.RunInUiThread(() => 
            {
                var dialog = new ErrorPopup(name, ex);
                dialog.ShowDialog();
            });
        }
    }
}
