using MyConnectionManager;
using MyModelManager;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDataEditManagerBusiness
{
    public class Helper
    {
        public void CheckPermissoinToEdit(DR_Requester requester, DR_ResultEdit result, List<QueryItem> allQueryItems)
        {
            BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
            bool permission = true;
            foreach (var entityGroup in allQueryItems.GroupBy(x => x.TargetEntity.ID))
            {
                if (!bizTableDrivedEntity.DataIsAccessable(requester, entityGroup.Key, new List<SecurityAction>() { SecurityAction.EditAndDelete }))
                {
                    permission = false;
                    var entity = allQueryItems.First(x => x.TargetEntity.ID == entityGroup.Key).TargetEntity;
                    result.Details.Add(ToResultDetail("عدم دسترسی", "عدم دسترسی ثبت به موجودیت" + " " + entity.Alias, ""));
                }
                else if (bizTableDrivedEntity.DataIsReadonly(requester, entityGroup.Key))
                {
                    permission = false;
                    var entity = allQueryItems.First(x => x.TargetEntity.ID == entityGroup.Key).TargetEntity;
                    result.Details.Add(ToResultDetail("عدم دسترسی", "عدم دسترسی ثبت به موجودیت" + " " + entity.Alias, ""));
                }
            }
            if (permission)
            {
                BizColumn bizColumn = new BizColumn();
                foreach (var query in allQueryItems)
                {
                    foreach (var column in query.EditingProperties)
                    {
                        if (!bizColumn.DataIsAccessable(requester, column.ColumnID))
                        {
                            permission = false;
                            result.Details.Add(ToResultDetail("عدم دسترسی", "عدم دسترسی به ستون" + " " + column.Column.Alias, ""));
                        }
                        else if (column.IsChanged && bizColumn.DataIsReadonly(requester, column.ColumnID))
                        {
                            permission = false;
                            result.Details.Add(ToResultDetail("عدم دسترسی", "عدم دسترسی ثبت به ستون" + " " + column.Column.Alias, ""));
                        }
                    }
                }
            }

            if (permission == false)
            {
                result.Result = Enum_DR_ResultType.ExceptionThrown;
                result.Message = "خطا در ثبت";
            }
        }
    }
}
