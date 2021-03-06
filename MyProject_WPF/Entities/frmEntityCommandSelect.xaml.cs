﻿
using ModelEntites;
using MyFormulaFunctionStateFunctionLibrary;
using MyModelManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Telerik.Windows.Controls;


namespace MyProject_WPF
{
    /// <summary>
    /// Interaction logic for frmFormula.xaml
    /// </summary>
    public partial class frmEntityCommandSelect : UserControl
    {
        public event EventHandler<EntityCommandSelectedArg> EntityCommandSelected;
        //public int EntityID { set; get; }
        BizEntityCommand bizEntityCommand = new BizEntityCommand();
        public frmEntityCommandSelect()
        {
            InitializeComponent();

            //EntityID = entityID;
            GetEntityCommands();
        }

     
        private void GetEntityCommands()
        {
            var listEntityCommands = bizEntityCommand.GetEntityCommands(MyProjectManager.GetMyProjectManager.GetRequester(), 0 ,false);
            dtgItems.ItemsSource = listEntityCommands;
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }
        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            var item = dtgItems.SelectedItem as EntityCommandDTO;
            if (item != null)
            {
                if (EntityCommandSelected != null)
                    EntityCommandSelected(this, new EntityCommandSelectedArg() { EntityCommandID = item.ID });
            }
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }
    }

    public class EntityCommandSelectedArg : EventArgs
    {
        public int EntityCommandID { set; get; }
    }

}
