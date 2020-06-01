using ModelEntites;
using MyCommonWPFControls;

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
using System.Windows.Shapes;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace MyProject_WPF
{
    /// <summary>
    /// Interaction logic for frmEntityListReport.xaml
    /// </summary>
    /// 

    public partial class frmEntitySubListReportColumns : UserControl
    {
        BizEntityRelationshipTail bizEntityRelationshipTail = new BizEntityRelationshipTail();
        EntityListReportDTO Message { set; get; }
        BizEntityListReport bizEntityListReport = new BizEntityListReport();
      
        int EntityID { set; get; }
        public frmEntitySubListReportColumns(List<EntityListReportSubsColumnsDTO> subsColumnsDTO,int parentLictViewID, int childLictViewID)
        {
            InitializeComponent();


            BizEntityListView biz = new BizEntityListView();
            var parentLictView = biz.GetEntityListView(MyProjectManager.GetMyProjectManager.GetRequester(), parentLictViewID);
            colParentListViewColumn.DisplayMemberPath = "Alias";
            colParentListViewColumn.SelectedValueMemberPath = "ID";
            colParentListViewColumn.ItemsSource = parentLictView.EntityListViewAllColumns;

            var childLictView = biz.GetEntityListView(MyProjectManager.GetMyProjectManager.GetRequester(), childLictViewID);
            colChildListViewColumn.DisplayMemberPath = "Alias";
            colChildListViewColumn.SelectedValueMemberPath = "ID";
            colChildListViewColumn.ItemsSource = childLictView.EntityListViewAllColumns;

            dtgSubReports.ItemsSource = subsColumnsDTO;
            ControlHelper.GenerateContextMenu(dtgSubReports);

        }



        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }

    
    }
}
