using DataAccess;
using ModelEntites;
using MyDataManagerBusiness;
using MyModelManager;
using MyRuleEngine;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRules.TableRules.TableDrivedEntity_IsReference.Action
{
    public class Action1 : IAction
    {
        public ActionResultEnum Execute(object[] objects)
        {
            var entity = ObjectExtractor.Extract<TableDrivedEntity>(objects);
            if (entity.IsDataReference == null && entity.IsStructurReferencee == null)
                if (entity.Relationship.Where(x => x.RelationshipType != null && x.RelationshipType.OneToManyRelationshipType != null).Count() > 0)
                {

                    if (entity.Relationship.Where(x => x.RelationshipType != null && (x.RelationshipType.ImplicitOneToOneRelationshipType != null ||
                        x.RelationshipType.SuperToSubRelationshipType != null ||
                        (x.RelationshipType.UnionToSubUnionRelationshipType != null && x.RelationshipType.UnionToSubUnionRelationshipType.UnionRelationshipType.UnionHoldsKeys == true) ||
                        (x.RelationshipType.SubUnionToUnionRelationshipType != null && x.RelationshipType.SubUnionToUnionRelationshipType.UnionRelationshipType.UnionHoldsKeys == false))).Count() == 0)
                    {
                        BizColumn biz = new BizColumn();
                        var columns = biz.GetColumns(entity.ID,true);
                        if (columns.Count <= 4)
                        {
                            if (columns.Any(x => x.ColumnType == Enum_ColumnType.String))
                                entity.IsDataReference = true;
                        }
                    }
                }

            if (entity.IsDataReference == null && entity.IsStructurReferencee == null)
                if (entity.Relationship.Count(x => x.RelationshipType != null && x.RelationshipType.ImplicitOneToOneRelationshipType != null) > 1)
                {
                    if (entity.Relationship.Where(x => x.RelationshipType != null && (x.RelationshipType.OneToManyRelationshipType != null ||
                      x.RelationshipType.SuperToSubRelationshipType != null ||
                      (x.RelationshipType.UnionToSubUnionRelationshipType != null && x.RelationshipType.UnionToSubUnionRelationshipType.UnionRelationshipType.UnionHoldsKeys == true) ||
                      (x.RelationshipType.SubUnionToUnionRelationshipType != null && x.RelationshipType.SubUnionToUnionRelationshipType.UnionRelationshipType.UnionHoldsKeys == false))).Count() == 0)
                    {
                        if (entity.IsStructurReferencee == null)
                        {
                            entity.IsStructurReferencee = true;
                        }
                    }
                }
            return ActionResultEnum.Successful;
        }




        public event EventHandler<ActionInfoArg> ActionEvent;
    }


}
