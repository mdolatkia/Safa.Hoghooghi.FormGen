
using MyModelManager;

using ProxyLibrary;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Telerik.Windows.Controls;

namespace MyProject_WPF
{
    public class MyProjectManager
    {
        static MyProjectManager _GetMyProjectManager;
        public static MyProjectManager GetMyProjectManager
        {
            get
            {
                if (_GetMyProjectManager == null)
                    _GetMyProjectManager = new MyProject_WPF.MyProjectManager();
                return _GetMyProjectManager;
            }
        }
        MainWindow MainWindow { set; get; }
        public void StartApp()
        {
            new BizUser().CheckAdminUserExists();

            var frm = new frmLogin();
            frm.LoginRequested += Frm_LoginRequested;
            //frmFormula frm = new frmFormula(0,67);

            //MainWindow = new MainWindow();
            //MainWindow.Show();
            MyProjectManager.GetMyProjectManager.ShowDialog(frm, "Form");
        }
        SecurityHelper SecurityHelper = new SecurityHelper();
        public UserInfoDTO UserInfo { get; private set; }
        private void Frm_LoginRequested(object sender, LoginRequestedArg e)
        {
            var result = SecurityHelper.Login(e.UserName, e.Password);
            if (result.Successful)
            {

                UserInfo = ConveSecurityUserInfo(result.UserID);
                if (UserInfo.OrganizationPosts.Any(x => x.IsSuperAdmin || x.IsAdmin))
                {
                    MainWindow frmMain = new MyProject_WPF.MainWindow();
                    frmMain.Show();
                    MyProjectManager.GetMyProjectManager.CloseDialog((sender as frmLogin));
                }
                else
                {
                    (sender as frmLogin).ShowMessage("کاربر گرامی، شما دسترسی لازم جهت ورود به این برنامه را ندارید");
                }
            }
            else
                (sender as frmLogin).ShowMessage("نام و یا کلمه عبور صحیح نمی باشد");

        }

        private UserInfoDTO ConveSecurityUserInfo(int userID)
        {
            var user = new BizUser().GetUser(userID);
            UserInfoDTO result = new UserInfoDTO();
            result.ID = user.ID;
            result.FirstName = user.FirstName;
            result.LastName = user.LastName;
            result.UserName = user.UserName;
            result.OrganizationPosts = new BizOrganization().GetOrganizationPostsByUserID(userID);
            return result;
        }
        System.Windows.Controls.TextBlock InfoTextBlock;
        internal void ShowInfo(string text, string detail, Color color)
        {
            if (MainWindow.pnlInfo.Content == null)
            {
                ScrollViewer scroll = new ScrollViewer();
                InfoTextBlock = new System.Windows.Controls.TextBlock();
                scroll.Content = InfoTextBlock;
                MainWindow.pnlInfo.Content = scroll;
            }
            MainWindow.pnlInfo.IsActive = true;
            //      grpInfo.ShowAllPanes();
            var runTitle = new Run(text + Environment.NewLine) { Foreground = new SolidColorBrush(color) };


            if (InfoTextBlock.Inlines.Any())
                InfoTextBlock.Inlines.InsertBefore(InfoTextBlock.Inlines.First(), runTitle);
            else
                InfoTextBlock.Inlines.Add(runTitle);

            if (!string.IsNullOrEmpty(detail))
            {
                var runDetail = new Run("   " + detail + Environment.NewLine);
                InfoTextBlock.Inlines.InsertAfter(InfoTextBlock.Inlines.First(), runDetail);
            }
        }
        public void CloseDialog(object view)
        {
            if (view is Window)
            {
                (view as Window).Close();
            }
            else if (view is UIElement)
            {
                var window = (view as UIElement).ParentOfType<RadWindow>();
                if (window != null)
                    window.Close();
            }
        }

        internal void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        public RadWindow ShowDialog(object view, string title, Enum_WindowSize windowSize = Enum_WindowSize.None)
        {
            if (view is Window)
            {
                (view as Window).ShowDialog();
            }
            else
            {
                RadWindow window = new RadWindow();
                window.Content = view;
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                //window.Width = 700;
                //window.Height = 500;
                window.Background = Brushes.Beige;
                window.BringToFront();
                if (windowSize == Enum_WindowSize.Maximized)
                    window.WindowState = WindowState.Maximized;
                else if (windowSize == Enum_WindowSize.Big)
                {
                    window.Width = 900;
                    window.Height = 600;
                }
                window.Header = title;
                 window.ShowDialog();
                return window;
            }
            return null;
        }


        public DR_Requester GetRequester()
        {
            DR_Requester requester = new DR_Requester(2);
            requester.SkipSecurity = true;
            //DR_Requester requester = new DR_Requester(2);
            //requester.Identity = 2;// MyProjectManager.GetMyProjectManager.UserInfo.ID;
            return requester;
        }

    }
    public enum Enum_WindowSize
    {
        None,
        Big,
        Maximized
    }
}
