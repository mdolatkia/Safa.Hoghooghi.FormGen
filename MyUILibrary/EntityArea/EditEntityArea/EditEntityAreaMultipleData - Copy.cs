

using CommonDefinitions.UISettings;
using ModelEntites;
using MySecurity;
using MyUILibrary;
using MyUILibrary.EntityArea.Commands;
using MyUILibraryInterfaces.DataViewArea;
using MyUILibraryInterfaces.EditEntityArea;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyUILibraryInterfaces.DataReportArea;


namespace MyUILibrary.EntityArea
{
    public class EditEntityAreaMultipleData : I_EditEntityAreaMultipleData
    {

        public FormulaHelper formulaHelper;
        StateHelper stateHelper = new StateHelper();
        CodeFunctionHelper codeFunctionHelper = new CodeFunctionHelper();
        DatabaseFunctionHelper DatabaseFunctionHelper = new DatabaseFunctionHelper();
        public List<Tuple<EntityUICompositionDTO, I_View_Container>> UICompositionContainers { set; get; }

        public event EventHandler<DisableEnableChangedArg> DisableEnableChanged;
        //public event EventHandler<DisableEnableCommandByTypeChangedArg> DisableEnableCommandByTypeChanged;
        public EditEntityAreaMultipleData()
        {
            formulaHelper = new FormulaHelper(this);
            UICompositionContainers = new List<Tuple<EntityUICompositionDTO, I_View_Container>>();
            SimpleColumnControls = new List<SimpleColumnControlMultipleData>();
            RelationshipColumnControls = new List<RelationshipColumnControlMultipleData>();
            ColumnFormulas = new List<ColumnFormula>();
        }
        public List<ColumnFormula> ColumnFormulas { set; get; }
        public AgentUICoreMediator AgentUICoreMediator
        {
            get
            {
                return AgentUICoreMediator.GetAgentUICoreMediator;
            }
        }


        public void SetAreaInitializer(EditEntityAreaInitializer initParam)
        {
            AreaInitializer = initParam;

            //DetermineDataMode();
            DetermineInteractionMode();
            //بعد از اینکه ظاهر همه لود شد EditEntityArea برای اولین
            //if (AreaInitializer.SourceRelation == null)
            //    ClearData(null);

            GenerateUIifNeeded();
        }

        private void GenerateUIifNeeded()
        {
            //if (AreaInitializer.SourceRelation == null || AreaInitializer.SourceRelation.SourceEditArea.AreaInitializer.DataMode != DataMode.Multiple)
            //{
            if (AreaInitializer.IntracionMode == IntracionMode.CreateDirect ||
                 AreaInitializer.IntracionMode == IntracionMode.CreateSelectDirect)
            {
                GenerateDataVieww();
            }
            if (AreaInitializer.IntracionMode == IntracionMode.CreateSelectDirect ||
               AreaInitializer.IntracionMode == IntracionMode.Select)
            {
                //SearchViewEntityArea = GenerateSearchViewArea();
            }

            if (AreaInitializer.IntracionMode == IntracionMode.Select)
                TemporaryDisplayView = AgentUICoreMediator.UIManager.GenerateTemporaryLinkUI(TemporaryLinkType.SerachView);
            if (AreaInitializer.IntracionMode == IntracionMode.CreateInDirect)
                TemporaryDisplayView = AgentUICoreMediator.UIManager.GenerateTemporaryLinkUI(TemporaryLinkType.DataView);
            else if (AreaInitializer.IntracionMode == IntracionMode.CreateSelectInDirect)
                TemporaryDisplayView = AgentUICoreMediator.UIManager.GenerateTemporaryLinkUI(TemporaryLinkType.DataSearchView);

            if (TemporaryDisplayView != null)
                TemporaryDisplayView.TemporaryDisplayViewRequested += TemporaryDisplayView_TemporaryDisplayViewRequested;
            //}
        }

        TableDrivedEntityDTO _FullEntity;
        public TableDrivedEntityDTO FullEntity
        {
            get
            {
                if (_FullEntity == null)
                    _FullEntity = AgentUICoreMediator.GetFullEntity(AreaInitializer.EntityID);
                //_FullEntity.Relationships.Clear();
                return _FullEntity;
            }
        }

        TableDrivedEntityDTO _EntityWithSimpleColumns;
        public TableDrivedEntityDTO EntityWithSimpleColumns
        {
            get
            {
                if (_EntityWithSimpleColumns == null)
                {
                    if (FullEntity != null)
                        return FullEntity;
                    else
                        _EntityWithSimpleColumns = AgentUICoreMediator.GetEntityWithSimpleColumns(AreaInitializer.EntityID);
                }
                return _EntityWithSimpleColumns;
            }
        }

        TableDrivedEntityDTO _SimpleEntity;
        public TableDrivedEntityDTO SimpleEntity
        {
            get
            {
                if (_SimpleEntity == null)
                {
                    if (FullEntity != null)
                        return FullEntity;
                    else if (EntityWithSimpleColumns != null)
                        return EntityWithSimpleColumns;
                    else
                        _SimpleEntity = AgentUICoreMediator.GetSimpleEntity(AreaInitializer.EntityID);
                }
                return _SimpleEntity;
            }
        }




        AssignedPermissionDTO _Permission;
        public AssignedPermissionDTO Permission
        {
            get
            {
                if (_Permission == null)
                    _Permission = AgentUICoreMediator.GetEntityPermissions(AreaInitializer.EntityID);
                return _Permission;
            }
        }

        List<ConditionalPermissionDTO> _ConditionalPermission;
        public List<ConditionalPermissionDTO> ConditionalPermission
        {
            get
            {
                if (_ConditionalPermission == null)
                    _ConditionalPermission = AgentUICoreMediator.GetEntityConditionalPermissions(AreaInitializer.EntityID);
                return _ConditionalPermission;
            }
        }
        bool UICompositionsCalled;
        EntityUICompositionCompositeDTO _UICompositions;
        public EntityUICompositionCompositeDTO UICompositions
        {
            get
            {
                if (_UICompositions == null && !UICompositionsCalled)
                {
                    UICompositionsCalled = true;
                    _UICompositions = AgentUICoreMediator.GetEntityUICompositionComposite(AreaInitializer.EntityID);
                }
                return _UICompositions;
            }
        }
        List<EntityCommandDTO> _Commands;
        public List<EntityCommandDTO> Commands
        {
            get
            {
                if (_Commands == null)
                    _Commands = AgentUICoreMediator.GetEntityCommands(AreaInitializer.EntityID);
                return _Commands;
            }
        }
        List<EntityValidationDTO> _Validations;
        public List<EntityValidationDTO> Validations
        {
            get
            {
                if (_Validations == null)
                    _Validations = AgentUICoreMediator.GetEntityValidations(AreaInitializer.EntityID);
                return _Validations;
            }
        }

        List<EntityStateDTO> _States;
        public List<EntityStateDTO> States
        {
            get
            {
                if (_States == null)
                    _States = AgentUICoreMediator.GetEntityStates(AreaInitializer.EntityID);
                return _States;
            }
        }
        List<ActionActivityDTO> _ActionActivities;
        public List<ActionActivityDTO> ActionActivities
        {
            get
            {
                if (_ActionActivities == null)
                    _ActionActivities = AgentUICoreMediator.GetEntityActionActivities(AreaInitializer.EntityID);
                return _ActionActivities;
            }
        }

        //private void DeterminDirectionMode()
        //{
        //    if (AreaInitializer.DirectionMode == DirectionMode.None)
        //    {
        //        AreaInitializer.DirectionMode = CommonDefinitions.UISettings.DirectionMode.Direct;
        //    }
        //}
        private void DetermineInteractionMode()
        {
            if (AreaInitializer.IntracionMode == IntracionMode.None)
            {
                AreaInitializer.IntracionMode = CommonDefinitions.UISettings.IntracionMode.CreateSelectDirect;
            }
        }
        //private void DetermineDataMode()
        //{
        //    if (AreaInitializer.DataMode == DataMode.None)
        //    {
        //        if (SimpleEntity.BatchDataEntry == null ||
        //       SimpleEntity.BatchDataEntry == false)
        //            AreaInitializer.DataMode = DataMode.One;
        //        else
        //            AreaInitializer.DataMode = DataMode.Multiple;
        //    }
        //}



        //باشد Direct صدا زده میشود ، اگر LoadTemplate یکبار در
        //.باشد AreaInitializer.FormComposed == false کلیک میشود و اگر Data زمانی که لینک ShowTemporaryDataView یکبار هم در
        public void ShowView(bool dialog, List<DP_DataRepository> dataRepository, bool? isExistingData = null)
        {
            if (dataRepository != null && dataRepository.Count > 0)
            {
                List<DP_DataRepository> foundDP = new List<DP_DataRepository>();
                foreach (var data in dataRepository)
                {
                    var result = AgentUICoreMediator.SearchDataForEditFromProperties(AreaInitializer.EntityID, data.DataInstance.Properties);
                    if (result.Count == 0)
                    {
                        throw new Exception("access?");
                    }
                    else if (result.Count > 1)
                    {
                        throw new Exception("asdad");
                    }
                    else
                    {
                        if (isExistingData == true)
                        {
                            if (!AreaInitializer.Data.Any(x => x == data))
                            {
                                throw (new Exception("asdasd"));
                            }
                        }
                        var fdata = result[0];
                        data.PairData = fdata;
                        foundDP.Add(fdata);

                    }

                }
                ShowDataInDataView(foundDP);
            }
            else
            {

            }
            if (dialog)
            {
                AgentUICoreMediator.UIManager.ShowDialog(DataView, SimpleEntity.Alias, Enum_WindowSize.Big);
            }
            else
            {
                AgentUICoreMediator.UIManager.ShowPane(DataView, SimpleEntity.Alias);
            }
        }
        private void GenerateDataVieww()
        {
            //if (AreaInitializer.DataMode == DataMode.One)
            //{

            //    DataView = AgentUICoreMediator.UIManager.GenerateEditEntityAreaOneDataView(EntityUISetting);

            //}
            //else
            DataView = AgentUICoreMediator.UIManager.GenerateEditEntityAreaMultipleDataView();
            // DataView.CommandExecuted += DataView_CommandExecuted;
            //فقط همینجا صدا زده میشود
            ManageDataView();
            GenerateCommands();
            ManageSecurity();
            CheckEntityStates();
            //ClearData(false, null);
        }

        //private EntityUISettingDTO GetGridSetting()
        //{
        //    EntityUISettingDTO gridSetting = new EntityUISettingDTO();
        //    if (EntityUISetting != null)
        //        gridSetting.ColumnsCount = EntityUISetting.UIColumnsCount;
        //    else
        //        gridSetting.ColumnsCount = 4;
        //    return gridSetting;
        //}

        private void GenerateCommands()
        {
            //Commands = AgentHelper.GenerateCommands<I_EntityAreaCommand>(AreaInitializer.IntracionMode,);

            CommandAttributes commandAttributeClear = new CommandAttributes();
            AreaInitializer.FormAttributes.CommandAttributes.Add(commandAttributeClear);
            commandAttributeClear.Command = new ClearCommand(this);
            DataView.AddCommand(commandAttributeClear.Command.CommandManager);
            if (AreaInitializer.SourceRelation == null)
            {
                CommandAttributes commandAttributeInfo = new CommandAttributes();
                AreaInitializer.FormAttributes.CommandAttributes.Add(commandAttributeInfo);
                commandAttributeInfo.Command = new InfoCommand(this);
                DataView.AddCommand(commandAttributeInfo.Command.CommandManager);

                CommandAttributes commandAttributeSave = new CommandAttributes();
                AreaInitializer.FormAttributes.CommandAttributes.Add(commandAttributeSave);
                commandAttributeSave.Command = new SaveCommand(this);
                DataView.AddCommand(commandAttributeSave.Command.CommandManager);

                CommandAttributes commandAttributeDelete = new CommandAttributes();
                AreaInitializer.FormAttributes.CommandAttributes.Add(commandAttributeDelete);
                commandAttributeDelete.Command = new DeleteCommand(this);
                DataView.AddCommand(commandAttributeDelete.Command.CommandManager);

                CommandAttributes commandAttributeLetter = new CommandAttributes();
                AreaInitializer.FormAttributes.CommandAttributes.Add(commandAttributeLetter);
                commandAttributeLetter.Command = new LetterCommand(this);
                DataView.AddCommand(commandAttributeLetter.Command.CommandManager);

                CommandAttributes commandAttributeArchive = new CommandAttributes();
                AreaInitializer.FormAttributes.CommandAttributes.Add(commandAttributeArchive);
                commandAttributeArchive.Command = new ArchiveCommand(this);
                DataView.AddCommand(commandAttributeArchive.Command.CommandManager);

                CommandAttributes commandAttributeDataView = new CommandAttributes();
                AreaInitializer.FormAttributes.CommandAttributes.Add(commandAttributeDataView);
                commandAttributeDataView.Command = new DataViewCommand(this);
                DataView.AddCommand(commandAttributeDataView.Command.CommandManager);

                CommandAttributes commandAttributeDataLink = new CommandAttributes();
                AreaInitializer.FormAttributes.CommandAttributes.Add(commandAttributeDataLink);
                commandAttributeDataLink.Command = new DataLinkCommand(this);
                DataView.AddCommand(commandAttributeDataLink.Command.CommandManager);

                CommandAttributes commandAttributeDataListReport = new CommandAttributes();
                AreaInitializer.FormAttributes.CommandAttributes.Add(commandAttributeDataListReport);
                commandAttributeDataListReport.Command = new DataListReportCommand(this);
                DataView.AddCommand(commandAttributeDataListReport.Command.CommandManager);
            }

            if (AreaInitializer.IntracionMode == IntracionMode.Select
                || AreaInitializer.IntracionMode == IntracionMode.CreateSelectDirect
                  || AreaInitializer.IntracionMode == IntracionMode.CreateSelectInDirect)
            {
                CommandAttributes commandAttributeSearch = new CommandAttributes();
                AreaInitializer.FormAttributes.CommandAttributes.Add(commandAttributeSearch);
                commandAttributeSearch.Command = new SearchCommand(this);
                DataView.AddCommand(commandAttributeSearch.Command.CommandManager);
            }

            if (AreaInitializer.DataMode == DataMode.Multiple)
            {
                CommandAttributes commandAttributeAdd = new CommandAttributes();
                AreaInitializer.FormAttributes.CommandAttributes.Add(commandAttributeAdd);
                commandAttributeAdd.Command = new AddCommand(this);
                DataView.AddCommand(commandAttributeAdd.Command.CommandManager);

                CommandAttributes commandAttributeRemove = new CommandAttributes();
                AreaInitializer.FormAttributes.CommandAttributes.Add(commandAttributeRemove);
                commandAttributeRemove.Command = new RemoveCommand(this);
                DataView.AddCommand(commandAttributeRemove.Command.CommandManager);
                if (AreaInitializer.IntracionMode == IntracionMode.CreateInDirect || AreaInitializer.IntracionMode == IntracionMode.CreateSelectInDirect)
                {
                    CommandAttributes closeAttributeRemove = new CommandAttributes();
                    AreaInitializer.FormAttributes.CommandAttributes.Add(closeAttributeRemove);
                    closeAttributeRemove.Command = new CloseDialogCommand(this);
                    DataView.AddCommand(closeAttributeRemove.Command.CommandManager);
                }
            }

            foreach (var command in Commands)
            {
                CommandAttributes commandAttribute = new CommandAttributes();
                commandAttribute.EntityCommandDTO = command;

                AreaInitializer.FormAttributes.CommandAttributes.Add(commandAttribute);
                commandAttribute.Command = new EntityArea.EntityCommand(commandAttribute);// CommandManager.GenerateCommand(commandAttribute); ;
                                                                                          //Commands.Add(commandAttribute.Command.CommandManager);
                DataView.AddCommand(commandAttribute.Command.CommandManager);

            }
        }

        //private void GenerateFormAndColumnAttributes()
        //{

        //    foreach (var columnControl in ColumnControls)
        //    {

        //    }
        //}
        //private void ManageEntityCommands()
        //{
        //    if (AreaInitializer.FormAttributes.CommandAttributes != null)
        //        foreach (var commandAttribute in AreaInitializer.FormAttributes.CommandAttributes)
        //        {


        //        }
        //}

        //bool SecurityNoAccessCommand = false;
        //bool SecurityReadOnlyCommand = false;
        //bool SecurityDeleteCommand = false;
        //bool SecuritySaveNewCommand = false;
        //bool SecuritySaveCommand = false;

        private void ManageSecurity()
        {
            //اگر فرم حالت ادیت داشت چک شود
            if (Permission.GrantedActions.Any(x => x == MySecurity.SecurityAction.NoAccess ||
               x == MySecurity.SecurityAction.ReadOnly || x == MySecurity.SecurityAction.Delete ||
               x == MySecurity.SecurityAction.Edit))
            {
                if (Permission.GrantedActions.Any(x => x == MySecurity.SecurityAction.NoAccess))
                    AreaInitializer.FormAttributes.SecurityNoAccess = true;
                else if (Permission.GrantedActions.Any(x => x == MySecurity.SecurityAction.ReadOnly))
                {
                    AreaInitializer.FormAttributes.SecurityReadOnly = true;
                }
                else
                {
                    if (Permission.GrantedActions.Any(x => x == MySecurity.SecurityAction.Delete))
                        AreaInitializer.FormAttributes.SecurityDelete = true;
                    else
                        AreaInitializer.FormAttributes.SecurityDelete = false;
                    if (Permission.GrantedActions.Any(x => x == MySecurity.SecurityAction.Edit))
                        AreaInitializer.FormAttributes.SecurityEdit = true;
                    else
                        AreaInitializer.FormAttributes.SecurityEdit = false;
                }
            }
            else
                AreaInitializer.FormAttributes.SecurityNoAccess = true;



            if (AreaInitializer.FormAttributes.SecurityNoAccess == false)
            {
                foreach (var commandAttribute in AreaInitializer.FormAttributes.CommandAttributes)
                {
                    if (commandAttribute.EntityCommandDTO != null)
                    {
                        var commandPermission = Permission.ChildsPermissions.FirstOrDefault(x => x.SecurityObjectID == commandAttribute.EntityCommandDTO.ID);
                        if (commandPermission != null)
                        {
                            if (commandPermission.GrantedActions.Any(x => x == MySecurity.SecurityAction.NoAccess ||
                                                   x == MySecurity.SecurityAction.Access))
                            {
                                if (commandPermission.GrantedActions.Any(x => x == MySecurity.SecurityAction.NoAccess))
                                    commandAttribute.SecurityNoAccess = true;
                                else
                                    commandAttribute.SecurityNoAccess = false;
                            }
                        }
                    }
                    else
                    {
                        if (commandAttribute.Command is I_DeleteCommand)
                        {
                            if (AreaInitializer.FormAttributes.SecurityDelete == true)
                                commandAttribute.SecurityNoAccess = false;
                            else
                                commandAttribute.SecurityNoAccess = true;
                        }
                        else if (commandAttribute.Command is I_SaveCommand)
                        {
                            if (AreaInitializer.FormAttributes.SecurityEdit == true)
                                commandAttribute.SecurityNoAccess = false;
                            else
                                commandAttribute.SecurityNoAccess = true;
                        }
                    }
                }

                foreach (var columnAtrributes in AreaInitializer.FormAttributes.ColumnAttributes)
                {

                    var columnPermission = Permission.ChildsPermissions.FirstOrDefault(x => x.SecurityObjectID == columnAtrributes.ColumnControl.Column.ID);
                    if (columnPermission != null)
                    {
                        if (columnPermission.GrantedActions.Any(x => x == MySecurity.SecurityAction.NoAccess ||
                         x == MySecurity.SecurityAction.ReadOnly || x == MySecurity.SecurityAction.Edit))
                        {
                            if (columnPermission.GrantedActions.Any(x => x == MySecurity.SecurityAction.NoAccess))
                                columnAtrributes.SecurityNoAccess = true;
                            else if (columnPermission.GrantedActions.Any(x => x == MySecurity.SecurityAction.ReadOnly))
                                columnAtrributes.SecurityReadOnly = true;
                            else
                            {
                                if (columnPermission.GrantedActions.Any(x => x == MySecurity.SecurityAction.Edit))
                                    columnAtrributes.SecurityEdit = true;
                            }
                        }
                        //else
                        //    columnAtrributes.SecurityNoAccess = true;



                    }
                    else
                    {

                        columnAtrributes.SecurityNoAccess = true;

                    }
                }
            }
            ImposeSecurity();
            //ImposeCommandSecurity();
        }

