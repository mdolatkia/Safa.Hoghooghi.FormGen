using ModelEntites;
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
    /// Interaction logic for frmColumnValueRange1.xaml
    /// </summary>
    public partial class frmColumnValueRange : UserControl
    {
        BizColumnValueRange bizColumnValueRange = new BizColumnValueRange();
        ColumnValueRangeDTO Message;
        int ColumnID { set; get; }
        //int EntityID { set; get; }
        public event EventHandler<SavedItemArg> ItemSaved;

        public frmColumnValueRange(int columnID)
        {
            InitializeComponent();
            ColumnID = columnID;
            ControlHelper.GenerateContextMenu(dtgColumnKeyValue);
            SetGridViewColumns();

            GetColumnValueRange(columnID);
            this.FlowDirection = FlowDirection.RightToLeft;
        }

        private void GetColumnValueRange(int id)
        {
            Message = bizColumnValueRange.GetColumnValueRange(id);
            if (Message == null)
            {
                Message = new ColumnValueRangeDTO();
                Message.ID = ColumnID;
            }
            ShowMessage();
        }

        private void SetGridViewColumns()
        {
            dtgColumnKeyValue.Columns.Add(ControlHelper.GenerateGridviewColumn("Value", "مقدار", false, null, GridViewColumnType.Numeric));
            dtgColumnKeyValue.Columns.Add(ControlHelper.GenerateGridviewColumn("KeyTitle", "عنوان/کلید", false, null, GridViewColumnType.Text));
            dtgColumnKeyValue.Columns.Add(ControlHelper.GenerateGridviewColumn("Tag1", "مشخصه یک", false, null, GridViewColumnType.Text));
            dtgColumnKeyValue.Columns.Add(ControlHelper.GenerateGridviewColumn("Tag2", "مشخصه دو", false, null, GridViewColumnType.Text));
        }
        private void btnUpdateKeyValue_Click(object sender, RoutedEventArgs e)
        {
            //Message.ValueFromTitleOrValue = optValueComesFromTitle.IsChecked == true;
            var id = bizColumnValueRange.UpdateColumnValueRange(Message);
            if (ItemSaved != null)
                ItemSaved(this, new MyProject_WPF.SavedItemArg() { ID = Message.ID });

            MessageBox.Show("اطلاعات ثبت شد");
        }
        private void ShowMessage()
        {
           // optValueComesFromTitle.IsChecked = Message.ValueFromTitleOrValue;
          //  optValueComesFromValue.IsChecked = !Message.ValueFromTitleOrValue;
            dtgColumnKeyValue.ItemsSource = Message.Details;
        }
        private void btnImportKeyValues_Click(object sender, RoutedEventArgs e)
        {
            //if (optValueComesFromTitle.IsChecked == false && optValueComesFromValue.IsChecked == false)
            //{
            //    MessageBox.Show("لطفا نوع جایگذاری مقادیر ستون در عنوان یا مقدار را مشخص نمایید");
            //    return;
            //}
            BizDatabase bizDatabase = new BizDatabase();
            BizColumn bizColumn = new BizColumn();
            var column = bizColumn.GetColumn(ColumnID, true);
            var database = bizDatabase.GetDatabaseByTableID(column.TableID);
            var dbHelper = MyModelGenerator.ModelGenerator.GetDatabaseImportHelper(database);
            var result = dbHelper.GetColumnValueRange(ColumnID);
            if (result != null)
            {
                Message.Details = result.Details;
                dtgColumnKeyValue.ItemsSource = null;
                dtgColumnKeyValue.ItemsSource = Message.Details;
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }

        private void btnRemoveKeyValue_Click(object sender, RoutedEventArgs e)
        {
            bizColumnValueRange.RemoveColumnValueRangeID(ColumnID);
        }
    }
}
