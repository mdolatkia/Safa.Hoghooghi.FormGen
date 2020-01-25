using DataAccess;
using ModelEntites;
using MyGeneralLibrary;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyModelManager
{
    public class BizRelationshipFilter
    {
        public List<RelationshipFilterDTO> GetRelationshipFilters(DR_Requester requester, int relationshipID)
        {
            List<RelationshipFilterDTO> result = new List<RelationshipFilterDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                IQueryable<RelationshipSearchFilter> listRelationshipFilter;
                listRelationshipFilter = projectContext.RelationshipSearchFilter.Where(x => x.RelationshipID == relationshipID);
                foreach (var item in listRelationshipFilter)
                    result.Add(ToRelationshipFilterDTO(  item));

            }
            return result;
        }
        //public RelationshipFilterDTO GetRelationshipFilter(int RelationshipFiltersID)
        //{
        //    List<RelationshipFilterDTO> result = new List<RelationshipFilterDTO>();
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var RelationshipFilters = projectContext.RelationshipSearchFilter.First(x => x.ID == RelationshipFiltersID);
        //        return ToRelationshipFilterDTO(RelationshipFilters);
        //    }
        //}
        public RelationshipFilterDTO ToRelationshipFilterDTO( RelationshipSearchFilter item)
        {
            RelationshipFilterDTO result = new RelationshipFilterDTO();
            result.ID = item.ID;
            //result.Enabled = item.Enabled;
            BizEntityRelationshipTail bizEntityRelationshipTail = new BizEntityRelationshipTail();
            //if (item.SearchRelationshipTailID != null)
            //{
            //    result.SearchRelationshipTailID = item.SearchRelationshipTailID.Value;
            //    result.SearchRelationshipTail = bizEntityRelationshipTail.ToEntityRelationshipTailDTO(item.EntityRelationshipTail);
            //}
            result.RelationshipID = item.RelationshipID;
            result.SearchColumnID = item.SearchColumnID;
            result.ValueColumnID = item.ValueColumnID;
            result.ValueRelationshipTailID = item.ValueRelationshipTailID ?? 0;
            if (item.EntityRelationshipTail != null)
                result.ValueRelationshipTail = bizEntityRelationshipTail.ToEntityRelationshipTailDTO(  item.EntityRelationshipTail);

  //          result.SearchRelationshipTailID = item.SearchRelationshipTailID ?? 0;
            //if (item.EntityRelationshipTail != null)
            //    result.SearchRelationshipTail = bizEntityRelationshipTail.ToEntityRelationshipTailDTO(item.EntityRelationshipTail);

            //foreach (var column in item.RelationshipFilterColumns)
            //{
            //    result.RelationshipFilterColumns.Add(new RelationshipFilterColumnDTO() { SearchColumnID = column.SearchColumnID, ValueColumnID = column.ValueColumnID });
            //}
            return result;
        }
        public bool UpdateRelationshipFilters(int relationshipID, List<RelationshipFilterDTO> RelationshipSearchFilter)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var relationship = projectContext.Relationship.FirstOrDefault(x => x.ID == relationshipID);
                while (relationship.RelationshipSearchFilter.Any())
                    projectContext.RelationshipSearchFilter.Remove(relationship.RelationshipSearchFilter.First());
                foreach (var item in RelationshipSearchFilter)
                {

                    var dbRelationshipFilter = new DataAccess.RelationshipSearchFilter();
                    dbRelationshipFilter.RelationshipID = relationshipID;
                    // dbRelationshipFilter.Enabled = item.Enabled;
                    //if (RelationshipSearchFilter.SearchRelationshipTailID != 0)
                    //    dbRelationshipFilter.SearchRelationshipTailID = RelationshipSearchFilter.SearchRelationshipTailID;
                    //else
                    //    dbRelationshipFilter.SearchRelationshipTailID = null;
                    dbRelationshipFilter.SearchColumnID = item.SearchColumnID;
                    dbRelationshipFilter.ValueColumnID = item.ValueColumnID;
                    if (item.ValueRelationshipTailID != 0)
                        dbRelationshipFilter.ValueRelationshipTailID = item.ValueRelationshipTailID;
                    else
                        dbRelationshipFilter.ValueRelationshipTailID = null;
                    //if (item.SearchRelationshipTailID != 0)
                    //    dbRelationshipFilter.SearchRelationshipTailID = item.SearchRelationshipTailID;
                    //else
                    //    dbRelationshipFilter.SearchRelationshipTailID = null;
                    //while (dbRelationshipFilter.RelationshipFilterColumns.Any())
                    //    projectContext.RelationshipFilterColumns.Remove(dbRelationshipFilter.RelationshipFilterColumns.First());
                    //foreach (var column in RelationshipSearchFilter.RelationshipFilterColumns)
                    //{
                    //    dbRelationshipFilter.RelationshipFilterColumns.Add(new  RelationshipFilterColumns() { SearchColumnID = column.SearchColumnID, ValueColumnID = column.ValueColumnID });
                    //}

                    projectContext.RelationshipSearchFilter.Add(dbRelationshipFilter);
                }
                projectContext.SaveChanges();

                return true;
            }
        }
    }

}
