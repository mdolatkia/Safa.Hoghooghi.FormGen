
using ModelEntites;
using MyUIGenerator.UIControlHelper;
using MyUILibrary;
using MyUILibrary.EntityArea;
using MyUILibrary.EntityArea.Commands;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telerik.Windows.Controls;
using MyUILibrary.Temp;

namespace MyUIGenerator.View
{
    /// <summary>
    /// Interaction logic for UC_ViewPackageArea.xaml
    /// </summary>
    public partial class UC_ViewEntityArea : View_MultipleDataContainer, I_View_ViewEntityArea
    {
        //درست شود مثل بقیه ارث بری
        //public event EventHandler<Arg_CommandExecuted> CommandExecuted;
        //DataGridHelper dataGridHelper;
        public UC_ViewEntityArea()
        {
            InitializeComponent();
            grdArea.Children.Add(dataGridHelper.dataGrid);
            //dataGridHelper = new DataGridHelper();
            //grdArea.Children.Add(dataGridHelper.dataGrid);
            //dataGridHelper.dataGrid.MouseDoubleClick += LayoutDataGrid_MouseDoubleClick;
            //dataGridHelper.dataGrid.IsReadOnly = true;

            // grdArea.Children.Add(dataGridHelper.dataGrid);
        }

        //public void DisableEnable(bool enable)
        //{
        //    this.IsEnabled = enable;
        //}
        public event EventHandler<DataSelectedEventArg> DataSelected;

        public bool AllowSelect { set; get; }
        public int SelectCount { set; get; }
        public override Grid Toolbar
        {
            get
            {
                return grdTool;
            }
        }
        public override Grid ControlArea
        {
            get
            {
                return grdArea;
            }
        }
        //  void LayoutDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        // {
        //      if (dataGridHelper.dataGrid.SelectedItem != null)
        //          if (dataGridHelper.dataGrid.SelectedItem is DP_DataRepository)
        //              if (DataSelected != null)
        //                   DataSelected(this, new DataSelectedEventArg() { DataItem = new List<DP_DataRepository>() { dataGridHelper.dataGrid.SelectedItem as DP_DataRepository } });
        //   }
        //List<Control> PropertyControls = new List<Control>();
        //private void InitializePackageArea()
        //{
        //    //foreach (var type in TemplatePackage.TypeConditions)
        //    //{
        //    //    //foreach (var property in type.NDType.Properties)
        //    //    //{
        //    //    //    var column = new DataGridViewTextBoxColumn();
        //    //    //    column.HeaderText = (property.Title == null ? property.Property.Title : property.Title);
        //    //    //    column.Tag = property;
        //    //    //    column.Name = property.Property.Name;
        //    //    //    //column.CellType=GetType(string);
        //    //    //    dataGridView1.Columns.Add(column);
        //    //    //}
        //    //}

        //}
        //public void ShowValues(List<DataAccess.Entity> dataPackages)
        //{
        //    //DataPackages = dataPackages;
        //    //foreach (var dataPackage in dataPackages)
        //    //{
        //    //    //    var row = new DataGridViewRow();
        //    //    int n = dataGridView1.Rows.Add();
        //    //    dataGridView1.Rows[n].Tag = dataPackage;
        //    //    int i = 0;
        //    //    foreach (DataGridViewColumn column in dataGridView1.Columns)
        //    //    {
        //    //        if (column.Tag is DataMaster.EntityDefinition.ND_Type_Property)
        //    //        {
        //    //            var typeProperty = column.Tag as DataMaster.EntityDefinition.ND_Type_Property;
        //    //            foreach (var type in dataPackage.TypeConditions)
        //    //            {
        //    //                var dateTypeProperty = type.NDType.Properties.FirstOrDefault(x => x.ID == typeProperty.ID);
        //    //                if (dateTypeProperty != null)
        //    //                    if (dateTypeProperty.Value != null)
        //    //                        dataGridView1.Rows[n].Cells[i].Value = dateTypeProperty.Value;
        //    //            }

        //    //        }
        //    //        i++;
        //    //    }