        //private void ImposeCommandSecurity()
        //{
        //    foreach (var item in AreaInitializer.FormAttributes.CommandAttributes)
        //    {
        //        if (item.SecurityNoAccess == true)
        //        {
        //            DisableEnableCommand(item, false);
        //        }
        //    }
        //}

        private void ImposeSecurity()
        {
            //CheckDefaultDataItem();

            foreach (var item in AreaInitializer.DataItemAttributes)
            {
                bool enableDisable = true;
                if (item.SecurityNoAccess == true)
                    enableDisable = false;
                else
                    enableDisable = !item.BusinessItemDisabled;
                DisableEnableDataItem(item, enableDisable);

                if (enableDisable)
                {
                    foreach (var col in item.ColumnAttributes)
                    {
                        bool enableDisableColumn = true;
                        if (col.SecurityNoAccess == true)
                            enableDisableColumn = false;
                        else
                            enableDisableColumn = !col.BusinessColumnDisabled;
                        DisableEnableDataItemColumn(item, col, enableDisableColumn);
                    }
                    bool readonlystate = false;
                    //منطق عوض شد.اگر به فرم ریداونلی داده باشده چه مستقیم و چه از پدرانش و به یکی از ستونها ادیت داده باشد باز هم فرم رید اونلیست
                    if (item.SecurityReadOnly == true)
                        readonlystate = true;
                    else
                        readonlystate = item.BusinessItemReadOnly;

                    //////if (item.ColumnAttributes.Where(x => x.CurrentDaisableEnableStare == false).All(x => x.SecurityReadOnly == true))
                    //////    readonlystate = true;
                    //////else
                    //////    readonlystate = item.BusinessItemReadOnly;

                    ReadonlyDataItem(item, readonlystate);
                    //چون ریدونلی کردن پرنت برعکس دیسیبل کردن ستونها را ریدونلی نمیکند به هر حال ریدونلی بودن ستونها باید چک شود
                    if (readonlystate)
                    {
                        foreach (var col in item.ColumnAttributes)
                        {
                            ReadonlyDataItemColumn(item, col, true);
                        }
                    }

                    if (!readonlystate)
                    {
                        foreach (var col in item.ColumnAttributes)
                        {

                            bool readonlyColumn = false;
                            if (col.SecurityReadOnly == true)
                                readonlyColumn = true;
                            else if (col.ColumnControl.IsPermanentReadOnly)
                                readonlyColumn = true;
                            else
                                readonlyColumn = col.BusinessColumnReadonly;
                            ReadonlyDataItemColumn(item, col, readonlyColumn);
                        }

                        foreach (var command in item.CommandAttributes)
                        {
                            if (command.SecurityNoAccess != null)
                                DisableEnableCommand(command, !command.SecurityNoAccess.Value);
                        }

                        //if (item.SecurityDelete != null)
                        //    EnableDisableDelete(item, item.SecurityDelete.Value);
                        //if (item.SecuritySaveNew != null)
                        //    EnableDisableSaveNew(item, item.SecuritySaveNew.Value);
                        //if (item.SecurityEdit != null)
                        //    EnableDisableUpdate(item, item.SecurityEdit.Value);
                    }

                }
            }


        }



        //private void CheckDefaultDataItem()
        //{
        //    if (AreaInitializer.DataMode == DataMode.One)
        //    {
        //        if (AreaInitializer.DataItemAttributes.Count == 0)
        //        {
        //            var newItem = new DataItemAttributes();
        //            newItem.DataItem = null;
        //            newItem.SecurityNoAccess = AreaInitializer.FormAttributes.SecurityNoAccess;
        //            newItem.BusinessItemDisabled = AreaInitializer.FormAttributes.BusinessNoAccess;
        //            newItem.SecurityReadOnly = AreaInitializer.FormAttributes.SecurityReadOnly;
        //            newItem.BusinessItemReadOnly = AreaInitializer.FormAttributes.BusinessReadOnly;
        //            newItem.SecurityDelete = AreaInitializer.FormAttributes.SecurityDelete;
        //            newItem.SecurityEdit = AreaInitializer.FormAttributes.SecurityEdit;
        //            newItem.ColumnAttributes = AreaInitializer.FormAttributes.ColumnAttributes;
        //            newItem.CommandAttributes = AreaInitializer.FormAttributes.CommandAttributes;
        //            AreaInitializer.DataItemAttributes.Add(newItem);
        //        }

        //    }
        //}

        private void DisableEnableDataItem(DataItemAttributes dataItemAttribute, bool enable)
        {
            //if (AreaInitializer.DataMode == DataMode.One)
            //{
            //    if (DataView != null)
            //    {
            //        //درست شود حالت غیر مستقیم و ...
            //        DataView.DisableEnable(enable);
            //        if (DisableEnableChanged != null)
            //            DisableEnableChanged(this, new DisableEnableChangedArg() { Enabled = enable });
            //        dataItemAttribute.CurrentDaisableEnableStare = !enable;
            //    }

            //}
            //else
            //{
            if (DataView is I_View_MultipleDataContainer)
            {
                //////AgentUICoreMediator.UIManager.DisableEnableMultiple((DataView as I_View_MultipleDataContainer), dataItemAttribute.DataItem, enable);
                dataItemAttribute.CurrentDaisableEnableStare = !enable;
            }
            //}


        }
        private void DisableEnableDataItemColumn(DataItemAttributes dataItemAttributes, ColumnAttributes columnAttribute, bool enable)
        {


            //if (AreaInitializer.DataMode == DataMode.One)
            //{
            //    if (DataView != null)
            //    {
            //        AgentUICoreMediator.UIManager.EnableDisableColumnControl(columnAttribute.ColumnControl.ControlPackage, columnAttribute.ColumnControl.Column, enable);
            //        columnAttribute.CurrentDaisableEnableStare = !enable;
            //    }
            //}
            //else
            //{
            if (DataView is I_View_MultipleDataContainer)
            {
                //////AgentUICoreMediator.UIManager.DisableEnableMultipleColumn(dataItemAttributes.DataItem, columnAttribute.ColumnControl as SimpleColumnControlMultipleData, enable);
                columnAttribute.CurrentDaisableEnableStare = !enable;
            }
            //}


        }
        private void ReadonlyDataItem(DataItemAttributes dataItemAttribute, bool readonlity)
        {
            //if (AreaInitializer.DataMode == DataMode.One)
            //{
            //    var saveCommandAttriburte = AreaInitializer.FormAttributes.CommandAttributes.FirstOrDefault(x => x.Command is I_SaveCommand);
            //    if (saveCommandAttriburte != null)
            //        DisableEnableCommand(saveCommandAttriburte, false);
            //    var deleteommandAttriburte = AreaInitializer.FormAttributes.CommandAttributes.FirstOrDefault(x => x.Command is I_DeleteCommand);
            //    if (deleteommandAttriburte != null)
            //        DisableEnableCommand(deleteommandAttriburte, false);
            //    //EnableDisableDelete(dataItemAttribute, false);
            //    //EnableDisableSaveNew(dataItemAttribute, false);
            //    //EnableDisableUpdate(dataItemAttribute, false);
            //    dataItemAttribute.CurrentReadonlyStare = readonlity;
            //}
            //else
            //{
            if (DataView is I_View_MultipleDataContainer)
            {
                //////AgentUICoreMediator.UIManager.SetReadonlyMultiple((DataView as I_View_MultipleDataContainer), dataItemAttribute.DataItem, readonlity);

                dataItemAttribute.CurrentReadonlyStare = readonlity;
            }
            //}
        }
        private void ReadonlyDataItemColumn(DataItemAttributes dataItemAttribute, ColumnAttributes columnAttribute, bool readonlity)
        {


            //if (AreaInitializer.DataMode == DataMode.One)
            //{
            //    if (DataView != null)
            //    {
            //        AgentUICoreMediator.UIManager.SetReadonly(columnAttribute.ColumnControl.ControlPackage, columnAttribute.ColumnControl.Column, readonlity);
            //        columnAttribute.CurrentReadonlyStare = readonlity;
            //    }
            //}
            //else
            //{
            if (DataView is I_View_MultipleDataContainer)
            {
                //////AgentUICoreMediator.UIManager.SetReadonlyMultipleColumn(dataItemAttribute.DataItem, columnAttribute.ColumnControl, readonlity);
                columnAttribute.CurrentReadonlyStare = readonlity;
            }
            //}

        }

        //void EnableDisableDelete(DataItemAttributes dataItemAttribute, bool enable)
        //{
        //    if (AreaInitializer.DataMode == DataMode.One)
        //    {
        //        var deleteCommand = AreaInitializer.FormAttributes.CommandAttributes.FirstOrDefault(x => x.Command is I_DeleteCommand);
        //        if (deleteCommand != null)
        //            DisableEnableCommand(deleteCommand, enable);
        //    }
        //    else
        //    {
        //        //موقع خود کلیک چک شود
        //    }
        //}


        //private void EnableDisableUpdate(DataItemAttributes dataItemAttribute, bool enable)
        //{
        //    if (AreaInitializer.DataMode == DataMode.One)
        //    {
        //        if (dataItemAttribute.DataItem != null && !dataItemAttribute.DataItem.IsNewItem)
        //        {
        //            var saveCommand = AreaInitializer.FormAttributes.CommandAttributes.FirstOrDefault(x => x.Command is I_SaveCommand);
        //            if (saveCommand != null)
        //                DisableEnableCommand(saveCommand, enable);
        //        }
        //    }
        //    else
        //    {
        //        //موقع خود کلیک چک شود
        //    }
        //}

        //private void EnableDisableSaveNew(DataItemAttributes dataItemAttribute, bool enable)
        //{
        //    if (AreaInitializer.DataMode == DataMode.One)
        //    {
        //        if (dataItemAttribute.DataItem == null || dataItemAttribute.DataItem.IsNewItem)
        //        {
        //            var saveCommand = AreaInitializer.FormAttributes.CommandAttributes.FirstOrDefault(x => x.Command is I_SaveCommand);
        //            if (saveCommand != null)
        //                DisableEnableCommand(saveCommand, enable);
        //        }
        //    }
        //    else
        //    {
        //        //موقع خود کلیک چک شود
        //    }
        //}


        private void ManageDataSecurity()
        {
            //if (AreaInitializer.DataMode == DataMode.One && AreaInitializer.Data.Count == 1)
            //{
            //    if (AreaInitializer.DataItemAttributes.Count != null && AreaInitializer.DataItemAttributes[0].DataItem == null)
            //        AreaInitializer.DataItemAttributes[0].DataItem = AreaInitializer.Data[0];
            //}
            var listRemove = AreaInitializer.DataItemAttributes.Where(x => !AreaInitializer.Data.Contains(x.DataItem)).ToList();
            foreach (var item in listRemove)
                AreaInitializer.DataItemAttributes.Remove(item);

            foreach (var data in AreaInitializer.Data)
            {
                var dataAttribue = AreaInitializer.DataItemAttributes.FirstOrDefault(x => x.DataItem == data);
                if (dataAttribue == null)
                {
                    dataAttribue = new DataItemAttributes();
                    dataAttribue.DataItem = data;
                    dataAttribue.SecurityNoAccess = AreaInitializer.FormAttributes.SecurityNoAccess;
                    dataAttribue.BusinessItemDisabled = AreaInitializer.FormAttributes.BusinessNoAccess;
                    dataAttribue.SecurityReadOnly = AreaInitializer.FormAttributes.SecurityReadOnly;
                    dataAttribue.BusinessItemReadOnly = AreaInitializer.FormAttributes.BusinessReadOnly;
                    dataAttribue.SecurityDelete = AreaInitializer.FormAttributes.SecurityDelete;
                    dataAttribue.SecurityEdit = AreaInitializer.FormAttributes.SecurityEdit;
                    dataAttribue.ColumnAttributes = AreaInitializer.FormAttributes.ColumnAttributes;
                    dataAttribue.CommandAttributes = AreaInitializer.FormAttributes.CommandAttributes;
                    AreaInitializer.DataItemAttributes.Add(dataAttribue);
                }
                var condition = false;
                bool? noAccess = null;
                bool? readonlyAccess = null;
                bool? editAccess = null;
                bool? deleteAccess = null;
                List<ColumnAttributes> columnAttributes = new List<ColumnAttributes>();
                List<CommandAttributes> commandAttributes = new List<CommandAttributes>();
                foreach (var conditionalPermission in ConditionalPermission)
                {//قبلش چک شود که ایا اصلا سکوریتی باید برای یوزر اعمال شود؟
                    bool internalCondition = false;
                    bool hasRoleCondition = AgentUICoreMediator.UserHasRoleCondition(conditionalPermission.SecuritySubject, conditionalPermission.HasNotRole);
                    if (hasRoleCondition)
                    {

                        if (conditionalPermission.ConditinColumnID != 0)
                        {
                            var dataColumn = data.DataInstance.Properties.FirstOrDefault(x => x.ColumnID == conditionalPermission.ConditinColumnID);
                            if (dataColumn != null)
                                if (dataColumn.Value == conditionalPermission.Value)
                                    internalCondition = true;
                        }
                        else if (conditionalPermission.FormulaID != 0)
                        {
                            var value = formulaHelper.CalculateFormula(conditionalPermission.Formula, data).ToString();
                            if (value == conditionalPermission.Value)
                            {
                                internalCondition = true;
                            }
                        }
                        if (internalCondition == true)
                        {
                            condition = true;
                            if (conditionalPermission.SecurityObject.Type == DatabaseObjectCategory.Column)
                            {

                                var columnAttribute = columnAttributes.FirstOrDefault(x => x.ColumnID == conditionalPermission.SecurityObject.ID);

                                if (columnAttribute == null)
                                {
                                    columnAttribute = new ColumnAttributes();
                                    columnAttribute.ColumnID = conditionalPermission.SecurityObject.ID;
                                    columnAttributes.Add(columnAttribute);
                                }
                                if (conditionalPermission.Actions.Any(x => x == MySecurity.SecurityAction.NoAccess))
                                    columnAttribute.SecurityNoAccess = true;
                                else if (conditionalPermission.Actions.Any(x => x == MySecurity.SecurityAction.ReadOnly))
                                    columnAttribute.SecurityReadOnly = true;
                                else if (conditionalPermission.Actions.Any(x => x == MySecurity.SecurityAction.Edit))
                                    columnAttribute.SecurityEdit = true;


                            }
                            else if (conditionalPermission.SecurityObject.Type == DatabaseObjectCategory.Command)
                            {
                                var commandAttribute = commandAttributes.FirstOrDefault(x => x.EntityCommandDTO != null && x.EntityCommandDTO.ID == conditionalPermission.SecurityObject.ID);
                                if (commandAttribute == null)
                                {
                                    commandAttribute = new CommandAttributes();
                                    commandAttribute.EntityCommandDTO = new EntityCommandDTO();
                                    commandAttribute.EntityCommandDTO.ID = conditionalPermission.SecurityObject.ID;
                                    commandAttributes.Add(commandAttribute);
                                }
                                if (conditionalPermission.Actions.Any(x => x == MySecurity.SecurityAction.NoAccess))
                                    commandAttribute.SecurityNoAccess = true;
                                else
                                    commandAttribute.SecurityNoAccess = false;
                            }
                            else if (conditionalPermission.SecurityObject.Type == DatabaseObjectCategory.Entity)
                            {

                                if (conditionalPermission.Actions.Any(x => x == MySecurity.SecurityAction.NoAccess ||
                                x == MySecurity.SecurityAction.Edit || x == MySecurity.SecurityAction.ReadOnly ||
                                x == MySecurity.SecurityAction.Delete))
                                {
                                    if (conditionalPermission.Actions.Any(x => x == MySecurity.SecurityAction.NoAccess))
                                        noAccess = true;
                                    else if (conditionalPermission.Actions.Any(x => x == MySecurity.SecurityAction.ReadOnly))
                                        readonlyAccess = true;
                                    else
                                    {
                                        if (conditionalPermission.Actions.Any(x => x == MySecurity.SecurityAction.Edit))
                                            editAccess = true;

                                        if (conditionalPermission.Actions.Any(x => x == MySecurity.SecurityAction.Delete))
                                            deleteAccess = true;
                                    }
                                }

                            }
                        }

                    }
                }
                if (condition == true)
                {
                    if (noAccess == true || readonlyAccess == true)
                    {
                        if (noAccess != null)
                            dataAttribue.SecurityNoAccess = true;
                        else if (readonlyAccess != null)
                            dataAttribue.SecurityReadOnly = true;

                        var dataSaveCommnad = dataAttribue.CommandAttributes.FirstOrDefault(x => x.Command is I_SaveCommand);
                        if (dataSaveCommnad != null)
                            dataSaveCommnad.SecurityNoAccess = true;

                        var dataDeleteCommnad = dataAttribue.CommandAttributes.FirstOrDefault(x => x.Command is I_DeleteCommand);
                        if (dataDeleteCommnad != null)
                            dataDeleteCommnad.SecurityNoAccess = true;
                    }
                    else
                    {//اگر دیتا آیتم جدید بود چی؟
                        if (editAccess != null)
                        {
                            dataAttribue.SecurityEdit = true;
                            var dataSaveCommnad = dataAttribue.CommandAttributes.FirstOrDefault(x => x.Command is I_SaveCommand);
                            if (dataSaveCommnad != null)
                                dataSaveCommnad.SecurityNoAccess = false;
                        }
                        if (deleteAccess != null)
                        {
                            dataAttribue.SecurityDelete = true;
                            var dataDeleteCommnad = dataAttribue.CommandAttributes.FirstOrDefault(x => x.Command is I_SaveCommand);
                            if (dataDeleteCommnad != null)
                                dataDeleteCommnad.SecurityNoAccess = false;
                        }
                    }
                    foreach (var columnAttribute in columnAttributes)
                    {
                        var dataColumn = dataAttribue.ColumnAttributes.FirstOrDefault(x => x.ColumnID == columnAttribute.ColumnID);
                        if (dataColumn != null)
                        {
                            dataColumn.SecurityNoAccess = columnAttribute.SecurityNoAccess;
                            dataColumn.SecurityReadOnly = columnAttribute.SecurityReadOnly;
                            dataColumn.SecurityEdit = columnAttribute.SecurityEdit;
                        }
                    }
                    foreach (var commandAttribute in commandAttributes)
                    {
                        var dataCommand = dataAttribue.CommandAttributes.FirstOrDefault(x => x.EntityCommandDTO.ID == commandAttribute.EntityCommandDTO.ID);
                        if (dataCommand != null)
                        {
                            dataCommand.SecurityNoAccess = commandAttribute.SecurityNoAccess;
                        }
                    }


                }
            }
            ImposeSecurity();
        }



