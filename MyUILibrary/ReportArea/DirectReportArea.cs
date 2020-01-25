using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelEntites;
using ProxyLibrary;
using MyCodeFunctionLibrary;

using MyUILibraryInterfaces.DataTreeArea;
using MyUILibrary.DataTreeArea;
using MyUILibraryInterfaces.EntityArea;

namespace MyUILibrary.EntityArea
{
    class DirectReportArea : I_DirectReportArea
    {



        public bool SecurityNoAccess { set; get; }
        public bool SecurityReadonly { set; get; }
        public bool SecurityEdit { set; get; }

        public I_EntitySelectArea EntitySelectArea { set; get; }
        public object MainView
        {
            set; get;
        }
        EntityDirectReportDTO EntityDirectReport { set; get; }
        public DirectReportAreaInitializer AreaInitializer { set; get; }
        public DirectReportArea(DirectReportAreaInitializer areaInitializer)
        {
            //بهتره اکسترنال ویو ست بشه .. یعنی یک فرم و کنترل که مثل مرورگر باشد
            AreaInitializer = areaInitializer;
            EntityDirectReport = AgentUICoreMediator.GetAgentUICoreMediator.ReportManager.GetEntityDirectReport(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), areaInitializer.ReportID);
            if (EntityDirectReport == null)
            {
                AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowInfo("دسترسی به گزارش به شناسه" + " " + areaInitializer.ReportID + " " + "امکانپذیر نمی باشد", "", Temp.InfoColor.Red);
                return;
            }
            MyUILibraryInterfaces.EntityArea.EntitySelectAreaInitializer selectAreaInitializer = new MyUILibraryInterfaces.EntityArea.EntitySelectAreaInitializer();
            selectAreaInitializer.EntityID = EntityDirectReport.TableDrivedEntityID;
            selectAreaInitializer.LockEntitySelector = true;
            EntitySelectArea = new EntitySelectArea.EntitySelectArea(selectAreaInitializer);
            EntitySelectArea.DataItemSelected += EntitySelectArea_DataItemSelected;
            MainView = EntitySelectArea.View;
            if (areaInitializer.DataInstance != null)
            {
                EntitySelectArea.EnableDisableSelectArea(false);
                EntitySelectArea.SelectData(areaInitializer.DataInstance);
            }
        }

        private void EntitySelectArea_DataItemSelected(object sender, EditAreaDataItemArg e)
        {
            var url = EntityDirectReport.URL;
            if (e.DataItem != null)
            {
                foreach (var item in e.DataItem.KeyProperties)
                {
                    var param = item.Column.Name + "=" + item.Value;
                    if (url.Contains("?"))
                        url += "&" + param;
                    else
                        url += "?" + param;
                }
                System.Diagnostics.Process.Start(url);
            }
        }


        //DP_DataView MainDataInstance { set; get; }







    }
}
