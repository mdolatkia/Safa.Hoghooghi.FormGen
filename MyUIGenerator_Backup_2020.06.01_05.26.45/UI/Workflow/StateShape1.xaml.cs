using MyUILibrary.EntityArea;
using MyUILibrary.WorkflowArea;
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
    /// Interaction logic for StateShape1.xaml
    /// </summary>
    public partial class StateShape1 : UserControl, I_View_StateShape
    {
        public StateShape1()
        {
            InitializeComponent();
        }

        public string CreationDate
        {
            set
            {
                lblCreationDate.Text = value;
            }
        }

        public int StateID
        {
            set;get;
        }

        public string Title
        {
            set
            {
                lblStateName.Text = value;
            }
        }

        public event EventHandler Clicked;

        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Clicked != null)
                Clicked(this, null);
        }
    }
}
