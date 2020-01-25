using DataAccess;
using ModelEntites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProxyLibrary;
using System.Data.Entity.Validation;


namespace MyModelManager
{
    public class BizLetterTemplate
    {
        SecurityHelper securityHelper = new SecurityHelper();
       BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
        //public bool DataIsAccessable(DR_Requester requester, int entityID)
        //{
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var permission = bizTableDrivedEntity.GetEntityAssignedPermissions(requester, entityID, false);
        //        if (permission.GrantedActions.Any(x => x == SecurityAction.LetterView || x == SecurityAction.LetterEdit))
        //            return true;
        //        else
        //            return false;
        //    }
        //}
        public MainLetterTemplateDTO GetMainLetterTepmplate(DR_Requester requester, int letterTempleteID)
        {
            using (var projectContext = new MyProjectEntities())
            {
                var dbitem = projectContext.MainLetterTemplate.First(x => x.ID == letterTempleteID);
                return ToMainLetterTemplateDTO(requester, dbitem, true);
            }
        }
        public List<MainLetterTemplateDTO> GetMainLetterTemplates(DR_Requester requester, int entityID)
        {
            List<MainLetterTemplateDTO> result = new List<MainLetterTemplateDTO>();
            using (var projectContext = new MyProjectEntities())
            {
                var list = projectContext.MainLetterTemplate.Where(x => x.LetterTemplate.TableDrivedEntityID == entityID);
                foreach (var item in list)
                    result.Add(ToMainLetterTemplateDTO(requester, item, false));
                return result;
            }
        }

        public LetterSettingDTO GetLetterSetting(bool withDetails)
        {
            using (var projectContext = new MyProjectEntities())
            {
                var letterSetting = projectContext.LetterSetting.FirstOrDefault();
                if (letterSetting == null)
                    return null;
                else
                    return ToLetterSettingDTO(letterSetting, withDetails);
            }
        }

        private LetterSettingDTO ToLetterSettingDTO(LetterSetting letterSetting, bool withDetails)
        {
            LetterSettingDTO result = new LetterSettingDTO();

            result.AfterLetterSaveCodeID = letterSetting.AfterLetterSaveCodeID ?? 0;
            result.BeforeLetterLoadCodeID = letterSetting.BeforeLetterLoadCodeID ?? 0;
            result.BeforeLetterSaveCodeID = letterSetting.BeforeLetterSaveCodeID ?? 0;
            result.LetterExternalInfoCodeID = letterSetting.LetterExternalInfoCodeID ?? 0;
            result.LetterSendToExternalCodeID = letterSetting.LetterConvertToExternalCodeID ?? 0;
            //////if (withDetails)
            //////{
            //////    BizCodeFunction bizCodeFunction = new MyModelManager.BizCodeFunction();
            //////    if (letterSetting.AfterLetterSaveCodeID != null)
            //////        result.AfterLetterSaveCode = bizCodeFunction.ToCodeFunctionDTO(letterSetting.CodeFunction, true);
            //////    if (letterSetting.BeforeLetterLoadCodeID != null)
            //////        result.BeforeLetterLoadCode = bizCodeFunction.ToCodeFunctionDTO(letterSetting.CodeFunction1, true);
            //////    if (letterSetting.BeforeLetterSaveCodeID != null)
            //////        result.BeforeLetterSaveCode = bizCodeFunction.ToCodeFunctionDTO(letterSetting.CodeFunction2, true);
            //////    if (letterSetting.LetterExternalInfoCodeID != null)
            //////        result.LetterExternalInfoCode = bizCodeFunction.ToCodeFunctionDTO(letterSetting.CodeFunction3, true);
            //////}
            return result;
        }



