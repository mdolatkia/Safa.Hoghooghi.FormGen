using ModelEntites;
using MyModelGenerator;
using MyModelManager;
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

namespace MyProject_WPF
{
    /// <summary>
    /// Interaction logic for frmImportFunctions.xaml
    /// </summary>
    public partial class frmImportViewsAndFunctions : UserControl, ImportWizardForm
    {
        public bool HasData()
        {
            return true;
        }
        BizDatabaseFunction bizDatabaseFunction = new BizDatabaseFunction();

        public event EventHandler<ItemImportingStartedArg> InfoUpdated;
        public event EventHandler FormIsBusy;
        public event EventHandler FormIsFree;

        DatabaseDTO Database { set; get; }
        public frmImportViewsAndFunctions(DatabaseDTO database)
        {
            InitializeComponent();

            var frmFunctions = new frmImportFunctions(database);
            grdMain.Children.Add(frmFunctions);
            Grid.SetColumn(frmFunctions, 2);

            var frmViews = new frmImportViews(database);
            grdMain.Children.Add(frmViews);

            frmFunctions.InfoUpdated += Frm_InfoUpdated;
            frmFunctions.FormIsBusy += Frm_FormIsBusy;
            frmFunctions.FormIsFree += Frm_FormIsFree;
            frmViews.InfoUpdated += Frm_InfoUpdated;
            frmViews.FormIsBusy += Frm_FormIsBusy;
            frmViews.FormIsFree += Frm_FormIsFree;
        }

        private void Frm_FormIsFree(object sender, EventArgs e)
        {
            FormIsFree(this, e);
        }

        private void Frm_FormIsBusy(object sender, EventArgs e)
        {
            FormIsBusy(this, e);
        }

        private void Frm_InfoUpdated(object sender, ItemImportingStartedArg e)
        {
            if (InfoUpdated != null)
                InfoUpdated(this, e);
        }

    }
}
