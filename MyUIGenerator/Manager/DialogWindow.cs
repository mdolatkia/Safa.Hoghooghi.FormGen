using MyUILibrary;
using MyUILibrary.EntityArea;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Telerik.Windows.Controls;

namespace MyUIGenerator
{
    public class DialogWindow : I_DialogWindow
    {
        public event EventHandler WindowClosed;
        public DialogWindow()
        {
          
        }
        private void DialogWindow_Closed(object sender, EventArgs e)
        {
            if (WindowClosed != null)
                WindowClosed(sender, null);
        }
        private void Window_Closed(object sender, WindowClosedEventArgs e)
        {
            if (WindowClosed != null)
                WindowClosed(sender, null);
        }
        public void ShowDialog(object view, string title, Enum_WindowSize windowSize = Enum_WindowSize.None, bool hideMaximizeButton = false)
        {
            ShowWindow(view, title, windowSize, hideMaximizeButton, true);
        }
        object View { set; get; }
        private void ShowWindow(object view, string title, Enum_WindowSize windowSize , bool hideMaximizeButton , bool dialog)
        {
            View = view;
            if (view is Window)
            {
                if (dialog)
                    (view as Window).ShowDialog();
                else
                    (view as Window).Show();
                (view as Window).Closed += DialogWindow_Closed;
            }
            else
            {
                RadWindow window = new RadWindow();
                window.Closed += Window_Closed;
                //window.SizeToContent = false;
                window.HideMaximizeButton = hideMaximizeButton;
                window.Content = view;
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                //window.Width = 700;
                //window.Height = 500;
                //window.Background = Brushes.Beige;
                window.BringToFront();
                if (windowSize == Enum_WindowSize.Maximized)
                    window.WindowState = WindowState.Maximized;
                else if (windowSize == Enum_WindowSize.Big)
                {
                    window.Width = 900;
                    window.Height = 600;
                }
                window.Header = title;
                if (dialog)
                    window.ShowDialog();
                else
                    window.Show();
            }
        }
        public void CloseDialog()
        {
            if (View is Window)
            {
                (View as Window).Close();
            }
            else if (View is UIElement)
            {
                var window = (View as UIElement).ParentOfType<RadWindow>();
                if (window != null)
                    window.Close();
            }
        }
        public void ShowWindow(object view, string title, Enum_WindowSize windowSize = Enum_WindowSize.None, bool hideMaximizeButton = false)
        {
            ShowWindow(view, title, windowSize, hideMaximizeButton, false);
        }
    }
}
