using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq;
namespace MyProject
{
    public partial class frmChooseItem<T> : Form
    {
        //bool MultiSelect { set; get; }
        public event EventHandler<SelectedItemArg<T>> ItemSelected;
        public frmChooseItem(List<T> items, bool multiSelect)
        {
            InitializeComponent();
            dtgItems.DataSource = items;

            dtgItems.MultiSelect = multiSelect;
            dtgItems.ReadOnly = true;
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (dtgItems.SelectedRows.Count > 0)
            {
                List<T> selectedList = new List<T>();
                foreach (DataGridViewRow item in dtgItems.SelectedRows)
                    selectedList.Add((T)item.DataBoundItem);
                if (ItemSelected != null)
                {
                    ItemSelected(this, new SelectedItemArg<T>() { Items = selectedList });
                    this.Close();
                }
            }
        }
    }
    public class SelectedItemArg<T> : EventArgs
    {
        public List<T> Items { set; get; }
    }
}
