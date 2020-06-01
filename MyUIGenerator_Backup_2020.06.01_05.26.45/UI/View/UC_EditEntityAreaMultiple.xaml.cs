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
    public partial class UC_EditEntityAreaMultiple : View_MultipleDataContainer, I_View_EditEntityAreaMultiple
    {
     
        //public View_Container Container { set; get; }
      
        public UC_EditEntityAreaMultiple()
        {
            InitializeComponent();


            grdArea.Children.Add(dataGridHelper.dataGrid);
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
        //GridHelper gridHelper;
        //public void PrepareView(EditEntityAreaInitializer EditTemplate)
        //{

        //}











        // public event EventHandler<Arg_DataDependentControlGeneration> DataControlGenerated;











    }


}
