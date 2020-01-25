using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRuleEngine
{
   public interface ICondition
    {
        bool Evaluate(params object[] objects);
    }
}
