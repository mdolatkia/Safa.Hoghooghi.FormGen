using Microsoft.Win32;
using ModelEntites;
using MyCommonWPFControls;
using MyFormulaFunctionStateFunctionLibrary;
using MyInterfaces;
using MyModelManager;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace MyProject_WPF
{
    /// <summary>
    /// Interaction logic for frmEntityCommands.xaml
    /// </summary>
    public partial class frmEntityDirectReport : UserControl
    {
        EntityDirectReportDTO Message { set; get; }
        BizEntityDirectReport bizEntityDirectReport = new BizEntityDirectReport();
        BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
        int EntityID { set; get; }

        public frmEntityDirectReport(int entityID, int entityDirectReportID)
        {
            InitializeComponent();
            EntityID = entityID;
            if (entityDirectReportID != 0)
            {
                GetEntityDirectReport(entityDirectReportID);
            }
            else
            {
                Message = new EntityDirectReportDTO();
                ShowMessage();
            }
        }


        private void GetEntityDirectReport(int entityViewReportID)
        {
            Message = bizEntityDirectReport.GetEntityDirectReport(MyProjectManager.GetMyProjectManager.GetRequester(), entityViewReportID);
            ShowMessage();
        }

        private void ShowMessage()
        {
            txtReportName.Text = Message.ReportTitle;
            txtURL.Text = Message.URL;
        }
        //private void AddFile_Click(object sender, RoutedEventArgs e)
        //{
        //    OpenFileDialog openFileDialog = new OpenFileDialog();
        //    openFileDialog.Filter = "Icons (*.png,*.ico)|*.png;*.ico|All files (*.*)|*.*";
        //    openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        //    //if (openFileDialog.ShowDialog() == true)
        //    //{
        //    //    txtFilePath.Text = openFileDialog.FileName;
        //    //}
        //}

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (txtURL.Text == "")
            {
                MessageBox.Show("URL اجباری می باشد");
                return;
            }
            Message.TableDrivedEntityID = EntityID;
            Message.ReportTitle = txtReportName.Text; ;
            Message.URL = txtURL.Text; ;

            Message.TableDrivedEntityID = EntityID;
            Message.ID = bizEntityDirectReport.UpdateEntityDirectReport(Message);
            MessageBox.Show("اطلاعات ثبت شد");
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            Message = new EntityDirectReportDTO();
            ShowMessage();
        }


        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            frmEntityDirectReportSelect view = new MyProject_WPF.frmEntityDirectReportSelect(EntityID);
            view.EntityDirectReportSelected += View_EntityDirectReportSelected1;
            MyProjectManager.GetMyProjectManager.ShowDialog(view, "frmEntityDirectReportSelect");
        }

        private void View_EntityDirectReportSelected1(object sender, EntityDirectReportSelectedArg e)
        {
            if (e.EntityDirectReportID != 0)
            {
                GetEntityDirectReport(e.EntityDirectReportID);
            }
        }


        //private void RemoveFile_Click(object sender, RoutedEventArgs e)
        //{
        //    txtFilePath.Text = "";
        //    grdExisting.Visibility = Visibility.Collapsed;
        //    grdAddFile.Visibility = Visibility.Visible;
        //    Message.IconContent = null;
        //}
    }

}