        //    //}
        //}
        //public void UpdateValues()
        //{
        //    //foreach (var item in PropertyControls)
        //    //{
        //    //    if (item.Tag is DataMaster.EntityDefinition.ND_Type_Property)
        //    //    {
        //    //        var typeProperty = item.Tag as DataMaster.EntityDefinition.ND_Type_Property;
        //    //        foreach (var type in DataPackage.NDTypes)
        //    //        {
        //    //            var dateTypeProperty = type.Properties.FirstOrDefault(x => x.ID == typeProperty.ID);
        //    //            if (dateTypeProperty != null)
        //    //                dateTypeProperty.Value = item.Text;
        //    //        }
        //    //    }
        //    //}
        //}

        //public I_SearchViewEntityArea Controller
        //{
        //    set;
        //    get;
        //}



        //public List<DataAccess.Entity> SelectedViewPackages
        //{
        //    set;
        //    get;
        //}









        //public void AddCommands(List<I_Command> commands, TemplateEntityUISettings templateEntityUISettings)
        //{
        //    foreach (var item in commands.OrderBy(x => x.Position))
        //    {
        //        AddCommand(item);
        //    }
        //}
        //public void AddCommand(I_CommandManager item)
        //{
        //    //Button btnCommand = UIHelper.GenerateCommand(item);
        //    //item.EnabledChanged += (sender, e) => item_EnabledChanged(sender, e, btnCommand);
        //    //btnCommand.Click += btnCommand_Click;
        //    toolbar.Items.Add((item as MyUIGenerator.UIControlHelper.CommandManager).Button);

        //}

        //private void item_EnabledChanged(object sender, EventArgs e, Button btnCommand)
        //{
        //    btnCommand.IsEnabled = (sender as I_Command).Enabled;
        //}



        //void btnCommand_Click(object sender, EventArgs e)
        //{
        //    if (CommandExecuted != null)
        //        CommandExecuted(this, new Arg_CommandExecuted() { Command = (sender as Button).Tag as I_Command });
        //    //Controller.ViewCommandExecuted(((sender as Button).Tag as I_ViewAreaCommand));
        //}
        //public void PrepareViewOfViewTemplate(SearchViewEntityAreaInitializer EditTemplate)
        //{
        //    //LayoutGrid = new Grid();
        //    //if (EditTemplate.UISettings.FlowDirection == MyUILibrary.EntityArea.FlowDirection.RightToLeft)
        //    //    LayoutGrid.FlowDirection = System.Windows.FlowDirection.RightToLeft;

        //    //SetMainGridForMultipleNDTypes();
        //}
        //private void SetMainGridForMultipleNDTypes()
        //{


        //}
        //Grid LayoutGrid;
        //RadGridView LayoutDataGrid;
        //public void ViewDataPckages(List<DataAccess.Entity> dataPackages)
        //{
        //    throw new NotImplementedException();
        //}
        //////public UIControlPackage GenerateMultipleDataDependentControl(ColumnDTO column, ColumnSetting columnSetting)
        //////{
        //////    //  List<object> result = new List<object>();

        //////    var controlPackage = DataGridHelper.GenerateMultipleDataDependentControl(column, columnSetting);
        //////    //if (controlPackage.DataDependentControl != null)
        //////    //{
        //////    //    controlPackage.DataDependentControl.DataControlGenerated += DataDependentControl_DataControlGenerated;
        //////    //}
        //////    return controlPackage;
        //////    //return null;
        //////}


        //public void AddUIControlPackage(UIControlPackageMultipleData controlPackage, string title, MyUILibrary.Temp.InfoColor titleColor, string tooltip = "")
        //{
        //    //propertyControl.UIControlSetting = new UIControlSetting();
        //    //propertyControl.UIControlSetting.DesieredColumns = 1;
        //    //propertyControl.UIControlSetting.DesieredRows = 1;
        //    //if (propertyControl.UI_PropertySetting.PropertyType == UISetting.DataPackageUISetting.Enum_UI_PropertyType.Text)
        //    //{
        //    //    if (propertyControl.UI_PropertySetting.TextPropertySetting.type == UISetting.DataPackageUISetting.Enum_UI_TextPropertyType.Small)
        //    //    {
        //    //        propertyControl.UIControlSetting = ControlHelper.TextBoxHelper.GenerateUISetting(propertyControl.TypeProperty, propertyControl.UI_PropertySetting);
        //    //    }


