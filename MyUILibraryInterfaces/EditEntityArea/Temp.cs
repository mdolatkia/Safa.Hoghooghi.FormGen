using ModelEntites;
using MyUILibrary;
using MyUILibrary.EntityArea;
using MyUILibrary.EntityArea.Commands;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUILibraryInterfaces.EditEntityArea
{
    public class ColumnFormula
    {
        public FormulaDTO Formula { set; get; }
        public SimpleColumnControl ResultColumnControl { set; get; }
    }
    //public class FormAttributes
    //{


    //    public FormAttributes()
    //    {
    //        ColumnAttributes = new List<ColumnAttributes>();
    //        CommandAttributes = new List<EditEntityArea.CommandAttributes>();
    //    }
    //    public bool SecurityNoAccess { set; get; }
    //    public bool SecurityReadOnly { set; get; }
    //    public bool SecurityEdit { set; get; }
    //    public bool SecurityDelete { set; get; }

    //    public bool BusinessNoAccess { set; get; }
    //    public bool BusinessReadOnly { set; get; }
    //    public List<ColumnAttributes> ColumnAttributes { set; get; }
    //    public List<CommandAttributes> CommandAttributes { set; get; }


    //    //public bool SecurityFormReadOnly { get; set;}
    //    //public bool BusinessFormReadOnly { get; set;}

    //    //public bool BusinessFormDisabled { get; set;}
    //    //public bool SecurityFormDisabled { get; set;}

    //    //public bool BusinessDeleteDisabled { get; set;}
    //    //public bool SecurityDeleteDisabled { get; set;}

    //    //public bool SecuritySaveNewDisabled { get; set;}
    //    //public bool BusinessSaveNewDisabled { get; set;}
    //    //public bool SecuritySaveEditDisabled { get; set;}
    //    //public bool BusinessSaveDisabled { get; set;}
    //    //public bool BusinessSaveEditDisabled { get; set;}
    //}
    //public class FormColumnAttributes
    //{
    //    internal bool BusinessColumnDisabled;

    //    public bool? SecurityNoAccess { set; get; }
    //    public bool? SecurityReadOnly { set; get; }
    //    public bool? SecurityEdit { set; get; }
    //    public bool SecurityColumnDisabled { get; set;}
    //    public bool SecurityColumnReadonly { get; set;}
    //    public bool BusinessColumnReadonly { get; set;}
    //}

    //public class DataItemAttributes
    //{
    //    public DataItemAttributes()
    //    {
    //        ColumnAttributes = new List<EditEntityArea.ColumnAttributes>();
    //        CommandAttributes = new List<EditEntityArea.CommandAttributes>();
    //        //BusinessItemDisabled = true;
    //        //BusinessItemReadOnly = false;
    //    }
    //    public bool CurrentDaisableEnableStare { set; get; }
    //    public bool CurrentReadonlyStare { set; get; }
    //    public DP_DataRepository DataItem { set; get; }
    //    public bool? SecurityNoAccess { set; get; }
    //    public bool? SecurityReadOnly { set; get; }
    //    public bool? SecurityEdit { set; get; }
    //    public bool? SecurityDelete { set; get; }
    //    public bool? SecuritySaveNew { get; set;}
    //    public List<ColumnAttributes> ColumnAttributes { set; get; }
    //    public List<CommandAttributes> CommandAttributes { set; get; }
    //    //public bool SecurityItemDisabled { get; set;}
    //    public bool BusinessItemDisabled { get; set;}
    //    //public bool SecurityItemReadOnly { get; set;}
    //    public bool BusinessItemReadOnly { get; set;}
    //    //public bool SecurityItemDeleteDisabled { get; set;}
    //    //public bool BusinessItemDeleteDisabled { get; set;}
    //    //public bool SecurityItemSaveDisabled { get; set;}
    //    //public bool BusinessItemSaveDisabled { get; set;}

    //}
    //public class ColumnAttributes
    //{
    //    public ColumnAttributes()
    //    {
    //        //BusinessColumnDisabled = false;
    //        //BusinessColumnReadonly = false;
    //    }
    //    public BaseSimpleColumn ColumnControl { set; get; }
    //    public int ColumnID { set; get; }
    //    public bool CurrentDaisableEnableStare { set; get; }
    //    public bool CurrentReadonlyStare { set; get; }
    //    public bool? SecurityNoAccess { set; get; }
    //    public bool? SecurityReadOnly { set; get; }
    //    public bool? SecurityEdit { set; get; }
    //    //public bool SecurityColumnDisabled { get; set;}
    //    public bool BusinessColumnDisabled { get; set;}
    //    //public bool SecurityColumnReadonly { get; set;}
    //    public bool BusinessColumnReadonly { get; set;}
    //}
    //public class CommandAttributes
    //{
    //    public CommandAttributes()
    //    {
    //        //BusinessColumnDisabled = false;
    //        //BusinessColumnReadonly = false;
    //    }
    //    public EntityCommandDTO EntityCommandDTO { set; get; }
    //    public bool SecurityNoAccess { set; get; }
    //    //public bool? SecurityReadOnly { set; get; }
    //    //public bool? SecurityEdit{ set; get; }
    //    public I_Command Command { get; set; }
    //}
    public enum SecurityBusinessType
    {
        Security,
        Business
    }
}
