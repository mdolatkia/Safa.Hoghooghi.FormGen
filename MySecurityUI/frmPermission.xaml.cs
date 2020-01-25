using ModelEntites;
using MyModelManager;
using MySecurity;
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

namespace MySecurityUI
{
    /// <summary>
    /// Interaction logic for frmObjects.xaml
    /// </summary>
    public partial class frmPermission : UserControl
    {
        BizDatabase bizDatabase = new BizDatabase();
        public ObjectDTO Object { set; get; }
        public RoleOrRoleGroupDTO RoleOrRoleGroup { set; get; }
        BizPermission bizPermission = new BizPermission();
        PermissionDTO Message { set; get; }
        public frmPermission()
        {
            InitializeComponent();
            ucObjectTree.ObjectSelected += ucObjectList_ObjectSelected;
            ucRoleList.RoleOrRoleGroupSelected += UcRoleList_RoleOrRoleGroupSelected;

            //درست شود
            var databases = bizDatabase.GetDatabases();
            ucObjectTree.ShowDatabaseObjects(databases.Select(x => x.ID).ToList());
        }

        private void UcRoleList_RoleOrRoleGroupSelected(object sender, RoleOrRoleGroupSelectedArg e)
        {
            RoleOrRoleGroup = e.RoleOrRoleGroup;
            GetMessage();
        }


        void ucObjectList_ObjectSelected(object sender, ObjectSelectedArg e)
        {
            Object = e.Object;
            var actions = SecurityHelper.GetActionsByCategory(Object.ObjectCategory);
            List<ActionDTO> list = new List<ActionDTO>();
            foreach (var action in actions)
            {
                ActionDTO item = new ActionDTO();
                item.Action = action;
                list.Add(item);
            }
            dtgRoleActions.ItemsSource = list;
            GetMessage();
        }
        private void GetMessage()
        {
            if (Object != null && RoleOrRoleGroup != null)
            {
                var permissionId = bizPermission.GetPermissionId(RoleOrRoleGroup, Object.ObjectCategory, Object.ObjectIdentity);
                if (permissionId != 0)
                {
                    Message = bizPermission.GetPermission(permissionId);
                    ShowData();
                }
                else
                {
                    Message = null;
                    ClearData();
                }

            }
        }

        private void ClearData()
        {
            var listActions = dtgRoleActions.ItemsSource as List<ActionDTO>;
            foreach (var action in listActions)
            {
                action.Selected = false;
            }
            dtgRoleActions.ItemsSource = null;
            dtgRoleActions.ItemsSource = listActions;
        }

        private void ShowData()
        {
            var listActions = dtgRoleActions.ItemsSource as List<ActionDTO>;
            foreach (var action in listActions)
            {
                action.Selected = Message.Actions.Any(x => x.Action == action.Action);
            
            }
            dtgRoleActions.ItemsSource = null;
            dtgRoleActions.ItemsSource = listActions;
        }



        //void ucObjectEdit_ObjectSaved(object sender, ObjectSavedArg e)
        //{
        //    ucObjectList.ShowObjects(e.Object.ParentID);
        //}

        //private void btnExtractObjectFromDB_Click(object sender, RoutedEventArgs e)
        //{
        //    BizObject bizObject = new BizObject();
        //    bizObject.ExtractObjectsFromDB();
        //    ucObjectList.ShowObjects(null);
        //}

        private void btnSaveRoleActions_Click(object sender, RoutedEventArgs e)
        {
            var listActions = (dtgRoleActions.ItemsSource as List<ActionDTO>).Where(x => x.Selected);
            if (listActions.Any(x => x.Action == SecurityAction.NoAccess))
            {
                if (listActions.Any(x =>x.Action != SecurityAction.NoAccess))
                {
                    MessageBox.Show("امکان انتخاب گزینه های عدم دسترسی و سایر گزینه ها نمی باشد");
                    return;
                }
            }
            if (listActions.Any(x => x.Action == SecurityAction.ReadOnly))
            {
                if (listActions.Any(x => x.Action != SecurityAction.NoAccess && x.Action != SecurityAction.MenuAccess && x.Action != SecurityAction.ReadOnly))
                {
                    MessageBox.Show("امکان انتخاب گزینه های فقط خواندنی و سایر گزینه ها نمی باشد");
                    return;
                }
            }
            if(RoleOrRoleGroup==null)
            {
                MessageBox.Show("نقشی انتخاب نشده است");
                return;
            }
            if (Message == null)
                Message = new PermissionDTO();
            Message.RoleOrRoleGroup = RoleOrRoleGroup;
            Message.ObjectCategory = Object.ObjectCategory;
            Message.ObjectID = Object.ObjectIdentity;
            Message.Actions = listActions.ToList();
            bizPermission.SavePermission(Message);
            MessageBox.Show("اطلاعات ثبت شد");
        }
    }
}
