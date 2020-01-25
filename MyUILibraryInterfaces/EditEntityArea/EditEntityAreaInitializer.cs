
using CommonDefinitions.UISettings;
using ModelEntites;

using MyUILibraryInterfaces.EditEntityArea;
using ProxyLibrary;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MyUILibrary.EntityArea
{
    public class EditEntityAreaInitializer
    {
        public EditEntityAreaInitializer()
        {
            Datas = new ObservableCollection<DP_DataRepository>();
            UISettings = new TemplateEntityUISettings();
            //FormAttributes = new FormAttributes();
            //DataItemAttributes = new List<DataItemAttributes>();
            //ConditionalPermissions = new List<ConditionalPermissionDTO>();
            //Commands = new List<EntityCommandDTO>();
            //UISettings.FlowDirection = FlowDirection.RightToLeft;
            UISettings.Language = "farsi";
        }
        public EditAreaRelationSource SourceRelation { set; get; }
        //public TableDrivedEntityDTO TemplateEntity { set; get; }
        public int EntityID { set; get; }
        //public string Title
        //{
        //    get
        //    {
        //        return TemplateEntity.Alias;
        //    }
        //}
        public I_EditAreaDataManager EditAreaDataManager { set; get; }
        public I_UIActionActivityManager ActionActivityManager { set; get; }
        public I_RelationshipFilterManager RelationshipFilterManager { get; set; }

        ObservableCollection<DP_DataRepository> _Datas;
        public ObservableCollection<DP_DataRepository> Datas
        {
            set { _Datas = value; }
            get
            {
                if (SourceRelation != null)
                    throw new Exception("sdfsdg");
                return _Datas;
            }
        }
        public TemplateEntityUISettings UISettings { set; get; }
        public DataMode DataMode { set; get; }
        //public DirectionMode DirectionMode { set; get; }
        public IntracionMode IntracionMode { set; get; }
        public int? DataCount { set; get; }
        public bool FormComposed { set; get; }
        //public bool SecurityReadOnly { get; set; }
        //public bool SecurityNoAccess { get; set; }
        //public bool SecurityEditAndDelete { get; set; }
        //public bool SecurityEditOnly { get; set; }
        //public bool SecurityReadOnlyByParent { get; set; }

        public bool BusinessReadOnlyByParent { get; set; }
        public Tuple<DP_DataView, EntityRelationshipTailDTO> TailDataValidation { get; set; }
        public bool Preview { get; set; }
        public I_EditAreaLogManager EntityAreaLogManager { get; set; }
        public I_UIFomulaManager UIFomulaManager { get; set; }
        public I_UIValidationManager UIValidationManager { get; set; }
        
        //  public bool RemoveInfo { get; set; }

        //      public EntityUICompositionCompositeDTO PreviewUICompositionItems { get; set; }

        public List<DP_DataRepository> ParentDataItemBusinessReadOnly = new List<DP_DataRepository>();

        //public List<DP_DataRepository> DataItemBusinessReadOnly = new List<DP_DataRepository>();
        public void AddBusinessReadOnlyByParent(DP_DataRepository dataItem)
        {
            if (!ParentDataItemBusinessReadOnly.Contains(dataItem))
                ParentDataItemBusinessReadOnly.Add(dataItem);
        }
        public void RemoveBusinessReadOnlyByParent(DP_DataRepository dataItem)
        {
            if (ParentDataItemBusinessReadOnly.Contains(dataItem))
                ParentDataItemBusinessReadOnly.Remove(dataItem);
        }
        //public FormAttributes FormAttributes { set; get; }
        //public List<DataItemAttributes> DataItemAttributes { set; get; }



        //public List<ConditionalPermissionDTO> ConditionalPermissions { set; get; }
        //public List<EntityUICompositionDTO> UICompositions { set; get; }
        //public List<EntityValidationDTO> Validations { set; get; }
        //public List<EntityStateDTO> States { get; set;}
        //public List<ActionActivityDTO> ActionActivities { get; set;}
        //public List<EntityCommandDTO> Commands { get; set;}
    }
    //public class EditAreaData
    //{
    //    public EditAreaData(DP_DataRepository data)
    //    {
    //        Data = data;
    //    }
    //    public DP_DataRepository Data { set; get; }
    //    public string BriefInfo { set; get; }
    //}
}
