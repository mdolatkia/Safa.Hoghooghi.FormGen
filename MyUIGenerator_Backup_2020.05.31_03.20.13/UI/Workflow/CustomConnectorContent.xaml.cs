using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyUIGenerator.View
{
    /// <summary>
    /// Interaction logic for CustomConnectorContent.xaml
    /// </summary>
    public partial class CustomConnectorContent : UserControl
    {
        public CustomConnectorContent()
        {
            InitializeComponent();
        }

        public bool Highlight
        {
            set
            {
                if (value)
                {
                    imgInfo.Source = GetImageSource(@"..\..\images\confirm.png");
                }
                else
                    imgInfo.Source = GetImageSource(@"..\..\images\info.png");
            }
        }

        private ImageSource GetImageSource(string path)
        {
            return new BitmapImage(new Uri(path, UriKind.Relative));
        }
    }
}
