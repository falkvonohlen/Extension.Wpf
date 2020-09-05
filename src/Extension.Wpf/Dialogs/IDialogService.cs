using System;
using System.Collections.Generic;
using System.Text;

namespace Extension.Wpf.Dialogs
{
    /// <summary>
    /// Dialog options from Extensions.Wpf
    /// </summary>
    public interface IDialogService
    {
        /// <summary>
        /// Popup dialog to inform the user about an exception
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ex"></param>
        void ExceptionPopup(string name, Exception ex);


    }
}
