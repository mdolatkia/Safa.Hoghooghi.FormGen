using MyUILibraryInterfaces.DataViewArea;
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
using ProxyLibrary;
using Telerik.Windows.Controls;
using System.Collections.ObjectModel;
using MyUILibraryInterfaces.DataMenuArea;

namespace MyUIGenerator.View
{
    /// <summary>
    /// Interaction logic for UC_DataViewItem.xaml
    /// </summary>
    public partial class UC_DataViewItem : UserControl, I_DataViewItem
    {
        public UC_DataViewItem()
        {
            InitializeComponent();
        }

        public DP_DataView DataView
        {
            set; get;
        }

        public string Title
        {
            get
            {
                return lblTitle.Text;
            }

            set
            {
                lblTitle.Text = value;
            }
        }
        public string Body
        {
            get
            {
                return lblBody.Text;
            }

            set
            {
                lblBody.Text = value;
            }
        }
        public event EventHandler InfoClicked;
        public event EventHandler Selected;

        //public void AddTitleRow(string title, string value)
        //{
        //    grdTitle.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
        //    //TextBlock lblTitle = new TextBlock();
        //    //lblTitle.Text = title;
        //    TextBlock lblValue = new TextBlock();
        //    lblValue.FontSize = 12;
        //    lblValue.FontWeight = FontWeights.Bold;
        //    lblValue.Text = value;
        //    //grdTitle.Children.Add(lblTitle);
        //    //Grid.SetRow(lblTitle, grdTitle.RowDefinitions.Count - 1);
        //    grdTitle.Children.Add(lblValue);
        //    Grid.SetRow(lblValue, grdTitle.RowDefinitions.Count - 1);
        //    //Grid.SetColumn(lblValue, 1);
        //}

        private void imgSetting_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (InfoClicked != null)
                InfoClicked(this, null);
        }
       

        public void OnSelected()
        {
            if (Selected != null)
                Selected(this, null);
        }

        //internal void SetMenu(RadRadialMenu menu)
        //{
        //    //menu.HorizontalAlignment = HorizontalAlignment.Right;
        //    //menu.VerticalAlignment = VerticalAlignment.Top;
        //  grdMenu.Children.Clear();
        //    grdMenu.Children.Add(menu);
        //}
    }
}
