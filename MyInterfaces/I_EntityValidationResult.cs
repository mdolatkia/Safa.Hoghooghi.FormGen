using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyInterfaces
{
    public interface I_EntityValidationResult
    {
        string Message { set; get; }
        bool Result { set; get; }
    }
}
