using MyUILibraryInterfaces.DataReportArea;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyUILibrary.EntityArea;
using ProxyLibrary;
using ModelEntites;

namespace MyUILibrary.ChartReportArea
{
    public class ChartReportArea : I_ChartReportArea
    {
        public ChartReportAreaInitializer AreaInitializer
        {
            set; get;
        }

        public bool InitialSearchShouldBeIncluded
        {
            set; get;
        }

        public I_View_ChartReportArea View
        {
            set; get;
        }

        public event EventHandler DataItemsSearchedByUser;

        public void SetAreaInitializer(ChartReportAreaInitializer initParam)
        {
            throw new NotImplementedException();
        }
    }
}
