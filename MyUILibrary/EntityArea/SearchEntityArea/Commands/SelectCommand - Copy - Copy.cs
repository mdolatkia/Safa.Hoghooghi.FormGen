using CommonDefinitions.UISettings;
using MyUILibrary;
using MyUILibrary.EntityArea;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUILibrary.EntityArea.Commands
{
    class SelectCommand1 : I_ViewAreaCommand
    {
        public bool IsGeneralCommand
        {
            get { return false; }
        }
        public string Name
        {
            get
            {
                return "mySelectCommand";
            }

        }

        public string Title
        {
            get
            {
                return "Test Data";
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string ImagePath
        {
            get
            {
                try
                {
                    return "";
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                throw new NotImplementedException();
            }
        }


        public void Execute(I_EditEntityArea packageArea)
        {

        }


        public void Execute(I_SearchViewEntityArea packageArea)
        {
            var list = new List<DP_DataRepository>();
            Random rnd = new Random();


            var nItem = AgentHelper.CreateEditTemplateNewData(packageArea.ViewTemplate);
            foreach (var item in nItem.DataInstance.Properties)
                item.Value = "i" + rnd.Next(1, 13);

            var nItem1 = AgentHelper.CreateEditTemplateNewData(packageArea.ViewTemplate);
            foreach (var item in nItem1.DataInstance.Properties)
                item.Value = "i" + rnd.Next(4, 22);

            var nItem2 = AgentHelper.CreateEditTemplateNewData(packageArea.ViewTemplate);
            foreach (var item in nItem2.DataInstance.Properties)
                item.Value = "i" + rnd.Next(3, 7);

            list.Add(nItem);
            list.Add(nItem1);
            list.Add(nItem2);
            packageArea.AddData(list, true);
        }



        public List<IntracionMode> CompatibaleIntractionMode
        {
            get { throw new NotImplementedException(); }
        }

        public List<DataMode> CompatibaleDataMode
        {
            get { throw new NotImplementedException(); }
        }


    }
}
