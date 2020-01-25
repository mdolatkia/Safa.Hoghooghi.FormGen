using DataAccess;
using MyRuleEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRules.TableRules.TableDrivedEntity_IndependentDataEntry.Condition
{
    public class Condition1 : ICondition
    {
        public bool Evaluate(params object[] objects)
        {
            //var entity = ObjectExtractor.Extract<Entity>(objects);
            return true;
        }
    }
}
