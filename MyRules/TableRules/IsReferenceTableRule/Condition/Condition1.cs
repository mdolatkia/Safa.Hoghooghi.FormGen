using DataAccess;
using MyRuleEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRules.TableRules.IsReferenceTableRule.Condition
{
    public class Condition1 : ICondition
    {
        public bool Evaluate(params object[] objects)
        {
            var table = ObjectExtractor.Extract<Table>(objects);
            return table.Column.Count <= Biz_Vocabulary.GetVocabulary<int>("Vocabluray_ReferenceTableColumnsCount", objects);
        }
    }
}
