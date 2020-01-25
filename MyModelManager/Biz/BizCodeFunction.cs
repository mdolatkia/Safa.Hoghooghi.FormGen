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
    public class BizCodeFunction
    {
        public List<CodeFunctionDTO> GetCodeFunctionsByEntityID(int entityID)
        {
            List<CodeFunctionDTO> result = new List<CodeFunctionDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                IQueryable<CodeFunction> listCodeFunction;
                listCodeFunction = projectContext.CodeFunction.Where(x => x.CodeFunction_TableDrivedEntity.Any(y => y.TableDrivedEntityID == entityID));
                foreach (var item in listCodeFunction)
                    result.Add(ToCodeFunctionDTO(item, false));

            }
            return result;
        }

        public List<CodeFunctionDTO> GetCodeFunctions(List<Enum_CodeFunctionParamType> paramTypes)
        {
            List<CodeFunctionDTO> result = new List<CodeFunctionDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                IQueryable<CodeFunction> listCodeFunction;
                // if (string.IsNullOrEmpty(catalog))
                listCodeFunction = projectContext.CodeFunction;
                // else
                //    listCodeFunction = projectContext.CodeFunction.Where(x => x.Catalog == catalog);
                if (paramTypes != null && paramTypes.Any())
                {
                    List<short> list = new List<short>();
                    paramTypes.ForEach(x => list.Add((short)x));
                    listCodeFunction = listCodeFunction.Where(x => list.Contains(x.Type));
                }
                foreach (var item in listCodeFunction)
                    result.Add(ToCodeFunctionDTO(item, false));

            }
            return result;
        }
        public CodeFunctionDTO GetCodeFunction(int CodeFunctionsID)
        {
            List<CodeFunctionDTO> result = new List<CodeFunctionDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var CodeFunctions = projectContext.CodeFunction.First(x => x.ID == CodeFunctionsID);
                return ToCodeFunctionDTO(CodeFunctions, true);
            }
        }
        public CodeFunctionDTO ToCodeFunctionDTO(CodeFunction item, bool withColumns)
        {
            BizColumn bizColumn = new BizColumn();
            CodeFunctionDTO result = new CodeFunctionDTO();

            result.ID = item.ID;
            result.ClassName = item.ClassName;
            //result.Catalog = item.Catalog;
            //if (item.TableDrivedEntityID != null)
            //    result.EntityID = item.TableDrivedEntityID.Value;
            result.FunctionName = item.FunctionName;
            result.Name = item.Name;
            result.ParamType = (Enum_CodeFunctionParamType)item.Type;
            result.RetrunType = item.ReturnType;
            result.RetrunDotNetType = Type.GetType(item.ReturnType);
            //if (item.ValueCustomType != null)
            //    result.ValueCustomType = (ValueCustomType)item.ValueCustomType;
            result.Path = item.Path;
            if (withColumns)
            {
                result.Parameters = ToCodeFunctionParameterDTO(item);
            }
            return result;
        }
        public int UpdateCodeFunctions(CodeFunctionDTO CodeFunction)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var dbCodeFunction = projectContext.CodeFunction.FirstOrDefault(x => x.ID == CodeFunction.ID);
                if (dbCodeFunction == null)
                    dbCodeFunction = new DataAccess.CodeFunction();
                dbCodeFunction.ID = CodeFunction.ID;
                if (CodeFunction.EntityID != 0)
                {
                    if (!dbCodeFunction.CodeFunction_TableDrivedEntity.Any(x => x.TableDrivedEntityID == CodeFunction.EntityID))
                    {
                        dbCodeFunction.CodeFunction_TableDrivedEntity.Add(new CodeFunction_TableDrivedEntity() { TableDrivedEntityID = CodeFunction.EntityID, ShowInFormula = CodeFunction.ShowInFormula });
                    }
                }
                //    dbCodeFunction.TableDrivedEntityID = CodeFunction.EntityID;
                //else
                //    dbCodeFunction.TableDrivedEntityID = null;
                //dbCodeFunction.ValueCustomType = (short)CodeFunction.ValueCustomType;

                dbCodeFunction.Name = CodeFunction.Name;
                dbCodeFunction.ClassName = CodeFunction.ClassName;
                dbCodeFunction.Path = CodeFunction.Path;
                dbCodeFunction.Type = (short)CodeFunction.ParamType;
                //dbCodeFunction.Catalog = CodeFunction.Catalog;
                dbCodeFunction.FunctionName = CodeFunction.FunctionName;
                dbCodeFunction.ReturnType = CodeFunction.RetrunType;
                //var entityRelation = dbCodeFunction.CodeFunction_TableDrivedEntity.FirstOrDefault(x => x.TableDrivedEntityID == CodeFunction.EntityID);
                //if(entityRelation==null)
                //{
                //    entityRelation = new CodeFunction_TableDrivedEntity();
                //    entityRelation.TableDrivedEntityID = CodeFunction.EntityID;
                //    dbCodeFunction.CodeFunction_TableDrivedEntity.Add(entityRelation);
                //}

                if (dbCodeFunction.ID == 0)
                    projectContext.CodeFunction.Add(dbCodeFunction);

                var listParams = CodeFunction.Parameters.Select(x => x.ParameterName).ToList();
                var removelist = dbCodeFunction.CodeFunctionParameter.Where(x => !listParams.Contains(x.ParamName)).ToList();
                foreach (var removeItem in removelist)
                {
                    var removeItemUsedList = removeItem.CodeFunction_TableDrivedEntity_Parameters.ToList();
                    foreach (var removeUsedItem in removeItemUsedList)
                    {
                        removeItem.CodeFunction_TableDrivedEntity_Parameters.Remove(removeUsedItem);
                    }
                    dbCodeFunction.CodeFunctionParameter.Remove(removeItem);
                }
                foreach (var column in CodeFunction.Parameters)
                {

                    CodeFunctionParameter dbColumn = dbCodeFunction.CodeFunctionParameter.FirstOrDefault(x => x.ParamName == column.ParameterName);
                    if (dbColumn == null)
                    {
                        dbColumn = new DataAccess.CodeFunctionParameter();
                        dbCodeFunction.CodeFunctionParameter.Add(dbColumn);
                    }
                    dbColumn.ParamName = column.ParameterName;
                    dbColumn.DataType = column.DataType;


                    //if (column.ColumnID != 0)
                    //{
                    //    var parameterUsed = dbColumn.CodeFunction_TableDrivedEntity_Parameters.FirstOrDefault(x => x.CodeFunction_TableDrivedEntity == entityRelation && x.ColumnID == column.ColumnID);
                    //    if(parameterUsed==null)
                    //    {
                    //        parameterUsed = new CodeFunction_TableDrivedEntity_Parameters();
                    //        parameterUsed.ColumnID = column.ColumnID;
                    //        parameterUsed.CodeFunctionParameter = dbColumn;
                    //        parameterUsed.CodeFunction_TableDrivedEntity = entityRelation;
                    //        dbColumn.CodeFunction_TableDrivedEntity_Parameters.Add(parameterUsed);
                    //    }
                    //}


                }

                //var codeFunctionEntity = dbCodeFunction.CodeFunction_TableDrivedEntity.FirstOrDefault(x => x.TableDrivedEntityID == CodeFunction.EntityID);
                //if (codeFunctionEntity == null)
                //{
                //    codeFunctionEntity = new CodeFunction_TableDrivedEntity();
                //    dbCodeFunction.CodeFunction_TableDrivedEntity.Add(codeFunctionEntity);
                //}


                projectContext.SaveChanges();

                return dbCodeFunction.ID;
            }
        }















        public List<CodeFunctionColumnDTO> GetCodeFunctionParameters(int functionID)
        {

            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var CodeFunction = projectContext.CodeFunction.First(x => x.ID == functionID);
                return ToCodeFunctionParameterDTO(CodeFunction);
            }
        }
        private List<CodeFunctionColumnDTO> ToCodeFunctionParameterDTO(CodeFunction cItem)
        {
            BizColumn bizColumn = new BizColumn();
            List<CodeFunctionColumnDTO> result = new List<CodeFunctionColumnDTO>();
            foreach (var column in cItem.CodeFunctionParameter)
            {
                result.Add(new CodeFunctionColumnDTO() { ID = column.ID, DataType = column.DataType, ParameterName = column.ParamName, DotNetType = Type.GetType(column.DataType) });
            }
            return result;
        }

        public CodeFunction_EntityDTO GetCodeFunctionEntity(int functionEntityID)
        {

            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var CodeFunctions = projectContext.CodeFunction_TableDrivedEntity.First(x => x.ID == functionEntityID);
                return ToCodeFunction_EntityDTO(CodeFunctions, true);
            }
        }
        public CodeFunction_EntityDTO GetCodeFunctionEntity(int functionID, int entityID)
        {

            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var CodeFunctions = projectContext.CodeFunction_TableDrivedEntity.FirstOrDefault(x => x.TableDrivedEntityID == entityID && x.CodeFunctionID == functionID);
                if (CodeFunctions != null)
                    return ToCodeFunction_EntityDTO(CodeFunctions, true);
                else
                    return null;
            }
        }
        public List<CodeFunction_EntityDTO> GetCodeFunctionEntities(int entityID)
        {
            List<CodeFunction_EntityDTO> result = new List<CodeFunction_EntityDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                //projectContext.Configuration.LazyLoadingEnabled = false;

                var listCodeFunction = projectContext.CodeFunction_TableDrivedEntity.Where(x => x.TableDrivedEntityID == entityID);
                foreach (var item in listCodeFunction)
                    result.Add(ToCodeFunction_EntityDTO(item, false));

            }
            return result;
        }
        private CodeFunction_EntityDTO ToCodeFunction_EntityDTO(CodeFunction_TableDrivedEntity cItem, bool withColumns)
        {
            BizColumn bizColumn = new BizColumn();
            var result = new CodeFunction_EntityDTO();
            result.ID = cItem.ID;
            result.ShowInFormula = cItem.ShowInFormula;
            result.EntityID = cItem.TableDrivedEntityID;
            result.CodeFunctionID = cItem.CodeFunctionID;
            result.CodeFunction = ToCodeFunctionDTO(cItem.CodeFunction, withColumns);
            if (withColumns)
            {
                result.CodeFunctionEntityColumns = ToCodeFunctionEntityColumnsDTO(cItem);
            }

            return result;
        }
        public int GetCodeFunctionEntityID(int entityID, int CodeFunctionID)
        {
            List<CodeFunction_EntityDTO> result = new List<CodeFunction_EntityDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                //projectContext.Configuration.LazyLoadingEnabled = false;

                var item = projectContext.CodeFunction_TableDrivedEntity.FirstOrDefault(x => x.TableDrivedEntityID == entityID && x.CodeFunctionID == CodeFunctionID);
                if (item != null)
                    return item.ID;
                else
                    return 0;

            }
        }
        private List<CodeFunction_Entity_ColumnDTO> ToCodeFunctionEntityColumnsDTO(CodeFunction_TableDrivedEntity cItem)
        {
            BizColumn bizColumn = new BizColumn();
            List<CodeFunction_Entity_ColumnDTO> result = new List<CodeFunction_Entity_ColumnDTO>();
            foreach (var column in cItem.CodeFunction_TableDrivedEntity_Parameters)
            {
                var item = new CodeFunction_Entity_ColumnDTO();
                item.ID = column.ID;
                item.CodeFunctionParameterID = column.CodeFunctionParameterID;
                item.CodeFunction_EntityID = column.CodeFunction_TableDrivedEntityID;
                item.ColumnID = column.ColumnID;

                item.FunctionColumnDotNetType = Type.GetType(column.CodeFunctionParameter.DataType);
                item.FunctionColumnParamName = column.CodeFunctionParameter.ParamName;
                result.Add(item);
            }
            return result;
        }
        public bool AddCodeFunctionToEntity(int codefuntionID, int entityID, bool showInFormula)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var dbCodeFunctionEntity = projectContext.CodeFunction_TableDrivedEntity.FirstOrDefault(x => x.TableDrivedEntityID == entityID && x.CodeFunctionID == codefuntionID);
                if (dbCodeFunctionEntity == null)
                {
                    projectContext.CodeFunction_TableDrivedEntity.Add(new CodeFunction_TableDrivedEntity() { CodeFunctionID = codefuntionID, TableDrivedEntityID = entityID, ShowInFormula = showInFormula });
                }
                else
                {
                    dbCodeFunctionEntity.ShowInFormula = showInFormula;
                }
                projectContext.SaveChanges();
            }
            return true;
        }
        //public int UpdateCodeFunctionEntity(CodeFunction_EntityDTO CodeFunctionEntity)
        //{
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var dbCodeFunctionEntity = projectContext.CodeFunction_TableDrivedEntity.FirstOrDefault(x => x.ID == CodeFunctionEntity.ID);
        //        if (dbCodeFunctionEntity == null)
        //            dbCodeFunctionEntity = new CodeFunction_TableDrivedEntity();
        //        dbCodeFunctionEntity.CodeFunctionID = CodeFunctionEntity.CodeFunctionID;
        //        dbCodeFunctionEntity.TableDrivedEntityID = CodeFunctionEntity.EntityID;

        //        while (dbCodeFunctionEntity.CodeFunction_TableDrivedEntity_Parameters.Any())
        //        {
        //            dbCodeFunctionEntity.CodeFunction_TableDrivedEntity_Parameters.Remove(dbCodeFunctionEntity.CodeFunction_TableDrivedEntity_Parameters.First());
        //        }
        //        if (!CodeFunctionEntity.CodeFunctionEntityColumns.Any(x => x.ColumnID == 0))
        //        {
        //            foreach (var column in CodeFunctionEntity.CodeFunctionEntityColumns)
        //            {
        //                CodeFunction_TableDrivedEntity_Parameters dbColumn = new DataAccess.CodeFunction_TableDrivedEntity_Parameters();
        //                dbColumn.ColumnID = column.ColumnID;
        //                dbColumn.CodeFunctionParameterID = column.CodeFunctionParameterID;
        //                dbCodeFunctionEntity.CodeFunction_TableDrivedEntity_Parameters.Add(dbColumn);
        //            }
        //        }
        //        if (dbCodeFunctionEntity.ID == 0)
        //            projectContext.CodeFunction_TableDrivedEntity.Add(dbCodeFunctionEntity);
        //        projectContext.SaveChanges();
        //        return dbCodeFunctionEntity.ID;
        //    }
        //}
    }

}