        //    //}
        //    //var labelUIControl = LabelHelper.GenerateLabelControl(title, "", MyUILibrary.Temp.InfoColor.Black);
        //    //controlPackage.RelatedUIControls.Add(new AG_RelatedConttol() { RelationType = AG_ControlRelationType.Label, RelatedUIControl = labelUIControl });

        //    //////dataGridHelper.AddControlToLayout(controlPackage as UIControlPackageMultipleData, title, titleColor, tooltip);



        //    //AddLabelControl(propertyControl);

        //}

        ////public bool ShowMultipleDateItemControlValue(DP_DataRepository dataItem, UIControlPackageMultipleData controlPackage, string value)
        ////{
        ////    return dataGridHelper.SetValue(dataItem, controlPackage, value);
        ////}
        ////public string FetchMultipleDateItemControlValue(DP_DataRepository dataItem, UIControlPackageMultipleData controlPackage)
        ////{
        ////    return dataGridHelper.GetValue(dataItem, controlPackage);
        ////}
        //public void AddDataContainers(List<DP_DataRepository> data)
        //{
        //    //if (AgentHelper.GetDataEntryMode(EditTemplate) == DataMode.Multiple)
        //    //{
        //    dataGridHelper.AddDataContainers(data);
        //    //}
        //}



        //public event EventHandler<Arg_DataContainer> DataCotainerIsReady;


        //public void RemoveDataContainers()
        //{
        //    //if (Controller.EditTemplate.Template.TableDrivedEntity.Table.BatchDataEntry == true)
        //    //{
        //    dataGridHelper.RemoveDataContainers();
        //    //}
        //}
        //public void RemoveDataContainers(DP_DataRepository data)
        //{
        //    //if (Controller.EditTemplate.Template.TableDrivedEntity.Table.BatchDataEntry == true)
        //    //{
        //    dataGridHelper.RemoveDataContainers(data);
        //    //}
        //}

        //public List<DP_DataRepository> GetSelectedData()
        //{
        //    //if (AgentHelper.GetDataEntryMode(EditTemplate) == DataMode.Multiple)
        //    //{
        //    return dataGridHelper.GetSelectedData();
        //    //}
        //    //else
        //    //    return null;
        //}
        //public void SetSelectedData(List<DP_DataRepository> dataItems)
        //{
        //    dataGridHelper.SetSelectedData(dataItems);

        //}

        //public void RemoveSelectedDataContainers()
        //{
        //    //if (AgentHelper.GetDataEntryMode( EditTemplate) == DataMode.Multiple)
        //    //{
        //    dataGridHelper.RemoveSelectedDataContainers();
        //    //}
        //}
        ////public CommonDefinitions.BasicUISettings.PackageAreaUISetting BasicUISetting
        ////{
        ////    set;
        ////    get;
        ////}




        ////bool _AllowExpand;
        ////public bool AllowExpand
        ////{
        ////    get
        ////    {
        ////        return _AllowExpand;
        ////    }
        ////    set
        ////    {
        ////        _AllowExpand = value;
        ////    }
        ////}

        ////public void Expand()
        ////{
        ////    expander.IsExpanded = true;
        ////}






        ////public void Collapse()
        ////{
        ////    expander.IsExpanded = false;
        ////}



        //public void SetBackgroundColor(string color)
        //{

        //}

        ////public GridSetting GridSetting
        ////{
        ////    set;
        ////    get;
        ////}

        //public void SetExpanderInfo(object header)
        //{

        //}

        //public void AddUIControlPackage(object controlPackage, string title, InfoColor titleColor, string tooltip = "")
        //{
        //    throw new NotImplementedException();
        //}

        //public I_MUltipleDataControlManagerSimpleColumn GenerateControlManager(ColumnDTO column, ColumnUISettingDTO columnUISettingDTO)
        //{
        //    throw new NotImplementedException();
        //}

        //public void AddUIControlPackage(I_MUltipleDataControlManagerSimpleColumn controlManager, string alias)
        //{
        //    throw new NotImplementedException();
        //}

        //public I_ControlManagerMultipleDataRelationship GenerateRelationshipControlManager(IAG_View_TemporaryView view, RelationshipUISettingDTO relationshipUISettingDTO)
        //{
        //    throw new NotImplementedException();
        //}

        //public void AddView(object view, string title)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
