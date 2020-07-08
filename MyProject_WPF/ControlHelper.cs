using MyCommonWPFControls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Telerik.Windows.Controls;


namespace MyProject_WPF
{
    public static class ControlHelper
    {
        public static GridViewBoundColumnBase GenerateGridviewColumn(string fieldName, string header)
        {
            return GenerateGridviewColumn(fieldName, header, false, null, GridViewColumnType.Text);
        }
        public static GridViewBoundColumnBase GenerateGridviewColumn(string fieldName, string header, bool readOnly, int? width, GridViewColumnType columnType, IEnumerable itemsSource = null)
        {
            var columnw = new GridViewHyperlinkColumn();

            GridViewBoundColumnBase column = null;
            if (columnType == GridViewColumnType.Text)
            {
                column = new GridViewDataColumn();
            }
            else if (columnType == GridViewColumnType.Numeric)
            {
                column = new GridViewDataColumn();
                //column = new GridViewMaskedInputColumn();
                //(column as GridViewMaskedInputColumn).MaskType = MaskType.Numeric;
            }
            else if (columnType == GridViewColumnType.CheckBox)
            {
                column = new GridViewCheckBoxColumn();
                (column as GridViewCheckBoxColumn).IsThreeState = true;
            }
            else if (columnType == GridViewColumnType.Command)
            {
                // column = new GridViewCommandColumn();
            }
            else if (columnType == GridViewColumnType.Link)
            {
                column = new GridViewHyperlinkColumn();
            }
            else if (columnType == GridViewColumnType.Color)
            {
                column = new MyColorPickerColumn();
            }
            else if (columnType == GridViewColumnType.Enum)
            {
                column = new GridViewComboBoxColumn();
                (column as GridViewComboBoxColumn).ItemsSource = itemsSource;
            }
            else if (columnType == GridViewColumnType.ComboBox)
            {
                column = new GridViewComboBoxColumn();
            
                (column as GridViewComboBoxColumn).ItemsSource = itemsSource;
            }
            //column.Name = fieldName;
            //column.TextAlignment = System.Windows.TextAlignment.Center;
            column.UniqueName = fieldName;
            column.DataMemberBinding = new System.Windows.Data.Binding(fieldName);
            column.Header = header;
            column.IsReadOnly = readOnly;
            if (width != null)
                column.Width = width.Value;
            return column;
        }

        internal static SelectorGrid SetSelectorGrid(RadGridView dtgItems, Dictionary<string, string> dictionary)
        {
            dtgItems.AutoGenerateColumns = false;
            dtgItems.IsReadOnly = true;
            dtgItems.ShowGroupPanel = false;
            foreach (var item in dictionary)
            {
                dtgItems.Columns.Add(GenerateGridviewColumn(item.Key, item.Value));
            }
            SelectorGrid selectorGrid = new MyProject_WPF.SelectorGrid(dtgItems);
            return selectorGrid;
        }

        public static RadContextMenu GenerateContextMenu(RadGridView gridView)
        {
            var contextMenu = new RadContextMenu();
            RadMenuItem insertMenuItem = new RadMenuItem();
            insertMenuItem.Header = "افزودن";
            insertMenuItem.Click += (sender, e) => InsertMenuItem_Click(sender, e, gridView);
            contextMenu.Items.Add(insertMenuItem);
            RadContextMenu.SetContextMenu(gridView, contextMenu);
            return contextMenu;
        }

        private static void InsertMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e, RadGridView gridView)
        {
            gridView.BeginInsert();
        }

        public static TextBlock GetTooltip(string tooltip, bool rtlDirection)
        {
            if (!string.IsNullOrEmpty(tooltip))
            {
                System.Windows.Controls.TextBlock text = new System.Windows.Controls.TextBlock();
                if (rtlDirection)
                    text.FlowDirection = System.Windows.FlowDirection.RightToLeft;
                else
                    text.FlowDirection = System.Windows.FlowDirection.LeftToRight;
                text.Text = tooltip;
                return text;
            }
            else return null;
        }


        internal static void AddComboColumnItemsSource(RadGridView dtgSuperToSub, string columnName, IEnumerable enumerable, string displayMember , string valueMember )
        {
            if (dtgSuperToSub.Columns[columnName] != null)
            {
                var column = dtgSuperToSub.Columns[columnName] as GridViewComboBoxColumn;
                if (column != null)
                {
                    column.DisplayMemberPath = displayMember;
                    column.SelectedValueMemberPath = valueMember;
                    column.ItemsSource = enumerable;
                }
            }
        }
    }

    public class SelectorGrid
    {
        RadGridView GridView;
        public SelectorGrid(RadGridView gridView)
        {
            GridView = gridView;
            GridView.MouseDoubleClick += GridView_MouseDoubleClick;
        }

        private void GridView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (GridView.SelectedItem != null)
            {
                if (DataItemSelected != null)
                    DataItemSelected(this, GridView.SelectedItem);
            }
        }
        public object GetSelectedItem
        {
            get
            {
                return GridView.SelectedItem;
            }
        }
        public event EventHandler<object> DataItemSelected;
    }
}