        public void UpdateLetterSetting(LetterSettingDTO message)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var dbLetterSetting = projectContext.LetterSetting.FirstOrDefault();
                if (dbLetterSetting == null)
                {
                    dbLetterSetting = new DataAccess.LetterSetting();
                }
                dbLetterSetting.AfterLetterSaveCodeID = (message.AfterLetterSaveCodeID == 0 ? null : (int?)message.AfterLetterSaveCodeID);
                dbLetterSetting.BeforeLetterLoadCodeID = (message.BeforeLetterLoadCodeID == 0 ? null : (int?)message.BeforeLetterLoadCodeID);
                dbLetterSetting.BeforeLetterSaveCodeID = (message.BeforeLetterSaveCodeID == 0 ? null : (int?)message.BeforeLetterSaveCodeID);
                dbLetterSetting.LetterExternalInfoCodeID = (message.LetterExternalInfoCodeID == 0 ? null : (int?)message.LetterExternalInfoCodeID);
                dbLetterSetting.LetterConvertToExternalCodeID = (message.LetterSendToExternalCodeID == 0 ? null : (int?)message.LetterSendToExternalCodeID);
                if (dbLetterSetting.ID == 0)
                    projectContext.LetterSetting.Add(dbLetterSetting);
                projectContext.SaveChanges();
            }

        }
        BizEntityRelationshipTail bizEntityRelationshipTail = new BizEntityRelationshipTail();

        public List<LetterRelationshipTailDTO> GetLetterRelationshipTails(DR_Requester requester, int entityID, bool withDetails)
        {
            List<LetterRelationshipTailDTO> result = new List<LetterRelationshipTailDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var list = projectContext.EntityLetterRelationshipTails.Where(x => x.TableDrivedEntityID == entityID);
                foreach (var item in list)
                {
                    if (bizEntityRelationshipTail.DataIsAccessable(requester, item.EntityRelationshipTail))
                        if (bizTableDrivedEntity.DataIsAccessable(requester, item.EntityRelationshipTail.TableDrivedEntity, new List<SecurityAction>() { SecurityAction.LetterView, SecurityAction.LetterEdit }))
                            result.Add(ToLetterRelationshipTailDTO(item, withDetails));
                }
            }
            return result;
        }

        private LetterRelationshipTailDTO ToLetterRelationshipTailDTO(EntityLetterRelationshipTails item, bool withDetails)
        {
            LetterRelationshipTailDTO result = new LetterRelationshipTailDTO();
            result.EntityID = item.TableDrivedEntityID;
            result.ID = item.TableDrivedEntityID;
            result.RelationshipTailID = item.EntityRelationshipTailID;
            if (withDetails)
            {
                BizEntityRelationshipTail bizEntityRelationshipTail = new BizEntityRelationshipTail();
                result.RelationshipTail = bizEntityRelationshipTail.ToEntityRelationshipTailDTO(item.EntityRelationshipTail);
            }
            return result;
        }
        public bool UpdateLetterRelationshipTails(int entityID, List<LetterRelationshipTailDTO> list)
        {

            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var entity = projectContext.TableDrivedEntity.First(x => x.ID == entityID);
                while (entity.EntityLetterRelationshipTails.Any(x => x.TableDrivedEntityID == entityID))
                    projectContext.EntityLetterRelationshipTails.Remove(entity.EntityLetterRelationshipTails.First(x => x.TableDrivedEntityID == entityID));
                foreach (var item in list)
                {
                    EntityLetterRelationshipTails dbItem = new EntityLetterRelationshipTails();
                    dbItem.TableDrivedEntityID = entityID;
                    dbItem.EntityRelationshipTailID = item.RelationshipTailID;
                    projectContext.EntityLetterRelationshipTails.Add(dbItem);
                }
                projectContext.SaveChanges();
            }
            return true;

        }

        private MainLetterTemplateDTO ToMainLetterTemplateDTO(DR_Requester requester, MainLetterTemplate dbitem, bool withDetails)
        {
            MainLetterTemplateDTO result = new MainLetterTemplateDTO();
            result.ID = dbitem.ID;
            result.FileExtension = dbitem.FileExtension.Replace(".", "");
            result.Type = (LetterTemplateType)dbitem.Type;
            if (withDetails)
                result.Content = dbitem.Content;
            SetOwnerPart(requester, result, dbitem.LetterTemplate, withDetails);

            return result;

        }




        public void UpdateMainLetterTemplate(MainLetterTemplateDTO message)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var dbLetterTemplate = projectContext.MainLetterTemplate.FirstOrDefault(x => x.ID == message.ID);
                if (dbLetterTemplate == null)
                {
                    dbLetterTemplate = new DataAccess.MainLetterTemplate();
                    dbLetterTemplate.LetterTemplate = new LetterTemplate();
                }

                dbLetterTemplate.Type = (short)message.Type;
                dbLetterTemplate.Content = message.Content;
                dbLetterTemplate.FileExtension = message.FileExtension.Replace(".", "");

                SetOwnerDBPart(projectContext, dbLetterTemplate.LetterTemplate, message);

                if (dbLetterTemplate.ID == 0)
                    projectContext.MainLetterTemplate.Add(dbLetterTemplate);
                try
                {
                    // Your code...
                    // Could also be before try if you know the exception occurs in SaveChanges

                    projectContext.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }

            }
        }



        public List<LetterTemplateType> GetLetterTemplateTypes()
        {
            return Enum.GetValues(typeof(LetterTemplateType)).Cast<LetterTemplateType>().ToList();
        }
        public List<LetterTemplateFieldType> GetLetterTemplateFieldTypes()
        {
            return Enum.GetValues(typeof(LetterTemplateFieldType)).Cast<LetterTemplateFieldType>().ToList();
        }

        public List<LetterTemplateDTO> GetLetterTemplates(int entityID, bool? extarnal)
        {
            List<LetterTemplateDTO> result = new List<LetterTemplateDTO>();
            using (var projectContext = new MyProjectEntities())
            {
                var list = projectContext.LetterTemplate.Where(x => x.TableDrivedEntityID == entityID);
                if (extarnal == true)
                    list = list.Where(x => x.MainLetterTemplate != null);
                else if (extarnal == false)
                    list = list.Where(x => x.PartialLetterTemplate != null);
                foreach (var item in list)
                    result.Add(ToLetterTemplateDTO(item));
                return result;
            }
        }

        private LetterTemplateDTO ToLetterTemplateDTO(LetterTemplate item)
        {
            LetterTemplateDTO result = new LetterTemplateDTO();
            result.Name = item.Name;
            result.TableDrivedEntityID = item.TableDrivedEntityID;
            result.ID = item.ID;
            return result;
        }

        private void SetOwnerPart(DR_Requester requester, LetterTemplateDTO result, LetterTemplate ownerDbItem, bool withDetails)
        {
            result.TableDrivedEntityID = ownerDbItem.TableDrivedEntityID;
            result.Name = ownerDbItem.Name;
            result.EntityListViewID = ownerDbItem.EntityListViewID;
            if (withDetails)
            {
                BizEntityListView bizEntityListView = new BizEntityListView();

                result.EntityListView = bizEntityListView.GetEntityListView(requester, ownerDbItem.EntityListViewID);
                if (result.EntityListView == null)
                {
                    throw new Exception("عدم دسترسی به لیست نمایش به شناسه" + " " + ownerDbItem.EntityListViewID);
                }

                BizFormula bizFormula = new BizFormula();
                foreach (var item in ownerDbItem.LetterTemplatePlainField)
                {
                    LetterTemplatePlainFieldDTO field = new LetterTemplatePlainFieldDTO();
                    field.FieldName = item.FieldName;
                    field.EntityListViewColumnsID = (item.EntityListViewColumnsID == null) ? 0 : item.EntityListViewColumnsID.Value;
                    if (field.EntityListViewColumnsID != 0)
                        field.EntityListViewColumns = result.EntityListView.EntityListViewAllColumns.First(x => x.ID == item.EntityListViewColumnsID);
                    field.FormulaID = (item.FormulaID == null) ? 0 : item.FormulaID.Value;
                    field.ID = item.ID;
                    //if (item.ColumnID != null)
                    //    field.Type = LetterTemplateFieldType.Column;
                    //else if (item.ParameterID != null)
                    //    field.Type = LetterTemplateFieldType.Parameter;
                    //if (item.RelationshipID != null)
                    //    field.Type = LetterTemplateFieldType.RangeRelationship;

                    result.PlainFields.Add(field);
                }
                BizEntityRelationshipTail bizEntityRelationshipTail = new MyModelManager.BizEntityRelationshipTail();
                foreach (var item in ownerDbItem.LetterTemplateRelationshipField)
                {
                    LetterTemplateRelationshipFieldDTO field = new LetterTemplateRelationshipFieldDTO();
                    field.RelationshipTailID = item.EntityRelationshipTailID;
                    //field.EntityPreDefinedSearchID = (item.EntityPreDefinedSearchID == null) ? 0 : item.EntityPreDefinedSearchID.Value;
                    field.ID = item.ID;
                    field.RelationshipTail = bizEntityRelationshipTail.ToEntityRelationshipTailDTO(item.EntityRelationshipTail);
                    //field.NextLine = item.NextLine==true;
                    field.IsRow = item.IsRow == true;
                    field.FieldName = item.FieldName;
                    field.PartialLetterTemplateID = item.PartialLetterTemplateID;
                    field.PartialLetterTemplate = ToPartialLetterTemplateDTO(requester, item.PartialLetterTemplate, true);
                    //field.PartialLetterTemplate = ToPartialLetterTemplate(item.PartialLetterTemplate, true);
                    result.RelationshipFields.Add(field);
                }
            }
        }

        private void SetOwnerDBPart(MyProjectEntities projectContext, LetterTemplate owner, LetterTemplateDTO message)
        {
            owner.TableDrivedEntityID = message.TableDrivedEntityID;
            owner.Name = message.Name;
            owner.EntityListViewID = message.EntityListViewID;
            while (owner.LetterTemplatePlainField.Any())
                projectContext.LetterTemplatePlainField.Remove(owner.LetterTemplatePlainField.First());
            foreach (var item in message.PlainFields)
            {
                LetterTemplatePlainField dbField = new DataAccess.LetterTemplatePlainField();
                dbField.FieldName = item.FieldName;
                dbField.EntityListViewColumnsID = (item.EntityListViewColumnsID == 0) ? (int?)null : item.EntityListViewColumnsID;
                dbField.FormulaID = (item.FormulaID == 0) ? (int?)null : item.FormulaID;
                owner.LetterTemplatePlainField.Add(dbField);
            }

            while (owner.LetterTemplateRelationshipField.Any())
                projectContext.LetterTemplateRelationshipField.Remove(owner.LetterTemplateRelationshipField.First());
            foreach (var item in message.RelationshipFields)
            {
                LetterTemplateRelationshipField dbField = new DataAccess.LetterTemplateRelationshipField();
                dbField.FieldName = item.FieldName;
                dbField.EntityRelationshipTailID = item.RelationshipTailID;
                //dbField.NextLine = item.NextLine;
                dbField.IsRow = item.IsRow;
                //dbField.EntityPreDefinedSearchID = (item.EntityPreDefinedSearchID == 0) ? (int?)null : item.EntityPreDefinedSearchID;
                dbField.PartialLetterTemplateID = item.PartialLetterTemplateID;
                owner.LetterTemplateRelationshipField.Add(dbField);
            }

        }

        //public List<LetterRelationshipTemplateDTO> GetLetterRelationshipTemplates(int entityID)
        //{
        //    List<LetterRelationshipTemplateDTO> result = new List<LetterRelationshipTemplateDTO>();
        //    using (var projectContext = new MyProjectEntities())
        //    {
        //        var list = projectContext.LetterRelationshipTemplate.Where(x => x.TableDrivedEntityID == entityID);
        //        foreach (var item in list)
        //            result.Add(ToLetterRelationshipTemplateDTO(item, false));
        //        return result;
        //    }
        //}
        public List<PartialLetterTemplateDTO> GetPartialLetterTemplates(DR_Requester requester, int entityID)
        {
            List<PartialLetterTemplateDTO> result = new List<PartialLetterTemplateDTO>();
            using (var projectContext = new MyProjectEntities())
            {
                var list = projectContext.PartialLetterTemplate.Where(x => x.LetterTemplate.TableDrivedEntityID == entityID);
                foreach (var item in list)
                    result.Add(ToPartialLetterTemplateDTO(requester, item, false));
                return result;
            }
        }
        public PartialLetterTemplateDTO GetPartialLetterTepmplate(DR_Requester requester, int letterTempleteID)
        {
            using (var projectContext = new MyProjectEntities())
            {
                var dbitem = projectContext.PartialLetterTemplate.First(x => x.ID == letterTempleteID);
                return ToPartialLetterTemplateDTO(requester, dbitem, true);
            }
        }
        private PartialLetterTemplateDTO ToPartialLetterTemplateDTO(DR_Requester requester, PartialLetterTemplate dbitem, bool withDetails)
        {
            PartialLetterTemplateDTO result = new PartialLetterTemplateDTO();
            result.ID = dbitem.ID;
            SetOwnerPart(requester, result, dbitem.LetterTemplate, withDetails);
            return result;
        }

        public int UpdatePartialLetterTemplate(PartialLetterTemplateDTO message)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var dbLetterTemplate = projectContext.PartialLetterTemplate.FirstOrDefault(x => x.ID == message.ID);
                if (dbLetterTemplate == null)
                {
                    dbLetterTemplate = new DataAccess.PartialLetterTemplate();
                    dbLetterTemplate.LetterTemplate = new LetterTemplate();
                }
                SetOwnerDBPart(projectContext, dbLetterTemplate.LetterTemplate, message);

                if (dbLetterTemplate.ID == 0)
                    projectContext.PartialLetterTemplate.Add(dbLetterTemplate);
                projectContext.SaveChanges();
                return dbLetterTemplate.ID;

            }


        }
    }
}