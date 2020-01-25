using MyUILibraryInterfaces.EntityArea;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyUILibrary.EntityArea;
using ProxyLibrary;
using MyCommonWPFControls;
using ModelEntites;

namespace MyUILibrary.EntitySelectArea
{
    public class EntitySelectArea : I_EntitySelectArea
    {
        MySearchLookup entitySearchLookup;
        public EntitySelectAreaInitializer EntitySelectAreaInitializer { set; get; }

        public TableDrivedEntityDTO SelectedEntity
        {
            get
            {
                return entitySearchLookup.SelectedItem as TableDrivedEntityDTO;
            }
        }
        public EntitySelectArea(EntitySelectAreaInitializer entitySelectAreaInitializer)
        {
            //تو این ویوی کاربردی مثلا آرشیو به این اصلی اضافه میشه ولی تو جنرال سرچ ویو اینجا به کاربردی مثلا دیتا ویو اضافه میشه. کدوم بهتره؟ اصلاح بشه
            EntitySelectAreaInitializer = entitySelectAreaInitializer;
            View = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GenerateViewOfEntityselectArea();
            if(EntitySelectAreaInitializer.ExternalView!=null)
            View.AddExternalArea(EntitySelectAreaInitializer.ExternalView);

            entitySearchLookup = new MySearchLookup();
            entitySearchLookup.DisplayMember = "Alias";
            entitySearchLookup.SelectedValueMember = "ID";
            entitySearchLookup.SearchFilterChanged += EntitySearchLookup_SearchFilterChanged;
            entitySearchLookup.SelectionChanged += EntitySearchLookup_SelectionChanged;
            View.AddEntitySelector(entitySearchLookup, "موجودیتها");
            if (EntitySelectAreaInitializer.LockEntitySelector)
                entitySearchLookup.IsEnabledLookup = false;
            if (EntitySelectAreaInitializer.EntityID != 0)
            {
                entitySearchLookup.SelectedValue = EntitySelectAreaInitializer.EntityID;
            }

        }
        private void EntitySearchLookup_SearchFilterChanged(object sender, SearchFilterArg e)
        {
            if (!string.IsNullOrEmpty(e.SingleFilterValue))
            {
                if (e.FilterBySelectedValue)
                {
                    var entity = AgentUICoreMediator.GetAgentUICoreMediator.tableDrivedEntityManagerService.GetSimpleEntity(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), Convert.ToInt32(e.SingleFilterValue), EntitySelectAreaInitializer.SpecificActions);
                    if (entity != null)
                        e.ResultItemsSource = new List<TableDrivedEntityDTO> { entity };
                }
                else
                {
                    var entities = AgentUICoreMediator.GetAgentUICoreMediator.tableDrivedEntityManagerService.SearchEntities(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), e.SingleFilterValue, false, EntitySelectAreaInitializer.SpecificActions);
                    e.ResultItemsSource = entities;
                }
            }
        }
        private void EntitySearchLookup_SelectionChanged(object sender, SelectionChangedArg e)
        {
            if (e.SelectedItem != null)
            {
                var entity = e.SelectedItem as TableDrivedEntityDTO;
                if (EntitySelected != null)
                    EntitySelected(this, entity.ID);

                EditEntityAreaInitializer editEntityAreaInitializer1 = new EditEntityAreaInitializer();
                editEntityAreaInitializer1.EntityID = entity.ID;
                editEntityAreaInitializer1.IntracionMode = CommonDefinitions.UISettings.IntracionMode.Select;
                editEntityAreaInitializer1.DataMode = CommonDefinitions.UISettings.DataMode.One;
                var FirstSideEditEntityAreaResult = EditEntityAreaConstructor.GetEditEntityArea(editEntityAreaInitializer1);
                if (FirstSideEditEntityAreaResult.Item1 != null && FirstSideEditEntityAreaResult.Item1 is I_EditEntityAreaOneData)
                {
                    SelectDataArea = FirstSideEditEntityAreaResult.Item1 as I_EditEntityAreaOneData;
                    SelectDataArea.DataItemSelected += FirstSideEditEntityArea_DataItemSelected;
                    SelectDataArea.SetAreaInitializer(editEntityAreaInitializer1);
                    View.AddDataSelector(SelectDataArea.TemporaryDisplayView, "داده");
                }

              
            }
            else
            {
                View.RemoveDataSelector();
                if (EntitySelected != null)
                    EntitySelected(this, null);

                if (DataItemSelected != null)
                    DataItemSelected(this, new EditAreaDataItemArg() { DataItem = null });

            }
        }

        public event EventHandler<EditAreaDataItemArg> DataItemSelected;
        public event EventHandler<int?> EntitySelected;

        private void FirstSideEditEntityArea_DataItemSelected(object sender, EditAreaDataItemArg e)
        {
            if (DataItemSelected != null)
                DataItemSelected(this, e);
        }

        public void EnableDisableSelectArea(bool enable)
        {
            SelectDataArea.TemporaryDisplayView.DisableEnable(enable);
        }

        public void SelectData(DP_DataView dataInstance)
        {
            //DP_SearchRepository searchItems = new DP_SearchRepository(dataInstance.TargetEntityID);
            //foreach (var item in dataInstance.KeyProperties)
            //{
            //    searchItems.Phrases.Add(new SearchProperty() { ColumnID = item.ColumnID, Value = item.Value });
            //}

            //var requester = AgentUICoreMediator.GetAgentUICoreMediator.GetRequester();
            //DR_SearchViewRequest request = new DR_SearchViewRequest(requester, searchItems);
            //var childViewData = AgentUICoreMediator.GetAgentUICoreMediator.SendSearchViewRequest(request);
            //if (childViewData.Result == Enum_DR_ResultType.SeccessfullyDone)
            //{
            //    if (childViewData.ResultDataItems.Count == 1)
            //    {

            SelectDataArea.ClearData(false);
            SelectDataArea.ShowDataFromExternalSource(dataInstance);
            //    }
            //}

        }

        I_EditEntityAreaOneData SelectDataArea
        {
            set; get;
        }

        public I_View_EntitySelectArea View
        {
            set; get;
        }
    }
}
