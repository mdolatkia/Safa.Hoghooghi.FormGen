
using CommonDefinitions.BasicUISettings;
using MyUIGenerator;
using MyUIGenerator;
using MyUILibrary;
using MyUILibrary.PackageArea;
using MyUILibrary.PackageArea.Commands;
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
using System.Windows.Shapes;

namespace MyUIGenerator.View
{
    /// <summary>
    /// Interaction logic for frmEditPackageArea.xaml
    /// </summary>
    public partial class frmEditPackageArea : UserControl, I_View_EditPackageArea
    {
        public frmEditPackageArea()
        {
            InitializeComponent();
            //UISetting = packageAreaUISetting;
        }




        //UC_EditNDTypeArea packageArea;
        //public IAG_View_EditNDTypeArea GenerateViewOfEditNDTypeArea()
        //{

        //}
        //    packageArea.IsEnabled = false;
        //packageArea.Width = UISetting.DefaultWidth;
        //packageArea.Height = UISetting.DefaultHeigh;
        public void ShowEditAreaView(object view)
        {
            (view as FrameworkElement).Margin = new Thickness(5);
            //grpEditor.Content = view;
            grdMain.Children.Add(view as UIElement);
        }

        //public void ShowDataPckages(List<DP_Package> dataPackages)
        //{
        //    if (dataPackages.Count > 0)
        //    {
        //        if (packageArea.IsEnabled == false)
        //            packageArea.IsEnabled = true; ;
        //        packageArea.ShowValues(dataPackages.Last());
        //    }
        //    //ایونت برای لود شدن و کارهای مرتبط ظاهری
        //}

        //public void UpdateDataPckages(List<DP_Package> dataPackages)
        //{
        //    foreach (var item in dataPackages)
        //    {
        //        if (packageArea.DataPackage == item)
        //            packageArea.UpdateValues();
        //        else
        //            throw (new Exception(""));
        //    }
        //}


        //public void AddCommands(List<I_PackageAreaCommand> Commands)
        //{
        //    foreach (var item in Commands.OrderBy(x => x.Position))
        //    {
        //        Button btnCommand = UIHelper.GenerateCommand(item);
        //        btnCommand.Click += btnCommand_Click;
        //        item.EnabledChanged += (sender, e) => item_EnabledChanged(sender, e, btnCommand);
        //        toolbar.Items.Add(btnCommand);
        //    }
        //}

        //void item_EnabledChanged(object sender, EventArgs e, Button btnCommand)
        //{
        //    btnCommand.IsEnabled = (sender as I_Command).Enabled;
        //}

        //void btnCommand_Click(object sender, EventArgs e)
        //{
        //    Controller.CommandExecuted(((sender as Button).Tag as I_PackageAreaCommand));
        //}


        //public List<DP_Package> CurrentShownDataPackages
        //{
        //    get
        //    {
        //        List<DP_Package> list = new List<DP_Package>();
        //        if (packageArea.DataPackage != null)
        //            list.Add(packageArea.DataPackage);
        //        return list;
        //    }
        //}


        //public CommonDefinitions.BasicUISettings.PackageAreaUISetting UISetting
        //{
        //    set;
        //    get;
        //}



        public I_EditPackageArea Controller
        {
            set;
            get;
        }
    }
}
