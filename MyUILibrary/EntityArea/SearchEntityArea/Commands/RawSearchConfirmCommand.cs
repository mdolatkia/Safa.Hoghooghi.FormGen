using CommonDefinitions.UISettings;
using MyUILibrary;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUILibrary.EntityArea.Commands
{
    public class RawSearchConfirmCommand : BaseCommand
    {
        I_RawSearchEntityArea SearchArea { set; get; }
        public RawSearchConfirmCommand(I_RawSearchEntityArea searchArea) : base()
        {
            SearchArea = searchArea;
            CommandManager.SetTitle("جستجو");
            CommandManager.ImagePath = "Images//search.png";
            CommandManager.Clicked += CommandManager_Clicked;
        }
        private void CommandManager_Clicked(object sender, EventArgs e)
        {


            //Enabled = false;
            //try
            //{
            var logicPhrase = SearchArea.GetSearchRepository();
            SearchArea.OnSearchDataDefined(logicPhrase);
            //Enabled = true;
        }
        public bool IsGeneralCommand
        {
            get { return true; }
        }
        public string Name
        {
            get
            {
                return "mySearchCommand";
            }

        }


        private string _Title;
        public string Title
        {
            get
            {
                return string.IsNullOrEmpty(_Title) ? "جستجو" : _Title;
            }
            set
            {
                _Title = value;
            }
        }
        public string ImagePath
        {
            get
            {
                try
                {
                    return "Images//Search.png";
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




            ////packageArea.DataPackages=
            ////////var command = new AG_CommandExecutionRequest();
            ////////command.Request = new DR_Request();
            ////////command.Request.Type = Enum_DR_RequestType.Edit;
            ////////command.Request.RequestExecutionTime = new List<DR_RequestExecutionTime>();
            ////////command.Request.RequestExecutionTime.Add(new DR_RequestExecutionTime() { EnumType = Enum_DR_ExecutionTime.Now });
            ////////command.Request.EditRequest = new DR_RequestEdit();
            //////////////   command.Request.EditRequest.EditPackages = packageArea.DataPackages;
            ////////command.SourcePackageArea = packageArea as IAG_PackageArea;
            ////////command.DestinationPackageArea = packageArea as IAG_PackageArea;
            ////////AgentUICoreMediator.GetAgentUICoreMediator.ExecuteCommand(command);





            //if (packageArea.View.SearchViewView != null)
            //{
            //    AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetDialogWindow().ShowDialog(packageArea.View.SearchViewView.SearchViewArea);
            //}
            //var request = new DataManager.PackageManager.DP_RequestRelatedPackage();
            //request.Packages = packageArea.AreaInitializer.
            //request.RelationCategories = new List<DataManager.DataPackage.DP_PackageRelationCategory>();
            //request.RelationCategories.Add(new DataManager.DataPackage.DP_PackageRelationCategory() { Name = "Search" });
            //request.RelationCategories.Add(new DataManager.DataPackage.DP_PackageRelationCategory() { Name = "View" });
            //var result = AgentUICoreMediator.GetAgentUICoreMediator.GetRelatedPackage(request);
            //if (result.RelatedPackages.Count == 0)
            //{
            //    var initializer = new SearchViewPackageAreaInitializer();
            //    initializer.AllowSelect = true;
            //    initializer.SelectCount = 1;
            //    initializer.SearchDataPackageTemplate = packageArea.AreaInitializer.
            //    initializer.ViewhDataPackageTemplate = packageArea.AreaInitializer.
            //    initializer.SourceEditPackageArea = packageArea;
            //    initializer.UISetting = UISettingHelper.DefaultPackageAreaSetting;
            //    AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GenerateSearchViewPackageArea(initializer);

            //    SearchViewPackageArea container = new SearchViewPackageArea();
            //    container.LoadTemplate(initializer);
            //    container.DataPackageSelected += packageArea_dataPackageSelected;
            //    AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowSearchViewPackageArea(initializer.View);
            //}




        }

        void packageArea_dataPackageSelected(object sender, Arg_PackageSelected e)
        {
            //e.SourceEditPackageArea.AddDataPackages(e.Packages);
            //e.SourceEditPackageArea.View.ShowDataPckages(e.Packages);
            ////ایونت دیسپوز شود

            //if (sender is I_SearchViewPackageArea)
            //{
            //    (sender as I_SearchViewPackageArea).DataPackageSelected -= packageArea_dataPackageSelected;
            //    AgentUICoreMediator.GetAgentUICoreMediator.UIManager.CloseSearchViewPackageArea((sender as I_SearchViewPackageArea).View);
            //}
        }
        bool _Enabled;
        public bool Enabled
        {
            get
            {
                return _Enabled;
            }
            set
            {
                _Enabled = value;
                OnEnabledChanged();
            }
        }

        public event EventHandler EnabledChanged;


        public void OnEnabledChanged()
        {
            if (EnabledChanged != null)
                EnabledChanged(this, null);
        }

        public void Execute(I_BaseSearchEntityArea searchEntityArea)
        {
            //////  searchEntityArea.UpdateSearchData();


            //var command = new AG_CommandExecutionRequest();
            //command.Request = new DR_Request();
            //command.Request.Type = Enum_DR_RequestType.SearchView;
            //command.Request.RequestExecutionTime = new List<DR_RequestExecutionTime>();
            //command.Request.RequestExecutionTime.Add(new DR_RequestExecutionTime() { EnumType = Enum_DR_ExecutionTime.Now });
            //command.Request.SearchViewRequest = new DR_SearchViewRequest();
            //command.Request.SearchViewRequest.SearchDataItem = searchEntityArea.GetData();
            //command.Request.SearchViewRequest.EntityID = searchEntityArea.SearchInitializer.SearchEntity.ID;
            //  command.Request.SearchViewRequest.ViewPackage = searchViewArea.SearchViewInitializer.ViewEntity;
            //////   command.Request.EditRequest.EditPackages = packageArea.DataPackages;
            //command.SourceSearchView = searchViewArea;

            //var result = AgentUICoreMediator.GetAgentUICoreMediator.ExecuteCommand(command);
            //if (result.SearchViewResult.DPPackages.Count > 0)
            //    searchEntityArea.ViewEntityArea.AddData(result.SearchViewResult.DPPackages, true);


            //var result = AgentUICoreMediator.GetAgentUICoreMediator.ExecuteCommand(command);
            //if (result.SearchViewResult.DPPackages.Count > 0)
            //    searchEntityArea.ViewEntityArea.AddData(result.SearchViewResult.DPPackages, true);

            //AgentUICoreMediator.GetAgentUICoreMediator.ExecuteCommand(command);

            //command.Request.EditRequest.EditPackages =
            //command.Request.EditRequest.RemovedPackages = packageArea.MainEntityArea.GetRemovedData();
            //command.SourcePackageArea = packageArea as IAG_PackageArea;
            //command.DestinationPackageArea = packageArea as IAG_PackageArea;



            //}
            //else
            //{
            //    AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowInfo(searchEntityArea.SearchInitializer.Title + " : " + "داده ای جهت ورود اطلاعات موجود نمیباشد", "", MyUILibrary.Temp.InfoColor.Red);
            //}
            //}

            //catch (Exception ex)
            //{
            //    var mesage = ex.Message;
            //    mesage += (ex.InnerException != null ? Environment.NewLine + ex.InnerException.Message : "");
            //    AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowInfo(searchEntityArea.SearchInitializer.Title + " : " + "خطا در عملیات", mesage, MyUILibrary.Temp.InfoColor.Red);
            //}



        }

        public List<IntracionMode> CompatibaleIntractionMode
        {
            get { throw new NotImplementedException(); }
        }

        public List<DataMode> CompatibaleDataMode
        {
            get { throw new NotImplementedException(); }
        }
        public int Position
        {
            get
            {
                return 2;
            }
        }

    }
}
