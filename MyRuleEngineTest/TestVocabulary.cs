using MyRuleEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRuleEngineTest
{
    class TestVocabulary : IVocabulary
    {
        //public bool Evaluate(int t, string h)
        //{
        //    throw new NotImplementedException();

        public object GetVocabulary(params object[] objects)
        {
            return 5;
        }
    }
}
