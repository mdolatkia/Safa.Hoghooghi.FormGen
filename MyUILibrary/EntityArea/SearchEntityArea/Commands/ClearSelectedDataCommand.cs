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
    class ClearSelectedDataCommand : I_ViewAreaCommand
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
                return "Clear Data";
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
                    return "Images//Clear.png";
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


        public void Execute(I_SearchViewEntityArea editArea)
        {
            //if (packageArea.View.SelectedViewPackages != null)
            //{
            //    packageArea.OnDataPackagesSelected(packageArea.View.SelectedViewPackages);
            //}
            //packageArea.ClearSelectedData();
            //DP_DataRepository RelationData = null;
            //if (editArea.SourceEditEntityArea.EditTemplate.SourceRelation != null)
            //{
            //    RelationData = editArea.SourceEditEntityArea.EditTemplate.SourceRelation.RelatedData;
            //}
            editArea.SourceEditEntityArea.ClearData(false);
        }



        public List<IntracionMode> CompatibaleIntractionMode
        {
            get
            {
                List<IntracionMode> list = new List<IntracionMode>();
                list.Add(IntracionMode.Select);
                return list;
            }
        }

        public List<DataMode> CompatibaleDataMode
        {
            get { throw new NotImplementedException(); }
        }

        public int Position
        {
            get
            {
                return 1;
            }
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
    }
}
