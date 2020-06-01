using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Windows.Controls;


namespace Paper_MetadataManagement
{
    public enum GridViewColumnType
    {
        Text,
        Numeric,
        CheckBox,
        Command,
        Link
    }
    public static class ControlHelper
    {
        public static GridViewBoundColumnBase GenerateGridviewColumn(string fieldName, string header, bool readOnly, int width, GridViewColumnType columnType)
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
            //column.Name = fieldName;
            //column.TextAlignment = System.Windows.TextAlignment.Center;

            column.DataMemberBinding = new System.Windows.Data.Binding(fieldName);
            column.Header = header;
            column.IsReadOnly = readOnly;
            column.Width = width;
            return column;
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
    }

}