        private void DisableEnableCommand(CommandAttributes commandAttributes, bool enabled)
        {
            commandAttributes.Command.CommandManager.SetEnabled(enabled);
        }
        //private void DisableEnableCommandByType(Type type, bool enabled)
        //{
        //    var commands = Commands.Where(x => x.GetType().GetInterfaces().Any(y => y == type));
        //    foreach (var command in commands)
        //    {
        //        if (command.Enabled != enabled)
        //        {
        //            command.Enabled = enabled;
        //        }

        //    }
        //    if (DisableEnableCommandByTypeChanged != null)
        //        DisableEnableCommandByTypeChanged(this, new DisableEnableCommandByTypeChangedArg() { Enabled = enabled, Type = type });

        //}

        private void CheckEntityStates()
        {
            if (States != null)
            {
                foreach (var state in States)
                {
                    if (state.ColumnID != 0)
                    {
                        var columnControl = SimpleColumnControls.FirstOrDefault(x => x.Column.ID == state.ColumnID);
                        if (columnControl != null)
                        {
                            columnControl.ControlPackage.ValueChanged += (sender, e) => ControlPackage_ValueChangedForState(sender, this, columnControl, e, state);
                        }
                    }
                    else if (state.FormulaID != 0)
                    {
                        //////var columns = AgentHelper.GetFormulaColumnsList(this, state.Formula.FormulaItems);

                        //////foreach (var columnControl in columns)
                        //////    columnControl.Item2.ControlPackage.ValueChanged += (sender, e) => ControlPackage_ValueChangedForState(sender, columnControl.Item1, columnControl.Item2, e, state);

                    }
                }
            }
        }
        List<int> NullDataItemStateIds = new List<int>();
        private void ControlPackage_ValueChangedForState(object sender, I_EditEntityArea editEntityArea, SimpleColumnControlMultipleData columnControl, ColumnValueChangeArg e, EntityStateDTO state)
        {
            bool stateIsValid = false;
            DP_DataRepository dataItem = null;
            if (editEntityArea.AreaInitializer.DataMode == DataMode.One)
            {
                //این روش اپدیت کلی ایراد دارد
                editEntityArea.UpdateData();
                dataItem = AreaInitializer.Data.First();
            }
            else
            {
                dataItem = e.DataItem as DP_DataRepository;
            }

            if (state.ColumnID != 0)
            {
                if (e.NewValue == state.Value)
                {
                    stateIsValid = true;
                }
            }
            else if (state.FormulaID != 0)
            {
                var value = formulaHelper.CalculateFormula(state.Formula, dataItem).ToString();
                if (value == state.Value)
                {
                    stateIsValid = true;
                }
            }


            if (stateIsValid)
            {
                if (!dataItem.StateIds.Any(x => x == state.ID))
                {

                    dataItem.StateIds.Add(state.ID);

                }
                ApplyState(state);
            }
            else
                dataItem.StateIds.Remove(state.ID);

        }
        //private void EntityCurrentStates_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        //{
        //    if (e.NewItems != null)
        //        if (e.NewItems.Count > 0)
        //        {
        //            var addedState = e.NewItems[0] as EntityStateDTO;

        //        }
        //}

        private void ApplyState(EntityStateDTO state)
        {
            if (state.ActionActivities != null && state.ActionActivities.Count != 0)
            {
                //if (state.ActionActivity.)
                foreach (var actionActivity in state.ActionActivities)
                    DoActionActivities(actionActivity, AreaInitializer.Data);
            }
        }

        void DataView_CommandExecuted(object sender, Arg_CommandExecuted e)
        {
            (e.Command as I_EntityAreaCommand).Execute(this);
        }
        // صدا زده میشود GenerateDataView یکبار در

        //باشد SerachViewArea == null  کلیک میشود اگر Search زمانی که لینک ShowTemporarySearchView یکبار هم در

        //private void ManageSearchView()
        //{
        //    throw new NotImplementedException();
        //}




        //void DataView_DataControlGenerated(object sender, Arg_DataDependentControlGeneration e)
        //{
        //    if (sender is IAG_DataDependentControl)
        //    {
        //        var typePropertyControl = ColumnControls.Where(x => x.ControlPackage.DataDependentControl == sender).FirstOrDefault();
        //        if (typePropertyControl != null)
        //        {
        //            if (typePropertyControl is RelationSourceControl)
        //            { e.ControlPackage = DataView.GenerateControl(typePropertyControl.Column); }

        //            else
        //                //if (typePropertyControl.ControlPackage.DataDependentControl)
        //                e.ControlPackage = DataView.GenerateControl(typePropertyControl.Column);
        //            //////DataView.ShowControlValue(e.ControlPackage, typePropertyControl, typePropertyControl.Column.Value);
        //        }
        //        //else
        //        //    return false;
        //    }
        //}

        //  List<SinglePropertyRelation> revisedSinglePropertyRelations = new List<SinglePropertyRelation>();
        private void ManageDataView()
        {

            //var manyToOneRelationships =FullEntity.ManyToOneRelationships;
            //var ontToManyRelationships = AgentHelper.GetOneToManyRelationships(AreaInitializer);
            //var explicitOneToOneRelationships = AgentHelper.GetExplicitOntToOneRelationships(AreaInitializer);
            //var implicitOneToOneRelationships = AgentHelper.GetImplicitOntToOneRelationships(AreaInitializer);
            //var superToSubRelationships = AgentHelper.GetSuperToSubRelationshipRelationships(AreaInitializer);
            //var subToSuperRelationships = AgentHelper.GetSubToSuperRelationshipRelationships(AreaInitializer);
            //var unionToSubUnion_UnionHoldsKeysRelationships = AgentHelper.GetUnionToSubUnion_UnionHoldsKeysRelationships(AreaInitializer).Where(x=>x.UnionHoldsKeys);
            //var unionToSubUnion_SubUnionHoldsKeysRelationships = AgentHelper.GetUnionToSubUnion_SubUnionHoldsKeysRelationships(AreaInitializer).Where(x => !x.UnionHoldsKeys);
            //var subUnionToUnion_UnionHoldsKeysRelationships = AgentHelper.GetSubUnionToUnion_UnionHoldsKeysRelationships(AreaInitializer).Where(x => x.UnionHoldsKeys);
            //var subUnionToUnion_SubUnionHoldsKeysRelationships = AgentHelper.GetSubUnionToUnion_SubUnionHoldsKeysRelationships(AreaInitializer).Where(x => !x.UnionHoldsKeys);


            //List<RelationSourceControl> postPonedRelationSourceControls = new List<RelationSourceControl>();
            //
            //foreach (var column in AgentHelper.GetColumnList(AreaInitializer).OrderBy(x => x.Position))
            //{
            //    if (
            //    !manyToOneRelationships.Any(x => x.SourceRelationProperty.Any(y => y.ID == column.ID))
            //    && !explicitOneToOneRelationships.Any(x => x.SourceRelationProperty.Any(y => y.ID == column.ID))
            //    && !subToSuperRelationships.Any(x => x.SourceRelationProperty.Any(y => y.ID == column.ID))
            //    && !subUnionToUnion_SubUnionHoldsKeysRelationships.Any(x => x.SourceRelationProperty.Any(y => y.ID == column.ID))
            //    && !unionToSubUnion_UnionHoldsKeysRelationships.Any(x => x.SourceRelationProperty.Any(y => y.ID == column.ID))
            //    )
            //    {
            //    }
            //}
            //var columns = AgentHelper.GetColumnList(AreaInitializer).OrderBy(x => x.Position);
            //foreach (var UIItem in UICompositions.OrderBy(x => x.Position))
            //{
            //    if(UIItem.ObjectCategory=="")

            //}


            //List<RelationSourceControl> GeneratedItems = new List<RelationSourceControl>();
            List<BaseColumnControl> sortedListOfColumnControls = new List<EntityArea.BaseColumnControl>();
            foreach (var column in FullEntity.Columns.OrderBy(x => x.Position).Where(x => x.DataEntryView == true))
            {
                //چرا DataEntryView؟ اگر رابطه روش بود چی؟


                if (FullEntity.Relationships.Any(x => x.MastertTypeEnum == Enum_MasterRelationshipType.FromForeignToPrimary && x.RelationshipColumns.Any(y => y.FirstSideColumnID == column.ID)))
                {
                    var rel = FullEntity.Relationships.Where(x => x.MastertTypeEnum == Enum_MasterRelationshipType.FromForeignToPrimary && x.RelationshipColumns.Any(y => y.FirstSideColumnID == column.ID)).ToList();
                    if (!RelationshipColumnControls.Any(x => x.Columns.Any(y => y.ID == column.ID)))
                    {//فعال و غیر فعال بودن و ... چک شود
                        var propertyControl = new RelationshipColumnControlMultipleData();
                        var relationship = FullEntity.Relationships.First(x => x.RelationshipColumns.Any(y => y.FirstSideColumnID == column.ID));
                        propertyControl.Relationship = relationship;
                        foreach (var relcolumn in relationship.RelationshipColumns)
                        {
                            propertyControl.Columns.Add(relcolumn.FirstSideColumn);
                        }
                        var relatedTuple = GenerateEditEntityAreaWithUIControlPackage(relationship);
                        if (relatedTuple != null)
                        {
                            propertyControl.EditNdTypeArea = relatedTuple.Item1;
                            propertyControl.ControlPackage = relatedTuple.Item2;

                            AgentHelper.SetPropertyTitle(propertyControl);
                            RelationshipColumnControls.Add(propertyControl);
                            sortedListOfColumnControls.Add(propertyControl);
                        }

                    }
                }
                else
                {
                    var propertyControl = new SimpleColumnControlMultipleData() { Column = column };
                    if (column.IsIdentity == true)
                        propertyControl.IsPermanentReadOnly = true;
                    if (column.DataEntryEnabled == false)
                        propertyControl.IsPermanentReadOnly = true;
                    if (AreaInitializer.SourceRelation != null)
                    {
                        if (AreaInitializer.SourceRelation.RelationshipColumns.Any(x => x.SecondSideColumnID == column.ID))
                        {
                            if (AreaInitializer.SourceRelation.MasterRelationshipType == Enum_MasterRelationshipType.FromForeignToPrimary)
                                propertyControl.IsPermanentReadOnly = true;
                        }
                    }

                    propertyControl.ControlPackage = new UIControlPackageMultipleDataSimpleColumn();
                    propertyControl.ControlPackage.ControlManager = DataView.GenerateControlManager(column, GetColumnUISetting(column));
                    if (propertyControl.IsPermanentReadOnly)
                        propertyControl.ControlPackage.ControlManager.SetReadonly(propertyControl.IsPermanentReadOnly);

                    //else
                    //{
                    //    propertyControl.ControlPackage = new UIControlPackageMultipleData();
                    //    propertyControl.ControlPackage.ControlManager= AgentUICoreMediator.UIManager.GenerateControlManager(column, GetColumnUISetting(column.ID));
                    //    if (propertyControl.IsPermanentReadOnly)
                    //        AgentUICoreMediator.UIManager.SetReadonly(propertyControl.ControlPackage, propertyControl.Column, propertyControl.IsPermanentReadOnly);
                    //}
                    AgentHelper.SetPropertyTitle(propertyControl);
                    SimpleColumnControls.Add(propertyControl);
                    sortedListOfColumnControls.Add(propertyControl);
                }
            }
            //if (AreaInitializer.SourceRelation != null)
            //{
            //    if (AreaInitializer.SourceRelation.RelationshipType == Enum_RelationshipType.SubToSuper
            //        || AreaInitializer.SourceRelation.RelationshipType == Enum_RelationshipType.SuperToSub)
            //    {
            //        //////if (!AreaInitializer.SourceRelation.TargetRelationColumns.Select(x => x.ID).Contains(column.ID))
            //        //////    if (AreaInitializer.SourceRelation.SourceEditArea.FullEntity.Columns.Any(x => x.ID == column.ID))
            //        //////        continue;
            //    }
            //}

            foreach (var relationship in FullEntity.Relationships
                            .Where(x => x.MastertTypeEnum == Enum_MasterRelationshipType.FromPrimartyToForeign))
            {
                var propertyControl = new RelationshipColumnControlMultipleData();
                propertyControl.Relationship = relationship;
                foreach (var relcolumn in relationship.RelationshipColumns)
                {
                    propertyControl.Columns.Add(relcolumn.FirstSideColumn);
                }
                var relatedTuple = GenerateEditEntityAreaWithUIControlPackage(relationship);
                if (relatedTuple != null)
                {
                    propertyControl.EditNdTypeArea = relatedTuple.Item1;
                    propertyControl.ControlPackage = relatedTuple.Item2;
                    AgentHelper.SetPropertyTitle(propertyControl);
                    RelationshipColumnControls.Add(propertyControl);
                    sortedListOfColumnControls.Add(propertyControl);
                }

            }

            //List<ColumnControl> removeList = new List<ColumnControl>();
            //foreach (var propertyControl in ColumnControls.Where(x => x.Relationship != null))
            //{
            //    var relatedTuple = GenerateEditEntityAreaWithUIControlPackage(propertyControl.Relationship, propertyControl.Column);
            //    if (relatedTuple != null)
            //    {
            //        propertyControl.EditNdTypeArea = relatedTuple.Item1;
            //        propertyControl.ControlPackage = relatedTuple.Item2;
            //    }
            //    else
            //        removeList.Add(propertyControl);
            //}
            //removeList.ForEach(x => ColumnControls.Remove(x));


            //uiControlPackageTree = new List<UIControlPackageTree>();
            //if (UICompositions != null)
            //    foreach (var uiCompositionItem in UICompositions.OrderBy(x => x.Position))
            //    {
            //        //if (uiCompositionItem.ObjectCategory == "Relationship")
            //        //{
            //        //    var columnControl = ColumnControls.FirstOrDefault(x => x is RelationSourceControl && (x as RelationSourceControl).EditNdTypeArea.AreaInitializer.SourceRelation.Relationship.ID == Convert.ToInt32(uiCompositionItem.ObjectIdentity));
            //        //    if (columnControl != null)
            //        //    {
            //        //        if (columnControl.ControlPackage.UIControl.Control is I_View_Container)
            //        //        {
            //        //            EntityUICompositionDTO nitem = new EntityUICompositionDTO();
            //        //            nitem.ObjectCategory = "Group";
            //        //            nitem.Title = AgentHelper.GetPropertyTitle(columnControl);
            //        //            nitem.ChildItems.Add(uiCompositionItem);
            //        //            GenerateUIComposition(null, nitem, uiControlPackageTree);


            //        //        }
            //        //    }
            //        //}
            //        //else
            //        //////GenerateUIComposition(null, uiCompositionItem);
            //    }
            if (UICompositions != null && UICompositions.TreeItems != null)
                GenerateUIComposition(UICompositions.TreeItems);

            foreach (var columnControl in sortedListOfColumnControls.Where(x => x.Visited == false))
            {
                if (columnControl is SimpleColumnControlMultipleData)
                {
                    columnControl.Visited = true;
                    DataView.AddUIControlPackage((columnControl as SimpleColumnControlMultipleData).ControlPackage.ControlManager, columnControl.Alias);
                }
                else if (columnControl is RelationshipColumnControlMultipleData)
                {
                    columnControl.Visited = true;
                    var relationshipControl = (columnControl as RelationshipColumnControlMultipleData);
                    DataView.AddView(relationshipControl.ControlPackage.View, relationshipControl.Alias);
                }
            }



            foreach (var columnControl in SimpleColumnControls)
            {
                if (columnControl.Column.CustomFormula != null)
                {
                    //////ColumnFormula columnFormula = new ColumnFormula();
                    //////columnFormula.ResultColumnControl = columnControl;
                    //////columnFormula.Formula = columnControl.Column.CustomFormula;
                    //////ColumnFormulas.Add(columnFormula);

                    //////var cpMenuFormula = new ConrolPackageMenu();
                    //////cpMenuFormula.Name = "mnuFormula";
                    //////cpMenuFormula.Title = "مدیریت فرمول";
                    //////AgentUICoreMediator.UIManager.GenerateMenuForControlPackage(columnControl.ControlPackage, cpMenuFormula);
                    //////cpMenuFormula.MenuClicked += cpMenuFormula_MenuClicked;


                    //////var cpMenuFormulaCalculation = new ConrolPackageMenu();
                    //////cpMenuFormulaCalculation.Name = "mnuFormulaCalculation";
                    //////cpMenuFormulaCalculation.Title = "محاسبه فرمول";
                    //////AgentUICoreMediator.UIManager.GenerateMenuForControlPackage(columnControl.ControlPackage, cpMenuFormulaCalculation);
                    //////cpMenuFormulaCalculation.MenuClicked += (sender, e) => CpMenuFormulaCalculation_MenuClicked(sender, e, columnControl);

                }
            }
            //////foreach (var columnControl in ColumnControls)
            //////{
            //////    if (columnControl.ControlPackage != null)
            //////        columnControl.ControlPackage.ValueChanged += (sender, e) => propertyControl_ValueChanged(sender, e, columnControl);

            //////    if (AreaInitializer.DataMode == DataMode.Multiple)
            //////        DataView.AddMultipleDataDependentControl(columnControl.ControlPackage as DataDependentControlPackage, AgentHelper.GetPropertyTitle(columnControl), AgentHelper.GetPropertyColor(columnControl), AgentHelper.GetPropertyTooltip(columnControl));
            //////    else
            //////        DataView.AddControls(columnControl.ControlPackage, AgentHelper.GetPropertyTitle(columnControl), AgentHelper.GetPropertyColor(columnControl), AgentHelper.GetPropertyTooltip(columnControl));
            //////}
            foreach (var columnControl in SimpleColumnControls)
            {
                ColumnAttributes columnAtrributes = new ColumnAttributes();
                columnAtrributes.ColumnID = columnControl.Column.ID;
                columnAtrributes.ColumnControl = columnControl;
                AreaInitializer.FormAttributes.ColumnAttributes.Add(columnAtrributes);
            }
            AreaInitializer.FormComposed = true;
        }
        private EntityUISettingDTO GetEntityUISetting()
        {
            if (UICompositions != null && UICompositions.TreeItems != null && UICompositions.TreeItems.Count > 0 && UICompositions.TreeItems.First().EntityUISetting != null)
            {
                var entityUISetting = UICompositions.TreeItems.First().EntityUISetting;
                return entityUISetting;
            }
            else
            {
                var setting = new EntityUISettingDTO();
                setting.UIColumnsCount = 4;
                return setting;
            }
        }
        private ColumnUISettingDTO GetColumnUISetting(ColumnDTO column)
        {
            if (UICompositions != null && UICompositions.ColumnItems != null
                && UICompositions.ColumnItems.Any(x => x.ColumnID == column.ID))
            {
                var setting = UICompositions.ColumnItems.First(x => x.ColumnID == column.ID);
                if (setting == null)
                {
                    setting = new ColumnUISettingDTO();
                    setting.UIColumnsType = Enum_UIColumnsType.Normal;
                    setting.UIRowsCount = 1;
                    setting.ColumnID = column.ID;
                    UICompositions.ColumnItems.Add(setting);
                }
                return setting;
            }
            return null;
        }

