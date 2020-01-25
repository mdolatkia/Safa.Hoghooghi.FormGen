using ModelEntites;
using MyCodeFunctionLibrary;
using MyDataManagerBusiness;

using MyFormulaFunctionStateFunctionLibrary;
using MyInterfaces;
using MyDataItemManager;
using MyModelManager;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyLogManager;

namespace MyDataManagerService
{

    public class SecurityManagerService
    {
        SecurityHelper securityHelper = new SecurityHelper();
        public LoginResult Login(string userName, string passWord)
        {
            return securityHelper.Login(userName, passWord);
        }

    }
}
