using MyRuleEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRuleEngineTest
{
    class TestAction : IAction
    {
        //public bool Evaluate(int t, string h)
        //{
        //    throw new NotImplementedException();

        public ActionResultEnum Execute(object[] objects)
        {
            var customer = ObjectExtractor.Extract<TestEntities.Customer>(objects);
            customer.Name = "Child";
            return ActionResultEnum.Successful;
        }
    }
}