        private RelationshipUISettingDTO GetRelationshipUISetting(RelationshipDTO relationship)
        {
            if (UICompositions != null && UICompositions.RelationshipItems != null
            && UICompositions.RelationshipItems.Any(x => x.RelationshipID == relationship.ID))
            {
                var setting = UICompositions.RelationshipItems.First(x => x.RelationshipID == relationship.ID);
                if (setting == null)
                {
                    setting = new RelationshipUISettingDTO();
                    setting.UIColumnsType = Enum_UIColumnsType.Normal;
                    UICompositions.RelationshipItems.Add(setting);
                }
                return setting;
            }
            return null;
        }

        private void CpMenuFormulaCalculation_MenuClicked(object sender, ConrolPackageMenuArg e, SimpleColumnControlMultipleData columnControl)
        {
            //if (AreaInitializer.DataMode == DataMode.One)
            //{//این روش ایراد دارد
            //    UpdateData();
            //    formulaHelper.CalculateFormula(columnControl, AreaInitializer.Data.First());
            //}
            //else
            //{
            formulaHelper.CalculateFormula(columnControl, e.data as DP_DataRepository);
            //}

        }

        private void cpMenuFormula_MenuClicked(object sender, ConrolPackageMenuArg e)
        {

        }



        public void GenerateUIComposition(List<EntityUICompositionDTO> UICompositions)
        {

            foreach (var uiCompositionItem in UICompositions.OrderBy(x => x.Position))
            {
                if (uiCompositionItem.ObjectCategory == DatabaseObjectCategory.Entity)
                {
                    GenerateUIComposition(uiCompositionItem.ChildItems);
                }
                else if (uiCompositionItem.ObjectCategory == DatabaseObjectCategory.Group)
                {
                    GenerateUIComposition(uiCompositionItem.ChildItems);
                }
                else if (uiCompositionItem.ObjectCategory == DatabaseObjectCategory.TabControl)
                {
                    GenerateUIComposition(uiCompositionItem.ChildItems);
                }
                else if (uiCompositionItem.ObjectCategory == DatabaseObjectCategory.TabPage)
                {
                    GenerateUIComposition(uiCompositionItem.ChildItems);
                }
                else if (uiCompositionItem.ObjectCategory == DatabaseObjectCategory.Column)
                {
                    var columnControl = SimpleColumnControls.FirstOrDefault(x => x.Column.ID == Convert.ToInt32(uiCompositionItem.ObjectIdentity));
                    if (columnControl != null)
                    {
                        DataView.AddUIControlPackage(columnControl.ControlPackage.ControlManager, columnControl.Alias);
                        columnControl.Visited = true;
                    }
                }
                else if (uiCompositionItem.ObjectCategory == DatabaseObjectCategory.Relationship)
                {
                    var columnControl = RelationshipColumnControls.FirstOrDefault(x => x.Relationship != null && x.Relationship.ID == Convert.ToInt32(uiCompositionItem.ObjectIdentity));
                    if (columnControl != null)
                    {
                        DataView.AddView(columnControl.ControlPackage.View, columnControl.Alias);
                        columnControl.Visited = true;
                    }
                }
                //حالت تب اضافه شود
                //uiControlPackageList.Add(item);
                //if (uiCompositionItem.ObjectCategory == DatabaseObjectCategory.Group)
                //{

                //}

            }




        }
        void propertyControl_ValueChanged(object sender, ColumnValueChangeArg e, SimpleColumnControlMultipleData columnControl)
        {
            //AgentUICoreMediator.UIManager.ShowInfo(e.NewValue, "", Temp.InfoColor.Blue);
        }



        //////private bool EditAreaIsDrivedFromTypePropertyRelation(ColumnDTO correspondingTypeProperty)
        //////{
        //////    var isDrived = false;
        //////    if (AreaInitializer.SourceRelation != null)
        //////    {
        //////        if (AreaInitializer.SourceRelation.SourceRelationSide == DataManager.DataPackage.Enum_DP_RelationSide.FirstSide)
        //////            if (AreaInitializer.SourceRelation.Relationship.Condition.SecondOperand.ID == correspondingTypeProperty.ID)
        //////                isDrived = true;

        //////        if (AreaInitializer.SourceRelation.SourceRelationSide == DataManager.DataPackage.Enum_DP_RelationSide.SecondSide)
        //////            if (AreaInitializer.SourceRelation.Relationship.Condition.FirstOperand.ID == correspondingTypeProperty.ID)
        //////                isDrived = true;
        //////    }
        //////    //if (AgentHelper.TypePropertyIsKeyOf(correspondingTypeProperty, AreaInitializer.Template))
        //////    //    checkPropertyRelation = false;

        //////    return isDrived;
        //////}
        //private bool TypePropertyIsKeyOfEditArea(ColumnDTO correspondingTypeProperty)
        //{

        //    return AgentHelper.TypePropertyIsKeyOf(correspondingTypeProperty, AreaInitializer.TemplateEntity);

        //}
        //private UIControlPackageMultipleData GenerateViewEditNDTypeAreaControl(ColumnDTO correspondingTypeProperty, AG_View_EditNDTypeArea view)
        //{
        //    UIControl ag_UIControl = new UIControl();
        //    ag_UIControl.UIControlSetting = new UIControlSetting();
        //    ag_UIControl.UIControlSetting.DesieredColumns = 10;
        //    ag_UIControl.UIControlSetting.DesieredRows = 1;

        //    if (TemporaryDisplayView != null)
        //    {
        //        ag_UIControl.Control = TemporaryDisplayView;
        //        return DataView.GenerateControl(ag_UIControl);
        //    }
        //    else
        //    {
        //        ag_UIControl.Control = DataView;
        //        return DataView.GenerateControl(ag_UIControl);
        //    }

        //}


        //public ColumnControl GerRelationSourceColumnControl(ColumnDTO targetColumn)
        //{
        //    foreach (var column in ColumnControls)
        //    {
        //        if (column.Column == targetColumn)
        //            //if (column is RelationSourceControl)
        //            //{
        //            return (column);
        //        //}
        //    }
        //    return null;
        //}
        //مهم
        private Tuple<I_EditEntityArea, UIControlPackageMultipleRelationshipColumn> GenerateEditEntityAreaWithUIControlPackage(RelationshipDTO relation)
        {
            if (RelationIsRedundant(AreaInitializer, relation))
                return null;
            if (!RelationshipIsValid(AreaInitializer, relation))
                return null;



            var newAreaInitializer = GenereateAreaInitializer(relation);
            if (newAreaInitializer != null)
            {
                ///////////

                var editArea = EditEntityAreaConstructor.GetEditEntityArea(newAreaInitializer);
                //ساخته میشود LoadTemplate در داخل View
                editArea.SetAreaInitializer(newAreaInitializer);
                //editArea.GenerateDataViewSearchView();
                //ساخته میشود EditEntityArea نیز در داخل خود ControlPackage
                UIControlPackageMultipleRelationshipColumn uiPackage = null;
                if (editArea.TemporaryDisplayView != null)
                {
                    uiPackage = new UIControlPackageMultipleRelationshipColumn();
                    uiPackage.View = DataView.GenerateRelationshipControlManager(editArea.TemporaryDisplayView, GetRelationshipUISetting(relation));
                }

                return new Tuple<I_EditEntityArea, UIControlPackageMultipleRelationshipColumn>(editArea, uiPackage);
            }
            return null;
        }



        private EditEntityAreaInitializer GenereateAreaInitializer(RelationshipDTO RelationshipDTO)
        {

            EditEntityAreaInitializer newAreaInitializer = new EditEntityAreaInitializer();


            newAreaInitializer.SourceRelation = new EditAreaRelationSource();
            newAreaInitializer.SourceRelation.SourceEditArea = this;
            newAreaInitializer.SourceRelation.SourceEntityID = RelationshipDTO.EntityID1;
            newAreaInitializer.SourceRelation.SourceTableID = RelationshipDTO.TableID1;
            newAreaInitializer.SourceRelation.RelationshipColumns = RelationshipDTO.RelationshipColumns;
            newAreaInitializer.SourceRelation.Relationship = RelationshipDTO;
            newAreaInitializer.SourceRelation.TargetEntityID = RelationshipDTO.EntityID2;
            newAreaInitializer.SourceRelation.TargetTableID = RelationshipDTO.TableID2;
            newAreaInitializer.SourceRelation.TargetSideIsMandatory = RelationshipDTO.IsOtherSideMandatory;

            if (RelationshipDTO.TypeEnum == Enum_RelationshipType.OneToMany)
            {
                newAreaInitializer.DataMode = DataMode.Multiple;
                //    newAreaInitializer.DataCount = (RelationshipDTO as OneToManyRelationshipDTO).DetailsCount;
                //newAreaInitializer.IntracionMode = IntracionMode.Create;
            }
            else
                newAreaInitializer.DataMode = DataMode.One;
            if (RelationshipDTO.IsOtherSideCreatable == true)
            {
                if (RelationshipDTO is OneToManyRelationshipDTO)
                    newAreaInitializer.IntracionMode = IntracionMode.CreateSelectInDirect;
                else
                    newAreaInitializer.IntracionMode = IntracionMode.CreateInDirect;
            }
            else if (RelationshipDTO.IsOtherSideCreatable == false)
            {
                if (RelationshipDTO.TypeEnum == Enum_RelationshipType.OneToMany)
                    return null;
                else
                    newAreaInitializer.IntracionMode = IntracionMode.Select;
            }


            //اینجا تصمیم گیری میشود که کدام فرم مسقیم و کدام با لینک نمایش داده شود
            //if (RelationshipDTO.IsOtherSideDirectlyCreatable == true)
            //    directionMode = DirectionMode.Direct;
            //else if (RelationshipDTO.IsOtherSideDirectlyCreatable == false)
            //    directionMode = DirectionMode.Indirect;
            //else
            //{
            //    //رابطه خود فرم جاری
            //    //if (AreaInitializer.SourceRelation != null)
            //    //{
            //    //    directionMode = DirectionMode.Indirect;
            //    //}
            //    //else
            //    //{
            //    //    if (newAreaInitializer.DataMode == DataMode.Multiple)
            //    //        directionMode = DirectionMode.Direct;
            //    //    else
            //    //    {
            //    //        if (AgentUICoreMediator.IndependentDataEntry(RelationshipDTO.EntityID2))
            //    //            directionMode = DirectionMode.Indirect;
            //    //        else
            //    //            directionMode = DirectionMode.Direct;
            //    //    }
            //    //}
            //}
            //newAreaInitializer.DirectionMode = directionMode;
            //////////////////////////////////////////////

            //newAreaInitializer.DirectionMode = DirectionMode.Indirect;




            //    var result = GetEntity(RelationshipDTO.EntityID2, newAreaInitializer.DirectionMode, newAreaInitializer.IntracionMode, newAreaInitializer.DataMode);

            newAreaInitializer.EntityID = RelationshipDTO.EntityID2;
            //newAreaInitializer.Permissoins = result.Permissoins;
            //newUICompositions = result.UICompositions;
            //newAreaInitializer.Validations = result.Validations;

            return newAreaInitializer;


        }
        //private DP_EntityResult GetFullEntity(int entityID2)
        //{
        //    var request = new DP_EntityRequest();
        //    request.EntityID = entityID2;
        //    return AgentUICoreMediator.GetEntity(request, EntityColumnInfoType.WithFullColumns, EntityRelationshipInfoType.WithRelationships, true, true, true, true, true, true);
        //}
        ////private DP_EntityResult GetSimpleEntity(int entityID2)
        //{
        //    var request = new DP_EntityRequest();
        //    request.EntityID = entityID2;
        //    return AgentUICoreMediator.GetEntity(request, EntityColumnInfoType.WithoutColumn, EntityRelationshipInfoType.WithoutRelationships, false, false, false,false,false);
        //}
        //private DP_EntityResult GetEntity(int entityID2, DirectionMode directionMode, IntracionMode intracionMode, DataMode dataMode)
        //{
        //    var request = new DP_EntityRequest();
        //    request.EntityID = entityID2;
        //    if (directionMode == DirectionMode.Direct)
        //    {
        //        if (intracionMode == IntracionMode.Create || intracionMode == IntracionMode.CreateSelect)
        //            return GetFullEntity(entityID2);
        //        else
        //            return GetSimpleEntity(entityID2);
        //    }
        //    else
        //    {
        //        return GetSimpleEntity(entityID2);
        //    }
        //}

        //بعضی از ارتباطات نمایش داده نمیشوند
        private bool RelationIsRedundant(EditEntityAreaInitializer AreaInitializer, RelationshipDTO relationship)
        {
            if (relationship.IsOtherSideMandatory)
                return false;
            else
            {
                //ارتباطاتی  که از روابط یک به چند با جداول مرجع بوجود آمده اند
                if (SimpleEntity.IsStructurReferencee == true ||
                     SimpleEntity.IsDataReference == true)
                {
                    if (relationship.TypeEnum == Enum_RelationshipType.OneToMany ||
                       relationship.TypeEnum == Enum_RelationshipType.ImplicitOneToOne)
                        return true;
                }
                //بوجود آمده SubUnionToUnion که خود از یک ارتباط Union برای یک UnionToSubUnion  سایر ارتباطات
                if (AreaInitializer.SourceRelation != null)
                {
                    if (AreaInitializer.SourceRelation.RelationshipType == Enum_RelationshipType.SubUnionToUnion_SubUnionHoldsKeys ||
                      AreaInitializer.SourceRelation.RelationshipType == Enum_RelationshipType.SubUnionToUnion_UnionHoldsKeys)
                    {
                        if (relationship.TypeEnum == Enum_RelationshipType.UnionToSubUnion_SubUnionHoldsKeys ||
                          relationship.TypeEnum == Enum_RelationshipType.UnionToSubUnion_UnionHoldsKeys)
                            return true;
                    }
                }
            }
            return false;
        }



        private static bool RelationshipIsValid(EditEntityAreaInitializer areaInitializer, RelationshipDTO relationship)
        {
            if (areaInitializer.SourceRelation != null)
            {
                if (IsReverseRelation(areaInitializer.SourceRelation.Relationship, relationship))
                    return false;

                if (RelationHistoryContains(areaInitializer.SourceRelation.SourceEditArea.AreaInitializer, areaInitializer.SourceRelation, ""))
                {
                    return false;
                }
            }
            return true;
        }

        private static bool IsReverseRelation(RelationshipDTO relationship1, RelationshipDTO relationship2)
        {
            if ((relationship1.PairRelationshipID == relationship2.ID) || (relationship2.PairRelationshipID == relationship1.ID))
                return true;
            return false;
        }

        private static bool RelationHistoryContains(EditEntityAreaInitializer areaInitializer, EditAreaRelationSource editAreaRelationSource, string history)
        {
            //if (AreaInitializer.SourceRelation != null)
            //    if (AreaInitializer.SourceRelation != null)
            //    {
            //        history += Environment.NewLine + AreaInitializer.SourceRelation.Relationship.Name;
            //        if (AreaInitializer.SourceRelation.Relationship.ID == editAreaRelationSource.Relationship.ID)
            //            return true;
            //        else
            //            return RelationHistoryContains(AreaInitializer.SourceRelation.SourceEditArea.AreaInitializer, editAreaRelationSource, history);

            //    }
            return false;
        }


        //اینجا از ویوها استفاده و تصمیم گیری میشود
        //private UIControlPackageMultipleData GenerateControlPackageOfView()
        //{
        //    //ColumnSetting columnSetting = new ColumnSetting();

        //    //if (AreaInitializer.SourceRelation.SourceEditArea.AreaInitializer.DataMode == DataMode.Multiple)
        //    //{
        //    //    TemporaryLinkType linkType = TemporaryLinkType.SerachView;
        //    //    columnSetting.UISetting = new UIControlSetting() { DesieredColumns = ColumnWidth.Normal, DesieredRows = 1 };
        //    //    if (AreaInitializer.IntracionMode == IntracionMode.Create)
        //    //    {
        //    //        linkType = TemporaryLinkType.DataView;
        //    //    }
        //    //    if (AreaInitializer.IntracionMode == IntracionMode.CreateSelect)
        //    //    {
        //    //        linkType = TemporaryLinkType.DataSearchView;
        //    //    }
        //    //    else if (AreaInitializer.IntracionMode == IntracionMode.Select)
        //    //    {
        //    //        linkType = TemporaryLinkType.SerachView;
        //    //    }

        //    //    var view = AgentUICoreMediator.UIManager.GenerateMultipleDataDependentViewControl(columnSetting, linkType);
        //    //    view.TemporaryViewRequested += arg_TemporaryViewRequested;
        //    //    return AgentUICoreMediator.UIManager.GenerateDataDependentControlPackage(view, columnSetting);
        //    //}
        //    //else
        //    //{



        //}





        //// Multiple برای حالت 
        //void arg_TemporaryViewRequested(object sender, Arg_TemporaryDisplayViewRequested e)
        //{//از پدر نخواند و اصلاح شود
        // //ColumnControl column = AreaInitializer.SourceRelation.SourceEditArea.GerRelationSourceColumnControl(e.Column);
        //    (AreaInitializer.SourceRelation.SourceEditArea.DataView as I_View_EditEntityAreaMultiple).SetSelectedData(AgentHelper.CreateListFromSingleObject<DP_DataRepository>(e.ParentDataItem as DP_DataRepository));

