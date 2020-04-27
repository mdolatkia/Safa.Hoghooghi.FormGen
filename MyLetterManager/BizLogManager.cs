using DataAccess;
using MyDataItemManager;
using MyGeneralLibrary;
using MyModelManager;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLogManager
{
    public class BizLogManager
    {
        BizDataItem bizDataItem = new BizDataItem();
        BizUser bizUser = new BizUser();
        BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();

        public List<DataLogDTO> SearchDataLogs(DR_Requester requester, int entityID, DateTime? fromDate, DateTime? toDate, DP_BaseData dataItem, DataLogType? logType, int columnID, int userID, bool? withMajorException, bool? withMinorException)
        {
            List<DataLogDTO> result = new List<DataLogDTO>();
            using (var context = new MyIdeaDataDBEntities())
            {
                var dataLogs = context.DataLog as IQueryable<DataLog>;
                if (entityID != 0)
                    dataLogs = dataLogs.Where(x => x.MyDataItem.TableDrivedEntityID == entityID);
                if (fromDate != null)
                {
                    dataLogs = dataLogs.Where(x => x.Date >= fromDate);
                }
                if (toDate != null)
                {
                    dataLogs = dataLogs.Where(x => x.Date <= toDate);
                }
                if (logType != null)
                {
                    dataLogs = dataLogs.Where(x => x.MainType == (short)logType);
                }
                if (dataItem != null)
                {
                    int dataItemID = bizDataItem.GetDataItemID(dataItem.TargetEntityID, dataItem.KeyProperties);
                    if (dataItemID == 0)
                        dataItemID = -1;
                    dataLogs = dataLogs.Where(x => x.MyDataItemID == dataItemID);
                }
                if (columnID != 0)
                    dataLogs = dataLogs.Where(x => x.EditDataItemColumnDetails.Any(y => y.ColumnID == columnID));
                if (userID != 0)
                    dataLogs = dataLogs.Where(x => x.UserID == userID);
                if (withMajorException != null)
                    dataLogs = dataLogs.Where(x => x.MajorFunctionException == withMajorException);
                if (withMinorException != null)
                    dataLogs = dataLogs.Where(x => x.MinorFunctionException == withMinorException);
                foreach (var item in dataLogs.OrderBy(x => x.ID))
                {
                    result.Add(ToDataLogDTO(requester, item, false));
                }
            }
            return result;
        }
        public List<DataLogDTO> SearchDataLogs(DR_Requester requester, int relatedITemID, DataLogType? logType)
        {
            List<DataLogDTO> result = new List<DataLogDTO>();
            using (var context = new MyIdeaDataDBEntities())
            {
                var dataLogs = context.DataLog as IQueryable<DataLog>;
                dataLogs = dataLogs.Where(x => x.RelatedItemID == relatedITemID);
                if (logType != null)
                {
                    dataLogs = dataLogs.Where(x => x.MainType == (short)logType);
                }
                foreach (var item in dataLogs.OrderBy(x => x.ID))
                {
                    result.Add(ToDataLogDTO(requester, item, false));
                }
            }
            return result;
        }


        public List<DataLogDTO> GetDataLogsByPackageID(DR_Requester requester, Guid guid)
        {
            List<DataLogDTO> result = new List<DataLogDTO>();
            using (var context = new MyIdeaDataDBEntities())
            {
                var dataLogs = context.DataLog.Where(x => x.PackageGuid == guid);
                foreach (var item in dataLogs.OrderBy(x => x.ID))
                {
                    result.Add(ToDataLogDTO(requester, item, false));
                }
            }
            return result;
        }

        public DataLogDTO GetDataLog(DR_Requester requester, int iD)
        {
            using (var context = new MyIdeaDataDBEntities())
            {
                var dbLog = context.DataLog.First(x => x.ID == iD);
                return ToDataLogDTO(requester, dbLog, true);
            }
        }

        private DataLogDTO ToDataLogDTO(DR_Requester requester, DataLog item, bool withDetails)
        {
            var message = new DataLogDTO();
            message.ID = item.ID;
            if (item.MyDataItemID != null && item.MyDataItemID != 0)
                message.DatItem = bizDataItem.ToDataViewDTO(requester, item.MyDataItem, withDetails);
            message.Date = item.Date;
            message.vwPersianDate = GeneralHelper.GetShamsiDate(item.Date);
            //  message.DataInfo = item.DataInfo;
            var entity = bizTableDrivedEntity.GetTableDrivedEntity(requester, item.MyDataItem.TableDrivedEntityID, ModelEntites.EntityColumnInfoType.WithSimpleColumns, ModelEntites.EntityRelationshipInfoType.WithoutRelationships);
            if (item.MyDataItem != null)
            {
                //بهتر شود

                message.vwEntityAlias = entity.Alias;
            }
            message.Duration = item.Duration ?? 0;
            message.MajorException = item.MajorFunctionException;
            message.MinorException = item.MinorFunctionException;
            message.MajorFunctionExceptionMessage = item.MajorFunctionExceptionMessage;
            message.LocationInfo = item.LocationInfo;
            message.PackageGuid = item.PackageGuid;
            message.Time = item.Time;
            message.MainType = (DataLogType)item.MainType;
            message.UserID = item.UserID;
            message.vwUserInfo = bizUser.GetUserFullName(item.UserID);
            if (withDetails)
            {
                foreach (var columnLog in item.EditDataItemColumnDetails)
                {
                    var colMsg = new EditDataItemColumnDetailsDTO();
                    colMsg.ColumnID = columnLog.ColumnID;
                    var column = entity.Columns.FirstOrDefault(x => x.ID == colMsg.ColumnID);
                    if (column != null)
                    {
                        colMsg.vwColumnName = column.Name;
                        colMsg.vwColumnAlias = column.Alias;
                    }
                    colMsg.Info = columnLog.Info;
                    colMsg.NewValue = columnLog.NewValue;
                    colMsg.OldValue = columnLog.OldValue;
                    message.EditDataItemColumnDetails.Add(colMsg);
                    colMsg.FormulaException = columnLog.FormulaException;
                    colMsg.FormulaID = columnLog.FormulaID ?? 0;
                    foreach (var formulaParam in columnLog.FormulaUsageParemeters)
                    {
                        FormulaUsageParemetersDTO paramDTO = new FormulaUsageParemetersDTO();
                        paramDTO.ID = formulaParam.ID;
                        paramDTO.ParameterName = formulaParam.ParameterName;
                        paramDTO.ParameterValue = formulaParam.ParameterValue;
                        paramDTO.RelationshipPropertyTail = formulaParam.RelationshipKeyColumnTail;
                        colMsg.FormulaUsageParemeters.Add(paramDTO);
                    }
                }
                if (item.EditDataItemExceptionLog != null)
                {
                    message.EditDataItemExceptionLog = new EditDataItemExceptionLogDTO();
                    message.EditDataItemExceptionLog.AfterSaveActionExceptionMessage = item.EditDataItemExceptionLog.AfterSaveActionExceptionMessage;
                    //message.EditDataItemExceptionLog.BeforeSaveActionExceptionMessage = item.EditDataItemExceptionLog.BeforeSaveActionExceptionMessage;
                    //message.EditDataItemExceptionLog.DataUpdateExceptionMessage = item.EditDataItemExceptionLog.DataUpdateExceptionMessage;
                    message.EditDataItemExceptionLog.DataUpdateQuery = item.EditDataItemExceptionLog.DataUpdateQuery;
                    //message.EditDataItemExceptionLog.ForumulaUsageExceptionMessage = item.EditDataItemExceptionLog.FormulaUsageExceptionMessage;
                    message.EditDataItemExceptionLog.ID = item.EditDataItemExceptionLog.ID;
                }
                //if (item.ArchiveItemLog != null)
                //{
                //    message.ArchiveItemLog = new  ArchiveItemLogDTO();
                //    message.ArchiveItemLog.ID = item.ArchiveItemLog.ID;
                //    message.ArchiveItemLog.Exception = item.ArchiveItemLog.Exception;
                //}
            }

            return message;
        }

        public string AddLog(DataLogDTO messages, DR_Requester requester)
        {
            try
            {
                using (var context = new MyIdeaDataDBEntities())
                {

                    AddLogInternal(messages, context, requester);

                    context.SaveChanges();
                    return "";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string AddLogs(List<DataLogDTO> messages, DR_Requester requester)
        {
            try
            {
                using (var context = new MyIdeaDataDBEntities())
                {
                    foreach (var item in messages)
                    {
                        AddLogInternal(item, context, requester);
                    }
                    context.SaveChanges();
                    return "";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        private void AddLogInternal(DataLogDTO message, MyIdeaDataDBEntities context, DR_Requester requester)
        {
            var dbLog = new DataLog();
            if (message.DatItem.DataItemID == 0)
            {
                dbLog.MyDataItemID = bizDataItem.GetOrCreateDataItem(message.DatItem);
            }
            else
            {
                dbLog.MyDataItemID = message.DatItem.DataItemID;
            }

            // dbLog.DataInfo = message.DatItem.ViewInfo;
            dbLog.Date = DateTime.Now;
            dbLog.Time = DateTime.Now.ToString("HH:mm:ss");
            dbLog.Duration = message.Duration;
            dbLog.MajorFunctionException = message.MajorException;
            dbLog.MinorFunctionException = message.MinorException;
            dbLog.MajorFunctionExceptionMessage = message.MajorFunctionExceptionMessage;
            dbLog.LocationInfo = message.LocationInfo;
            dbLog.RelatedItemID = message.RelatedItemID;
            dbLog.PackageGuid = message.PackageGuid;
            dbLog.MainType = (short)message.MainType;
            dbLog.UserID = requester.Identity;
            if (message.EditDataItemExceptionLog != null)
            {
                dbLog.EditDataItemExceptionLog = new EditDataItemExceptionLog();
                dbLog.EditDataItemExceptionLog.AfterSaveActionExceptionMessage = message.EditDataItemExceptionLog.AfterSaveActionExceptionMessage;
                //dbLog.EditDataItemExceptionLog.BeforeSaveActionExceptionMessage = message.EditDataItemExceptionLog.BeforeSaveActionExceptionMessage;
                //dbLog.EditDataItemExceptionLog.DataUpdateExceptionMessage = message.EditDataItemExceptionLog.DataUpdateExceptionMessage;
                //dbLog.EditDataItemExceptionLog.FormulaUsageExceptionMessage = message.EditDataItemExceptionLog.ForumulaUsageExceptionMessage;
                dbLog.EditDataItemExceptionLog.DataUpdateQuery = message.EditDataItemExceptionLog.DataUpdateQuery;

            }
            if (message.EditDataItemColumnDetails != null)
            {
                foreach (var messageCol in message.EditDataItemColumnDetails)
                {
                    EditDataItemColumnDetails dbCol = new EditDataItemColumnDetails();
                    dbCol.ColumnID = messageCol.ColumnID;
                    dbCol.Info = messageCol.Info;
                    dbCol.NewValue = messageCol.NewValue == null ? "<Null>" : messageCol.NewValue.ToString();
                    dbCol.OldValue = messageCol.OldValue == null ? "<Null>" : messageCol.OldValue.ToString();
                    dbCol.FormulaException = messageCol.FormulaException;
                    dbCol.FormulaID = messageCol.FormulaID;
                    dbLog.EditDataItemColumnDetails.Add(dbCol);
                    foreach (var formulaParam in messageCol.FormulaUsageParemeters)
                    {
                        FormulaUsageParemeters paramDB = new FormulaUsageParemeters();
                        paramDB.ID = formulaParam.ID;
                        paramDB.ParameterName = formulaParam.ParameterName;
                        paramDB.ParameterValue = formulaParam.ParameterValue;
                        paramDB.RelationshipKeyColumnTail = formulaParam.RelationshipPropertyTail;
                        dbCol.FormulaUsageParemeters.Add(paramDB);
                    }
                }
            }
            //if (message.ArchiveItemLog != null)
            //{
            //    dbLog.ArchiveItemLog = new ArchiveItemLog();
            //    dbLog.ArchiveItemLog.Exception = message.ArchiveItemLog.Exception;
            //}
            context.DataLog.Add(dbLog);
        }
    }
}
