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
    /// Interaction logic for frmISARelationshipCreateSelect.xaml
    /// </summary>
    public partial class frmISARelationshipCreateSelect: UserControl
    {
        public event EventHandler<ISARelationshipSelectedArg> ISARelationshipSelected;
        public frmISARelationshipCreateSelect(List<ISARelationshipDTO> list, string defaultName = "")
        {
            InitializeComponent();
            txtName.Text = defaultName;
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

                var isaRelationship = dtgList.SelectedItem as ISARelationshipDTO;
                if (ISARelationshipSelected != null)
                    ISARelationshipSelected(this, new ISARelationshipSelectedArg() { ISARelationship = isaRelationship });
                MyProjectManager.GetMyProjectManager.CloseDialog(this);
                //txtName.Text = isaRelationship.Name;
                //if (isaRelationship.IsGeneralization == true)
                //    optIsGeneralization.Checked = true;
                //else if (isaRelationship.IsSpecialization == true)
                //    optIsSpecialization.Checked = true;
                //if (isaRelationship.IsTolatParticipation == true)
                //    optIsTolatParticipation.Checked = true;
                //else if (isaRelationship.IsPartialParticipation == true)
                //    optIsPartialParticipation.Checked = true;
                //if (isaRelationship.IsDisjoint == true)
                //    optIsDisjoint.Checked = true;
                //else if (isaRelationship.IsOverlap == true)
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
            if (optIsDisjoint.IsChecked == false && optIsOverlap.IsChecked == false)
            {
                MessageBox.Show("نوع " + "Disjoint/IsOverlap" + "مشخص نشده است");
                return;
            }
            if (optIsTolatParticipation.IsChecked == false && optIsPartialParticipation.IsChecked == false)
            {
                MessageBox.Show("نوع " + "TolatParticipation/PartialParticipation" + "مشخص نشده است");
                return;
            }
            BizISARelationship biz = new BizISARelationship();
            var item = new ISARelationshipDTO();
            item.Name = txtName.Text;
            item.IsDisjoint = optIsDisjoint.IsChecked == true;
            item.IsTolatParticipation = optIsTolatParticipation.IsChecked == true;
            item.IsGeneralization = optIsGeneralization.IsChecked == true;
            item.ID = biz.Save(item);
            if (ISARelationshipSelected != null)
                ISARelationshipSelected(this, new ISARelationshipSelectedArg() { ISARelationship = item });
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }

    }
    public class ISARelationshipSelectedArg : EventArgs
    {
        public ISARelationshipDTO ISARelationship { set; get; }
    }
}
