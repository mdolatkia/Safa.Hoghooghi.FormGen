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
    /// Interaction logic for UC_ViewFilePanel.xaml
    /// </summary>
    public partial class UC_ViewFilePanel : UserControl
    {
        public bool AllowSave
        {
            get
            {
                return imgSave.IsEnabled;
            }

            set
            {
                imgSave.IsEnabled = value;
            }
        }

        public event EventHandler SaveFile;
        public event EventHandler DownloadFile;
        public event EventHandler NextFile;
        public event EventHandler PreviousFile;
        public UC_ViewFilePanel()
        {
            InitializeComponent();
        }

        private void imgNext_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (NextFile != null)
                NextFile(this, null);
        }

        private void imgPrevious_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (PreviousFile != null)
                PreviousFile(this, null);
        }

        private void imgSave_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (SaveFile != null)
                SaveFile(this, null);
        }

        private void imgDownload_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (DownloadFile != null)
                DownloadFile(this, null);
        }
    }
}
