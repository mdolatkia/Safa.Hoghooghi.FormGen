using ModelEntites;
using MyModelManager;

using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRelationshipDataManager
{
    public class RelationshipTailDataManager
    {
        BizRelationship bizRelationship = new BizRelationship();
        BizEntityRelationshipTail bizRelationshipTail = new BizEntityRelationshipTail();
       // SecurityHelper securityHelper = new SecurityHelper();
        //public EntityRelationshipTailDTO GetRelationshipTail(int id)
        //{
        //    return bizRelationshipTail.GetEntityRelationshipTail(id);
        //}
        //مهم اینه که داده مبدا از ویو نباشد
        public DP_SearchRepository GetTargetSearchItemFromRelationshipTail(DP_BaseData firstDataItem, EntityRelationshipTailDTO relationshipTail)
        {
            DP_SearchRepository searchItem = null;
            var linkedRelationshipTails = GetLinkedRelationshipTails(relationshipTail.ReverseRelationshipTail);
            searchItem = GetSourceSideSearchItemFromRelationshipTail(linkedRelationshipTails.First, true, firstDataItem);
            return searchItem;
        }

            private DP_SearchRepository GetSourceSideSearchItemFromRelationshipTail(LinkedListNode<EntityRelationshipTailDTO> linkedListNode, bool addPreDefinedSearch, DP_BaseData lastDataItem)
        {

            if (linkedListNode != null)
            {
                DP_SearchRepository result = new DP_SearchRepository(linkedListNode.Value.Relationship.EntityID1);
                var childPhrase = GetSourceSideSearchItemFromRelationshipTail(linkedListNode.Next, addPreDefinedSearch, lastDataItem);
                childPhrase.SourceRelationship = linkedListNode.Value.Relationship;
                result.Phrases.Add(childPhrase);
                return result;
            }
            else
            {
                DP_SearchRepository result = new DP_SearchRepository(lastDataItem.TargetEntityID);
                foreach (var column in lastDataItem.KeyProperties)
                    result.Phrases.Add(new SearchProperty() { ColumnID = column.ColumnID, Value = column.Value });
                return result;

            }
                     
        }
            

      
        private LinkedList<EntityRelationshipTailDTO> GetLinkedRelationshipTails(EntityRelationshipTailDTO firstRelationshipTail)
        {
            LinkedList<EntityRelationshipTailDTO> result = new LinkedList<EntityRelationshipTailDTO>();

            List<EntityRelationshipTailDTO> list = GetListOfRelationshipTails(firstRelationshipTail);
            foreach (var item in list)
            {
                result.AddLast(item);
            }
            return result;
        }

        private List<EntityRelationshipTailDTO> GetListOfRelationshipTails(EntityRelationshipTailDTO relationshipTail, List<EntityRelationshipTailDTO> items = null)
        {
            if (items == null)
                items = new List<EntityRelationshipTailDTO>();
            if (relationshipTail != null)
            {
                items.Add(relationshipTail);
                return GetListOfRelationshipTails(relationshipTail.ChildTail, items);
            }
            else
                return items;
        }


    }
    public enum RelationshipTailSreachType
    {
        SourceBasedOnTargetKeyColumn,
        TargetBasedOnSourceKeyColumn
    }
}
