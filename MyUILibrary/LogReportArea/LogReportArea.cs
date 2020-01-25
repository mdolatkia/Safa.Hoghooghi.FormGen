using ModelEntites;
using MyCommonWPFControls;

using MyUILibrary.EntityArea;
using MyUILibraryInterfaces.LogReportArea;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUILibrary.EntityArea
{
    class LogReportArea : I_LogReportArea
    {
        MySearchLookup entitySearchLookup;
        MySearchLookup userSearchLookup;
        I_EditEntityAreaOneData EditEntityArea { set; get; }

        public LogReportArea(LogReportAreaInitializer initializer)
        {
            AreaInitializer = initializer;
            View = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GenerateViewOfLogReportArea();
            View.Confirmed += View_Confirmed;
            View.ExitRequested += View_ExitRequested;
            View.DetailsClicked += View_DetailsClicked;
            View.PackageClicked += View_PackageClicked;
            SetLookups();

            View.PackageDatagridVisiblity = false;
            if (AreaInitializer.EntityID != 0)
            {
                entitySearchLookup.SelectedValue = AreaInitializer.EntityID;
            }
            if (AreaInitializer.DataItem != null)
            {
                if (EditEntityArea != null)
                {
                    EditEntityArea.ShowDataFromExternalSource(AreaInitializer.DataItem);
                    if (EditEntityArea.GetDataList().Any())
                        SearchConfirmed();
                }
            }
        }

        private void SetLookups()
        {
            entitySearchLookup = new MySearchLookup();
            entitySearchLookup.DisplayMember = "Alias";
            entitySearchLookup.SelectedValueMember = "ID";
            entitySearchLookup.SearchFilterChanged += EntitySearchLookup_SearchFilterChanged;
            entitySearchLookup.SelectionChanged += EntitySearchLookup_SelectionChanged;
            View.AddEntitySelector(entitySearchLookup);

            userSearchLookup = new MySearchLookup();
            userSearchLookup.DisplayMember = "FullName";
            userSearchLookup.SelectedValueMember = "ID";
            userSearchLookup.SearchFilterChanged += UserSearchLookup_SearchFilterChanged;
            View.AddUserSelector(userSearchLookup);

            View.SetMainTypeItems(Enum.GetValues(typeof(DataLogType)).Cast<DataLogType>().ToList());
        }
        private void View_PackageClicked(object sender, DataLogDTO e)
        {
            if (e.PackageGuid != null)
            {
                var logData = AgentUICoreMediator.GetAgentUICoreMediator.logManagerService.GetDataLogsByPackageID(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), e.PackageGuid.Value);
                View.AddPackageDataLogs(logData);
                View.PackageDatagridVisiblity = true;
            }
        }

        //   I_View_EditLogReportDetails EditLogReportDetailsView;
        private void View_DetailsClicked(object sender, DataLogDTO e)
        {
            if (e.MainType == DataLogType.DataEdit || e.MainType == DataLogType.DataInsert)
            {
                //if (EditLogReportDetailsView == null)
                //{
                var logData = AgentUICoreMediator.GetAgentUICoreMediator.logManagerService.GetDataLog(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), e.ID);

                var EditLogReportDetailsView = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GenereateViewOfEditLogReportDetails();
                EditLogReportDetailsView.ColumnParameterVisibility = false;
                EditLogReportDetailsView.ExitRequested += EditLogReportDetailsView_ExitRequested;
                // }
                EditLogReportDetailsView.ColmnSelected += EditLogReportDetailsView_ColmnSelected;
                var window = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetDialogWindow();
                EditLogReportDetailsView.AddColumnEditLogs(logData.EditDataItemColumnDetails);
                EditLogReportDetailsView.AddExceptionLogs(new List<EditDataItemExceptionLogDTO>() { logData.EditDataItemExceptionLog });
                window.ShowDialog(EditLogReportDetailsView, "جزئیات", Enum_WindowSize.Maximized);
            }
            else
            if (e.MainType == DataLogType.ArchiveDelete || e.MainType == DataLogType.ArchiveInsert)
            {
                //var logData = AgentUICoreMediator.GetAgentUICoreMediator.logManagerService.GetDataLog(e.ID);

                ////if (EditLogReportDetailsView == null)
                ////{
                //var archiveLogDetailsView = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GenereateViewOfArchiveLogReportDetails();
                //archiveLogDetailsView.ExitRequested += EditLogReportDetailsView_ExitRequested;
                //var window = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetDialogWindow();
                //archiveLogDetailsView.AddExceptionLogs(new List<ArchiveItemLogDTO>() { logData.ArchiveItemLog });
                //window.ShowDialog(archiveLogDetailsView, "جزئیات", Enum_WindowSize.Maximized);
            }
        }

        private void EditLogReportDetailsView_ColmnSelected(object sender, EditDataItemColumnDetailsDTO e)
        {
            if (e == null || !e.FormulaUsageParemeters.Any())
                (sender as I_View_EditLogReportDetails).ColumnParameterVisibility = false;
            else
            {
                (sender as I_View_EditLogReportDetails).ColumnParameterVisibility = true;
                (sender as I_View_EditLogReportDetails).AddColumnFormulaParameters(e.FormulaUsageParemeters);
            }
        }

        private void EditLogReportDetailsView_ExitRequested(object sender, EventArgs e)
        {
            AgentUICoreMediator.GetAgentUICoreMediator.UIManager.CloseDialog(sender);
        }

        public LogReportAreaInitializer AreaInitializer
        {
            set; get;
        }
        public I_View_LogReportArea View
        {
            set; get;
        }
        private void EntitySearchLookup_SelectionChanged(object sender, SelectionChangedArg e)
        {
            if (e.SelectedItem != null)
            {
                var entity = e.SelectedItem as TableDrivedEntityDTO;
                EditEntityAreaInitializer editEntityAreaInitializer1 = new EditEntityAreaInitializer();
                editEntityAreaInitializer1.EntityID = entity.ID;
                editEntityAreaInitializer1.IntracionMode = CommonDefinitions.UISettings.IntracionMode.Select;
                editEntityAreaInitializer1.DataMode = CommonDefinitions.UISettings.DataMode.One;
                var FirstSideEditEntityAreaResult = EditEntityAreaConstructor.GetEditEntityArea(editEntityAreaInitializer1);
                if (FirstSideEditEntityAreaResult.Item1 != null && FirstSideEditEntityAreaResult.Item1 is I_EditEntityAreaOneData)
                {
                    EditEntityArea = FirstSideEditEntityAreaResult.Item1 as I_EditEntityAreaOneData;
                    EditEntityArea.SetAreaInitializer(editEntityAreaInitializer1);
                    View.AddDataSelector(EditEntityArea.TemporaryDisplayView);
                }
            }
            else
            {
                View.RemoveDataSelector();
            }
        }

    

        private void UserSearchLookup_SearchFilterChanged(object sender, SearchFilterArg e)
        {
            if (!string.IsNullOrEmpty(e.SingleFilterValue))
            {
                if (e.FilterBySelectedValue)
                {
                    var user = AgentUICoreMediator.GetAgentUICoreMediator.userManagerService.GetUser(Convert.ToInt32(e.SingleFilterValue));
                    e.ResultItemsSource = new List<UserDTO> { user };
                }
                else
                {
                    var users = AgentUICoreMediator.GetAgentUICoreMediator.userManagerService.SearchUsersByString(e.SingleFilterValue);
                    e.ResultItemsSource = users;
                }
            }
        }

        private void EntitySearchLookup_SearchFilterChanged(object sender, SearchFilterArg e)
        {
            if (!string.IsNullOrEmpty(e.SingleFilterValue))
            {
                if (e.FilterBySelectedValue)
                {
                    var entity = AgentUICoreMediator.GetAgentUICoreMediator.tableDrivedEntityManagerService.GetSimpleEntity(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), Convert.ToInt32(e.SingleFilterValue));
                    if (entity != null)
                        e.ResultItemsSource = new List<TableDrivedEntityDTO> { entity };
                }
                else
                {
                    var entities = AgentUICoreMediator.GetAgentUICoreMediator.tableDrivedEntityManagerService.SearchEntities(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), e.SingleFilterValue,false);
                    e.ResultItemsSource = entities;
                }
            }
        }

        private void View_ExitRequested(object sender, EventArgs e)
        {
            AgentUICoreMediator.GetAgentUICoreMediator.UIManager.CloseDialog(sender);
        }

        private void View_Confirmed(object sender, EventArgs e)
        {
            SearchConfirmed();

        }
        private void SearchConfirmed()
        {
            int entityID = 0;
            if (entitySearchLookup.SelectedItem != null)
                entityID = (entitySearchLookup.SelectedItem as TableDrivedEntityDTO).ID;
            int userID = 0;
            if (userSearchLookup.SelectedItem != null)
                userID = (userSearchLookup.SelectedItem as UserDTO).ID;
            DP_DataRepository data = null;
            if (EditEntityArea != null && EditEntityArea.GetDataList().Count != 0)
                data = EditEntityArea.GetDataList().First();
            int columnID = 0;
            if (View.SelectedColumn != null)
                columnID = (View.SelectedColumn as ColumnDTO).ID;

            var logs = AgentUICoreMediator.GetAgentUICoreMediator.logManagerService.SearchDataLogs(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), entityID, View.FromData, View.ToDate, data,
                View.SelectedMainType, columnID, userID, View.withMajorException, View.withMinorException);
            View.SetLogs(logs);
            View.PackageDatagridVisiblity = false;
        }

    }
}
