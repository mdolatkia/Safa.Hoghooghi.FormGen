using MyUILibrary;
using MyUILibrary.EntityArea;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUIGenerator
{
    class PromptDialogManager
    {
        UserDialogResult result;
        DialogWindow window;
        public UserDialogResult GetDialogResult(I_ViewDialog view, string title, Enum_WindowSize windowSize = Enum_WindowSize.None)
        {
             window = new DialogWindow();
            view.ButtonClicked += View_ButtonClicked;
            window.ShowDialog(view, title, windowSize, false);
            return result;
        }
        private void View_ButtonClicked(object sender, ConfirmModeClickedArg e)
        {
            result = e.Result;
            window.CloseDialog();
        }
    }
}
