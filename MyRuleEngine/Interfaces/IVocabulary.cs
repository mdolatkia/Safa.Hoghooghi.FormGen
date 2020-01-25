using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRuleEngine
{
    public interface IVocabulary
    {
        object GetVocabulary(params object[] objects);
    }
}
