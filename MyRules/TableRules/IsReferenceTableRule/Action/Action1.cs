using DataAccess;
using MyRuleEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRules.TableRules.IsReferenceTableRule.Action
{
    public class Action1 : IAction
    {
        public ActionResultEnum Execute(object[] objects)
        {
            var table = ObjectExtractor.Extract<Table>(objects);
            table.IsReferenceDataTable = true;
            return ActionResultEnum.Successful;
        }
    }
}
