using CommonDefinitions.UISettings;
using MyUILibrary;
using MyUILibrary.PackageArea;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUILibrary.EntityArea.Commands
{
    class UpdateDataCommand : I_EntityAreaCommand
    {
        public string Name
        {
            get
            {
                return "myUpdateDataCommand";
            }

        }

        public string Title
        {
            get
            {
                return "Update Data";
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


        //public void Execute(I_EditEntityArea packageArea)
        //{

        //}


        public void Execute(I_SearchViewEntityArea packageArea)
        {
            //var newDataPackage = AgentHelper.Clone<DataManager.DataPackage.DP_Package>(packageArea.SearchTemplate);
            //packageArea.SearchDataPackage = newDataPackage;
            //packageArea.View.ShowSearchDataPckage(packageArea.SearchDataPackage);
        }

        public List<IntracionMode> CompatibaleIntractionMode
        {
            get { throw new NotImplementedException(); }
        }

        public List<DataMode> CompatibaleDataMode
        {
            get { throw new NotImplementedException(); }

        }

        //public void Execute(I_EditPackageArea packageArea)
        //{
        //    packageArea.EditTemplate.Data.Clear();


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
            //List<DP_DataRepository> RelationData = null;
            //if (editArea.EditTemplate.SourceRelation != null)
            //{
            //    RelationData = editArea.EditTemplate.SourceRelation.RelationData;
            //}
            //  var ndTypeData = AgentHelper.ExtractDataFromDataRepository(packageData, MainEntityArea.TemplateEntity.Template, null, DataManager.DataPackage.Enum_DP_RelationSide.FirstSide);
             editArea.UpdateData();

            //     editArea.ShowData(AgentHelper.CreateListFromSingleObject<DP_DataRepository>(newData));
        }


        public bool IsGeneralCommand
        {
            get { return false; }
        }


    }
}
