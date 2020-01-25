using MyUILibrary.EntityArea;
using MyUILibrary.WorkflowArea;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Telerik.Windows.Controls.Diagrams;
using Telerik.Windows.Diagrams.Core;

namespace MyUIGenerator.View
{
    class CustomConnector : I_View_StateConnection
    {
        CustomConnectorContent Content;
        public CustomConnector()
        {
            Content = new CustomConnectorContent();
            Content.MouseLeftButtonUp += Content_MouseLeftButtonUp;
            //Content.Cursor = Cursors.Hand;
        }

        private void Content_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Clicked != null)
            {
                Clicked(this, null);
            }
        }

        IConnection _Connector;
        public IConnection Connector
        {
            set
            {
                _Connector = value;
                _Connector.Content = Content;
            }
        }
  

        public string OrgnizatoinPostUserInfo
        {
            set
            {

                Content.lblUser.Text = value;
            }
        }
        public string Action
        {
            set
            {
                Content.lblAction.Text = value;
            }
        }
      
        public string Duration
        {
            set
            {
                Content.lblDuration.Text = value;
            }
        }
    
        public string Tooltip
        {
            set
            {
                ToolTipService.SetToolTip(Content, value);
            }
        }

        public bool Highlight
        {

            set
            {
                Content.Highlight = value;
            }
        }

        public event EventHandler Clicked;
    }
}
