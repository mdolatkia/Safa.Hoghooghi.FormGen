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
    public partial class frmTableFinalSettings : UserControl, ImportWizardForm
    {
        BizDatabaseFunction bizDatabaseFunction = new BizDatabaseFunction();

        public event EventHandler<ItemImportingStartedArg> InfoUpdated;

        DatabaseDTO Database { set; get; }
        public frmTableFinalSettings(DatabaseDTO database)
        {
            InitializeComponent();

            var frmTableUIComposition = new frmTableUIComposition(database);
            grdMain.Children.Add(frmTableUIComposition);
            Grid.SetColumn(frmTableUIComposition, 1);

            var frmViews = new frmTableDefaultListView(database);
            grdMain.Children.Add(frmViews);

            frmTableUIComposition.InfoUpdated += FrmViews_InfoUpdated;
            frmViews.InfoUpdated += FrmViews_InfoUpdated;
        }

        private void FrmViews_InfoUpdated(object sender, ItemImportingStartedArg e)
        {
            if (InfoUpdated != null)
                InfoUpdated(this, e);
        }
    }
}
