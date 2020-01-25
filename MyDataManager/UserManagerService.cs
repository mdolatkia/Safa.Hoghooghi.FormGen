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
    public class UserManagerService
    {
        BizUser bizUser = new BizUser();
        public List<UserDTO> SearchUsersByString(string search)
        {
            //رکوئستر گرفته و فیلتر شود
            return bizUser.GetAllUsers(search);
        }
        public UserDTO GetUser(int userID)
        {
            return bizUser.GetUser(userID);
        }

      
    }
}
