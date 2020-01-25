using ModelEntites;
using MyModelManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDataManagerService
{
    public class EntityUICompositionService
    {
        public EntityUICompositionCompositeDTO GetEntityUICompositionTree(int entityID)
        {
            BizEntityUIComposition bizEntityUIComposition = new BizEntityUIComposition();
            return bizEntityUIComposition.GetEntityUICompositionTree(entityID);
        }
    }
}
