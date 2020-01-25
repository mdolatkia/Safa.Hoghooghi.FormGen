using DataAccess;
using MyDataManagerBusiness;
using MyRuleEngine;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRules.TableRules.TableDrivedEntity_IsAssociative.Action
{
    public class Action1 : IAction
    {
        public event EventHandler<ActionInfoArg> ActionEvent;
        public ActionResultEnum Execute(object[] objects)
        {
            var entity = ObjectExtractor.Extract<TableDrivedEntity>(objects);
        
            if (entity.IsAssociative == null)
            {
                if (entity.Relationship.Count(x => x.RelationshipType != null && x.RelationshipType.ManyToOneRelationshipType != null && x.TableDrivedEntityID1 != x.TableDrivedEntityID2 && (x.TableDrivedEntity1.IsDataReference != true)
                    && (x.TableDrivedEntity1.IsStructurReferencee != true)) > 1)
                {
                    entity.IsAssociative = true;
                }
            }
            return ActionResultEnum.Successful;
        }
    }


}
