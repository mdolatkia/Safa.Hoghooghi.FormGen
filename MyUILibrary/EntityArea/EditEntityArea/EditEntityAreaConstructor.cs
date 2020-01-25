using CommonDefinitions.UISettings;
using ModelEntites;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUILibrary.EntityArea
{
    public static class EditEntityAreaConstructor
    {

        public static Tuple<I_EditEntityArea, string> GetEditEntityArea(EditEntityAreaInitializer initializer)
        {
            I_EditEntityArea result = null;
            var simpleEntity = AgentUICoreMediator.GetAgentUICoreMediator.tableDrivedEntityManagerService.GetSimpleEntity(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), initializer.EntityID);
            if (simpleEntity == null)
                return new Tuple<I_EditEntityArea, string>(null, "عدم دسترسی به موجودیت به شناسه" + " " + initializer.EntityID);
            var _Permission = AgentUICoreMediator.GetAgentUICoreMediator.tableDrivedEntityManagerService.GetEntityAssignedPermissions(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), initializer.EntityID, false);
            if (!_Permission.GrantedActions.Any(x => x == SecurityAction.ReadOnly
            || x == SecurityAction.Edit || x == SecurityAction.EditAndDelete))
                return new Tuple<I_EditEntityArea, string>(null, "عدم دسترسی فرمی به موجودیت به شناسه" + " " + initializer.EntityID);
            if (initializer.DataMode == DataMode.None)
            {
                if (simpleEntity.BatchDataEntry == false)
                    initializer.DataMode = DataMode.One;
                else
                    initializer.DataMode = DataMode.Multiple;

            }
            if (initializer.DataMode == DataMode.One)
            {
                result = new EditEntityAreaOneData(simpleEntity);
            }
            else if (initializer.DataMode == DataMode.Multiple)
                result = new EditEntityAreaMultipleData(simpleEntity);
            if (result != null)
            {
                initializer.EditAreaDataManager = new EditAreaDataManager();
                initializer.ActionActivityManager = new UIActionActivityManager(result);
                initializer.RelationshipFilterManager = new RelationshipFilterManager(result);
                initializer.EntityAreaLogManager = new EntityAreaLogManager();
                initializer.UIFomulaManager = new UIFomulaManager(result as BaseEditEntityArea);
                initializer.UIValidationManager = new UIValidationManager(result as BaseEditEntityArea);
            }
            return new Tuple<I_EditEntityArea, string>(result, "");
        }
    }
}
