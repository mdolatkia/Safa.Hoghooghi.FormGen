using CommonDefinitions.UISettings;
using MyUILibrary.PackageArea;
using System;
using System.Collections.Generic;
namespace MyUILibrary
{
    public interface I_Command
    {
        I_CommandManager CommandManager { set; get; }
    }
    public interface I_CommandManager
    {
        event EventHandler EnabledChanged;
        void SetEnabled(bool enabled);
        void OnEnabledChanged();

        event EventHandler Clicked;
        void SetTitle(string title);

        string ImagePath
        {
            get;
            set;
        }
      
    }

}
