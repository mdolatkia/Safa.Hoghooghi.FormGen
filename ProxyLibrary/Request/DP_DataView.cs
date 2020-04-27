using ModelEntites;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
namespace ProxyLibrary
{
    public class DP_DataView : DP_BaseData
    {
        public DP_DataView(int targetEntityID, string targetEntityAlias) : base( targetEntityID,  targetEntityAlias)
        {
        }

        public string ViewInfo
        {
            get
            {
                string text = "";
                var list = Properties.Where(x => x.IsDescriptive==true);
                if (list.Count() == 0)
                {
                    list = Properties.Where(x => x.Value!=null && !string.IsNullOrEmpty(x.Value.ToString())  && x.Value.ToString().Length <= 50).Take(5);
                }
                if (list.Count() == 0)
                {
                    list = Properties.Where(x => x.Value != null && !string.IsNullOrEmpty(x.Value.ToString()) ).Take(1);
                }
                //if (list.Count() <= 15)
                //{
                foreach (var prop in list)
                {
                    if (prop.Value != null && !string.IsNullOrEmpty(prop.Value.ToString()) )
                        text += (text == "" ? "" : ", ") + prop.Column.Alias + ":" + prop.Value;
                }
                //}
                //else
                //{
                //    foreach (var prop in Properties.Where(x => x.IsDescriptive))
                //    {
                //        if (!string.IsNullOrEmpty(prop.Value) && prop.Value != "<Null>")
                //            text += (text == "" ? "" : ", ") + prop.Value;
                //    }

                //}
                if (text == "")
                    text = "داده از موجودیت" + " " + TargetEntityID;
                return text;
            }
        }
        public Guid GUID;

    }
}
