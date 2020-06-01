
//using MyUIGenerator.UIContainerHelper;
using ModelEntites;
using MyUIGenerator.UIControlHelper;
using MyUILibrary;
using MyUILibrary.EntityArea;
using MyUILibrary.Temp;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Telerik.Windows.Controls.GridView;

namespace MyUIGenerator.UIControlHelper
{
    public class View_MultipleDataContainer : View_Container, I_View_MultipleDataContainer
    {
        public DataGridHelper dataGridHelper;

        bool _MultipleSelection;
        public bool MultipleSelection
        {
            get
            {
                return _MultipleSelection;
            }

            set
            {
                _MultipleSelection = value;
                if (_MultipleSelection)
                    dataGridHelper.dataGrid.SelectionMode = SelectionMode.Extended;
                else
                    dataGridHelper.dataGrid.SelectionMode = SelectionMode.Single;
            }
        }

        public View_MultipleDataContainer()
        {
            dataGridHelper = new DataGridHelper();
            dataGridHelper.DataContainerLoaded += DataGridHelper_DataContainerLoaded;
            dataGridHelper.dataGrid.MouseDoubleClick += DataGrid_MouseDoubleClick;
        }

        private void DataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (dataGridHelper.dataGrid.SelectedItem != null)
            {
                if (ItemDoubleClicked != null)
                    ItemDoubleClicked(this, new DataContainerLoadedArg() { DataItem = dataGridHelper.dataGrid.SelectedItem });
            }
        }

        private void DataGridHelper_DataContainerLoaded(object sender, DataContainerLoadedArg e)
        {
            if (DataContainerLoaded != null)
                DataContainerLoaded(this, e);
        }

        //public CommonDefinitions.BasicUISettings.GridSetting GridSetting
        //{
        //    get;
        //    set;
        //}

        //public void AddUIControlPackage(object controlPackage, string title, InfoColor titleColor, string tooltip = "")
        //{
        //    //////dataGridHelper.AddControlToLayout(controlPackage as UIControlPackageMultipleData, title, titleColor, tooltip);
        //}




        //private List<RadTabControl> TabControls = new List<RadTabControl>();








        //public bool ShowMultipleDateItemControlValue(DP_DataRepository dataItem, UIControlPackageMultipleData controlPackage, string value)
        //{
        //    return false;
        //    //////return dataGridHelper.SetValue(dataItem, controlPackage, value, columnSetting);
        //}
        //public string FetchMultipleDateItemControlValue(DP_DataRepository dataItem, UIControlPackageMultipleData controlPackage)
        //{
        //    return dataGridHelper.GetValue(dataItem, controlPackage);
        //}








        public void AddDataContainer(object data)
        {
            //if (AgentHelper.GetDataEntryMode(EditTemplate) == DataMode.Multiple)
            //{
            dataGridHelper.AddDataContainer(data);
            //}
        }



        public event EventHandler<Arg_DataContainer> DataCotainerIsReady;
        public event EventHandler<DataContainerLoadedArg> DataContainerLoaded;
        public event EventHandler<DataContainerLoadedArg> ItemDoubleClicked;


        //public void RemoveDataContainers(object data)
        //{
        //    //if (Controller.EditTemplate.Template.TableDrivedEntity.Table.BatchDataEntry == true)
        //    //{
        //    dataGridHelper.RemoveDataContainers(data);
        //    //}
        //}

        public List<object> GetSelectedData()
        {
            //if (AgentHelper.GetDataEntryMode(EditTemplate) == DataMode.Multiple)
            //{
            return dataGridHelper.GetSelectedData();
            //}
            //else
            //    return null;
        }
        public void SetSelectedData(List<object> dataItems)
        {
            dataGridHelper.SetSelectedData(dataItems);

        }
        public List<object> RemoveDataContainers()
        {
            //if (Controller.EditTemplate.Template.TableDrivedEntity.Table.BatchDataEntry == true)
            //{
            return dataGridHelper.RemoveDataContainers();
            //}
        }
        public void RemoveDataContainer(object data)
        {
            dataGridHelper.RemoveDataContainer(data);
        }

        //public void RemoveSelectedDataContainers()
        //{
        //    //if (AgentHelper.GetDataEntryMode( EditTemplate) == DataMode.Multiple)
        //    //{
        //    dataGridHelper.RemoveSelectedDataContainers();
        //    //}
        //}

        //private List<DataMessageItem> rowValidationMessages = new List<DataMessageItem>();
        //public void AddValidation(DataMessageItem item)
        //{
        //    rowValidationMessages.Add(item);
        //    SetTooltip(item.CausingDataItem);
        //    SetColor(item.CausingDataItem);
        //}
        //public void ClearValidation(DP_DataRepository dataitem)
        //{
        //    var validationMessage = rowValidationMessages.Where(x => x.CausingDataItem == dataitem).ToList();
        //    foreach (var item in validationMessage)
        //        validationMessage.Remove(item);

