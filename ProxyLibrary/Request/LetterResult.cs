using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxyLibrary.Request
{
    public class LetterResult
    {
        //public bool Result { set; get; }
        public string Message { set; get; }

        public List<ResultDetail> Details { set; get; }
        public bool Result { get; set; }
        public int SavedID { set; get; }
    }

    public class LetterDeleteResult
    {
        public LetterDeleteResult()
        {
            Details = new List<ProxyLibrary.ResultDetail>();
            //Items = new List<Request.ArchiveResult>();
        }
        //public List<ArchiveResult> Items { set; get; }
        public bool Result { set; get; }
        public string Message { set; get; }
        public List<ResultDetail> Details { set; get; }
    }
}
