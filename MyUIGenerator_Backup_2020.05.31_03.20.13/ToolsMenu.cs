using MyUILibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MyUIGenerator
{
    public class ToolsMenu : I_MainFormMenu
    {
       public Button button;
        public event EventHandler Clicked;
        public ToolsMenu(string title, string image)
        {
            button = new Button();
            button.Click += Button_Click;
            Image img = new Image();
            Uri uriSource = null;
            uriSource = new Uri(image, UriKind.Relative);
            img.Source = new BitmapImage(uriSource);
            button.Content = img;
            ToolTipService.SetToolTip(button, title);

        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (Clicked != null)
                Clicked(this, null);
        }
    }
}
