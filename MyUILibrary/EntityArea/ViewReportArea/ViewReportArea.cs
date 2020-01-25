using CommonDefinitions.UISettings;
using ModelEntites;
using MyUILibrary;
using MyUILibrary.EntityArea;
using MyUILibrary.EntityArea.Commands;
using MyUILibrary.PackageArea.Commands;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySecurity;

namespace MyUILibrary.EntityArea
{
    public class ViewReportArea : I_ViewReportArea
    {
        public ViewReportAreaInitializer ViewInitializer
        {
            set; get;
        }

        public I_View_ViewReportArea ReportView
        {
            set; get;
        }

        public ViewReportArea()
        {

        }

        public void SetAreaInitializer(ViewReportAreaInitializer initParam)
        {
            ViewInitializer = initParam;
        }

        public void GenerateView()
        {
            ReportView = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GenerateViewOfViewReportArea();
        }
    }
}
