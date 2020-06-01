using ModelEntites;

using MyFormulaFunctionStateFunctionLibrary;
using MyInterfaces;
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

namespace MyProject_WPF
{
    /// <summary>
    /// Interaction logic for frmEntityUISettings.xaml
    /// </summary>
    public partial class frmEntityUISetting : Window
    {
        EntityUISettingDTO Message { set; get; }
        BizEntityUISetting bizEntityUISetting = new BizEntityUISetting();
        int EntityID { set; get; }

        public frmEntityUISetting(int entityID)
        {
            InitializeComponent();
            EntityID = entityID;
            GetEntityUISetting(entityID);
        }

        private void GetEntityUISetting(int EntityUISettingID)
        {
            Message = bizEntityUISetting.GetEntityUISetting(EntityID);
            if (Message != null)
                ShowMessage();
        }

        private void ShowMessage()
        {
            txtColumnsCount.Text = Message.UIColumnsCount.ToString();
        }


        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            if (Message == null)
                Message = new EntityUISettingDTO();
            Message.UIColumnsCount = Convert.ToInt16(txtColumnsCount.Text);
            bizEntityUISetting.UpdateEntityUISettings(EntityID, Message);
            MessageBox.Show("اطلاعات ثبت شد");
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    


    }

}
