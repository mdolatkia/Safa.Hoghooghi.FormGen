using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelEntites;
using ProxyLibrary;
using MyUILibraryInterfaces.EntityArea;

using MyUILibraryInterfaces.DataTreeArea;
using MyUILibraryInterfaces.ContextMenu;

namespace MyUILibrary.EntityArea
{
    class EntityLettersArea : I_EntityLettersArea
    {
        public I_EntitySelectArea EntitySelectArea { set; get; }
        public object MainView { set; get; }
        public LettersAreaInitializer AreaInitializer { get; set; }
        //   public DP_DataView DataItem { set; get; }
        //public bool SecurityNoAccess { set; get; }
        //public bool SecurityReadonly { set; get; }
        //public bool SecurityEdit { set; get; }
        //public bool SecurityEditAndDelete { set; get; }
        public EntityLettersArea(LettersAreaInitializer areaInitializer)
        {
            AreaInitializer = areaInitializer;
            //DataItem = dataInstance;
            //var keyColumns = dataInstance.KeyProperties;
            //AgentUICoreMediator.GetAgentUICoreMediator.DataItemManager.SetDataItemDTO(dataInstance);
            //EntityID = entityId;
            LetterView = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GenerateViewOfEntityLettersArea();
            LetterView.NewLetterClicked += View_NewLetterClicked;
            LetterView.EnableAdd = false;
            LetterView.DataTreeRequested += View_DataTreeRequested;

            LetterView.ContextMenuLoaded += LetterView_ContextMenuLoaded;
            MyUILibraryInterfaces.EntityArea.EntitySelectAreaInitializer selectAreaInitializer = new MyUILibraryInterfaces.EntityArea.EntitySelectAreaInitializer();
            selectAreaInitializer.ExternalView = LetterView;
            if (areaInitializer.EntityID != 0)
                selectAreaInitializer.LockEntitySelector = true;
            selectAreaInitializer.EntityID = areaInitializer.EntityID;
            selectAreaInitializer.SpecificActions = new List<SecurityAction>() { SecurityAction.LetterView, SecurityAction.LetterEdit };
            EntitySelectArea = new EntitySelectArea.EntitySelectArea(selectAreaInitializer);
            EntitySelectArea.EntitySelected += EntitySelectArea_EntitySelected;
            EntitySelectArea.DataItemSelected += SelectArea_DataItemSelected;
            MainView = EntitySelectArea.View;

            if (areaInitializer.DataInstance != null)
            {
                EntitySelectArea.EnableDisableSelectArea(false);
                EntitySelectArea.SelectData(areaInitializer.DataInstance);
            }

        }



        private void EntitySelectArea_EntitySelected(object sender, int? entityID)
        {
            //if (entityID != null)
            //    ApplySecurity();
        }

        //private void ApplySecurity()
        //{
        //    var permissions = AgentUICoreMediator.GetAgentUICoreMediator.tableDrivedEntityManagerService.GetEntityAssignedPermissions(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), EntitySelectArea.SelectedEntity.ID, false);
        //    if (!permissions.GrantedActions.Any(x => x == SecurityAction.LetterEdit))
        //    {
        //        LetterView.EnableAdd = false;
        //        LetterView.EnableDelete = false;
        //        LetterView.EnableEdit = false;
        //    }
        //    else
        //    {
        //        LetterView.EnableAdd = true;
        //        LetterView.EnableDelete = true;
        //        LetterView.EnableEdit = true;
        //    }
        //}


        private void LetterView_ContextMenuLoaded(object sender, MyUILibraryInterfaces.ContextMenu.ContextMenuArg e)
        {
            var letter = e.ContextObject as LetterDTO;
            if (letter != null)
            {
                List<ContextMenuItem> menus = new List<ContextMenuItem>();
                var permissions = AgentUICoreMediator.GetAgentUICoreMediator.tableDrivedEntityManagerService.GetEntityAssignedPermissions(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), letter.DataItem.TargetEntityID, false);
                if (permissions.GrantedActions.Any(x => x == SecurityAction.LetterEdit))
                {
                    ContextMenuItem editMenu = new ContextMenuItem() { Title = "اصلاح نامه" };
                    editMenu.Clicked += (sender1, e1) => EditMenu_Clicked(sender1, e1, letter);
                    menus.Add(editMenu);

                    ContextMenuItem deleteMenu = new ContextMenuItem() { Title = "حذف نامه" };
                    deleteMenu.Clicked += (sender1, e1) => DeleteMenu_Clicked1(sender1, e1, letter);
                    menus.Add(deleteMenu);
                }
                e.ContextMenuManager.SetMenuItems(menus);
            }
        }

