using DataAccess;
using ModelEntites;
using MyGeneralLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyModelManager
{
    public class BizUIActionActivity
    {
        public List<UIActionActivityDTO> GetActionActivities(int entityID, bool withDetails)
        {
            List<UIActionActivityDTO> result = new List<UIActionActivityDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var listActionActivity = projectContext.UIActionActivity.Where(x => x.TableDrivedEntityID == entityID);


                foreach (var item in listActionActivity)
                    result.Add(ToActionActivityDTO(item, withDetails));

            }
            return result;
        }
        public UIActionActivityDTO GetActionActivity(int ActionActivitysID)
        {
            List<UIActionActivityDTO> result = new List<UIActionActivityDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var ActionActivitys = projectContext.UIActionActivity.First(x => x.ID == ActionActivitysID);
                return ToActionActivityDTO(ActionActivitys, true);
            }
        }
        public UIActionActivityDTO ToActionActivityDTO(UIActionActivity item, bool withDetails)
        {
            UIActionActivityDTO result = new UIActionActivityDTO();
            result.Type = (Enum_ActionActivityType)item.Type;



            if (withDetails && result.Type == Enum_ActionActivityType.ColumnValue)
            {
                foreach (var dbitem in item.UIColumnValue)
                    result.UIColumnValue.Add(ToColumnValueDTO(dbitem));
            }
            if (withDetails && result.Type == Enum_ActionActivityType.UIEnablity)
            {
                foreach (var dbitem in item.UIEnablityDetails)
                    result.UIEnablityDetails.Add(ToUIEnablityDetailsDTO(result.UIEnablityDetails, dbitem));
            }
            if (withDetails && result.Type == Enum_ActionActivityType.ColumnValueRange)
            {
                foreach (var dbitem in item.UIColumnValueRange)
                    result.UIColumnValueRange.Add(ToUIColumnValueRangeDTO(dbitem));
            }
            //if (withDetails && result.Type == Enum_ActionActivityType.ColumnValueRangeReset)
            //{
            //    foreach (var dbitem in item.UIColumnValueRangeReset)
            //        result.UIColumnValueRangeReset.Add(ToUIColumnValueRangeResetDTO(dbitem));
            //}
            //if (withDetails && result.Type == Enum_ActionActivityType.RelationshipEnablity)
            //{
            //    var relationshipEnablity = item.RelationshipEnablity.First();
            //    result.RelationshipEnablityID = relationshipEnablity.ID;
            //    result.RelationshipEnablity = ToRelationshipEnablityDTO(relationshipEnablity);
            //}
            result.ID = item.ID;
            result.EntityID = item.TableDrivedEntityID;
            //   result.Step = (Enum_EntityActionActivityStep)item.StepType;
            //   result.ResultSensetive = item.ResultSensetive == true;
            //result.ActionActivityType = (Enum_ActionActivityType)item.ActivityType;
            result.Title = item.Title;
            result.RelatedStates = "";
            foreach (var state in item.EntityState_UIActionActivity)
            {
                result.RelatedStates += (result.RelatedStates == "" ? "" : " , ") + state.TableDrivedEntityState.Title;
            }
            return result;
        }

        private UIColumnValueRangeDTO ToUIColumnValueRangeDTO(UIColumnValueRange item)
        {
            UIColumnValueRangeDTO msgtem = new UIColumnValueRangeDTO();
            msgtem.ColumnValueRangeID = item.ColumnValueRangeID;
            msgtem.ID = item.ID;
            msgtem.EnumTag = (EnumColumnValueRangeTag)item.EnumTag;
            msgtem.Value = item.Value;
            return msgtem;
        }
        //private UIColumnValueRangeResetDTO ToUIColumnValueRangeResetDTO(UIColumnValueRangeReset item)
        //{
        //    UIColumnValueRangeResetDTO msgtem = new UIColumnValueRangeResetDTO();
        //    msgtem.ColumnValueRangeID = item.ColumnValueRangeID;
        //    msgtem.ID = item.ID;
        //    return msgtem;
        //}

        public UIColumnValueDTO ToColumnValueDTO(DataAccess.UIColumnValue item)
        {
            UIColumnValueDTO result = new UIColumnValueDTO();
            result.ColumnID = item.ColumnID;
            result.ExactValue = item.ExactValue;
            result.EvenIsNotNew = item.EvenIsNotNew;
            result.EvenHasValue = item.EvenHasValue;
            //     result.EntityRelationshipTailID = item.EntityRelationshipTailID ?? 0;
            //BizEntityRelationshipTail bizEntityRelationshipTail = new BizEntityRelationshipTail();
            //if (result.EntityRelationshipTailID != 0)
            //    result.EntityRelationshipTail = bizEntityRelationshipTail.ToEntityRelationshipTailDTO(item.EntityRelationshipTail);
            result.ID = item.ID;
            //foreach (var validValue in item.ColumnValue_ValidValues)
            //{
            //    result.ValidValues.Add(new ColumnValueValidValuesDTO() { Value = validValue.ValidValue });
            //}
            return result;
        }
        //private RelationshipEnablityDTO ToRelationshipEnablityDTO(RelationshipEnablity relationshipEnablity)
        //{
        //    RelationshipEnablityDTO result = new RelationshipEnablityDTO();
        //    result.ID = relationshipEnablity.ID;
        //    result.EntityRelationshipTailID = relationshipEnablity.EntityRelationshipTailID;
        //    BizEntityRelationshipTail bizEntityRelationshipTail = new BizEntityRelationshipTail();
        //    result.EntityRelationshipTail = bizEntityRelationshipTail.ToEntityRelationshipTailDTO(relationshipEnablity.EntityRelationshipTail);
        //    result.Enable = relationshipEnablity.Enabled;
        //    result.Readonly = relationshipEnablity.Readonly;
        //    return result;
        //}
        private UIEnablityDetailsDTO ToUIEnablityDetailsDTO(List<UIEnablityDetailsDTO> list, UIEnablityDetails dbitem)
        {
            var cItem = new UIEnablityDetailsDTO();
            if (dbitem.ColumnID != null)
            {
                //short relType = (short)Enum_MasterRelationshipType.FromForeignToPrimary;
                //if (dbitem.Column.RelationshipColumns.Any(x => x.Relationship.MasterTypeEnum == relType))
                //{
                //    var relID = dbitem.Column.RelationshipColumns.First(x => x.Relationship.MasterTypeEnum == relType).Relationship.ID;
                //    if (!list.Any(x => x.RelationshipID == relID))
                //        cItem.RelationshipID = relID;
                //}
                //else

                cItem.ColumnID = dbitem.ColumnID.Value;


            }
            cItem.ID = dbitem.ID;
            if (dbitem.RelationshipID != null)
                cItem.RelationshipID = dbitem.RelationshipID.Value;

            //if (dbitem.EntityUICompositionID != null)
            //    cItem.UICompositionID = dbitem.EntityUICompositionID.Value;
            cItem.Hidden = dbitem.Hidden;
            cItem.Readonly = dbitem.Readonly;
            return cItem;
        }
        public int UpdateActionActivitys(UIActionActivityDTO UIActionActivity)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var dbActionActivity = projectContext.UIActionActivity.FirstOrDefault(x => x.ID == UIActionActivity.ID);
                if (dbActionActivity == null)
                    dbActionActivity = new DataAccess.UIActionActivity();

                if (dbActionActivity.UIColumnValue != null)
                    while (dbActionActivity.UIColumnValue.Any())
                        projectContext.UIColumnValue.Remove(dbActionActivity.UIColumnValue.First());
                if (UIActionActivity.UIColumnValue.Any())
                {
                    foreach (var item in UIActionActivity.UIColumnValue)
                    {
                        var dbColumnValue = new UIColumnValue();
                        dbColumnValue.ColumnID = item.ColumnID;

                        dbColumnValue.ExactValue = item.ExactValue;
                        dbColumnValue.EvenIsNotNew = item.EvenIsNotNew;
                        dbColumnValue.EvenHasValue = item.EvenHasValue;

                        //while (dbColumnValue.ColumnValue_ValidValues.Any())
                        //    dbColumnValue.ColumnValue_ValidValues.Remove(dbColumnValue.ColumnValue_ValidValues.First());
                        //foreach (var value in UIActionActivity.ColumnValue.ValidValues)
                        //    dbColumnValue.ColumnValue_ValidValues.Add(new ColumnValue_ValidValues() { ValidValue = value.Value });

                        dbActionActivity.UIColumnValue.Add(dbColumnValue);
                    }
                }

                //if (UIActionActivity.UIEnablity.EntityRelationshipTailID != 0)
                //    dbUIEnablity.EntityRelationshipTailID = UIActionActivity.UIEnablity.EntityRelationshipTailID;
                //else
                //    dbUIEnablity.EntityRelationshipTailID = null;
                while (dbActionActivity.UIEnablityDetails.Any())
                    projectContext.UIEnablityDetails.Remove(dbActionActivity.UIEnablityDetails.First());
                foreach (var item in UIActionActivity.UIEnablityDetails)
                {
                    //if (item.RelationshipID != 0)
                    //{
                    //    var relationship = projectContext.Relationship.First(x => x.ID == item.RelationshipID);
                    //    if (relationship.MasterTypeEnum == (short)Enum_MasterRelationshipType.FromPrimartyToForeign)
                    //    {
                    //        throw new Exception("امکان تععین وضعیت برای رابطه" + " " + relationship.ID + " " + "میسر نمی باشد");
                    //    }
                    //    else
                    //    {
                    //        foreach (var relCol in relationship.RelationshipColumns)
                    //        {
                    //            UIEnablityDetails dbItem = new UIEnablityDetails();
                    //            dbItem.Hidden = item.Hidden;
                    //            dbItem.Readonly = item.Readonly;
                    //            dbItem.ColumnID = relCol.FirstSideColumnID;
                    //            dbActionActivity.UIEnablityDetails.Add(dbItem);
                    //        }
                    //    }
                    //}
                    //else
                    //{

                    UIEnablityDetails dbItem = new UIEnablityDetails();
                    dbItem.Hidden = item.Hidden;
                    dbItem.Readonly = item.Readonly;


                    if (item.ColumnID != 0)
                        dbItem.ColumnID = item.ColumnID;
                    else
                        dbItem.ColumnID = null;
                    if (item.RelationshipID != 0)
                        dbItem.RelationshipID = item.RelationshipID;
                    else
                        dbItem.RelationshipID = null;
                    //if (item.UICompositionID != 0)
                    //    dbItem.EntityUICompositionID = item.UICompositionID;
                    //else
                    //    dbItem.EntityUICompositionID = null;

                    dbActionActivity.UIEnablityDetails.Add(dbItem);
                    //}
                }


                while (dbActionActivity.UIColumnValueRange.Any())
                    projectContext.UIColumnValueRange.Remove(dbActionActivity.UIColumnValueRange.First());
                foreach (var item in UIActionActivity.UIColumnValueRange)
                {
                    UIColumnValueRange dbItem = new UIColumnValueRange();
                    dbItem.ColumnValueRangeID = item.ColumnValueRangeID;
                    dbItem.EnumTag = (short)item.EnumTag;
                    dbItem.Value = item.Value;
                    dbActionActivity.UIColumnValueRange.Add(dbItem);
                }
                //while (dbActionActivity.UIColumnValueRangeReset.Any())
                //    projectContext.UIColumnValueRangeReset.Remove(dbActionActivity.UIColumnValueRangeReset.First());
                //foreach (var item in UIActionActivity.UIColumnValueRangeReset)
                //{
                //    UIColumnValueRangeReset dbItem = new UIColumnValueRangeReset();
                //    dbItem.ColumnValueRangeID = item.ColumnValueRangeID;
                //    dbActionActivity.UIColumnValueRangeReset.Add(dbItem);
                //}
                //if (UIActionActivity.RelationshipEnablity != null && UIActionActivity.RelationshipEnablity.EntityRelationshipTailID != 0)
                //{
                //    var dbRelationshipEnablity = dbActionActivity.RelationshipEnablity.FirstOrDefault();
                //    if (dbRelationshipEnablity == null)
                //        dbRelationshipEnablity = new RelationshipEnablity();
                //    dbRelationshipEnablity.EntityRelationshipTailID = UIActionActivity.RelationshipEnablity.EntityRelationshipTailID;
                //    dbRelationshipEnablity.Enabled = UIActionActivity.RelationshipEnablity.Enable;
                //    dbRelationshipEnablity.Readonly = UIActionActivity.RelationshipEnablity.Readonly;
                //    if (dbRelationshipEnablity.ID == 0)
                //        dbActionActivity.RelationshipEnablity.Add(dbRelationshipEnablity);
                //}
                //else
                //{
                //    //پاک کردنش
                //}


                dbActionActivity.ID = UIActionActivity.ID;
                dbActionActivity.Type = (short)UIActionActivity.Type;
                //dbActionActivity.ActivityType = (short)UIActionActivity.ActionActivityType;
                dbActionActivity.Title = UIActionActivity.Title;
                dbActionActivity.TableDrivedEntityID = UIActionActivity.EntityID;
                //dbActionActivity.StepType = (short)UIActionActivity.Step;
                //dbActionActivity.ResultSensetive = UIActionActivity.ResultSensetive;
                if (dbActionActivity.ID == 0)
                    projectContext.UIActionActivity.Add(dbActionActivity);
                projectContext.SaveChanges();
                return dbActionActivity.ID;
            }
        }

        public void DeleteActionActivity(int iD)
        {
            throw new NotImplementedException();
        }
    }
    public enum Enum_GetActionActivityType
    {
        CodeFunctionOrDatabaseFunction,
        UIActions,
        All
    }

}
