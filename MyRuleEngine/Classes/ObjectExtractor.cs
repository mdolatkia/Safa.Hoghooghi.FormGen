using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRuleEngine
{
    public class ObjectExtractor
    {
        public static T Extract<T>(params object[] objects)
        {
            foreach (var obj in objects)
            {
                if (obj.GetType() == typeof(T))
                    return (T)obj;
                else if (ObjectContext.GetObjectType(obj.GetType()) == typeof(T))
                    return (T)obj;

            }
            return default(T);
        }

    }
}
