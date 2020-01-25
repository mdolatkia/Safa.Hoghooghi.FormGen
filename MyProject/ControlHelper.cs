using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.WinControls.UI;

namespace MyProject
{
  public static  class ControlHelper
    {
      public static GridViewDataColumn GenerateGridviewColumn(string fieldName, string header, bool readOnly, int width, GridViewColumnType columnType)
        {
            var columnw = new GridViewHyperlinkColumn();

            GridViewDataColumn column = null;
            if (columnType == GridViewColumnType.Text)
            {
                column = new GridViewTextBoxColumn();
            }
            else if (columnType == GridViewColumnType.Numeric)
            {
                column = new GridViewTextBoxColumn();
            }
            else if (columnType == GridViewColumnType.CheckBox)
            {
                column = new GridViewCheckBoxColumn();
                (column as GridViewCheckBoxColumn).ThreeState = true;
            }
            else if (columnType == GridViewColumnType.Command)
            {
                column = new GridViewCommandColumn();
            }
            else if (columnType == GridViewColumnType.Link)
            {
                column = new GridViewHyperlinkColumn();
            }
            column.Name = fieldName;
            column.FieldName = fieldName;
            column.HeaderText = header;
            column.ReadOnly = readOnly;
            column.Width = width;
            return column;
        }

    }
}
