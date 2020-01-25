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
    public class EntityValidationManagerService
    {
        BizEntityValidation bizEntityValidation = new BizEntityValidation();
        public List<EntityValidationDTO> GetEntityValidations(int entityID)
        {
          return bizEntityValidation.GetEntityValidations(entityID, true);
        }

      
    }
}