        private void EditMenu_Clicked(object sender, EventArgs e, LetterDTO letter)
        {
            var initializer = new LetterAreaInitializer();
            initializer.LetterID = letter.ID;
            var editLetterArea = new EditLetterArea(initializer);
            editLetterArea.LetterUpdated += EditLetterArea_LetterUpdated;
            AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetDialogWindow().ShowDialog(editLetterArea.View, "نامه", Enum_WindowSize.Big);
        }
        private void DeleteMenu_Clicked1(object sender, EventArgs e, LetterDTO letter)
        {
            var text = "آیا از حذف نامه مطمئین هستید؟";
            var ask = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowConfirm("تایید", text, UserPromptMode.YesNo);
            if (ask == Temp.ConfirmResul.Yes)
            {
                var result = AgentUICoreMediator.GetAgentUICoreMediator.LetterManager.DeleteLetter(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), letter.ID);
                AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowMessage("پیام", result.Message, result.Details);
                if (result.Result)
                    ShowLetters();
            }
        }
        private void View_DataTreeRequested(object sender, EventArgs e)
        {
            LetterView.DataTreeVisibility = !LetterView.DataTreeVisibility;

            //       LetterView.ShowDataTree(DataTreeArea.View);
        }
        //AssignedPermissionDTO _Permission;
        //public AssignedPermissionDTO Permission
        //{
        //    get
        //    {
        //        if (_Permission == null)
        //            _Permission = AgentUICoreMediator.GetAgentUICoreMediator.tableDrivedEntityManagerService.GetEntityAssignedPermissions(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), EntitySelectArea.SelectedEntity.ID, false);
        //        return _Permission;
        //    }
        //}
        //private void ManageSecurity()
        //{
        //    if (Permission.GrantedActions.Any(x => x == SecurityAction.NoAccess))
        //    {
        //        SecurityNoAccess = true;
        //    }
        //    else
        //    {
        //        if (Permission.GrantedActions.Any(x => x == SecurityAction.EditAndDelete))
        //        {
        //            SecurityEditAndDelete = true;
        //        }
        //        else if (Permission.GrantedActions.Any(x => x == SecurityAction.Edit))
        //        {
        //            SecurityEdit = true;
        //        }
        //        else if (Permission.GrantedActions.Any(x => x == SecurityAction.ReadOnly))
        //        {
        //            SecurityReadonly = true;
        //        }
        //        else
        //            SecurityNoAccess = true;
        //    }
        //    ImposeSecurity();
        //}
        //private void ImposeSecurity()
        //{
        //    if (SecurityNoAccess)
        //    {
        //        MainView = null;
        //        //ArchiveView.EnableDisable(false);
        //        AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowInfo("عدم دسترسی به آرشیو");
        //    }
        //    else
        //    {
        //        if (SecurityReadonly)
        //        {
        //            LetterView.EnableDelete = false;
        //            LetterView.EnableAdd = false;
        //            LetterView.EnableEdit = false;

        //        }
        //        else if (SecurityEditAndDelete)
        //        {
        //        }
        //        else if (SecurityEdit)
        //        {
        //            LetterView.EnableDelete = false;
        //        }
        //        else
        //        {
        //            MainView = null;
        //            AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowInfo("عدم دسترسی به آرشیو");
        //        }
        //    }
        //}
        private void SelectArea_DataItemSelected(object sender, EditAreaDataItemArg e)
        {
            if (e.DataItem != null)
            {
                LetterView.EnableDisable(true);
                DataTreeArea = new MyUILibrary.DataTreeArea.DataTreeArea();
                DataTreeArea.ContextMenuLoaded += DataTreeArea_ContextMenuLoaded;
                DataTreeArea.DataAreaConfirmed += DataTreeArea_DataAreaConfirmed;
                var dataTreeInistializer = new DataTreeAreaInitializer();
                dataTreeInistializer.EntitiyID = EntitySelectArea.SelectedEntity.ID;
                dataTreeInistializer.RelationshipTailsLoaded = EntitySelectArea.SelectedEntity.LoadLetterRelatedItems;
                dataTreeInistializer.FirstDataItem = e.DataItem.DataView;
                dataTreeInistializer.RelationshipTails = LetterRelationshipTails.Select(x => x.RelationshipTail).ToList();
                DataTreeArea.SetAreaInitializer(dataTreeInistializer);
                DataTreeArea.SelectAll();
                LetterView.DataTreeAreaEnabled = true;
                LetterView.ShowDataTree(DataTreeArea.View);
                if (dataTreeInistializer.RelationshipTailsLoaded)
                    LetterView.ShowDataTree(DataTreeArea.View);

                ShowLetters();
            }
            else
                LetterView.EnableDisable(false);
        }
        //   List<LetterRelationshipTailDTO> _LetterRelationshipTails;

        List<LetterRelationshipTailDTO> LetterRelationshipTails
        {
            get
            {

                return AgentUICoreMediator.GetAgentUICoreMediator.LetterManager.GetLetterRelationshipTails(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), EntitySelectArea.SelectedEntity.ID);

            }
        }
        I_DataTreeArea DataTreeArea { set; get; }
        //DP_DataView MainDataInstance { set; get; }

        //private void ShowLetters(DP_DataView dataInstance, bool loadRelatedItems)
        //{


        //}

        DP_DataView singleDataItem = null;
        private void ShowLetters()
        {
            singleDataItem = null;
            var selectedDatas = DataTreeArea.GetSelectedDataItems();
            if (selectedDatas.Count == 0 || selectedDatas.Count > 1)
                LetterView.EnableAdd = false;
            else
            {
                var permissions = AgentUICoreMediator.GetAgentUICoreMediator.tableDrivedEntityManagerService.GetEntityAssignedPermissions(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), selectedDatas.First().TargetEntityID, false);
                if (permissions.GrantedActions.Any(x => x == SecurityAction.LetterEdit))
                {
                    singleDataItem = selectedDatas.First();
                    LetterView.EnableAdd = true;
                }
                else
                    LetterView.EnableAdd = false;
            }
            Letters = AgentUICoreMediator.GetAgentUICoreMediator.LetterManager.GetLetters(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), GetDataItemIds());
            LetterView.ShowList(Letters);
        }
        private List<int> GetDataItemIds()
        {
            List<int> result = new List<int>();
            List<DP_DataView> list = DataTreeArea.GetSelectedDataItems();
            foreach (var item in list)
            {
                if (item.DataItemID != 0)
                    result.Add(item.DataItemID);
                else
                {
                    if (!item.DataItemSearched)
                    {
                        if (AgentUICoreMediator.GetAgentUICoreMediator.DataItemManager.SetDataItemDTO(item))
                            result.Add(item.DataItemID);
                    }
                }
            }
            return result;
        }

        private void DataTreeArea_DataAreaConfirmed(object sender, EventArgs e)
        {
            ShowLetters();
        }

        private void DataTreeArea_ContextMenuLoaded(object sender, ContextMenuArg e)
        {
            if (e.ContextObject is DP_DataView)
            {
                var data = (e.ContextObject as DP_DataView);
                List<ContextMenuItem> menus = new List<ContextMenuItem>();
                var permissions = AgentUICoreMediator.GetAgentUICoreMediator.tableDrivedEntityManagerService.GetEntityAssignedPermissions(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), data.TargetEntityID, false);
                if (permissions.GrantedActions.Any(x => x == SecurityAction.LetterEdit))
                {
                    ContextMenuItem newMenu = new ContextMenuItem() { Title = "افزودن نامه" };
                    newMenu.Clicked += (sender1, e1) => AddMenu_Clicked(sender1, e1, data);
                    menus.Add(newMenu);

                }
                e.ContextMenuManager.SetMenuItems(menus);
            }
        }
        private void AddMenu_Clicked(object sender, EventArgs e, DP_DataView dataItem)
        {
            AddNewLetter(dataItem);
        }

        private void AddNewLetter(DP_DataView dataItem)
        {
            var initializer = new LetterAreaInitializer();
            initializer.DataInstance = dataItem;
            var editLetterArea = new EditLetterArea(initializer);
            editLetterArea.LetterUpdated += EditLetterArea_LetterUpdated;
            AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetDialogWindow().ShowDialog(editLetterArea.View, "نامه", Enum_WindowSize.Big);
        }


        private void View_NewLetterClicked(object sender, EventArgs e)
        {
            if (singleDataItem != null)
                AddNewLetter(singleDataItem);
        }




        private void EditLetterArea_LetterUpdated(object sender, EventArgs e)
        {
            ShowLetters();
        }

        //public I_EditEntityLetterArea EditLetterArea
        //{
        //    set; get;
        //}

        //public int EntityID
        //{
        //    set; get;
        //}

        public List<LetterDTO> Letters
        {
            set; get;
        }

        public I_View_EntityLettersArea LetterView
        {
            set; get;
        }
    }
}