        //    //if (column != null)
        //    if (e.LinkType == TemporaryLinkType.DataView)
        //    {
        //        AreaInitializer.SourceRelation.RelatedData = e.ParentDataItem as DP_DataRepository;
        //        ShowTemporaryDataView(true);
        //    }
        //    else if (e.LinkType == TemporaryLinkType.SerachView)
        //    {
        //        AreaInitializer.SourceRelation.RelatedData = e.ParentDataItem as DP_DataRepository;
        //        ShowTemporarySearchView();
        //    }
        //    else if (e.LinkType == TemporaryLinkType.Clear)
        //    {
        //        AreaInitializer.SourceRelation.RelatedData = e.ParentDataItem as DP_DataRepository;
        //        ClearData(true);

        //    }
        //    else if (e.LinkType == TemporaryLinkType.Info)
        //    {
        //        AgentHelper.ShowEditEntityAreaInfo(this);
        //    }
        //}

        // Multiple برای حالت غیر
        void TemporaryDisplayView_TemporaryDisplayViewRequested(object sender, Arg_TemporaryDisplayViewRequested e)
        {

            //برای این حالت به داده پدر نیاز نیست
            //ColumnControl columnControl = GerRelationSourceColumnControl(e.Column);
            if (e.LinkType == TemporaryLinkType.DataView || e.LinkType == TemporaryLinkType.SerachView)
            {
                //var sourceData = AreaInitializer.SourceRelation.SourceEditArea.AreaInitializer.Data.FirstOrDefault();

                //if (sourceData == null)
                //    sourceData = AreaInitializer.SourceRelation.SourceEditArea.CreateDefaultData();
                //AreaInitializer.SourceRelation.RelatedData = sourceData;

                //if (AreaInitializer.SourceRelation.RelatedData == null)
                //    AreaInitializer.SourceRelation.RelatedData = AreaInitializer.SourceRelation.SourceEditArea.CreateDefaultData();
            }

            if (e.LinkType == TemporaryLinkType.DataView)
            {

                //    sourceData = AreaInitializer.SourceRelation.SourceEditArea.CreateDefaultData();
                //else
                //    AgentHelper.CreateListFromSingleObject<DP_DataRepository>(sourceData);
                ShowTemporaryDataView(e.ParentDataItem as DP_DataRepository);
            }
            else if (e.LinkType == TemporaryLinkType.SerachView)
                ShowTemporarySearchView(false);
            else if (e.LinkType == TemporaryLinkType.Clear)
            {
                //////ClearData(true);
            }
            else if (e.LinkType == TemporaryLinkType.Info)
            {
                AgentHelper.ShowEditEntityAreaInfo(this);
            }
        }

        //در نهایت برای نمایش ویوهای موقت اینجا صدا زده میشود
        public void ShowTemporaryDataView(DP_DataRepository parenttData)
        {
            if (AreaInitializer.FormComposed == false)
            {
                GenerateDataVieww();
            }


            var existingData = AgentHelper.ExtractAreaInitializerData(AreaInitializer, parenttData);
            //میتواند نال باشد
            ShowView(true, existingData);

            //طوری نوشته شود که هربار بصورت غیر ضروری صدا زده نشود
            //ShowData(null, true);
            ////if (!AreaInitializer.FormAttributes.CommandAttributes.Any(x => x.Command is CloseDialogCommand))
            ////{
            ////    var closeCommand = new CloseDialogCommand();
            ////    Commands.Add(closeCommand);
            ////    DataView.AddCommand(closeCommand);

            ////    var closeSaveCommand = new SaveAndCloseDialogCommand();
            ////    Commands.Add(closeSaveCommand);
            ////    DataView.AddCommand(closeSaveCommand);

            ////}

            //AgentUICoreMediator.UIManager.ShowDialog(DataView, SimpleEntity.Alias);

        }

        //public void OnTemporaryViewLoaded()
        //{

        //}

        //میرود SearchViewEntityArea به سراغ
        public void ShowTemporarySearchView(bool fromDataView)
        {
            if (SearchViewEntityArea == null)
            {
                var searchViewEntityArea = new SearchViewEntityArea();
                var searchViewInit = new SearchViewAreaInitializer();
                searchViewInit.EntityID = AreaInitializer.EntityID;
                searchViewInit.TempEntity = FullEntity;
                searchViewEntityArea.SetAreaInitializer(searchViewInit);
                searchViewEntityArea.DataSelected += SearchViewEntityArea_DataSelected;
            }
            SearchViewEntityArea.IsCalledFromDataView = fromDataView;
            SearchViewEntityArea.ShowTemporarySearchView();
        }

        //private I_SearchViewEntityArea GenerateSearchViewArea()
        //{
        //    var searchViewEntityArea = new SearchViewEntityArea();
        //    var searchViewInit = new SearchViewAreaInitializer();
        //    searchViewInit.SearchEntity = AreaInitializer.TemplateEntity;
        //    searchViewEntityArea.SetAreaInitializer(searchViewInit);
        //    searchViewEntityArea.DataSelected += SearchViewEntityArea_DataSelected;
        //    return searchViewEntityArea;
        //}

        private void SearchViewEntityArea_DataSelected(object sender, DataSelectedEventArg e)
        {
            int relationID = 0;
            DP_DataRepository RelationData = null;
            if (AreaInitializer.SourceRelation != null)
            {
                relationID = AreaInitializer.SourceRelation.Relationship.ID;
                RelationData = AreaInitializer.SourceRelation.RelatedData;
            }
            foreach (var item in e.DataItem)
            {
                List<EntityInstanceProperty> relationColumns = AgentHelper.GetKeyProperties(this, item);
                if (relationColumns != null && relationColumns.Count > 0)
                {
                    if (relationColumns.Any(x => !AgentHelper.ValueIsEmpty(x.Value)))
                    {
                        var result = AgentUICoreMediator.SearchDataForEditFromProperties(AreaInitializer.EntityID, relationColumns);
                        if (result.Count > 0)
                            AddData(result);

                    }

                    //AddData(selectedData);
                }
            }
        }




        //private EditEntityArea GenerateEditEntityArea(AreaInitializer entityTemplate)
        //{



        //    return editNDTypeArea;
        //}
        //private ColumnDTO GetCorrespondingTypeProperty(TableDrivedEntityDTO entity, int p)
        //{
        //    return entity.Table.Column.FirstOrDefault(x => x.ID == p);
        //}






        public I_View_EditEntityAreaMultiple DataView { set; get; }

        //هنوز مشخص نیست که چه موقع استفاده میشود TemporarySearchView.که یک ویو ساده از نمایش تلفیق دو ویو اول است SearchViewArea و ViewView و SearchView.داره View در خودش چهار تا

        public IAG_View_TemporaryView TemporaryDisplayView { set; get; }

        //public I_View_SearchEntityArea SearchView { set; get; }

        //public I_View_ViewEntityArea ViewView { set; get; }



        public I_SearchViewEntityArea SearchViewEntityArea { set; get; }
        //public I_ViewEntityArea ViewEntityArea { set; get; }




        //public List<I_EntityAreaCommand> Commands
        //{
        //    set;
        //    get;
        //}

        public EditEntityAreaInitializer AreaInitializer
        {
            set;
            get;
        }

        //public List<AG_MainNDTypeAndRelatedItems> NDTypes
        //{
        //    set;
        //    get;
        //}
        //public List<DP_DataRepository> DataRepository
        //{
        //    set;
        //    get;
        //}
        //List<ColumnControl> ColumnControls
        //{
        //    set;
        //    get;
        //}
        List<SimpleColumnControlMultipleData> SimpleColumnControls
        {
            set;
            get;
        }
        List<RelationshipColumnControlMultipleData> RelationshipColumnControls
        {
            set;
            get;
        }



        public I_EditEntityLetterArea EditLetterArea
        {
            set; get;
        }

        public I_EntityLettersArea EntityLettersArea
        {
            set; get;
        }
        public I_DataViewAreaContainer DataViewAreaContainer
        {
            set; get;
        }

        public I_EditEntityArchiveArea EditArchiveArea
        {
            set; get;
        }

        public I_DataListReportAreaContainer DataListReportAreaContainer
        {
            set; get;
        }

        List<SimpleColumnControlMultipleData> I_EditEntityAreaMultipleData.SimpleColumnControls
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        List<RelationshipColumnControlMultipleData> I_EditEntityAreaMultipleData.RelationshipColumnControls
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        List<UIControlPackageTree> I_EditEntityArea.UICompositionContainers
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public List<DP_DataRepository> GetData(List<DP_DataRepository> dataList = null, DP_DataRepository pItem = null)
        {
            bool isMainData = false;
            if (dataList == null)
            {
                dataList = new List<DP_DataRepository>();
                isMainData = true;
            }
            //List<DP_DataRepository> result = new List<DP_DataRepository>();
            var existData = AgentHelper.ExtractAreaInitializerData(AreaInitializer, pItem);
            foreach (var item in existData)
            {
                if (isMainData)
                {
                    //////if (item.IsNewItem)
                    item.GUID = Guid.NewGuid();
                }
                if (!item.ExcludeFromDataEntry)
                {
                    if (item.ValueChanged)
                        dataList.Add(item);
                    foreach (var relationshupControl in RelationshipColumnControls)
                    {
                        //var relationSourceControl = (typePropertyControl as RelationSourceControl);
                        bool GetChildData = false;
                        //if (relationSourceControl.EditNdTypeArea.AreaInitializer.IntracionMode != IntracionMode.Select)
                        //{
                        if (relationshupControl.EditNdTypeArea.AreaInitializer.FormComposed)
                            GetChildData = true;
                        //////else if (relationSourceControl.EditNdTypeArea.AreaInitializer.SourceRelation.RelationshipType == Enum_RelationshipType.OneToMany
                        //////    || relationSourceControl.EditNdTypeArea.AreaInitializer.SourceRelation.RelationshipType == Enum_RelationshipType.ImplicitOneToOne
                        //////    || relationSourceControl.EditNdTypeArea.AreaInitializer.SourceRelation.RelationshipType == Enum_RelationshipType.SuperToSub)
                        //////    GetChildData = true;

                        //}
                        if (GetChildData)
                            relationshupControl.EditNdTypeArea.GetData(dataList, item);
                        // ShowTypePropertyControlValue((typePropertyControl as RelationSourceControl), "");

                    }
                }


            }
            return dataList;
        }
        //public List<DP_DataRepository> GetRemovedData(List<DP_DataRepository> dataList = null)
        //{

        //    if (dataList == null)
        //    {
        //        dataList = new List<DP_DataRepository>();
        //    }
        //    //List<DP_DataRepository> result = new List<DP_DataRepository>();
        //    foreach (var item in AreaInitializer.RemovedData)
        //    {
        //        dataList.Add(item);

        //    }
        //    foreach (var property in AgentHelper.GetColumnList(AreaInitializer))
        //    {
        //        var typePropertyControls = ColumnControls.Where(x => x.Column.ID == property.ID);
        //        foreach (var typePropertyControl in typePropertyControls)
        //            if (typePropertyControl != null)
        //            {
        //                if (typePropertyControl is RelationSourceControl)
        //                {
        //                    var relationSourceControl = (typePropertyControl as RelationSourceControl);
        //                    //////bool GetChildData = false;
        //                    //////if (relationSourceControl.EditNdTypeArea.AreaInitializer.IntracionMode != IntracionMode.Select)
        //                    //////{
        //                    //////    if (relationSourceControl.EditNdTypeArea.AreaInitializer.FormComposed)
        //                    //////        GetChildData = true;
        //                    //////}
        //                    //////else if (relationSourceControl.EditNdTypeArea.AreaInitializer.SourceRelation.RelationshipType == Enum_RelationshipType.OneToMany
        //                    //////    || relationSourceControl.EditNdTypeArea.AreaInitializer.SourceRelation.RelationshipType == Enum_RelationshipType.ImplicitOneToOne
        //                    //////    || relationSourceControl.EditNdTypeArea.AreaInitializer.SourceRelation.RelationshipType == Enum_RelationshipType.SuperToSub)
        //                    //////{
        //                    //////    GetChildData = true;

        //                    //////}
        //                    //////if (GetChildData)
        //                    relationSourceControl.EditNdTypeArea.GetRemovedData(dataList);

        //                }
        //            }

        //    }
        //    return dataList;

        //}
        public void AddData(List<DP_DataRepository> data)
        {

            //Guid? relationID = null;
            //DataManager.DataPackage.Enum_DP_RelationSide? relationSide = null;
            //List<DataManager.DataPackage.DP_DataRepository> relationData = null;
            //if (AreaInitializer.SourceRelation != null)
            //{
            //    relationID = AreaInitializer.SourceRelation.Relationship.ID;
            //    relationSide = AreaInitializer.SourceRelation.SourceRelationSide;
            //    relationData = AreaInitializer.SourceRelation.SourceEditArea.GetCurrentData();
            //    if (relationData == null || relationData.Count == 0)
            //        return;
            //    foreach (var item in data)
            //    {
            //        item.RelationID = relationID;
            //        item.SourceRelationSide = relationSide;
            //        item.SourceRelatedData = relationData;
            //    }
            //یا رو اد دیتا ریلیشن دیتا رو اضاقه کنم یا در شو دریتا برای مالتیپل سلکتد دیتا رو ست کنم
            //هر کدوم اصولی تره
            //مشکل وقینه که گرید سرچ سلکت میشه و میخواد روابط اتوماتیک سلکت بشن اما برای اد دیتای روابط برای بدست آوردن داده فعلی سلکتد دیتای گرید مشخص نیست
            //}
            //int relationID = 0;
            //List<int> relationColumnIDs = new List<int>();





            if (AreaInitializer.SourceRelation != null)
            {
                if (AreaInitializer.SourceRelation.RelatedData == null)
                {
                    throw new Exception("fgh");
                    //AreaInitializer.SourceRelation.RelatedData = AreaInitializer.SourceRelation.SourceEditArea.CreateDefaultData();
                }
                //relationID = AreaInitializer.SourceRelation.Relationship.ID;
                //foreach (var column in AreaInitializer.SourceRelation.Relationship.RelationshipColumns)
                //{
                //    relationColumnIDs.Add(column.ColumnID1);
                //}

                //var sourceData = AreaInitializer.SourceRelation.SourceEditArea.AreaInitializer.Data.FirstOrDefault();

                //if (sourceData == null)


                //if (AreaInitializer.SourceRelation.RelatedData == null)
                //    AreaInitializer.SourceRelation.RelatedData = AreaInitializer.SourceRelation.SourceEditArea.CreateDefaultData();



                foreach (var item in data)
                {
                    item.SourceRelatedData = AreaInitializer.SourceRelation.RelatedData;
                    item.SourceEntityID = AreaInitializer.SourceRelation.SourceEntityID;
                    item.SourceTableID = AreaInitializer.SourceRelation.SourceTableID;
                    //item.RelationshipColumns = AreaInitializer.SourceRelation.RelationshipColumns;
                    item.SourceRelationID = AreaInitializer.SourceRelation.Relationship.ID;
                    item.TargetEntityID = AreaInitializer.SourceRelation.TargetEntityID;
                    item.TargetTableID = AreaInitializer.SourceRelation.TargetTableID;
                    //      item.TargetColumnIDs = AreaInitializer.SourceRelation.TargetRelationColumns.Select(x => x.ID).ToList();
                    item.SourceToTargetRelationshipType = AreaInitializer.SourceRelation.RelationshipType;
                    item.SourceToTargetMasterRelationshipType = AreaInitializer.SourceRelation.MasterRelationshipType;
                }

            }




            //if (AreaInitializer.DataMode == DataMode.One)
            //{
            //    if (data == null || data.Count == 0)
            //    {
            //        //اینجا باید اگر کلید پرشده بود دیتا را بگیرد
            //        throw new Exception("sdf");

            //    }
            //    else if (data.Count > 1)
            //    {
            //        AgentUICoreMediator.UIManager.ShowInfo("برای موجودیت " + SimpleEntity.Alias + " بیش از یک داده موجود است و تنها داده اول نمایش داده خواهد شد!", "", Temp.InfoColor.Red);
            //        data.RemoveRange(1, data.Count - 1);
            //        //throw new Exception("sdf");
            //    }




            //    //var existingData = AgentHelper.ExtractAreaInitializerData(AreaInitializer, relationData);
            //    //if (existingData.Count != 1)
            //    //    throw new Exception("sdf");

            //    ClearData(false);
            //    //اینجا باید اگر کلید پرشده بود دیتا را بگیرد

            //    AreaInitializer.Data.AddRange(data);

            //}
            //else
            //{
            if (data == null || data.Count == 0)
            {
                //اینجا باید اگر کلید پرشده بود دیتا را بگیرد
                throw new Exception("sdf");

            }

            AreaInitializer.Data.AddRange(data);

            //int take = -1;
            //if (AreaInitializer.DataCount != null && AreaInitializer.DataCount != 0)
            //{
            //    var existData = AgentHelper.ExtractAreaInitializerData(AreaInitializer);
            //    if (existData.Count + data.Count > AreaInitializer.DataCount)
            //    {
            //        take = AreaInitializer.DataCount.Value - existData.Count;
            //    }
            //}
            //if (take == -1)
            //    AreaInitializer.Data.AddRange(data);
            //else
            //{
            //    data = data.Take(take).ToList();
            //    AreaInitializer.Data.AddRange(data);
            //    AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowInfo(SimpleEntity.Alias + " : " + " تنها امکان افزودن" + AreaInitializer.DataCount + " قلم داده وجود دارد ", "", MyUILibrary.Temp.InfoColor.Red);
            //    if (take == 0)
            //        return;
            //}
            //}
            //if (show)
            //{

            //CheckEditPermission();
            ManageDataSecurity();
            //CheckSaveAcsess();

            //bool allowShowData = true;
            //var tempView = GetTemporaryView();
            //if (fromUser == false && tempView != null)
            //    allowShowData = false;

            //if (allowShowData)
            //    if (AreaInitializer.FormComposed)
            //        ShowData(data, fromUser);

            //if (tempView != null)
            //    ShowTemporaryLinkTitle(tempView);


            //ایده قشنگیه اما بره روی ریلیشن changed بهتره
            //if (AreaInitializer.SourceRelation != null)
            //{
            //    if (AreaInitializer.SourceRelation.MasterRelationshipType==Enum_MasterRelationshipType.FromForeignToPrimary)
            //    {
            //        foreach (var dataItem in data)
            //        {
            //            //به یک دیتا اد شود به طرف ان میگوید
            //            foreach (var targetCol in AreaInitializer.SourceRelation.RelationshipColumns)
            //            {
            //                var prop = dataItem.DataInstance.Properties.FirstOrDefault(x => x.ColumnID == targetCol.SecondSideColumnID1);
            //                if (prop != null)
            //                {
            //                    var arg = new ColumnValueChangeArg();
            //                    arg.NewValue = prop.Value;
            //                    AreaInitializer.SourceRelation.SourceEditArea.ColumnControls.First(x => x.Column.ID == targetCol.FirstSideColumnID1).ControlPackage.OnValueChanged(null, arg);
            //                    //index++;
            //                }
            //            }
            //        }
            //    }

            //}





            //}
            //if (AreaInitializer.UI_NDTypeSettings.DataMode == DataManager.DataPackage.DataPackageUISetting.Enum_DPUI_TypeDataMode.One)
            //{
            //    DataRepository.Clear();
            //    DataRepository.Add(data.Last());
            //}
            //else
            //    DataRepository.AddRange(data);
        }



