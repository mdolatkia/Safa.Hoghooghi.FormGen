
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MyProject_WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            //frmTestLookup frm = new frmTestLookup();
            ////frm.ShowDialog();
         //   MyProjectManager.GetMyProjectManager.ShowDialog(frm,"");
         MyProjectManager.GetMyProjectManager.StartApp();


        }

       
    }
}
