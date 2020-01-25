using MyReportRestServices.ReportResolvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Telerik.Reporting.Services.WebApi;

namespace MyReportRestServices.Controllers
{
    public class MyReportController : ReportsControllerBase
    {
        static Telerik.Reporting.Services.ReportServiceConfiguration configurationInstance =
          new Telerik.Reporting.Services.ReportServiceConfiguration
          {
              HostAppId = "Application1",
              ReportResolver = new MyReportResolver()
                ,
              Storage = new Telerik.Reporting.Cache.File.FileStorage(),
          };

        public MyReportController()
        {
            this.ReportServiceConfiguration = configurationInstance;
        }
    }
}
