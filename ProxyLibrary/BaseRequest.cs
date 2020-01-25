using ModelEntites;
using System;
using System.Collections.Generic;
namespace ProxyLibrary
{
    public class BaseRequest
    {

        public BaseRequest()
        {
           
        }
        public BaseRequest(DR_Requester requester)
        {
            Requester = requester;
            //RequestExecutionTime = new List<DR_RequestExecutionTime>();
        }

        public SecurityMode SecurityMode { set; get; }
        public Guid Identity;


        public string Name;


        //public Enum_DR_RequestType Type;


        public DR_Requester Requester;


        //public List<DR_RequestExecutionTime> RequestExecutionTime;







    }





}
