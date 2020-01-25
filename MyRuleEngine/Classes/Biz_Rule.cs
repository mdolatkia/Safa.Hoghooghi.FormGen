using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRuleEngine
{
    public class Biz_Rule
    {
        public ICondition Condition { set; get; }
        public IAction Action { set; get; }
        public int Priority { set; get; }
    }
}
