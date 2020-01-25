using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUILibrary
{
    public interface I_Login
    {
        event EventHandler<LoginRequestedArg> LoginRequested;
        void ShowMessage(string message);
        void ShowForm();
        void Close();
    }
    public class LoginRequestedArg : EventArgs
    {
        public string UserName { set; get; }
        public string Password { set; get; }
    }
}
