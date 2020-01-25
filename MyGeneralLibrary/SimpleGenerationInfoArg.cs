using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGeneralLibrary
{
    public class SimpleGenerationInfoArg : EventArgs
    {
        public int TotalProgressCount;
        public int CurrentProgress;
        public string Title;
    }
}
