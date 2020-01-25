using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxyLibrary.Request
{
    public class ArchiveResult
    {
        public bool Result { set; get; }
        public string Message { set; get; }

        public int ID { set; get; }
    }

    public class ArchiveDeleteResult
    {
        public ArchiveDeleteResult()
        {
            Details = new List<ProxyLibrary.ResultDetail>();
            //Items = new List<Request.ArchiveResult>();
        }
        //public List<ArchiveResult> Items { set; get; }
        public string Message { set; get; }
        public List<ResultDetail> Details { set; get; }
    }
}
