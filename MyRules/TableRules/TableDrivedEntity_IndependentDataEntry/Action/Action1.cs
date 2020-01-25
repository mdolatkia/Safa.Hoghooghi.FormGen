using DataAccess;
using MyDataManagerBusiness;
using MyRuleEngine;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRules.TableRules.TableDrivedEntity_IndependentDataEntry.Action
{
    public class Action1 : IAction
    {
        public event EventHandler<ActionInfoArg> ActionEvent;
        public ActionResultEnum Execute(object[] objects)
        {
            var entity = ObjectExtractor.Extract<TableDrivedEntity>(objects);

            if (entity.IndependentDataEntry == null)
            {
                if ((entity.IsStructurReferencee == null || entity.IsStructurReferencee == false)
                   && (entity.IsAssociative == null || entity.IsAssociative == false))
                    entity.IndependentDataEntry = true;
            }
            return ActionResultEnum.Successful;
        }

        //public event EventHandler<ActionInfoArg> ActionEvent;
    }


}
