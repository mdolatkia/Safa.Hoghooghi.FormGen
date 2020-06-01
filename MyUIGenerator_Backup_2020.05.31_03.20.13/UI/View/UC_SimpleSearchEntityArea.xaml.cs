
using ModelEntites;
using MyUIGenerator.UIControlHelper;
using MyUILibrary;
using MyUILibrary.EntityArea;
using MyUILibrary.EntityArea.Commands;
using MyUILibrary.Temp;
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
    /// Interaction logic for UC_ViewPackageArea.xaml
    /// </summary>
    public partial class UC_SimpleSearchEntityArea : View_GridContainer, I_View_SimpleSearchEntityArea
    {
   

        //View_Container View_Container { set; get; }
        public UC_SimpleSearchEntityArea(short columnsCount)
            : base(columnsCount)
        {
            InitializeComponent();
            grdArea.Children.Add(ContentScrollViewer as UIElement);
            //View_Container = new View_Container(basicGridSetting);
            //if (AgentHelper.GetAppMode() != AppMode.Paper)
                FlowDirection = System.Windows.FlowDirection.RightToLeft;
            //grdArea.Children.Add(View_Container.Grid);




        }

        public void EnableDisable(bool enable)
        {
            this.IsEnabled = enable;
        }
        //public override Expander Expander
        //{
        //    get
        //    {
        //        return expander;
        //    }
        //}
        public override Grid Toolbar
        {
            get
            {
                return grdTool;
            }
        }
        public override Grid ControlArea
        {
            get
            {
                return grdArea;
            }
        }
        //public override Grid expanderHeader
        //{
        //    get
        //    {
        //        return grdExpanderHeader;
        //    }
        //}
        //private void InitializePackageArea()
        //{
        //    //foreach (var type in TemplatePackage.TypeConditions)
        //    //{
        //    //    //foreach (var property in type.NDType.Properties)
        //    //    //{
        //    //    //    var column = new DataGridViewTextBoxColumn();
        //    //    //    column.HeaderText = (property.Title == null ? property.Property.Title : property.Title);
        //    //    //    column.Tag = property;
        //    //    //    column.Name = property.Property.Name;
        //    //    //    //column.CellType=GetType(string);
        //    //    //    dataGridView1.Columns.Add(column);
        //    //    //}
        //    //}

        //}


        //public I_SearchViewEntityArea Controller
        //{
        //    set;
        //    get;
        //}



        public bool AddControlPackageToHeader(object uiControlPackage, string title, InfoColor titleColor, string tooltip = "")
        {
            return false;
            //////var labelControl = LabelHelper.GenerateLabelControl(title, tooltip, titleColor);
            ////////////uiControlPackage.RelatedUIControls.Add(new AG_RelatedConttol() { RelationType = AG_ControlRelationType.Label, RelatedUIControl = labelControl });
            //////stkHeaderControls.Children.Add(labelControl.Control as UIElement);
            ////////////stkHeaderControls.Children.Add(uiControlPackage.UIControl.Control as UIElement);
            //////return true;
        }

        //public bool AddControlPackageToHeader(object uiControlPackage, string title, InfoColor titleColor, string tooltip = "")
        //{
        //    throw new NotImplementedException();
        //}
    }
}
