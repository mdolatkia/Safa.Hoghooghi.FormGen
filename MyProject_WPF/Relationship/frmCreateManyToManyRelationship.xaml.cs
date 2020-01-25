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
    /// Interaction logic for frmCreateManyToManyRelationship.xaml
    /// </summary>
    public partial class frmCreateManyToManyRelationship: UserControl
    {
        public event EventHandler<ManyToManyCreatedArg> ManyToManyCreated;

        int TableID { set; get; }
        public frmCreateManyToManyRelationship(int associativeTableID)
        {
            InitializeComponent();
            TableID = associativeTableID;
            BizRelationship bizRelationship = new BizRelationship();
            dtgList.ItemsSource = bizRelationship.GetRelationshipsByTableID(TableID).Where(x => x.TypeEnum == Enum_RelationshipType.ManyToOne);
            BizTable bizTable = new BizTable();
            var table = bizTable.GetTable(TableID);
            txtName.Text = table.Name;

        }

        private void btnCreateMode_Click(object sender, EventArgs e)
        {
            if (txtName.Text != "")
            {
                if (dtgList.SelectedItems.Count == 2)
                {
                    ManyToManyCreatedArg arg = new ManyToManyCreatedArg();
                    arg.Name = txtName.Text;
                    arg.ManyToOneIDs = new List<int>();
                    arg.TableID = TableID;
                    arg.ManyToOneIDs = new List<int>();
                    foreach (var row in dtgList.SelectedItems)
                    {
                        arg.ManyToOneIDs.Add((row as RelationshipDTO).ID);
                    }
                    ManyToManyCreated(this, arg);

                    MyProjectManager.GetMyProjectManager.CloseDialog(this);
                }
                else
                    MessageBox.Show("لطفاً دو رابطه چند به یک را انتخاب نمایید");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {

            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }
    }

    public class ManyToManyCreatedArg : EventArgs
    {
        public int TableID { set; get; }
        public string Name { set; get; }
        public List<int> ManyToOneIDs { set; get; }
    }

}
