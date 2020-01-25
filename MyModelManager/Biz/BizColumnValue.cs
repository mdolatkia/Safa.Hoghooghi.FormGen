using DataAccess;
using ModelEntites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyModelManager
{
    public class BizColumnValue
    {
        //public List<UIColumnValueDTO> GetColumnValue(int ID)
        //{
        //    List<UIColumnValueDTO> result = new List<UIColumnValueDTO>();
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var list = projectContext.ColumnValue.Where(x => x.ID == ID);
        //        foreach (var item in list)
        //        {
        //            result.Add(ToColumnValueDTO(item));
        //        }
        //    }
        //    return result;
        //}

       

        //public List<ColumnValue_RelationshipDTO> GetColumnValue_RelationshipDTO(int arcGroupID)
        //{
        //    List<ColumnValue_RelationshipDTO> result = new List<ColumnValue_RelationshipDTO>();
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var listRel = projectContext.ColumnValue_Relationship.Where(x => x.ColumnValueID == arcGroupID);
        //        foreach (var item in listRel)
        //        {
        //            result.Add(ToColumnValue_RelationshipDTO(item));
        //        }
        //    }
        //    return result;
        //}

        //private ColumnValue_RelationshipDTO ToColumnValue_RelationshipDTO(DataAccess.ColumnValue_Relationship item)
        //{
        //    ColumnValue_RelationshipDTO result = new ColumnValue_RelationshipDTO();
        //    result.TableDrivedEntityStateID = item.TableDrivedEntityStateID;
        //    result.RelationshipID = item.RelationshipID;
        //    result.Enabled = item.Enabled==true;
        //    return result;
        //}
        //private ColumnValue_ColumnDTO ToColumnValue_ColumnDTO(DataAccess.ColumnValue_Column item)
        //{
        //    ColumnValue_ColumnDTO result = new ColumnValue_ColumnDTO();
        //    result.TableDrivedEntityStateID = item.TableDrivedEntityStateID;
        //    result.ColumnID = item.ColumnID;
        //    result.ValidValue = item.ValidValue;

        //    return result;
        //}

        //public void SaveColumnValue(List<ColumnValueDTO> list)
        //{
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        foreach (var item in list)
        //        {
        //            ColumnValue dbItem = null;
        //            if (item.ID == 0)
        //            {
        //                dbItem = new ColumnValue();
        //                projectContext.ColumnValue.Add(dbItem);
        //            }
        //            else
        //                dbItem = projectContext.ColumnValue.First(x => x.ID == item.ID);
        //            dbItem.ColumnID = item.ColumnID;
        //            dbItem.Value = item.Value;
        //            while (dbItem.ColumnValue_Relationship.Any())
        //                dbItem.ColumnValue_Relationship.Remove(dbItem.ColumnValue_Relationship.First());
        //            foreach (var relation in item.Relationships)
        //            {
        //                dbItem.ColumnValue_Relationship.Add(new ColumnValue_Relationship() { RelationshipID = relation.RelationshipID,Enabled=relation.Enabled });
        //            }
        //            while (dbItem.ColumnValue_Column.Any())
        //                dbItem.ColumnValue_Column.Remove(dbItem.ColumnValue_Column.First());
        //            foreach (var column in item.Columns)
        //            {
        //                dbItem.ColumnValue_Column.Add(new ColumnValue_Column() { ColumnID = column.ColumnID, ValidValue = column.ValidValue });
        //            }
        //            projectContext.SaveChanges();
        //        }
        //    }
        //}
    }

}