        //    SetTooltip(dataitem);
        //    SetColor(dataitem);
        //}
        //public void ClearValidation()
        //{
        //    List<DP_DataRepository> dataItems = new List<DP_DataRepository>();
        //    foreach (var item in rowValidationMessages)
        //    {
        //        dataItems.Add(item.CausingDataItem);
        //    }
        //    rowValidationMessages.Clear();
        //    foreach (var item in dataItems)
        //    {
        //        SetTooltip(item);
        //        SetColor(item);
        //    }
        //}
        //private void SetColor(object dataItem)
        //{
        //    var color = InfoColor.Black;
        //    var validationMessage = rowValidationMessages.Where(x => x.CausingDataItem == dataItem);
        //    if (validationMessage.Any())
        //        color = validationMessage.Last().Color;
        //    if (color != InfoColor.Black)
        //        dataGridHelper.SetColor(dataItem, color);
        //    else
        //        dataGridHelper.ClearColor(dataItem);
        //}

        //private void SetTooltip(object dataItem)
        //{
        //    var tooltip = "";
        //    var validationMessage = rowValidationMessages.Where(x => x.CausingDataItem == dataItem);
        //    foreach (var item in validationMessage)
        //        tooltip += (tooltip == "" ? "" : Environment.NewLine) + item.Message;
        //    if (tooltip != "")
        //        dataGridHelper.SetTooltip(dataItem, tooltip);
        //    else
        //        dataGridHelper.ClearTooltip(dataItem);
        //}

        public override void AddUIControlPackage(I_SimpleControlManager control, I_LabelControlManager labelControlManager)
        {
            //if (!(control as LocalDataGridControlManager).RelatedControl.Any())
            //{
            // var labelControl = new DataGridUIControl();
            //var labelControl = LabelHelper.GenerateLabelControl(title, (control as SimpleControlManagerForMultipleDataForm).DataGridColumn.ColumnSetting);
            //////(control as SimpleControlManagerForMultipleDataForm).RelatedControl.Add(labelControl);
            (control as SimpleControlManagerForMultipleDataForm).DataGridColumn.Header = (labelControlManager as LabelControlManager).WholeControl;
            //   }

            dataGridHelper.dataGrid.Columns.Add((control as SimpleControlManagerForMultipleDataForm).DataGridColumn);
        }
        public void CleraUIControlPackages()
        {
            dataGridHelper.dataGrid.Columns.Clear();
        }

        public override void AddView(object view, I_LabelControlManager labelControlManager)
        {
            //if (!(view as LocalDataGridRelationshipControlManager).RelatedControl.Any())
            //{
            //   DataGridUIControl labelControl = new DataGridUIControl();
            //if (!string.IsNullOrEmpty(title))
            //{

       //     var labelControl = LabelHelper.GenerateLabelControl(title, new ColumnUISettingDTO());


            //////(view as RelationshipControlManagerForMultipleDataForm).RelatedControl.Add(labelControl);
            (view as RelationshipControlManagerForMultipleDataForm).DataGridColumn.Header = (labelControlManager as LabelControlManager).WholeControl;
            //}
            dataGridHelper.dataGrid.Columns.Add((view as RelationshipControlManagerForMultipleDataForm).DataGridColumn);
            //if (controlPackage.UIControl.Control is I_View_Container)
            //    (controlPackage.UIControl.Control as I_View_Container).SetExpanderInfo(labelControl.Control);
            //else
            //AddControlToGrid(labelControl);
            //}
            //var uiControl = controlPackage.UIControl;

        }

        public void RemoveUIControlPackage(I_SimpleControlManager controlManager)
        {
            dataGridHelper.dataGrid.Columns.Add((controlManager as SimpleControlManagerForMultipleDataForm).DataGridColumn);
        }

        public void RemoveView(I_RelationshipControlManager controlManager)
        {
            dataGridHelper.dataGrid.Columns.Add((controlManager as RelationshipControlManagerForMultipleDataForm).DataGridColumn);
        }

        public override void ClearControls()
        {
            dataGridHelper.dataGrid.Columns.Clear();
        }
        public int ControlsCount { get { return dataGridHelper.dataGrid.Columns.Count; } }

        public void SetTooltip(object dataItem, string tooltip)
        {
            dataGridHelper.SetTooltip(dataItem, tooltip);
        }

        public void SetBorderColor(object dataItem, InfoColor color)
        {
            dataGridHelper.SetBorderColor(dataItem, color);
        }

        public void SetBackgroundColor(object dataItem, InfoColor color)
        {
            dataGridHelper.SetBackgroundColor(dataItem, color);
        }

        public void SetForegroundColor(object dataItem, InfoColor color)
        {
            dataGridHelper.SetForegroundColor(dataItem, color);
        }

        public void Visiblity(object dataItem, bool visible)
        {
            dataGridHelper.Visiblity(dataItem, visible);
        }
        public void EnableDisable(object dataItem, bool enable)
        {
            dataGridHelper.EnableDisable(dataItem, enable);
        }
    }


    public class DataGridUIControl
    {
        public DataGridUIControl()
        {

        }
        public ColumnUISettingDTO ColumnSetting { get; internal set; }
        public FrameworkElement Control { set; get; }
    }

    public class DataGridUIrelationshipControl
    {
        public DataGridUIrelationshipControl()
        {

        }
        public RelationshipUISettingDTO RelationshipSetting { get; internal set; }
        public FrameworkElement Control { set; get; }
    }
}
