using CommonDefinitions.UISettings;


using MyUIGenerator.UIControlHelper;
using MyUIGenerator;
using MyUILibrary;
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
using Telerik.Windows.Controls;

using MyUILibrary.EntityArea;
using MyUILibrary.EntityArea.Commands;
using ProxyLibrary;

using ModelEntites;
using MyUILibrary.Temp;

//using MyUIGenerator.UIContainerHelper;


namespace MyUIGenerator.View
{
    /// <summary>
    /// Interaction logic for UC_EditPackageArea.xaml
    /// </summary>
    public partial class UC_EditEntityArea : View_GridContainer, I_View_EditEntityAreaDataView
    {

        public void SetTooltip(object dataItem, string tooltip)
        {
            if (!string.IsNullOrEmpty(tooltip))
                ToolTipService.SetToolTip(this, tooltip);
            else
                ToolTipService.SetToolTip(this, null);
        }

        public void SetBorderColor(object dataItem, InfoColor color)
        {
            this.BorderBrush = UIManager.GetColorFromInfoColor(color);
            this.BorderThickness = new Thickness(1);
        }

        public void SetBackgroundColor(object dataItem, InfoColor color)
        {
            this.Background = UIManager.GetColorFromInfoColor(color);
        }

        public void SetForegroundColor(object dataItem, InfoColor color)
        {
            this.Foreground = UIManager.GetColorFromInfoColor(color);
        }

        //public View_Container Container { set; get; }

        public UC_EditEntityArea(short columnsCount)
            : base(columnsCount)
        {
            InitializeComponent();

            //if (AgentHelper.GetAppMode() != AppMode.Paper)
            FlowDirection = System.Windows.FlowDirection.RightToLeft;
            grdArea.Children.Add(ContentScrollViewer as UIElement);
        }
        public override Grid ControlArea
        {
            get
            {
                return grdArea;
            }
        }
        //public override Expander Expander
        //{
        //    get
        //    {
        //        return null;
        //    }
        //}
        public override Grid Toolbar
        {
            get
            {
                return grdTool;
            }
        }
        //public override Grid expanderHeader
        //{
        //    get
        //    {
        //        return null;
        //    }
        //}
        //public I_EditEntityArea Controller
        //{
        //    get;
        //    set;
        //}




        //public void GenerateEditTemplate(EditTemplate editTemplate)
        //{
        //    throw new NotImplementedException();
        //}
        //DataGridHelper dataGridHelper;
        //GridHelper gridHelper;
        //public void PrepareView(EditEntityAreaInitializer EditTemplate)
        //{

        //}
















    }


}
