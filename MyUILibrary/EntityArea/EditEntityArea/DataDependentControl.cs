using CommonDefinitions.BasicUISettings;
using CommonDefinitions.UISettings;
using System;
using System.Collections.Generic;
namespace MyUILibrary.EntityArea
{
    public class DataDependentControl : I_View_DataDependentControl
    {

        public void OnTemporaryViewRequested(object sender, Arg_TemporaryDisplayViewRequested arg)
        {
            if (TemporaryViewRequested != null)
                TemporaryViewRequested(sender, arg);
        }


        public DataAccess.Column Column
        {
            set;
            get;
        }

        public IAG_View_TemporaryView GenerateTemporaryView()
        {
            //if (LinkType == TemporaryLinkType.DataView)
            return AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GenerateTemporaryLinkUI(LinkType);
            //else if (LinkType == TemporaryLinkType.SerachView)
            //    return AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GenerateTemporarySearchViewLinkUI();
            //else if (LinkType == TemporaryLinkType.DataSearchView)
            //    return AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GenerateTemporaryDataSearchViewLinkUI();
            //return null;
        }


        public TemporaryLinkType LinkType
        {
            set;
            get;
        }

        public event EventHandler<Arg_TemporaryDisplayViewRequested> TemporaryViewRequested;
    }

}
