using CommonDefinitions.UISettings;
using MyUILibrary;
using MyUILibrary.EntityArea;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUILibrary.EntityArea.Commands
{
    public class SimpleSearchClearCommand : I_Command
    {
        public bool IsGeneralCommand
        {
            get { return true; }
        }
        public string Name
        {
            get
            {
                return "mySelectCommand";
            }

        }

      
        private string _Title;
        public string Title
        {
            get
            {
                return string.IsNullOrEmpty(_Title) ? "پاک کردن" : _Title;
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


      
        //public void Execute(I_BaseSearchEntityArea packageArea)
        //{
           
        //}



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
