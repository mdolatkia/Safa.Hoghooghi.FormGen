using MyRuleEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRuleEngineTest
{
    class TestCondition : ICondition
    {
        //public bool Evaluate(int t, string h)
        //{
        //    throw new NotImplementedException();

        public bool Evaluate(params object[] objects)
        {
            var customer = ObjectExtractor.Extract<TestEntities.Customer>(objects);
            return customer.Age < Biz_Vocabulary.GetVocabulary<int>("Age", objects);
        }
    }
}
