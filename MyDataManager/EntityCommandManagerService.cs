using ModelEntites;
using MyDataManagerBusiness;
using MyModelManager;
using MyPackageManager;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDataManagerService
{
    public class EntityCommandManagerService
    {
        BizEntityCommand bizEntityCommand = new BizEntityCommand();
        public List<EntityCommandDTO> GetEntityCommands(DR_Requester requester, int entityID)
        {
            return bizEntityCommand.GetEntityCommands(requester,entityID, true);
        }
    }
}
