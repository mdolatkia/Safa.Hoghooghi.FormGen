using ModelEntites;
using MyModelManager;
using MyProject_WPF.Biz;
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

namespace MyProject_WPF
{
    /// <summary>
    /// Interaction logic for frmUnionRelationshipCreateSelect.xaml
    /// </summary>
    public partial class frmUnionRelationshipCreateSelect: UserControl
    {
        public event EventHandler<UnionRelationshipSelectedArg> UnionRelationshipSelected;
        public frmUnionRelationshipCreateSelect(List<UnionRelationshipDTO> list,bool unionHoldsKeys, string defaultName = "")
        {
            InitializeComponent();
            txtName.Text = defaultName;
            chkUnionHoldsKeys.IsChecked = unionHoldsKeys ;
            if (list.Count == 0)
            {
                GotoCreateMode();
            }
            else
            {
                dtgList.ItemsSource = list;
            }
        }
        private void GotoCreateMode()
        {
            dtgList.Visibility = System.Windows.Visibility.Collapsed;
            btnCreateMode.Visibility = System.Windows.Visibility.Collapsed;
            pnlEdit.Visibility = System.Windows.Visibility.Visible;
            btnSave.Visibility = System.Windows.Visibility.Visible;
            btnChoose.Visibility = System.Windows.Visibility.Collapsed;
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }

        private void btnCreateMode_Click(object sender, RoutedEventArgs e)
        {
            GotoCreateMode();
        }

        private void btnChoose_Click(object sender, RoutedEventArgs e)
        {
            if (dtgList.SelectedItem != null)
            {

                var UnionRelationship = dtgList.SelectedItem as UnionRelationshipDTO;
                if (UnionRelationshipSelected != null)
                    UnionRelationshipSelected(this, new UnionRelationshipSelectedArg() { UnionRelationship = UnionRelationship });
                MyProjectManager.GetMyProjectManager.CloseDialog(this);
                //txtName.Text = UnionRelationship.Name;
                //if (UnionRelationship.IsGeneralization == true)
                //    optIsGeneralization.Checked = true;
                //else if (UnionRelationship.IsSpecialization == true)
                //    optIsSpecialization.Checked = true;
                //if (UnionRelationship.IsTolatParticipation == true)
                //    optIsTolatParticipation.Checked = true;
                //else if (UnionRelationship.IsPartialParticipation == true)
                //    optIsPartialParticipation.Checked = true;
                //if (UnionRelationship.IsDisjoint == true)
                //    optIsDisjoint.Checked = true;
                //else if (UnionRelationship.IsOverlap == true)
                //    optIsOverlap.Checked = true;


            }
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (txtName.Text == "")
            {
                MessageBox.Show("نام رابطه مشخص نشده است");
                return;
            }
         
            if (optIsTolatParticipation.IsChecked == false && optIsPartialParticipation.IsChecked == false)
            {
                MessageBox.Show("نوع " + "TolatParticipation/PartialParticipation" + "مشخص نشده است");
                return;
            }
            BizUnionRelationship biz = new BizUnionRelationship();
            var item = new UnionRelationshipDTO();
            item.Name = txtName.Text;
            item.UnionHoldsKeys = chkUnionHoldsKeys.IsChecked == true;
            item.IsTolatParticipation = optIsTolatParticipation.IsChecked == true;
            item.ID = biz.Save(item);
            if (UnionRelationshipSelected != null)
                UnionRelationshipSelected(this, new UnionRelationshipSelectedArg() { UnionRelationship = item });
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }

    }
    public class UnionRelationshipSelectedArg : EventArgs
    {
        public UnionRelationshipDTO UnionRelationship { set; get; }
    }
}
