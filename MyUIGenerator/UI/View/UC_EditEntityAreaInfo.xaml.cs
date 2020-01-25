
using MyUIGenerator.UIControlHelper;
using MyUILibrary;
using MyUILibrary.EntityArea;
using MyUILibrary.EntityArea.Commands;
using ProxyLibrary;
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

namespace MyUIGenerator.View
{
    /// <summary>
    /// Interaction logic for UC_ViewPackageArea.xaml
    /// </summary>
    public partial class UC_EditEntityAreaInfo : UserControl, I_View_EditEntityAreaInfo
    {

        public UC_EditEntityAreaInfo()
        {
            InitializeComponent();
            dtgData.SelectionChanged += dtgData_SelectionChanged;
            dtgDataRemoved.SelectionChanged += dtgDataRemoved_SelectionChanged;
        }

        void dtgDataRemoved_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var data = e.AddedItems[0] as EditEntityAreaDataInfo;
                if (data != null)
                {
                    dtgRelatedDataRemoved.ItemsSource = data.RelatedDataInfo;
                }
            }
        }

        void dtgData_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
           if(e.AddedItems.Count>0)
           {
               var data = e.AddedItems[0] as EditEntityAreaDataInfo;
               if(data!=null)
               {
                   dtgRelatedData.ItemsSource = data.RelatedDataInfo;
               }
           }
        }



        public void ShowInfo(EditEntityAreaInfo Info)
        {
            lblTemplateEntityName.Content = Info.TemplateEntityName;
            chkFormComposed.IsChecked = Info.FormComposed;
            lblDataCount.Content = Info.DataCount;
            lblDataMode.Content = Info.DataMode;
            lblDirectionMode.Content = Info.DirectionMode;
            lblIntracionMode.Content = Info.IntracionMode;

            lblSourceEntityName.Content = Info.SourceEntityName;
            lblSourceRalationType.Content = Info.SourceRalationType;
            lblSourceRalationName.Content = Info.SourceRalationName;
            chkrelationIsMandatory.IsChecked = Info.relationIsMandatory;

            dtgData.ItemsSource = Info.DataInfo;
            dtgSkippedRelationships.ItemsSource = Info.SkippedRelationships;
            dtgDataRemoved.ItemsSource = Info.RemovedDataInfo;
        }
    }
}