        public IAG_View_TemporaryView GetTemporaryView()
        {
            IAG_View_TemporaryView view = null;
            if (TemporaryDisplayView != null)
                view = TemporaryDisplayView;
            else
            {
                //////if (AreaInitializer.SourceRelation != null)
                //////    if (AreaInitializer.SourceRelation.SourceEditArea.AreaInitializer.DataMode == DataMode.Multiple)
                //////    {
                //////        var relationshipControl = AreaInitializer.SourceRelation.SourceEditArea.RelationshipColumnControls.FirstOrDefault(x => x.Relationship.ID == AreaInitializer.SourceRelation.Relationship.ID);
                //////        if (relationshipControl != null)
                //////        {
                //////            //////if (relationshipControl.ControlPackage is DataDependentControlPackage)
                //////            //////{
                //////            //////    if ((relationshipControl.ControlPackage as DataDependentControlPackage).UIControl is I_View_DataDependentControl)
                //////            //////    {
                //////            //////        var viewColumn = (relationshipControl.ControlPackage as DataDependentControlPackage).UIControl as I_View_DataDependentControl;
                //////            //////        view = viewColumn.GetTemporaryView(AreaInitializer.SourceRelation.RelatedData);
                //////            //////    }
                //////            //////}
                //////        }
                //////    }
            }
            return view;
        }



        //private void ShowTemporaryLinkTitle(List<DP_DataRepository> specificDate)
        //{

        //}
        //public void RemoveData(List<DP_DataRepository> data)
        //{
        //    foreach (var item in data)
        //        DataRepository.Remove(item);
        //}



        //public List<DP_DataRepository> CreateDefaultData()
        //{
        //    DP_DataRepository RelationData = null;
        //    if (AreaInitializer.SourceRelation != null)
        //    {
        //        RelationData = AreaInitializer.SourceRelation.RelatedData;
        //    }
        //    return CreateDefaultData(RelationData);
        //}
        //public List<DP_DataRepository> CreateDefaultData(DP_DataRepository relationData)
        //{

        //}

        //public DP_DataRepository CreateDefaultData()
        //{
        //    if (AreaInitializer.DataMode == DataMode.Multiple)
        //        return null;

        //    if (AreaInitializer.SourceRelation != null)
        //    {
        //        if (AreaInitializer.SourceRelation.RelatedData == null)
        //        {
        //            AreaInitializer.SourceRelation.RelatedData = AreaInitializer.SourceRelation.SourceEditArea.CreateDefaultData();
        //        }
        //    }

        //    //اینطوری فرض میشود که این متود تنها برای موجودیتها با رابطه یک تنها و نه حتی رابطه یکی که در گرید استفاده شده (ومیتواند چندین داده داشته باشد) استفاده میشود
        //    if (AreaInitializer.Data.Count > 1)
        //    {
        //        throw new Exception("Asdsd");
        //    }
        //    else if (AreaInitializer.Data.Count == 1)
        //    {
        //        return AreaInitializer.Data.First();
        //    }
        //    else
        //    {
        //        DP_DataRepository newData = AgentHelper.CreateAreaInitializerNewData(this);

        //        AreaInitializer.Data.Add(newData);

        //        var view = GetTemporaryView();
        //        if (view != null)
        //            ShowTemporaryLinkTitle(view);
        //        return newData;
        //    }
        //    //}


        //}

        //////public void RemoveFromRelation(List<DP_DataRepository> specifiData = null)
        //////{
        //////    if (AreaInitializer.SourceRelation != null)
        //////    {
        //////        List<DP_DataRepository> dataList = null;
        //////        if (specifiData == null)
        //////            dataList = AgentHelper.ExtractAreaInitializerData(AreaInitializer);
        //////        else
        //////            dataList = specifiData;
        //////        if (dataList.Count(x => x.IsNewItem == false) > 0)
        //////        {
        //////            if (AgentUICoreMediator.GetAgentUICoreMediator.ReverseRelationshipIsMandatory(AreaInitializer.SourceRelation.Relationship.ID))
        //////            {
        //////                if (AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowConfirm("تائید", "بعلت وجود رابطه اجباری ، داده موجود به همراه تمامی اطلاعات وابسته حذف خواهند شد" +
        //////                     Environment.NewLine + "آیا مطمئن هستید؟") == MyUILibrary.Temp.ConfirmResul.No)
        //////                {
        //////                    return;
        //////                }

        //////            }

        //////        }

        //////        foreach (var data in dataList)
        //////        {
        //////            ClearData(AgentHelper.CreateListFromSingleObject(data));
        //////            if (data.IsNewItem == false)
        //////            {
        //////                AreaInitializer.RemovedData.Add(data);
        //////            }
        //////        }

        //////    }
        //////    else
        //////        throw new Exception("sdfsdf");
        //////}
        public void ClearDataFromCommand()
        {
            List<DP_DataRepository> specificData = null;
            if (AreaInitializer.SourceRelation == null)
            {
                if (AreaInitializer.Data.Count > 0)
                    specificData = AreaInitializer.Data;
            }
            else
            {
                if (AreaInitializer.SourceRelation.RelatedData == null)
                    throw new Exception("asd");
                else
                {
                    var dataList = AgentHelper.ExtractAreaInitializerData(AreaInitializer, AreaInitializer.SourceRelation.RelatedData);
                    if (AreaInitializer.Data.Count > 0)
                        specificData = dataList;
                }
            }
            if (specificData != null && specificData.Any())
            {
                foreach (var item in specificData)
                    ClearData(item);

            }
        }
        public void ClearData(DP_DataRepository specifiData)
        {
            //List<DP_DataRepository> dataList = null;
            //if (specifiData == null)
            //    dataList = AgentHelper.ExtractAreaInitializerData(AreaInitializer);
            //else
            //    dataList = specifiData;
            //if (callFromCommand)
            //{
            //    if (AreaInitializer.SourceRelation != null)
            //    {
            //        if (AreaInitializer.SourceRelation.RelationshipType == Enum_RelationshipType.OneToMany
            //            || AreaInitializer.SourceRelation.RelationshipType == Enum_RelationshipType.ImplicitOneToOne
            //            || AreaInitializer.SourceRelation.RelationshipType == Enum_RelationshipType.SuperToSub)
            //            if (AreaInitializer.SourceRelation.RelatedData != null && AreaInitializer.SourceRelation.RelatedData.IsNewItem == false)
            //            {
            //                if (dataList.Count(x => x.IsNewItem == false) > 0)
            //                {
            //                    if (AgentUICoreMediator.GetAgentUICoreMediator.ReverseRelationshipIsMandatory(AreaInitializer.SourceRelation.Relationship.ID))
            //                    {
            //                        if (AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowConfirm("تائید", "بعلت وجود رابطه اجباری ، داده موجود به همراه تمامی اطلاعات وابسته حذف خواهند شد" +
            //                             Environment.NewLine + "آیا مطمئن هستید؟") == MyUILibrary.Temp.ConfirmResul.No)
            //                        {
            //                            return;
            //                        }

            //                    }

            //                }

            //                foreach (var data in dataList)
            //                {
            //                    AreaInitializer.SourceRelation.RelatedData.RemovedItems.Add(data);
            //                }
            //            }

            //    }
            //}

            //if (dataList.Count > 0)
            //{
            //foreach (var data in specifiData)
            //{
            foreach (var typePropertyControl in RelationshipColumnControls)
            {
                // AgentUICoreMediator.UIManager.ClearValidationMessage(typePropertyControl.ControlPackage); پاک شدن پیغامها و اعتبارسنجی چی؟
                //typePropertyControl.EditNdTypeArea.AreaInitializer.SourceRelation.RelatedData = data;
                //typePropertyControl.EditNdTypeArea.ClearDataFromCommand();
                var childData = typePropertyControl.EditNdTypeArea.AreaInitializer.Data.Where(x => x.SourceRelatedData == specifiData);
                foreach (var item in childData.ToList())
                    typePropertyControl.EditNdTypeArea.ClearData(item);
            }


            if (DataView != null)
                DataView.RemoveDataContainers(specifiData);
            AreaInitializer.Data.RemoveAll(x => specifiData == x);


            //var view = GetTemporaryView();
            //if (TemporaryDisplayView != null)
            //    ShowTemporaryLinkTitle(view);

            //}

            if (AreaInitializer.IntracionMode != IntracionMode.CreateDirect
              && AreaInitializer.IntracionMode != IntracionMode.CreateSelectDirect)
            {//مهم باید اطلاعات موجود را نمایش دهد
                ///ClearTemporaryLinkTitle();
            }
            //}

            ManageDataSecurity();








        }


        //public void ClearDataFromParent(DP_DataRepository parentData)
        //{
        //    var dataList = AreaInitializer.Data.Where(x => x.SourceRelatedData == parentData).ToList();
        //    ClearData(dataList);
        //}

        private void ShowDefaultData(DP_DataRepository newData)
        {
            //زیاد جالب نیست
            foreach (var typePropertyControl in SimpleColumnControls)
            {
                //////AgentUICoreMediator.UIManager.ClearValidationMessage(typePropertyControl.ControlPackage);
                var property = newData.DataInstance.Properties.FirstOrDefault(x => x.ColumnID == typePropertyControl.Column.ID);
                if (property != null)
                    ShowTypePropertyControlValue(newData, typePropertyControl, property.Value);
                else
                {
                    //?????
                }
            }
        }

        public void RemoveSelectedData()
        {

            //if (AreaInitializer.UI_NDTypeSettings.DataMode == DataMode.One)
            //{

            if (AreaInitializer.SourceRelation == null)
                return;
            if (AreaInitializer.DataMode == DataMode.Multiple)
            {
                //var data = DataView.GetSelectedData();
                //RemoveFromRelation(DataView.GetSelectedData());
                var dataITems = (DataView as I_View_EditEntityAreaMultiple).GetSelectedData();

                foreach (var dataItem in dataITems)
                {
                    var dataAttribute = AreaInitializer.DataItemAttributes.FirstOrDefault(x => x.DataItem == dataItem);
                    if (dataAttribute != null)
                        if (dataAttribute.SecurityDelete != true)
                        {
                            //جزئیات رکورد اضافه شود
                            AgentUICoreMediator.UIManager.ShowInfo("کاربر گرامی، شما مجاز به حذف رکوردهای انتخاب شده نمی باشید", "", Temp.InfoColor.Red);
                            return;
                        }
                }
                //////ClearData(true, (DataView as I_View_EditEntityAreaMultiple).GetSelectedData());


                //DataView.RemoveSelectedDataContainers();

            }


            //////AgentHelper.RemoveData(AreaInitializer.DataPackageTemplate.Data, selectedData);
        }

        //داده ها ی داده شده را از خود و فرمهای وابسته حذف میکند
        //public void RemoveData(DP_DataRepository data)
        //{



        //}
        //public void RemoveDataWithRelations(DP_DataRepository data)
        //{

        //    if (AreaInitializer.DataMode == DataMode.Multiple)
        //    {
        //        DataView.RemoveDataContainers(data);
        //    }

        //    //AgentHelper.RemoveData(AreaInitializer.Data, data);
        //    AreaInitializer.Data.RemoveAll(x => data == x);


        //    foreach (var property in ColumnControls)
        //    {
        //        if (property is RelationSourceControl)
        //        {
        //            var RelationSourceControl = (property as RelationSourceControl);
        //            RelationSourceControl.EditNdTypeArea.RemoveDataFromRelation(data);
        //        }
        //    }

        //}
        ////همه داده ها را حذف نمیکند و بلکه داده ها را با توجه به داده های پدر موجود حذف میکند
        //public void RemoveDataFromRelation(DP_DataRepository relationData)
        //{
        //    if (AreaInitializer.SourceRelation != null)
        //        if (AreaInitializer.SourceRelation.RelatedData == relationData)
        //        {
        //            AreaInitializer.SourceRelation.RelatedData = null;
        //        }
        //    List<DP_DataRepository> removeData = new List<DP_DataRepository>();
        //    foreach (var data in AreaInitializer.Data)
        //    {
        //        if (data.SourceRelatedData != null)
        //            //if (relationData.Count() == data.SourceRelatedData.Count)
        //            if (data.SourceRelatedData == relationData)
        //            {
        //                removeData.Add(data);
        //            }
        //    }

        //    if (removeData.Count > 0)
        //        foreach (var remove in removeData)
        //            RemoveDataWithRelations(remove);
        //}

        public List<DP_DataRepository> GetSelectedData()
        {
            List<DP_DataRepository> selectedData = (DataView as I_View_EditEntityAreaMultiple).GetSelectedData();
            return selectedData;
        }




        //صدا زده میشود RelationData تنها بوسیله ShowData برای نمایش داده های اضافه شده در آن صدا زده میشود.در مابقی موارد ShowData که AddData فقط یکجا مشخص میشود و آنهم در specificDate
        public void ShowDataInDataView(List<DP_DataRepository> specificDate)
        {
            //if (!AreaInitializer.FormComposed)
            //    return;
            var actionResult = DoActionActivities(Enum_EntityActionActivityStep.BeforeLoad, specificDate);

            //if(AreaInitializer.SourceRelation!=null)
            //{

            //}


            //List<DP_DataRepository> showItems = specificDate;

            ////if (AreaInitializer.SourceRelation != null)
            ////{
            ////    AreaInitializer.SourceRelation.RelatedData = relationData;
            ////}

            //var existData = AgentHelper.ExtractAreaInitializerData(AreaInitializer);
            //if (showItems == null)
            //    showItems = existData;





            //if (fromUser)
            //{//حذف شد؟؟
            //////if (AreaInitializer.SourceRelation != null)
            //////{
            //////    if (AreaInitializer.SourceRelation.RelationshipType == Enum_RelationshipType.OneToMany
            //////               || AreaInitializer.SourceRelation.RelationshipType == Enum_RelationshipType.ImplicitOneToOne
            //////               || AreaInitializer.SourceRelation.RelationshipType == Enum_RelationshipType.SuperToSub
            //////               || AreaInitializer.SourceRelation.RelationshipType == Enum_RelationshipType.UnionToSubUnion_SubUnionHoldsKeys
            //////               || AreaInitializer.SourceRelation.RelationshipType == Enum_RelationshipType.SubUnionToUnion_UnionHoldsKeys)
            //////    {
            //////        if (ColumnControls.Count > 0)
            //////        {
            //////            if (existData.Count == 0 && showItems.Count == 0)
            //////            {
            //////                if (AreaInitializer.DataMode == DataMode.One)
            //////                {
            //////                    //int index = 0;
            //////                    foreach (var sourceCol in AreaInitializer.SourceRelation.SourceRelationColumns)
            //////                    {
            //////                        var relationCol = AreaInitializer.SourceRelation.TargetRelationColumns[index];

            //////                        //var   value = AreaInitializer.SourceRelation.RelatedData.DataInstance.Properties.First(x => x.ColumnID == AreaInitializer.SourceRelation.SourceRelationColumns[index].ID).Value;
            //////                        var value = AreaInitializer.SourceRelation.SourceEditArea.FetchTypePropertyControlValue(AreaInitializer.SourceRelation.RelatedData, AreaInitializer.SourceRelation.SourceEditArea.ColumnControls.First(x => x.Column.ID == sourceCol.ID));
            //////                        ShowTypePropertyControlValue(null, ColumnControls.First(x => x.Column.ID == relationCol.ID), value);
            //////                        //index++;
            //////                    }
            //////                }
            //////            }
            //////        }

            //////    }
            //////}
            //}
            foreach (var dataRepository in specificDate)
            {
                foreach (var propertyControl in SimpleColumnControls)
                {
                    var property = dataRepository.DataInstance.Properties.FirstOrDefault(x => x.ColumnID == propertyControl.Column.ID);
                    if (property != null)
                    {
                        //if (AreaInitializer.IntracionMode == IntracionMode.Create
                        //                           || AreaInitializer.IntracionMode == IntracionMode.CreateSelect)
                        //{
                        ShowTypePropertyControlValue(dataRepository, propertyControl, property.Value);
                        //}
                    }
                    else
                    {
                        //????
                    }
                }

                foreach (var relationshipControl in RelationshipColumnControls)
                {
                    bool relationshipFirstSideHasValue = relationshipControl.Relationship.RelationshipColumns.Any()
                   && relationshipControl.Relationship.RelationshipColumns.All(x => dataRepository.DataInstance.Properties.Any(y => !string.IsNullOrEmpty(y.Value) && y.ColumnID == x.FirstSideColumnID)) ;
                    if (relationshipFirstSideHasValue)
                    {
                        if (relationshipControl.EditNdTypeArea is I_EditEntityAreaOneData)
                        {

                            List<DP_DataRepository> result = AgentUICoreMediator.SerachDataViewFromParentRelation(relationshipControl.Relationship.ID, dataRepository);
                            if (result.Count > 1)
                            {
                                throw (new Exception("asdf"));
                            }
                            else if (result.Count == 1)
                            {
                                var data = result[0];
                                data.IsFullData = false;
                                relationshipControl.EditNdTypeArea.AreaInitializer.SourceRelation.RelatedData = dataRepository;
                                (relationshipControl.EditNdTypeArea as I_EditEntityAreaOneData).AddData(data);
                                (relationshipControl.EditNdTypeArea as I_EditEntityAreaOneData).ShowDataInTempView(data);

                            }


                        }
                        else if (relationshipControl.EditNdTypeArea is I_EditEntityAreaMultipleData)
                        {


                            List<DP_DataRepository> result = AgentUICoreMediator.SerachDataViewFromParentRelation(relationshipControl.Relationship.ID, dataRepository);

                            if (result.Count > 0)
                            {
                                foreach (var data in result)
                                {
                                    data.IsFullData = false;
                                }
                                relationshipControl.EditNdTypeArea.AreaInitializer.SourceRelation.RelatedData = dataRepository;
                                (relationshipControl.EditNdTypeArea as I_EditEntityAreaMultipleData).AddData(result);
                                (relationshipControl.EditNdTypeArea as I_EditEntityAreaMultipleData).ShowDataInTempView(result);
                            }
                        }
                    }
                }
            }


            //if (AreaInitializer.SourceRelation != null)
            //{
            //    if (existData.Count == 0 && showItems.Count == 0)
            //    {


            //        //int relationID = 0;
            //        //if (AreaInitializer.SourceRelation != null)
            //        //{
            //        //    relationID = AreaInitializer.SourceRelation.Relationship.ID;

            //        //}
            //        var result = AgentUICoreMediator.SerachDataFromParentRelation(AreaInitializer.SourceRelation.Relationship.ID, AreaInitializer.SourceRelation.RelatedData);
            //        if (result != null && result.Count > 0)
            //            AddData(result, fromUser);


            //    }
            //}






            //List<DP_DataRepository> candidateDataRepositories = new List<DP_DataRepository>();


            //باید کنترل شود که اسپسفیک دیتا با دیتای این فرم هماهنگی دارد یا خیر
            //Guid? relationID = null;
            //DataManager.DataPackage.Enum_DP_RelationSide? relationSide = null;
            //if (AreaInitializer.SourceRelation != null)
            //{
            //    relationID = AreaInitializer.SourceRelation.Relationship.ID;
            //    relationSide = AreaInitializer.SourceRelation.SourceRelationSide;
            //}

            //if (AreaInitializer.DataMode == DataMode.One)
            //{
            //    if (showItems == null)
            //    {
            //        ////////اینجا باید اگر کلید پرشده بود دیتا را بگیرد
            //        //////DP_DataRepository newData = AgentHelper.CreateNewDataRepository(AreaInitializer, relationID, relationSide, relationData);
            //        ////////AreaInitializer.DataPackageTemplate.Data.Add(newData);
            //        //////AddData(AgentHelper.CreateListFromSingleObject<DP_DataRepository>(newData), true);
            //        //////specificDate.Add(newData);
            //        throw new Exception("sdf");

            //    }
            //    else if (showItems.Count > 1)
            //    {
            //        throw new Exception("sdf");
            //    }
            //    //candidateDataRepositories.Add(specificDate.First());

            //    //if (AreaInitializer.UI_NDTypeSettings.DataMode == DataMode.One)
            //    //{
            //    //   
            //    //}
            //    if (showItems.Count != 0)
            //        ShowTypePropertyData(showItems);
            //}
            //else
            //{


            //    if (showItems.Count > 0)
            //{
            //    //if (!select)
            //    if (specificDate == null || specificDate.Count == 0)
            //        (DataView as I_View_EditEntityAreaMultiple).RemoveDataContainers();

            //    (DataView as I_View_EditEntityAreaMultiple).AddDataContainers(showItems);
            //    ShowTypePropertyData(showItems);
            //}
            //else
            //{
            //    if (AreaInitializer.FormComposed)
            //        (DataView as I_View_EditEntityAreaMultiple).RemoveDataContainers();

            //}

            //}



        }

