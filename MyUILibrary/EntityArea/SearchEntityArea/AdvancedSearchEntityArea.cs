using AutoMapper;
using CommonDefinitions.UISettings;
using ModelEntites;

using MyUILibrary;
using MyUILibrary.EntityArea;
using MyUILibrary.EntityArea.Commands;

using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUILibrary.EntityArea
{
    public class AdvancedSearchEntityArea : I_AdvancedSearchEntityArea
    {
        public AdvancedSearchEntityArea()
        {
            SearchCommands = new List<I_Command>();
        }

        public I_View_AdvancedSearchEntityArea AdvancedSearchView { set; get; }

        public List<I_Command> SearchCommands
        {
            set;
            get;
        }


        public SearchEntityAreaInitializer SearchInitializer { set; get; }

        //public event EventHandler<Arg_PackageSelected> DataPackageSelected;
        public event EventHandler<SearchDataArg> SearchDataDefined;





        TableDrivedEntityDTO _SimpleEntity;
        public TableDrivedEntityDTO SimpleEntity
        {
            get
            {
                if (_SimpleEntity == null)
                {
                    if (FullEntity != null)
                        return FullEntity;
                    else
                        _SimpleEntity = AgentUICoreMediator.GetAgentUICoreMediator.tableDrivedEntityManagerService.GetSimpleEntity(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), SearchInitializer.EntityID);
                }
                return _SimpleEntity;
            }
        }

        TableDrivedEntityDTO _FullEntity;
        public TableDrivedEntityDTO FullEntity
        {
            get
            {
                if (_FullEntity == null)
                    _FullEntity = AgentUICoreMediator.GetAgentUICoreMediator.tableDrivedEntityManagerService.GetPermissionedEntity(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), SearchInitializer.EntityID);
                return _FullEntity;
            }
        }



        //AssignedPermissionDTO _Permission;
        //public AssignedPermissionDTO Permission
        //{
        //    get
        //    {
        //        if (_Permission == null)
        //            _Permission = AgentUICoreMediator.GetAgentUICoreMediator.SecurityHelper.GetAssignedPermissions(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(),SearchInitializer.EntityID, true);
        //        return _Permission;
        //    }
        //}

        public void SetAreaInitializer(SearchEntityAreaInitializer newAreaInitializer)
        {
            SearchInitializer = newAreaInitializer;
            //if (SearchInitializer.TempEntity != null)
            //    _FullEntity = SearchInitializer.TempEntity;
            GenerateSearchView();
        }

        private void GenerateSearchView()
        {
            AdvancedSearchView = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GenerateViewOfAdvancedSearch();
            ManageAdvancedSearchView();

            var clearSearchcommand = new SearchClearCommand(this);
            AdvancedSearchView.AddCommand(clearSearchcommand.CommandManager);
            var searchConfirmcommand = new AdvancedSearchConfirmCommand(this);
            AdvancedSearchView.AddCommand(searchConfirmcommand.CommandManager);
            //AdvancedSearchView.CommandExecuted += SearchView_CommandExecuted;
        }
        AdvanceSearchNode RootNode;
        DP_SearchRepository firstRepository { set; get; }
        private void ManageAdvancedSearchView()
        {

            var searchRepository = new DP_SearchRepository(SearchInitializer.EntityID);
            searchRepository.Title = FullEntity.Alias;
            AddLogicNode(null, searchRepository);
            //}
            //else
            //{
            //    AddLogicPhrase(null, SearchInitializer.EditSearchRepository);
            //}




        }

        public bool ShowSearchRepository(DP_SearchRepository item)
        {
            if (RootNode != null)
            {
                RootNode = null;
                AdvancedSearchView.ClearTreeItems();
            }
            AddLogicNode(null, item);
            return true;
        }
        private void RootAndMenu_Clicked(object sender, EventArgs e, AdvanceSearchNode node, bool and)
        {

            var logicPhrase = new LogicPhrase();
            logicPhrase.AndOrType = (and ? AndORType.And : AndORType.Or);
            AddLogicNode(node, logicPhrase);


        }
        //private AdvanceSearchNode AddSearchRepository(AdvanceSearchNode parentNode, DP_SearchRepository searchRepository)
        //{
        //    if (searchRepository == null)
        //        return null;
        //    var newnode = new AdvanceSearchNode();
        //    newnode.ParentNode = parentNode;
        //    newnode.Title = searchRepository.Name;
        //    newnode.NodeManager.SetHeader(newnode.Title);
        //    newnode.Phrase = searchRepository;
        //    if (parentNode != null)
        //    {
        //        parentNode.ChildItems.Clear();
        //        parentNode.NodeManager.ClearItems();
        //        newnode.EntityID = searchRepository.TargetEntityID;
        //        newnode.NodeManager = parentNode.NodeManager.AddChildItem();

        //    }
        //    else
        //    {
        //        newnode.EntityID = SearchInitializer.EntityID;
        //        RootNode = newnode;
        //        newnode.NodeManager = AdvancedSearchView.AddTreeItem();
        //    }
        //    foreach (var item in searchRepository.Phrases)
        //    {
        //        if (item is DP_SearchRepository)
        //        {
        //            AddSearchRepository(newnode, (item as DP_SearchRepository));
        //        }
        //        else if (item is LogicPhrase)
        //        {
        //            AddLogicPhrase(newnode, (item as LogicPhrase));
        //        }
        //        else if (item is SearchProperty)
        //        {
        //            AddChildNode((item as SearchProperty), newnode);
        //        }
        //    }
        //    return newnode;
        //}
        private AdvanceSearchNode AddLogicNode(AdvanceSearchNode parentNode, LogicPhrase logicPhrase)
        {
            if (RootNode == null && logicPhrase is DP_SearchRepository)
                firstRepository = logicPhrase as DP_SearchRepository;

            var newnode = new AdvanceSearchNode();
            newnode.ParentNode = parentNode;
            if (logicPhrase is DP_SearchRepository)
            {
                newnode.Title = (logicPhrase as DP_SearchRepository).Title;
            }
            else
            {
                newnode.Title = (logicPhrase.AndOrType == AndORType.And ? "And" : "Or");
            }
            if (parentNode != null)
            {
                if (logicPhrase is DP_SearchRepository)
                {
                    newnode.EntityID = (logicPhrase as DP_SearchRepository).TargetEntityID;
                }
                else
                {
                    newnode.EntityID = parentNode.EntityID;
                }
            }
            else
            {
                newnode.EntityID = SearchInitializer.EntityID;
            }
            if (parentNode != null)
            {
                parentNode.ChildItems.Add(newnode);
            }
            else
            {
                RootNode = newnode;
            }
            if (parentNode != null)
                newnode.NodeManager = parentNode.NodeManager.AddChildItem();
            else
                newnode.NodeManager = AdvancedSearchView.AddTreeItem();
            newnode.NodeManager.SetHeader(newnode.Title);
            newnode.Phrase = logicPhrase;
            //(newnode.Phrase as LogicPhrase).AndOrType = logicPhrase.AndOrType;
            foreach (var item in logicPhrase.Phrases)
            {
                if (item is DP_SearchRepository)
                {
                    AddLogicNode(newnode, (item as LogicPhrase));
                }
                else if (item is LogicPhrase)
                {
                    AddLogicNode(newnode, (item as LogicPhrase));
                }
                else if (item is SearchProperty)
                {
                    AddSearchPropertyNode((item as SearchProperty), newnode);
                }

            }
            if (GetParentSearchRepository(newnode))
            {
                I_AdvanceSearchMenu rootAndMenu = newnode.NodeManager.AddMenu("And");
                rootAndMenu.Clicked += (sender, e) => RootAndMenu_Clicked(sender, e, newnode, true);
                I_AdvanceSearchMenu rootOrMenu = newnode.NodeManager.AddMenu("Or");
                rootOrMenu.Clicked += (sender, e) => RootAndMenu_Clicked(sender, e, newnode, false);

                I_AdvanceSearchMenu simpleSearchMenu = newnode.NodeManager.AddMenu("خصوصیات");
                simpleSearchMenu.Clicked += (sender1, e1) => RootAndMenu_Clicked1(sender1, e1, newnode);

                foreach (var relationship in FullEntity.Relationships) //.Where(x => x.SearchEnabled == true))
                {
                    I_AdvanceSearchMenu relationshipSearchMenu = newnode.NodeManager.AddMenu(relationship.Alias);
                    relationshipSearchMenu.Clicked += (sender1, e1) => Relationship_ClickedNew(sender1, e1, newnode, relationship);
                }
            }
            else
            {
                if (logicPhrase is DP_SearchRepository)
                {
                    I_AdvanceSearchMenu editRelationshipSearchMenu = newnode.NodeManager.AddMenu("اصلاح");
                    editRelationshipSearchMenu.Clicked += (sender1, e1) => Relationship_ClickedEdit(sender1, e1, newnode, (logicPhrase as DP_SearchRepository));

                }
            }
            if (parentNode != null)
            {
                I_AdvanceSearchMenu removeMenu = newnode.NodeManager.AddMenu("حذف");
                removeMenu.Clicked += (sender1, e1) => RemoveMenu_Clicked(sender1, e1, parentNode, newnode);
            }
            return newnode;
        }
        private void Relationship_ClickedNew(object sender1, EventArgs e1, AdvanceSearchNode node, RelationshipDTO relationship)
        {
            SearchEntityArea advancedAndRaw = new EntityArea.SearchEntityArea();
            var searchViewInitializer = new SearchEntityAreaInitializer();
            searchViewInitializer.EntityID = relationship.EntityID2;
            searchViewInitializer.Title = relationship.Alias;
            searchViewInitializer.SourceRelationship = relationship;
            advancedAndRaw.SearchDataDefined += (sender, e) => AdvancedAndRaw_SearchDataDefinedNew(sender, e, relationship, node, advancedAndRaw.SearchView);
            advancedAndRaw.SetAreaInitializer(searchViewInitializer);
            AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetDialogWindow().ShowDialog(advancedAndRaw.SearchView, searchViewInitializer.Title, Enum_WindowSize.Big);
        }
        private void AdvancedAndRaw_SearchDataDefinedNew(object sender, SearchDataArg e, RelationshipDTO relationship, AdvanceSearchNode node, object view)
        {
            AgentUICoreMediator.GetAgentUICoreMediator.UIManager.CloseDialog(view);

            if (node.Phrase is LogicPhrase)
                (node.Phrase as LogicPhrase).Phrases.Add(e.SearchItems);

            AddLogicNode(node, e.SearchItems);
        }
        private void Relationship_ClickedEdit(object sender1, EventArgs e1, AdvanceSearchNode node, DP_SearchRepository dP_SearchRepository)
        {
            SearchEntityArea advancedAndRaw = new EntityArea.SearchEntityArea();
            var searchViewInitializer = new SearchEntityAreaInitializer();
            searchViewInitializer.PreDefinedSearch = dP_SearchRepository;
            searchViewInitializer.EntityID = dP_SearchRepository.TargetEntityID;
            searchViewInitializer.Title = dP_SearchRepository.Title;
            searchViewInitializer.SourceRelationship = dP_SearchRepository.SourceRelationship;
            advancedAndRaw.SearchDataDefined += (sender, e) => AdvancedAndRaw_SearchDataDefinedEdit(sender, e, node, advancedAndRaw.SearchView);
            advancedAndRaw.SetAreaInitializer(searchViewInitializer);
            AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetDialogWindow().ShowDialog(advancedAndRaw.SearchView, searchViewInitializer.Title, Enum_WindowSize.Big);
        }
        private void AdvancedAndRaw_SearchDataDefinedEdit(object sender, SearchDataArg e, AdvanceSearchNode node, object view)
        {
            AgentUICoreMediator.GetAgentUICoreMediator.UIManager.CloseDialog(view);

            if (e.SearchItems != null)
            {
                node.ParentNode.NodeManager.RemoveItem(node.NodeManager);
                node.ParentNode.ChildItems.Remove(node);
                (node.ParentNode.Phrase as LogicPhrase).Phrases.Remove(node.Phrase);
                (node.ParentNode.Phrase as LogicPhrase).Phrases.Add(e.SearchItems);
                AddLogicNode(node.ParentNode, e.SearchItems);
            }


        }
        private bool GetParentSearchRepository(AdvanceSearchNode newnode)
        {
            if (newnode.Phrase is DP_SearchRepository)
            {
                if ((newnode.Phrase as DP_SearchRepository) == firstRepository)
                    return true;
                else
                    return false;
            }

            if (newnode.ParentNode == null)
                return false;
            else if (newnode.ParentNode.Phrase is DP_SearchRepository)
            {
                if ((newnode.ParentNode.Phrase as DP_SearchRepository) == firstRepository)
                    return true;
                else return false;
            }
            else
                return GetParentSearchRepository(newnode.ParentNode);
        }

        private void AddSearchPropertyNode(SearchProperty phrase, AdvanceSearchNode andOrNode)
        {
            var newnode = new AdvanceSearchNode();
            newnode.ParentNode = andOrNode;
            newnode.Title = phrase.ColumnID.ToString();
            andOrNode.ChildItems.Add(newnode);
            newnode.NodeManager = andOrNode.NodeManager.AddChildItem();
            newnode.NodeManager.SetHeader(newnode.Title);
            newnode.Phrase = phrase;

            I_AdvanceSearchMenu removeMenu = newnode.NodeManager.AddMenu("حذف");
            removeMenu.Clicked += (sender1, e1) => RemoveMenu_Clicked(sender1, e1, andOrNode, newnode);
        }


        private void RemoveMenu_Clicked(object sender, EventArgs e, AdvanceSearchNode parentNode, AdvanceSearchNode node)
        {
            if (parentNode != null)
            {
                parentNode.NodeManager.RemoveItem(node.NodeManager);
                parentNode.ChildItems.Remove(node);
                if (parentNode.Phrase is LogicPhrase)
                {
                    (parentNode.Phrase as LogicPhrase).Phrases.Remove(node.Phrase);
                }
            }
        }

        //Tuple<int,>
        //  object lastSearchView;
        private void RootAndMenu_Clicked1(object sender1, EventArgs e1, AdvanceSearchNode andOrNode)
        {

            I_RawSearchEntityArea rawSearchEntityArea = new RawSearchEntityArea();
            var searchViewInitializer = new SearchEntityAreaInitializer();
            searchViewInitializer.EntityID = SearchInitializer.EntityID;
            //if (SearchInitializer.TempEntity != null && SearchInitializer.TempEntity.ID == SearchInitializer.EntityID)
            //    searchViewInitializer.TempEntity = SearchInitializer.TempEntity;
            rawSearchEntityArea.SetAreaInitializer(searchViewInitializer);
            rawSearchEntityArea.SearchDataDefined += (sender, e) => RawSearchEntityArea_SearchDataDefined(sender, e, andOrNode, rawSearchEntityArea.RawSearchView);
            //lastSearchView = rawSearchEntityArea.RawSearchView;
            AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetDialogWindow().ShowDialog(rawSearchEntityArea.RawSearchView, "خصوصیات", Enum_WindowSize.Big);

        }
        private void RawSearchEntityArea_SearchDataDefined(object sender, SearchPropertyArg e, AdvanceSearchNode andOrNode, object view)
        {
            AgentUICoreMediator.GetAgentUICoreMediator.UIManager.CloseDialog(view);
            foreach (var phrase in e.SearchItems)
            {
                AddSearchPropertyNode(phrase, andOrNode);
                if (andOrNode.Phrase is LogicPhrase)
                    (andOrNode.Phrase as LogicPhrase).Phrases.Add(phrase);
            }
        }
        public DP_SearchRepository GetSearchRepository()
        {
            if (RootNode != null)
            {
                //if (rootNode.ChildItems.Any())
                //{
                var result = firstRepository;// MakeLogicPhrase(RootNode) as DP_SearchRepository;
                result.IsSimpleSearch = false;
                return result;
                //}
            }
            return null;
        }

        //private LogicPhrase MakeLogicPhrase(AdvanceSearchNode node)
        //{
        //    if (node.Phrase is LogicPhrase)
        //    {
        //        LogicPhrase result;
        //        if (node.Phrase is DP_SearchRepository)
        //        {
        //            result = new DP_SearchRepository((node.Phrase as DP_SearchRepository).TargetEntityID);
        //            (result as DP_SearchRepository).SourceRelationship = (node.Phrase as DP_SearchRepository).SourceRelationship;
        //            (result as DP_SearchRepository).Name = (node.Phrase as DP_SearchRepository).Name;
        //        }
        //        else
        //        {
        //            result = new LogicPhrase();
        //        }
        //        result.AndOrType = (node.Phrase as LogicPhrase).AndOrType;
        //        foreach (var item in node.ChildItems)
        //        {
        //            if (item.Phrase is LogicPhrase)
        //            {
        //                result.Phrases.Add(MakeLogicPhrase(item));
        //            }
        //            else
        //                result.Phrases.Add(item.Phrase);
        //        }
        //        return result;
        //    }
        //    else return null;
        //}

        EntityUISettingDTO _EntityUISetting;
        private EntityUISettingDTO GetEntityUISetting()
        {
            if (_EntityUISetting == null)
            {
                _EntityUISetting = new EntityUISettingDTO();
                _EntityUISetting.UIColumnsCount = 4;
            }
            return _EntityUISetting;
        }

        ColumnUISettingDTO _ColumnUISetting;
        private ColumnUISettingDTO GetColumnUISetting(ColumnDTO column)
        {
            if (_ColumnUISetting == null)
            {
                _ColumnUISetting = new ColumnUISettingDTO();
                _ColumnUISetting.UIColumnsType = Enum_UIColumnsType.Normal;
                _ColumnUISetting.UIRowsCount = 1;
            }
            return _ColumnUISetting;
        }






        public void ClearSearchData()
        {


        }







        public void OnSearchDataDefined(DP_SearchRepository searchData)
        {
            if (SearchDataDefined != null)
            {
                SearchDataDefined(this, new SearchDataArg() { SearchItems = searchData });
            }
        }


    }
}
