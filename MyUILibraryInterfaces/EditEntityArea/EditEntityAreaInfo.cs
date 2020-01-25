using ModelEntites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUILibrary.EntityArea
{
    public class EditEntityAreaInfo
    {
        public EditEntityAreaInfo()
        {
            DataInfo = new List<EditEntityAreaDataInfo>();
            RemovedDataInfo = new List<EditEntityAreaDataInfo>();
        }
        public int DataCount { set; get; }
        public List<EditEntityAreaDataInfo> DataInfo { set; get; }
        public List<EditEntityAreaDataInfo> RemovedDataInfo { set; get; }
        public List<RelationshipDTO> SkippedRelationships { set; get; }
        public bool FormComposed { set; get; }
        public string TemplateEntityName { set; get; }

        public string DataMode { set; get; }
        public string DirectionMode { set; get; }
        public string IntracionMode { set; get; }

        public string SourceRalationType { set; get; }
        public string SourceEntityName { set; get; }
        public string SourceRalationName { set; get; }
        public bool relationIsMandatory { set; get; }
    }
    public class EditEntityAreaDataInfo
    {
        public EditEntityAreaDataInfo()
        {
            RelatedDataInfo = new List<EditEntityAreaDataInfo>();
        }
        public bool IsNew { set; get; }
        public string ColumnWithValues { set; get; }
        public bool ExcludeFromDataEntry { set; get; }

        public bool HasData { set; get; }

        public List<EditEntityAreaDataInfo> RelatedDataInfo { set; get; }
    }
}