        public bool DoActionActivities(Enum_EntityActionActivityStep step, List<DP_DataRepository> specificDate)
        {
            //بازدارنده بودن اقدام کنترل شود
            if (ActionActivities != null)
                foreach (var entityActionActivity in ActionActivities.Where(x => x.Step == step))
                {

                    DoActionActivities(entityActionActivity, specificDate);
                }
            return true;
        }

        private bool DoActionActivities(ActionActivityDTO actionActivity, List<DP_DataRepository> specificDate)
        {
            //if (actionActivity.CodeFunctionID != 0)
            //{
            //    foreach (var data in specificDate)
            //        codeFunctionHelper.CalculateCodeFunction(this, actionActivity.CodeFunction, data);
            //}
            //else if (actionActivity.DatabaseFunctionID != 0)
            //{
            //    foreach (var data in specificDate)
            //        DatabaseFunctionHelper.CalculateCodeFunction(this, actionActivity.DatabaseFunction, data);
            //}
            ////؟؟؟؟؟؟؟؟؟ نقش specificDate
            if (actionActivity.RelationshipEnablity != null)
            {
                AgentHelper.ApplyRelationshipEnablity(this, actionActivity.RelationshipEnablity);
            }
            //else if (actionActivity.UIEnablity != null)
            //{
            //    AgentHelper.ApplyUIEnablityOneData(this, actionActivity.UIEnablity);
            //}
            else if (actionActivity.ColumnValue != null)
            {
                AgentHelper.ApplyColumnValueMultipleData(this, actionActivity.ColumnValue);
            }
            return true;
        }

        //void View_DataCotainerIsReady(object sender, Arg_DataContainer e)
        //{
        //    if (e.DataItem != null)
        //        ShowData(AgentHelper.CreateListFromSingleObject<DP_DataRepository>(e.DataItem));
        //}


        //.صدا زده میشود MultipleData دوبار ، یکبار برای یکی ها و یکبار برای ShowData تنها در
        //private void ShowTypePropertyData(List<DP_DataRepository> specificDate)
        //{


        //}
        //اونی که کلید نداره قتی اول میشه موقع آپدیت اونی که کلید داره مقدار موجود اولیو نمیگیره؟؟

        //تنها یکبار و برای نمایش مقادیر در کنترلهای ساده و غیر فرمی صدا زده میشود
        public bool ShowTypePropertyControlValue(DP_DataRepository dataItem, SimpleColumnControlMultipleData typePropertyControl, string value)
        {

            //ColumnSetting columnSetting = new ColumnSetting();

            //if (typePropertyControl.Column.PrimaryKey == true && (dataItem != null && !dataItem.IsNewItem))
            //{
            //    columnSetting.IsReadOnly = true;
            //}
            //else
            //    columnSetting.IsReadOnly = typePropertyControl.ColumnSetting.IsReadOnly;
            //بهتره جور دیگه نوشته بشه
            //if (typePropertyControl.ControlPackage != null)
            return typePropertyControl.ControlPackage.SetValue(dataItem, value);
            //else
            //    return typePropertyControl.ControlPackage.SetValue(value);
            //AgentUICoreMediator.UIManager.ShowControlValue(typePropertyControl.ControlPackage, typePropertyControl.Column, value, columnSetting);

        }
        //public DeleteResult DeleteData()
        //{

        //}
        public void ClearTemporaryLinkTitle()
        {
            var view = GetTemporaryView();
            view.SetLinkText("");
        }
        public UpdateValidationResult UpdateData()
        {
            UpdateValidationResult result = new UpdateValidationResult();
            //////result.Items.CollectionChanged += Items_CollectionChanged;

            var data = AgentHelper.ExtractAreaInitializerData(AreaInitializer);

            if (data.Count > 1)
                throw new Exception("sdf");

            var dataRepository = data[0];

            dataRepository.ExcludeFromDataEntry = false;
            foreach (var simplePropertyControl in SimpleColumnControls)
            {
                var dataColumn = dataRepository.DataInstance.Properties.FirstOrDefault(x => x.ColumnID == simplePropertyControl.Column.ID);
                var val = FetchTypePropertyControlValue(dataRepository, simplePropertyControl);
                if (dataColumn != null)
                {
                    dataColumn.Value = val;
                    if (AgentHelper.IsColumnValueChanged(simplePropertyControl, dataColumn.Value, val))
                        dataRepository.ValueChanged = true;
                }
                else
                {
                    dataColumn = new EntityInstanceProperty(simplePropertyControl.Column.ID, val);
                    dataRepository.DataInstance.Properties.Add(dataColumn);
                    dataRepository.ValueChanged = true;
                }

            }

            foreach (var relationshipControl in RelationshipColumnControls)
            {
                //if (AreaInitializer.IntracionMode == IntracionMode.CreateDirect || AreaInitializer.IntracionMode==IntracionMode.CreateSelectDirect)
                //{
                if (relationshipControl.EditNdTypeArea.AreaInitializer.IntracionMode == IntracionMode.CreateDirect
                   || relationshipControl.EditNdTypeArea.AreaInitializer.IntracionMode == IntracionMode.CreateSelectDirect)
                {

                    var tempView = relationshipControl.EditNdTypeArea.GetTemporaryView();
                    if (tempView == null)
                    {

                        relationshipControl.EditNdTypeArea.AreaInitializer.SourceRelation.RelatedData = dataRepository;
                        var subUpdateValidationResult = relationshipControl.EditNdTypeArea.UpdateData();
                        //اعتبارسنجی
                        //////foreach (var item in subUpdateValidationResult.Items)
                        //////{
                        //////    result.Items.Add(item);
                        //////}
                    }
                }


                //}

                //}






                //if (relationSourceControl.EditNdTypeArea.AreaInitializer.FormComposed)
                //{
                //if (relationSourceControl.EditNdTypeArea.AreaInitializer.SourceRelation.RelationshipType == Enum_RelationshipType.ManyToOne
                //    || relationSourceControl.EditNdTypeArea.AreaInitializer.SourceRelation.RelationshipType == Enum_RelationshipType.ExplicitOneToOne
                //    || relationSourceControl.EditNdTypeArea.AreaInitializer.SourceRelation.RelationshipType == Enum_RelationshipType.SubToSuper
                //    || (relationSourceControl.EditNdTypeArea.AreaInitializer.SourceRelation.RelationshipType == Enum_RelationshipType.UnionToSubUnion_UnionHoldsKeys)
                //        || (relationSourceControl.EditNdTypeArea.AreaInitializer.SourceRelation.RelationshipType == Enum_RelationshipType.SubUnionToUnion_SubUnionHoldsKeys))
                //{
                //    if (relationSourceControl.EditNdTypeArea.AreaInitializer.IntracionMode == IntracionMode.Select
                //      || relationSourceControl.EditNdTypeArea.AreaInitializer.IntracionMode == IntracionMode.CreateSelect)
                //    {
                //        var val = FetchTypePropertyControlValue(dataRepository, typePropertyControl);
                //        if (IsColumnValueChanged(typePropertyControl, typeProperty.Value, val))
                //            dataRepository.ValueChanged = true;

                //        typeProperty.Value = val;

                //    }
                //}
                //else
                //{

                //}
                //}
                //else
                //{

                //}

                //}


            }
            //}
            //}




            ValidateData(result, data);

            //////var view = GetTemporaryView();
            //////if (view != null)
            //////    ShowTemporaryLinkTitle(view);

            return result;
        }



        void Items_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)

        {
            //////if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            //////{
            //////    if (e.NewItems.Count > 0)
            //////    {
            //////        var item = e.NewItems[0] as UpdateValidationItem;
            //////        object view = null;
            //////        if (item.EditEntityArea.TemporaryDisplayView != null)
            //////            view = item.EditEntityArea.TemporaryDisplayView;
            //////        else if (item.EditEntityArea.DataView != null)
            //////            view = item.EditEntityArea.TemporaryDisplayView;
            //////        if (item.ColumnControl != null)
            //////            AgentUICoreMediator.UIManager.ShowValidationMessage(item.ColumnControl.ControlPackage, item.Message);
            //////        else
            //////        {
            //////            AgentUICoreMediator.UIManager.ShowValidationMessage(this, item.Message);
            //////            //////   AgentUICoreMediator.UIManager.ShowValidationMessage(item.ColumnControl.ControlPackage, item.Message);
            //////        }
            //////    }
            //////}
        }



        private void ValidateData(UpdateValidationResult result, List<DP_DataRepository> data)
        {
            //foreach (var dataRepository in data)
            //{

            //    if (AgentHelper.DataHasValue(dataRepository))
            //    {
            //        foreach (var typeProperty in dataRepository.DataInstance.Properties)
            //        {
            //            foreach (var typePropertyControl in ColumnControls.Where(x => x.Column.ID == typeProperty.ColumnID))
            //            {
            //                if (typePropertyControl != null)
            //                {
            //                    //اینجا کنترل میشود Null روابط اجباری و همچنین مقادیر اجباری و
            //                    ValidateValue(typePropertyControl, dataRepository, typeProperty, result);
            //                }
            //            }
            //        }
            //    }
            //    else
            //    {
            //        dataRepository.ExcludeFromDataEntry = true;
            //    }

            //}
            //List<Tuple<ISARelationshipDTO, ColumnControl, List<DP_DataRepository>>> superToSubsTuples = new List<Tuple<ISARelationshipDTO, ColumnControl, List<DP_DataRepository>>>();
            //foreach (var dataRepository in data)
            //{
            //    //اینجا دسته بندی میشوند  Sub به Super تمام روابط
            //    foreach (var typePropertyControl in ColumnControls.Where(x => x.EditNdTypeArea != null))
            //    {
            //        //var relationSourceControl = (typePropertyControl as RelationSourceControl);
            //        if (typePropertyControl.EditNdTypeArea.AreaInitializer.SourceRelation.RelationshipType == Enum_RelationshipType.SuperToSub)
            //        {
            //            var otherSideOneData = typePropertyControl.EditNdTypeArea.AreaInitializer.Data.FirstOrDefault(x => x.SourceRelatedData == dataRepository);
            //            var superToSubsTuple = superToSubsTuples.FirstOrDefault(x => x.Item1.ID == (typePropertyControl.EditNdTypeArea.AreaInitializer.SourceRelation.Relationship as SuperToSubRelationshipDTO).ISARelationship.ID);
            //            if (superToSubsTuple == null)
            //            {
            //                List<DP_DataRepository> dps = new List<DP_DataRepository>() { otherSideOneData };
            //                superToSubsTuple = new Tuple<ISARelationshipDTO, ColumnControl, List<DP_DataRepository>>((typePropertyControl.EditNdTypeArea.AreaInitializer.SourceRelation.Relationship as SuperToSubRelationshipDTO).ISARelationship, typePropertyControl, dps);
            //                superToSubsTuples.Add(superToSubsTuple);
            //            }
            //            else
            //            {
            //                superToSubsTuple.Item3.Add(otherSideOneData);
            //            }
            //            //superToSubsTuple.Add(new Tuple<ISARelationshipDTO, ColumnControl, DP_DataRepository>(relationSourceControl.EditNdTypeArea.AreaInitializer.SourceRelation.Relationship.RelationshipType.SuperToSubRelationshipType.ISARelationship, typePropertyControl, otherSideOneData));
            //        }
            //    }
            //}
            //if (AreaInitializer.SourceRelation != null)
            //{
            //    //نیز به دسته بندی های قبلی اضافه میشوند Sub به وجود آمده باشد آن ارتباط Super به Sub همچنین اگر خود فرم از یک رابطه
            //    if (AreaInitializer.SourceRelation.RelationshipType == Enum_RelationshipType.SubToSuper)
            //    {
            //        var otherSideOneData = AreaInitializer.SourceRelation.RelatedData;
            //        var columnControl = ColumnControls.FirstOrDefault(x => (x.EditNdTypeArea == null) && x.Column.ID == AreaInitializer.SourceRelation.RelationshipColumns.First().SecondSideColumnID1);

            //        var superToSubsTuple = superToSubsTuples.FirstOrDefault(x => x.Item1.ID == (AreaInitializer.SourceRelation.Relationship as SuperToSubRelationshipDTO).ISARelationship.ID);
            //        if (superToSubsTuple == null)
            //        {
            //            List<DP_DataRepository> dps = new List<DP_DataRepository>() { otherSideOneData };
            //            superToSubsTuple = new Tuple<ISARelationshipDTO, ColumnControl, List<DP_DataRepository>>((AreaInitializer.SourceRelation.Relationship as SuperToSubRelationshipDTO).ISARelationship, columnControl, dps);
            //            superToSubsTuples.Add(superToSubsTuple);
            //        }
            //        else
            //        {
            //            superToSubsTuple.Item3.Add(otherSideOneData);
            //        }

            //    }
            //}

            //if (superToSubsTuples.Count > 0)
            //{
            //    foreach (var isa in superToSubsTuples)
            //    {
            //        if (isa.Item1.IsTolatParticipation == true)
            //        {
            //            if (!isa.Item3.Any(x => x != null && AgentHelper.DataHasValue(x)))
            //            {
            //                result.Items.Add(new UpdateValidationItem(this, isa.Item2, isa.Item2.Alias + " برقراری یکی از رابطه ها الزامی است  "));
            //            }
            //        }
            //        if (isa.Item1.IsDisjoint == true)
            //        {
            //            if (isa.Item3.Count(x => x != null && AgentHelper.DataHasValue(x)) > 1)
            //            {
            //                result.Items.Add(new UpdateValidationItem(this, isa.Item2, isa.Item2.Alias + " بیش از یک رابطه  نمیتواند برقرار باشد "));
            //            }
            //            else
            //            {
            //                foreach (var dataItem in isa.Item3)
            //                {
            //                    if (dataItem != null && !AgentHelper.DataHasValue(dataItem))
            //                    {
            //                        dataItem.ExcludeFromDataEntry = true;
            //                    }
            //                }


            //            }
            //        }
            //    }
            //}





            ////عملکرد مانند ارث بری در بالا میباشد
            //List<Tuple<UnionRelationshipDTO, ColumnControl, List<DP_DataRepository>>> uinonToSubUnionTuples = new List<Tuple<UnionRelationshipDTO, ColumnControl, List<DP_DataRepository>>>();
            //foreach (var dataRepository in data)
            //{

            //    foreach (var typePropertyControl in ColumnControls.Where(x => x.EditNdTypeArea != null))
            //    {

            //        if (typePropertyControl.EditNdTypeArea.AreaInitializer.SourceRelation.RelationshipType == Enum_RelationshipType.UnionToSubUnion_UnionHoldsKeys
            //            || typePropertyControl.EditNdTypeArea.AreaInitializer.SourceRelation.RelationshipType == Enum_RelationshipType.UnionToSubUnion_SubUnionHoldsKeys)
            //        {
            //            var otherSideOneData = typePropertyControl.EditNdTypeArea.AreaInitializer.Data.FirstOrDefault(x => x.SourceRelatedData == dataRepository);
            //            var uinonToSubUnionTuple = uinonToSubUnionTuples.FirstOrDefault(x => x.Item1.ID == (typePropertyControl.EditNdTypeArea.AreaInitializer.SourceRelation.Relationship as SuperUnionToSubUnionRelationshipDTO).UnionRelationship.ID);
            //            if (uinonToSubUnionTuple == null)
            //            {
            //                List<DP_DataRepository> dps = new List<DP_DataRepository>() { otherSideOneData };
            //                uinonToSubUnionTuple = new Tuple<UnionRelationshipDTO, ColumnControl, List<DP_DataRepository>>((typePropertyControl.EditNdTypeArea.AreaInitializer.SourceRelation.Relationship as SuperUnionToSubUnionRelationshipDTO).UnionRelationship, typePropertyControl, dps);
            //                uinonToSubUnionTuples.Add(uinonToSubUnionTuple);
            //            }
            //            else
            //            {
            //                uinonToSubUnionTuple.Item3.Add(otherSideOneData);
            //            }
            //        }
            //    }
            //}

            //if (AreaInitializer.SourceRelation != null)
            //{
            //    if (AreaInitializer.SourceRelation.RelationshipType == Enum_RelationshipType.SubUnionToUnion_UnionHoldsKeys ||
            //        AreaInitializer.SourceRelation.RelationshipType == Enum_RelationshipType.SubUnionToUnion_SubUnionHoldsKeys)
            //    {
            //        var otherSideOneData = AreaInitializer.SourceRelation.RelatedData;
            //        var columnControl = ColumnControls.FirstOrDefault(x => (x.EditNdTypeArea == null) && x.Column.ID == AreaInitializer.SourceRelation.RelationshipColumns.First().SecondSideColumnID1);

            //        var uinonToSubUnionTuple = uinonToSubUnionTuples.FirstOrDefault(x => x.Item1.ID == (AreaInitializer.SourceRelation.Relationship as SuperUnionToSubUnionRelationshipDTO).UnionRelationship.ID);
            //        if (uinonToSubUnionTuple == null)
            //        {
            //            List<DP_DataRepository> dps = new List<DP_DataRepository>() { otherSideOneData };
            //            uinonToSubUnionTuple = new Tuple<UnionRelationshipDTO, ColumnControl, List<DP_DataRepository>>((AreaInitializer.SourceRelation.Relationship as SuperUnionToSubUnionRelationshipDTO).UnionRelationship, columnControl, dps);
            //            uinonToSubUnionTuples.Add(uinonToSubUnionTuple);
            //        }
            //        else
            //        {
            //            uinonToSubUnionTuple.Item3.Add(otherSideOneData);
            //        }

            //    }
            //}
            //if (uinonToSubUnionTuples.Count > 0)
            //{
            //    foreach (var union in uinonToSubUnionTuples)
            //    {
            //        if (union.Item1.IsTolatParticipation == true)
            //        {
            //            if (!union.Item3.Any(x => x != null && AgentHelper.DataHasValue(x)))
            //            {
            //                result.Items.Add(new UpdateValidationItem(this, union.Item2, union.Item2.Alias + " یکی از رابطه ها میبایست برقرار باشد "));
            //            }
            //        }

            //        if (union.Item3.Count(x => x != null && AgentHelper.DataHasValue(x)) > 1)
            //        {
            //            result.Items.Add(new UpdateValidationItem(this, union.Item2, union.Item2.Alias + " نمیتواند بیش از یک رابطه برقرار باشد "));
            //        }
            //        else
            //        {
            //            foreach (var dataItem in union.Item3)
            //            {
            //                if (dataItem != null && !AgentHelper.DataHasValue(dataItem))
            //                {
            //                    dataItem.ExcludeFromDataEntry = true;
            //                }
            //            }


            //        }

            //    }
            //}

            //foreach (var validation in AreaInitializer.Validations)
            //{
            //    bool booleanMode = string.IsNullOrEmpty(validation.Value);

            //    foreach (var dataRepository in data)
            //    {
            //        object calculatedValue = null;
            //        if (validation.FormulaID != 0)
            //        {
            //            calculatedValue = formulaHelper.CalculateFormula(this, validation.Formula, dataRepository);
            //        }

            //        else if (validation.CodeFunctionID != 0)
            //        {
            //            calculatedValue = codeFunctionHelper.CalculateCodeFunction(this, validation.CodeFunction, dataRepository).Result;
            //        }

            //        bool isNotValid;
            //        if (booleanMode)
            //            isNotValid = Convert.ToBoolean(calculatedValue) == true;
            //        else
            //            isNotValid = calculatedValue.ToString() == validation.Value;
            //        if (isNotValid)
            //        {
            //            result.Items.Add(new UpdateValidationItem(this, null, validation.Message));
            //        }
            //    }


            //}

        }


