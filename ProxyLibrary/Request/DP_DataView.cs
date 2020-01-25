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
                var list = Properties.Where(x => x.IsDescriptive);
                if (list.Count() == 0)
                {
                    list = Properties.Where(x => !string.IsNullOrEmpty(x.Value) && x.Value != "<Null>" && x.Value.Length <= 50).Take(5);
                }
                if (list.Count() == 0)
                {
                    list = Properties.Where(x => !string.IsNullOrEmpty(x.Value) && x.Value != "<Null>").Take(1);
                }
                //if (list.Count() <= 15)
                //{
                foreach (var prop in list)
                {
                    if (!string.IsNullOrEmpty(prop.Value) && prop.Value != "<Null>")
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
