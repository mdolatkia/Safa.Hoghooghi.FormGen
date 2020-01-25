using CommonDefinitions.UISettings;
using MyUILibrary;
using MyUILibrary.PackageArea;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUILibrary.EntityArea.Commands
{
    class TestCommand : I_EntityAreaCommand
    {
        public string Name
        {
            get
            {
                return "myClearCommand";
            }

        }

        public string Title
        {
            get
            {
                return "Test";
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public byte[] Image
        {
            get
            {
                try
                {
                    return AgentHelper.GetBytesFromFilePath("Icons\\png_Clear.png");
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


        //public void Execute(I_EditEntityArea packageArea)
        //{

        //}


        public void Execute(I_SearchViewEntityArea packageArea)
        {
            //////var newDataPackage = AgentHelper.Clone<DataManager.DataPackage.DP_Package>(packageArea.SearchTemplate);
            //////packageArea.SearchDataPackage = newDataPackage;
            //////packageArea.View.ShowSearchDataPckage(packageArea.SearchDataPackage);
        }

        public List<IntracionMode> CompatibaleIntractionMode
        {
            get { throw new NotImplementedException(); }
        }

        public List<DataMode> CompatibaleIntractionDataMode
        {
            get { throw new NotImplementedException(); }
        }

        //public void Execute(I_EditPackageArea packageArea)
        //{
        //    //packageArea.EditTemplate.Data.Clear();


        //    packageArea.MainNDTypeArea.RemoveData(packageArea.MainNDTypeArea.DataRepository);
        //    packageArea.MainNDTypeArea.ClearUIData();
        //    //if (packageArea.MainNDTypeArea.EditTemplate.Template.TypeCondition != null)
        //    //{
        //    //    data.TypeID = packageArea.MainNDTypeArea.EditTemplate.Template.TypeCondition.NDTypeID;
        //    //    data.TypeConditionID = packageArea.MainNDTypeArea.EditTemplate.Template.TypeCondition.ID;
        //    //}
        //    //else
        //    //    data.TypeID = packageArea.MainNDTypeArea.EditTemplate.Template.Type.ID;
        //    //var list = new List<DP_DataRepository>();
        //    //list.Add(data);
        //    //packageArea.AddData(list);

        //    //packageArea.ShowData(list);
        //}

        public void Execute(I_EditEntityArea editArea)
        {
            editArea.View.DataView.SetBackgroundColor("FF00E6E6");
        }
    }
}