        //اینجا کنترل میشود Null روابط اجباری و همچنین مقادیر اجباری و
        private bool ValidateValue(SimpleColumnControlMultipleData typePropertyControl, DP_DataRepository dataRepository, EntityInstanceProperty typeProperty, UpdateValidationResult validationResultList)
        {
            bool result = true;
            //if (typePropertyControl.EditNdTypeArea != null)
            //{
            //    //var relationSourceControl = (typePropertyControl as RelationSourceControl);
            //    if (typePropertyControl.EditNdTypeArea.AreaInitializer.SourceRelation.RelationshipType == Enum_RelationshipType.ManyToOne
            //         || typePropertyControl.EditNdTypeArea.AreaInitializer.SourceRelation.RelationshipType == Enum_RelationshipType.ExplicitOneToOne
            //         || typePropertyControl.EditNdTypeArea.AreaInitializer.SourceRelation.RelationshipType == Enum_RelationshipType.SubToSuper
            //         || typePropertyControl.EditNdTypeArea.AreaInitializer.SourceRelation.RelationshipType == Enum_RelationshipType.SubUnionToUnion_SubUnionHoldsKeys
            //        || typePropertyControl.EditNdTypeArea.AreaInitializer.SourceRelation.RelationshipType == Enum_RelationshipType.UnionToSubUnion_UnionHoldsKeys
            //        )
            //    {
            //        var otherSideOneData = typePropertyControl.EditNdTypeArea.AreaInitializer.Data.FirstOrDefault(x => x.SourceRelatedData == dataRepository);
            //        if (typePropertyControl.EditNdTypeArea.AreaInitializer.SourceRelation.TargetSideIsMandatory)
            //        {
            //            bool otherSideHasNoData = false;
            //            if (otherSideOneData == null || !AgentHelper.DataHasValue(otherSideOneData))
            //            {
            //                otherSideHasNoData = true;

            //            }
            //            if (typePropertyControl.EditNdTypeArea.AreaInitializer.IntracionMode == IntracionMode.Select)
            //                if (AgentHelper.ValueIsEmpty(typeProperty.Value) || otherSideHasNoData)
            //                {
            //                    validationResultList.Items.Add(new UpdateValidationItem(this, typePropertyControl, typePropertyControl.Alias + " انتخاب نشده است "));
            //                    result = false;
            //                }

            //            if (typePropertyControl.EditNdTypeArea.AreaInitializer.IntracionMode == IntracionMode.Create)
            //            {
            //                if (!AgentHelper.ValueIsEmpty(typeProperty.Value))
            //                {
            //                    validationResultList.Items.Add(new UpdateValidationItem(this, typePropertyControl, typePropertyControl.Alias + " تنها میبایست ایجاد شود و نه انتخاب "));
            //                    result = false;
            //                }

            //                if (otherSideHasNoData)
            //                {
            //                    validationResultList.Items.Add(new UpdateValidationItem(this, typePropertyControl, typePropertyControl.Alias + " داده ای ایجاد و مقداردهی نشده است "));
            //                    result = false;
            //                }
            //            }

            //            if (typePropertyControl.EditNdTypeArea.AreaInitializer.IntracionMode == IntracionMode.CreateSelect)
            //                if (AgentHelper.ValueIsEmpty(typeProperty.Value) && otherSideHasNoData)
            //                {
            //                    validationResultList.Items.Add(new UpdateValidationItem(this, typePropertyControl, typePropertyControl.Alias + " نه داده ای انتخاب شده و نه ایجاد و مقداردهی شده است "));
            //                    result = false;
            //                }
            //        }
            //        else
            //        {
            //            if (otherSideOneData != null)
            //            {
            //                if (!AgentHelper.DataHasValue(otherSideOneData))
            //                {
            //                    otherSideOneData.ExcludeFromDataEntry = true;
            //                    typeProperty.Value = "<Null>";
            //                }
            //            }
            //            else
            //                typeProperty.Value = "<Null>";
            //        }

            //    }

            //    else if (typePropertyControl.EditNdTypeArea.AreaInitializer.SourceRelation.RelationshipType == Enum_RelationshipType.OneToMany
            //        || typePropertyControl.EditNdTypeArea.AreaInitializer.SourceRelation.RelationshipType == Enum_RelationshipType.ImplicitOneToOne
            //        || typePropertyControl.EditNdTypeArea.AreaInitializer.SourceRelation.RelationshipType == Enum_RelationshipType.SubUnionToUnion_UnionHoldsKeys
            //         || typePropertyControl.EditNdTypeArea.AreaInitializer.SourceRelation.RelationshipType == Enum_RelationshipType.UnionToSubUnion_SubUnionHoldsKeys
            //        )
            //    {
            //        var otherSideData = typePropertyControl.EditNdTypeArea.AreaInitializer.Data.Where(x => x.SourceRelatedData == dataRepository);
            //        if (typePropertyControl.EditNdTypeArea.AreaInitializer.SourceRelation.TargetSideIsMandatory)
            //        {

            //            if (otherSideData.Count() == 0 || otherSideData.All(x => !AgentHelper.DataHasValue(x)))
            //            {
            //                validationResultList.Items.Add(new UpdateValidationItem(this, typePropertyControl, typePropertyControl.Alias + " داده ای ایجاد و مقداردهی نشده است "));
            //                result = false;
            //            }

            //        }

            //    }
            //}
            //else
            //{
            //    if (!typePropertyControl.Column.IsIdentity == true)
            //    {
            //        if (typePropertyControl.Column.IsNull == false)
            //        {
            //            if (typeProperty.Value == null || typeProperty.Value.ToLower() == "<null>")
            //            {
            //                bool isTargetColumn = false;
            //                if (AreaInitializer.SourceRelation != null)
            //                    if (AreaInitializer.SourceRelation.RelationshipType == Enum_RelationshipType.OneToMany
            //                      || AreaInitializer.SourceRelation.RelationshipType == Enum_RelationshipType.ImplicitOneToOne
            //                      || AreaInitializer.SourceRelation.RelationshipType == Enum_RelationshipType.SuperToSub
            //                      || AreaInitializer.SourceRelation.RelationshipType == Enum_RelationshipType.UnionToSubUnion_SubUnionHoldsKeys
            //                      || AreaInitializer.SourceRelation.RelationshipType == Enum_RelationshipType.SubUnionToUnion_UnionHoldsKeys)
            //                    {
            //                        isTargetColumn = AreaInitializer.SourceRelation.RelationshipColumns.Any(x => x.SecondSideColumnID1 == typePropertyControl.Column.ID);
            //                    }
            //                if (!isTargetColumn)
            //                {
            //                    validationResultList.Items.Add(new UpdateValidationItem(this, typePropertyControl, typePropertyControl.Alias + " امکان مقداردهی Null وجود ندارد "));
            //                    result = false;
            //                }
            //            }
            //        }

            //        if (typePropertyControl.Column.IsMandatory == true)
            //        {
            //            if (typeProperty.Value == null || typeProperty.Value == "")
            //            {
            //                validationResultList.Items.Add(new UpdateValidationItem(this, typePropertyControl, typePropertyControl.Alias + " مقداردهی اجباری میباشد "));
            //                result = false;
            //            }
            //        }


            //        if (typePropertyControl.Column.StringColumnType != null)
            //        {
            //            if (typePropertyControl.Column.StringColumnType.MaxLength != 0)
            //                if (typeProperty.Value != null && typeProperty.Value.Length > typePropertyControl.Column.StringColumnType.MaxLength)
            //                {
            //                    validationResultList.Items.Add(new UpdateValidationItem(this, typePropertyControl, typePropertyControl.Alias + " حداکثر طول " + typePropertyControl.Column.StringColumnType.MaxLength + " قابل قبول است "));
            //                    result = false;
            //                }
            //        }
            //    }
            //}
            return result;
        }


        public string FetchTypePropertyControlValue(DP_DataRepository dataRepository, SimpleColumnControlMultipleData SimpleColumnControlMultipleData)
        {
            //if (AreaInitializer.DataMode == DataMode.Multiple)
            return SimpleColumnControlMultipleData.ControlPackage.GetValue(dataRepository);
            //else
            //    return SimpleColumnControlMultipleData.ControlPackage.GetValue();


            //////if (typePropertyControl.EditNdTypeArea != null)
            //////{
            //////    //var relationSourceControl = (typePropertyControl as RelationSourceControl);
            //////    if (typePropertyControl.EditNdTypeArea.AreaInitializer.SourceRelation != null)
            //////    {
            //////        if (typePropertyControl.EditNdTypeArea.AreaInitializer.SourceRelation.RelationshipType == Enum_RelationshipType.ManyToOne
            //////            || typePropertyControl.EditNdTypeArea.AreaInitializer.SourceRelation.RelationshipType == Enum_RelationshipType.ExplicitOneToOne
            //////            || typePropertyControl.EditNdTypeArea.AreaInitializer.SourceRelation.RelationshipType == Enum_RelationshipType.SubToSuper
            //////            || (typePropertyControl.EditNdTypeArea.AreaInitializer.SourceRelation.RelationshipType == Enum_RelationshipType.UnionToSubUnion_UnionHoldsKeys)
            //////            || (typePropertyControl.EditNdTypeArea.AreaInitializer.SourceRelation.RelationshipType == Enum_RelationshipType.SubUnionToUnion_SubUnionHoldsKeys))
            //////        {
            //////            var data = typePropertyControl.EditNdTypeArea.AreaInitializer.Data.FirstOrDefault(x => x.SourceRelatedData == dataRepository);
            //////            //AreaInitializer.DataMode == DataMode.Multiple
            //////            //&& 
            //////            if (data == null)
            //////                return "";
            //////            else
            //////                return typePropertyControl.EditNdTypeArea.FetchTypePorpertyValue(data, typePropertyControl.EditNdTypeArea.AreaInitializer.SourceRelation.RelationshipColumns.First().SecondSideColumn1);
            //////        }
            //////        else if (typePropertyControl.EditNdTypeArea.AreaInitializer.SourceRelation.RelationshipType == Enum_RelationshipType.OneToMany
            //////            || typePropertyControl.EditNdTypeArea.AreaInitializer.SourceRelation.RelationshipType == Enum_RelationshipType.ImplicitOneToOne
            //////              || typePropertyControl.EditNdTypeArea.AreaInitializer.SourceRelation.RelationshipType == Enum_RelationshipType.SuperToSub)
            //////        {
            //////            //return FetchTypePorpertyValue(dataRepository, typePropertyControl.Column);
            //////            throw (new Exception("asfsdf"));
            //////        }
            //////    }
            //////    return "";


            //////}
            //////else
            //////{

            //////}


            //if (typePropertyControl is RelationSourceControl)
            //{
            //    var relationSourceControl = (typePropertyControl as RelationSourceControl);
            //    //////return relationSourceControl.EditNdTypeArea.FetchTypePorpertyValue(dataRepository, AgentHelper.GetRelationOperand(relationSourceControl.Relation, relationSourceControl.RelationSide == Enum_DP_RelationSide.FirstSide ? Enum_DP_RelationSide.SecondSide : Enum_DP_RelationSide.FirstSide));
            //    return "";
            //    // return FetchTypePropertyRelationSourceControl(typePropertyControl as RelationSourceControl);
            //}
            //else
            //    return DataView.FetchTypePropertyControlValue(dataRepository, typePropertyControl);
        }

        //private string FetchTypePropertyRelationSourceControl(RelationSourceControl relationSourceControl)
        //{

        //}
        public string FetchTypePorpertyValue(DP_DataRepository dataRepository, ColumnDTO typeProperty)
        {
            //////if (AreaInitializer.IntracionMode == IntracionMode.Select)
            //////{
            return AgentHelper.GetTypePropertyValue(dataRepository, typeProperty);
            //////}
            //////else
            //////{
            //////    var typePropertyControl = ColumnControls.Where(x => x.Column.ID == typeProperty.ID).FirstOrDefault();
            //////    if (typePropertyControl != null)
            //////    {
            //////        return FetchTypePropertyControlValue(dataRepository, typePropertyControl);
            //////    }
            //////    else
            //////        throw (new Exception("SDf"));
            //////}
        }

        public void ShowDataInTempView(List<DP_DataRepository> specificDate)
        {
            ShowTemporaryLinkTitle(GetTemporaryView(), specificDate);
        }
        private void ShowTemporaryLinkTitle(IAG_View_TemporaryView view, List<DP_DataRepository> specificDate)
        {


            //var existData = AgentHelper.ExtractAreaInitializerData(AreaInitializer);

            //if (AreaInitializer.DataMode == DataMode.One)
            //{
            //    if (existData == null)
            //        throw new Exception("sdf");
            //    else if (existData.Count > 1)
            //    {
            //        throw new Exception("sdf");
            //        //AgentUICoreMediator.UIManager.ShowInfo("برای موجودیت " + AgentHelper.GetEntityTitle(AreaInitializer.TemplateEntity) + " بیش از یک داده موجود است!", "", Temp.InfoColor.Red);
            //        //return;
            //    }
            //}
            //candidateDataRepositories.Add(specificDate.First());

            //if (AreaInitializer.UI_NDTypeSettings.DataMode == DataMode.One)
            //{
            //   
            //}

            string title = "";
            var columnList = EntityWithSimpleColumns.Columns;
            int columnID = 0;
            if (columnList.Any(x => x.Name.ToLower() == "name"))
            {
                columnID = columnList.First(x => x.Name.ToLower() == "name").ID;
            }
            else if (columnList.Any(x => x.Name.ToLower() == "title"))
            {
                columnID = columnList.First(x => x.Name.ToLower() == "title").ID;
            }
            else if (columnList.Any(x => x.Name.ToLower() == "desc"))
            {
                columnID = columnList.First(x => x.Name.ToLower() == "desc").ID;
            }
            else if (columnList.Any(x => x.Name.ToLower() == "description"))
            {
                columnID = columnList.First(x => x.Name.ToLower() == "description").ID;
            }
            else if (columnList.Any(x => x.Name.ToLower() == "code"))
            {
                columnID = columnList.First(x => x.Name.ToLower() == "code").ID;
            }
            else if (columnList.Any(x => x.Name.ToLower() == "id"))
            {
                columnID = columnList.First(x => x.Name.ToLower() == "id").ID;
            }
            //else if (columnList.Any(x => !string.IsNullOrEmpty(x.Value)))
            //{
            //    columnID = columnList.First(x => !string.IsNullOrEmpty( x.Value)).ID;
            //}
            //if (columnID != 0)
            //{
            foreach (var dataRepository in specificDate)
            {
                EntityInstanceProperty property = null;
                if (columnID != 0)
                    property = dataRepository.DataInstance.Properties.FirstOrDefault(x => x.ColumnID == columnID);
                else
                    property = dataRepository.DataInstance.Properties.FirstOrDefault(x => !string.IsNullOrEmpty(x.Value));
                if (property != null)
                {
                    if (property.Value == "<Null>" || property.Value == "0" || property.Value == "")
                    {
                        title += (title == "" ? "" : ",") + " Selected " + (string.IsNullOrEmpty(SimpleEntity.Alias) ? SimpleEntity.Name : SimpleEntity.Alias);
                    }
                    else
                        title += (title == "" ? "" : ",") + property.Value;
                }

            }
            //}
            //view.CurrentDataItems = existData;
            view.SetLinkText(title);


        }
        //public UIControl DefaultDisplayView
        //{
        //    set;
        //    get;
        //}
        //public List<DataMaster.EntityDefinition.ND_Type> CurrentShownNDTypes
        //{
        //    get
        //    {
        //        List<DataMaster.EntityDefinition.ND_Type> list = new List<DataMaster.EntityDefinition.ND_Type>();
        //        foreach (var item in NDTypes)
        //            list.Add(item.MainNDType);
        //        return list;

        //    }
        //}


        //List<DataMaster.EntityDefinition.ND_Type> I_EditEntityArea.CurrentShownNDTypes
        //{
        //    get { throw new NotImplementedException(); }
        //}




























        //public bool ShowTypePorpertyValue(DP_DataRepository dataRepository, ColumnDTO nD_Type_Property, string value)
        //{
        //    throw new NotImplementedException();
        //}







        //public void DataSelected(List<DP_DataRepository> selectedData, I_ViewEntityArea viewArea)
        //{

        //    //    ShowData(null, selectedData, true);
        //}








        //public void ShowValidationTips(UpdateValidationResult result)
        //{
        //   foreach(var item in result.Items)
        //   {
        //       if(item.EditEntityArea!=null)
        //       {

        //       }
        //   }
        //}

    }



}
