using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLogManager
{
    public class LogHelper
    {
        public bool InsertRequestLog(List<MainLogDTO> logs)
        {
            using (var logContext = new MyLogDBEntities())
            {
                foreach (var log in logs)
                {
                    var result = InsertRequestLog(log, logContext);
                }
            }
            return true;
        }

        public bool InsertRequestLog(MainLogDTO log, MyLogDBEntities logContext)
        {
            MainLog dbLog = new MainLog();
            dbLog.UserID = log.UserID;
            if (log.PackageGuid != null && log.PackageGuid != Guid.Empty)
                dbLog.PackageGuid = log.PackageGuid;
            dbLog.Date = DateTime.Now.ToShortDateString();
            dbLog.Time = DateTime.Now.TimeOfDay;
            dbLog.Type = (Int16)log.Type;
            dbLog.LocationInfo = log.LocationInfo;
            if (log.EnityID != 0)
                dbLog.EntityID = log.EnityID;
            else
                dbLog.EntityID = null;

            foreach (var keyColumn in log.KeyColumns)
            {
                var dbKeycolumn = new MyLogManager.EntityKeyColumns() { ColumnID = keyColumn.ColumnID, Value = keyColumn.Value };
                dbLog.EntityKeyColumns.Add(dbKeycolumn);
            }
            foreach (var dataColumn in log.DataEntryColumns)
            {
                var dbdataColumn = new DataEntryColumns() { ColumnID = dataColumn.ColumnID, Value = dataColumn.Value };
                dbLog.DataEntryColumns.Add(dbdataColumn);
            }
            foreach (var parameter in log.RecievedParameters)
            {
                var dbparameter = new RecievedParameters() { Name = parameter.Name, Value = parameter.Value };
                dbLog.RecievedParameters.Add(dbparameter);
            }

            logContext.MainLog.Add(dbLog);
            //logContext.SaveChanges();
            log.ID = dbLog.ID;
            return true;

        }

        public bool UpdateResponseLog(List<MainLogDTO> logs)
        {
            using (var logContext = new MyLogDBEntities())
            {
                foreach (var log in logs)
                {
                    var result = UpdateResponseLog(log, logContext);
                }
            }
            return true;
        }

        public bool UpdateResponseLog(MainLogDTO log, MyLogDBEntities logContext)
        {

            MainLog dbLog = logContext.MainLog.FirstOrDefault(x => x.ID == log.ID);
            if (dbLog != null)
            {
                dbLog.Succeed = log.Succeed;
                dbLog.Exception = log.Exception;
                if (dbLog.Exception == true)
                {
                    dbLog.ExceptionDetail = new MyLogManager.ExceptionDetail();
                    dbLog.ExceptionDetail.Description = log.ExceptionDesc;
                }
                dbLog.Duration = (int)(DateTime.Now.TimeOfDay - dbLog.Time).TotalSeconds;
                foreach (var parameter in log.ResultParameters)
                {
                    var dbparameter = new ResultParameters() { Name = parameter.Name, Value = parameter.Value };
                    dbLog.ResultParameters.Add(dbparameter);
                }
                logContext.SaveChanges();
                return true;
            }



            return false;
        }
    }

    public class MainLogDTO
    {
        public MainLogDTO()
        {
            DataEntryColumns = new List<MyLogManager.DataEntryColumnDTO>();
            KeyColumns = new List<EntityKeyColumnDTO>();
            RecievedParameters = new List<RecievedParametersDTO>();
            ResultParameters = new List<ResultParametersDTO>();
        }
        public int ID { get; set; }
        public Guid PackageGuid { set; get; }

        public Guid GUID { set; get; }
        public int UserID { get; set; }
        public string Date { get; set; }
        public int EnityID { get; set; }
        public string Time { get; set; }
        public bool Succeed { get; set; }
        public LogType Type { get; set; }
        public Nullable<bool> Exception { get; set; }
        public string ExceptionDesc { get; set; }
        public int Duration { get; set; }
        public string LocationInfo { get; set; }

        //public DataEntryLogDTO DataEntryLog { set; get; }
        //public DataDeleteLogDTO DataDeleteLog { set; get; }
        public List<RecievedParametersDTO> RecievedParameters { set; get; }
        public List<ResultParametersDTO> ResultParameters { set; get; }

        public List<DataEntryColumnDTO> DataEntryColumns { set; get; }

        public List<EntityKeyColumnDTO> KeyColumns { set; get; }

    }
    //public class DataEntryLogDTO
    //{
    //    public DataEntryLogDTO()
    //    {
    //        Columns = new List<MyLogManager.DataEntryColumnDTO>();
    //        KeyColumns = new List<MyLogManager.EntityKeyColumnDTO>();
    //    }
    //    public int ID { get; set; }
    //    public int EnityID { get; set; }
    //    public bool NewItem { get; set; }
    //    public List<DataEntryColumnDTO> Columns { set; get; }

    //    public List<EntityKeyColumnDTO> KeyColumns { set; get; }
    //}

    //public class DataDeleteLogDTO
    //{
    //    public DataDeleteLogDTO()
    //    {
    //        KeyColumns = new List<MyLogManager.EntityKeyColumnDTO>();
    //    }
    //    public int ID { get; set; }
    //    public int EnityID { get; set; }
    //    public List<EntityKeyColumnDTO> KeyColumns { set; get; }

    //}
    public class DataEntryColumnDTO
    {
        public int ID { get; set; }
        public int ColumnID { get; set; }
        public int FormulaID { get; set; }
        public string Value { get; set; }
    }
    public class EntityKeyColumnDTO
    {
        public int ColumnID { get; set; }
        public string Value { get; set; }
    }
    public class RecievedParametersDTO
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
    public class ResultParametersDTO
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
    public enum LogType
    {
        InsertData,
        UpdateData,
        DeleteData,
        Other
    }

    //public class InsertLogResult
    //{
    //    public InsertLogResult()
    //    {
          
    //    }
    //    public bool Result { set; get; }
    //    public string Message { set; get; }
    //    public LogGuidPair LogGuidPair { set; get; }
    //}
    //////public class InsertLogListResult
    //////{
    //////    public InsertLogListResult()
    //////    {
    //////        LogGuidPairs = new List<LogGuidPair>();
    //////    }
    //////    public bool Result { set; get; }
    //////    public string Message { set; get; }
    //////    public List<LogGuidPair> LogGuidPairs { set; get; }
    //////}
    //////public class LogGuidPair
    //////{
    //////    public LogGuidPair(int logId, Guid guid)
    //////    {
    //////        LogID = logId;
    //////        GUID = guid;
    //////    }
    //////    public int LogID { set; get; }
    //////    public Guid GUID { set; get; }
    //////}
}
