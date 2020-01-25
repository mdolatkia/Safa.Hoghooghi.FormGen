using DataAccess;
using ModelEntites;
using MyDataManagerBusiness;
using MyGeneralLibrary;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProxyLibrary;

namespace MyModelManager
{
    public class BizFormula
    {


        public FormulaDTO GetFormula(int FormulaID, bool withDetails)
        {

            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var Formula = projectContext.Formula.First(x => x.ID == FormulaID);
                return ToFormulaDTO(Formula, withDetails);
            }
        }

        public List<FormulaDTO> GetFormulas(int entityID)
        {
            List<FormulaDTO> result = new List<FormulaDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var Formulas = projectContext.Formula.Where(x => x.TableDrivedEntityID == entityID);
                foreach (var item in Formulas)
                    result.Add(ToFormulaDTO(item, false));
                return result;
            }
        }

        public FormulaDTO ToFormulaDTO(DataAccess.Formula item, bool withDetails)
        {
            FormulaDTO result = new FormulaDTO();
            result.Name = item.Name;
            result.ID = item.ID;
            result.EntityID = item.TableDrivedEntityID;
            result.Formula = item.Formula1;
            result.Title = item.Title;
            //result.ValueCustomType = (ValueCustomType)item.ValueCustomType;
            result.ResultDotNetType = GetFormulaDotNetType(item.ResultType);
            result.ResultType = item.ResultType;
            //////BizFormulaUsage
            //////result.FormulaUsed = item.FormulaUsage.Any();
            if (withDetails)
            {
                foreach (var dbFormulaItem in item.FormulaItems1)
                {
                    result.FormulaItems.Add(ToFormualaItemDTO(dbFormulaItem));
                }
            }
            return result;
        }





        private FormulaItemDTO ToFormualaItemDTO(FormulaItems dbFormulaItem)
        {
            FormulaItemDTO formulaItem = new FormulaItemDTO();
            formulaItem.ID = dbFormulaItem.ID;
            formulaItem.RelationshipIDTail = dbFormulaItem.RelationshipIDTail;
            formulaItem.RelationshipNameTail = dbFormulaItem.RelationshipNameTail;
            formulaItem.FormulaID = dbFormulaItem.FormulaID ?? 0;
            formulaItem.ItemTitle = dbFormulaItem.ItemTitle;
            if (dbFormulaItem.ColumnID != null)
            {
                formulaItem.ItemID = dbFormulaItem.ColumnID.Value;
                formulaItem.ItemType = FormuaItemType.Column;
            }
            else if (dbFormulaItem.FormulaParameterID != null)
            {
                formulaItem.ItemID = dbFormulaItem.FormulaParameterID.Value;
                formulaItem.ItemType = FormuaItemType.FormulaParameter;
            }
            else if (dbFormulaItem.RelationshipID != null)
            {
                formulaItem.ItemID = dbFormulaItem.RelationshipID.Value;
                formulaItem.ItemType = FormuaItemType.Relationship;
            }

            //foreach (var dbcItem in dbFormulaItem.FormulaItems1)
            //{
            //    formulaItem.ChildFormulaItems.Add(ToFormualaItemDTO(dbcItem));
            //}

            return formulaItem;
        }

        public Type GetFormulaDotNetType(int forumlaID)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var Formula = projectContext.Formula.First(x => x.ID == forumlaID);
                return GetFormulaDotNetType(Formula.ResultType);
            }
        }

        private Type GetFormulaDotNetType(string resultType)
        {
            return Type.GetType(resultType);
        }



        //public FormulaParameterDTO GetFormulaParameter(int formulaParameterID)
        //{
        //    List<FormulaParameterDTO> result = new List<FormulaParameterDTO>();
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        return ToFormulaParameterDTO(projectContext.FormulaParameter.First(x => x.ID == formulaParameterID));
        //    }
        //}
        //public List<FormulaParameterDTO> GetFormulaParameters(int entityID)
        //{
        //    List<FormulaParameterDTO> result = new List<FormulaParameterDTO>();
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var list = projectContext.FormulaParameter.Where(x => x.TableDrivedEntityID == entityID);
        //        foreach (var item in list)
        //            result.Add(ToFormulaParameterDTO(item));
        //        return result;
        //    }
        //}

        //private FormulaParameterDTO ToFormulaParameterDTO(FormulaParameter cItem)
        //{
        //    var result = new FormulaParameterDTO();
        //    result.ID = cItem.ID;
        //    result.FormulaID = cItem.FormulaID;
        //    result.EntityID = cItem.TableDrivedEntityID;
        //    result.Name = cItem.Name;
        //    result.Title = cItem.Title;
        //    result.ResultType = GetFormulaDotNetType(cItem.Formula.ResultType);
        //    return result;
        //}







        //public int UpdateDatabaseFunction(DatabaseFunctionDTO DatabaseFunction)
        //{
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var dbDatabaseFunction = projectContext.DatabaseFunction.FirstOrDefault(x => x.ID == DatabaseFunction.ID);
        //        if (dbDatabaseFunction == null)
        //            dbDatabaseFunction = new DataAccess.DatabaseFunction();
        //        dbDatabaseFunction.FunctionName = DatabaseFunction.FunctionName;
        //        dbDatabaseFunction.ID = DatabaseFunction.ID;
        //        if (DatabaseFunction.EntityID != 0)
        //            dbDatabaseFunction.TableDrivedEntityID = DatabaseFunction.EntityID;
        //        dbDatabaseFunction.Title = DatabaseFunction.Title;
        //        dbDatabaseFunction.ReturnType = DatabaseFunction.ReturnType;
        //        dbDatabaseFunction.RelatedSchema = DatabaseFunction.Schema;
        //        while (dbDatabaseFunction.DatabaseFunctionParameter.Any())
        //        {
        //            dbDatabaseFunction.DatabaseFunctionParameter.Remove(dbDatabaseFunction.DatabaseFunctionParameter.First());
        //        }
        //        foreach (var column in DatabaseFunction.DatabaseFunctionParameter)
        //        {
        //            DatabaseFunctionParameter dbColumn = new DataAccess.DatabaseFunctionParameter();
        //            dbColumn.ColumnID = column.ColumnID;
        //            dbColumn.DataType = column.DataType;
        //            dbColumn.ParamName = column.ParameterName;
        //            dbDatabaseFunction.DatabaseFunctionParameter.Add(dbColumn);
        //        }
        //        if (dbDatabaseFunction.ID == 0)
        //            projectContext.DatabaseFunction.Add(dbDatabaseFunction);
        //        projectContext.SaveChanges();
        //        return dbDatabaseFunction.ID;
        //    }
        //}


        //private Enum_ColumnType ConvertParameterResultType(string resultType)
        //{
        //    if (string.IsNullOrEmpty(resultType))
        //        return Enum_ColumnType.None;
        //    else if (resultType == "String")
        //        return Enum_ColumnType.String;
        //    else if (resultType == "Numeric")
        //        return Enum_ColumnType.Numeric;
        //    else if (resultType == "Boolean")
        //        return Enum_ColumnType.Boolean;
        //    else if (resultType == "Date")
        //        return Enum_ColumnType.Date;
        //    return Enum_ColumnType.None;
        //}
        //private string ConvertParameterResultType(Enum_ColumnType resultType)
        //{
        //    return resultType.ToString();
        //}
       

     

       

        public int UpdateFormulas(FormulaDTO Formula)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var dbFormula = projectContext.Formula.FirstOrDefault(x => x.ID == Formula.ID);
                if (dbFormula == null)
                    dbFormula = new DataAccess.Formula();
                dbFormula.Name = Formula.Name;
                dbFormula.ID = Formula.ID;
                dbFormula.TableDrivedEntityID = Formula.EntityID;
                dbFormula.Formula1 = Formula.Formula;
                dbFormula.ResultType = Formula.ResultType;
                //dbFormula.ValueCustomType = (Int16)Formula.ValueCustomType;
                dbFormula.Title = Formula.Title;
                while (dbFormula.FormulaItems1.Any())
                    projectContext.FormulaItems.Remove(dbFormula.FormulaItems1.First());
                foreach (var formulaItem in Formula.FormulaItems)
                {
                    var dbFormulaItem = ToFormualaItem(formulaItem, dbFormula);
                    dbFormula.FormulaItems1.Add(dbFormulaItem);
                }
                if (dbFormula.ID == 0)
                    projectContext.Formula.Add(dbFormula);
                projectContext.SaveChanges();
                return dbFormula.ID;
            }
        }

        //private void RemoveFormulaItem(MyProjectEntities projectContext, FormulaItems formulaItems)
        //{
        //    while (formulaItems.Any())
        //    {
        //        projectContext.FormulaItems.Remove(formulaItems);
        //    }
        //}

    
        private FormulaItems ToFormualaItem(FormulaItemDTO formulaItem, Formula dbFormula)
        {
            FormulaItems dbItem = new FormulaItems();
            dbItem.ID = formulaItem.ID;
            dbItem.RelationshipIDTail = formulaItem.RelationshipIDTail;
            dbItem.RelationshipNameTail = formulaItem.RelationshipNameTail;
            dbItem.ItemTitle = formulaItem.ItemTitle;
            if (formulaItem.ItemType == FormuaItemType.Column)
                dbItem.ColumnID = formulaItem.ItemID;
            else if (formulaItem.ItemType == FormuaItemType.FormulaParameter)
                dbItem.FormulaParameterID = formulaItem.ItemID;
            else if (formulaItem.ItemType == FormuaItemType.Relationship)
                dbItem.RelationshipID = formulaItem.ItemID;
            else if (formulaItem.ItemType == FormuaItemType.DatabaseFunction)
                dbItem.DatabaseFunctionID = formulaItem.ItemID;
            else if (formulaItem.ItemType == FormuaItemType.Code)
                dbItem.CodeFunctionID = formulaItem.ItemID;
            else if (formulaItem.ItemType == FormuaItemType.State)
                dbItem.TableDrivedEntityStateID = formulaItem.ItemID;
            //foreach (var citem in formulaItem.ChildFormulaItems)
            //{
            //    var sendRelationshipTail = relationshipTail;
            //    if (dbItem.RelationshipID != null)
            //        sendRelationshipTail = (sendRelationshipTail == "" ? dbItem.RelationshipID.ToString() : sendRelationshipTail + "," + dbItem.RelationshipID);

            //    var childItem = ToFormualaItem(citem, dbFormula, sendRelationshipTail);
            //    dbItem.FormulaItems1.Add(childItem);
            //}

            return dbItem;
        }

        //private void AddDBFormulaItem(Formula dbFormula, ICollection<FormulaItems> formulaItems, FormulaItemDTO formulaItem)
        //{

        //    dbItem.Formula = dbFormula;
        //    formulaItems.Add(dbItem);



        //}

        //public int UpdateFormulaParameterss(FormulaParameterDTO formulaParameter)
        //{
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var dbFormulaParameter = projectContext.FormulaParameter.FirstOrDefault(x => x.ID == formulaParameter.ID);
        //        if (dbFormulaParameter == null)
        //            dbFormulaParameter = new DataAccess.FormulaParameter();
        //        dbFormulaParameter.Name = formulaParameter.Name;
        //        dbFormulaParameter.ID = formulaParameter.ID;
        //        if (formulaParameter.EntityID != 0)
        //            dbFormulaParameter.TableDrivedEntityID = formulaParameter.EntityID;
        //        dbFormulaParameter.FormulaID = formulaParameter.FormulaID;
        //        dbFormulaParameter.Title = formulaParameter.Title;

        //        if (dbFormulaParameter.ID == 0)
        //            projectContext.FormulaParameter.Add(dbFormulaParameter);
        //        projectContext.SaveChanges();
        //        return dbFormulaParameter.ID;
        //    }
        //}



    }

}
