using DataAccess;
using ModelEntites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using ProxyLibrary;
using MyCodeFunctionLibrary;
using ProxyLibrary.Request;
using MyLogManager;
using MyModelManager;

namespace MyDataItemManager
{
    public class BizLetter
    {
        BizDataItem bizDataItem = new BizDataItem();
        BizFileRepository bizFileRepository = new BizFileRepository();
        public List<LetterDTO> GetLetters(DR_Requester requester, List<int> dataitemIDS)
        {
            List<LetterDTO> result = new List<LetterDTO>();
            using (var letterModel = new MyIdeaDataDBEntities())
            {
                using (var projectContext = new MyProjectEntities())
                {
                    var list = letterModel.Letter.Where(x => dataitemIDS.Contains(x.MyDataItemID));
                    foreach (var item in list)
                        result.Add(ToLetterDTO(requester,item, false, projectContext));
                    return result;
                }
            }
        }

        public LetterDeleteResult DeleteLetter(DR_Requester requester, int letterID)
        {
            
            LetterDeleteResult result = new LetterDeleteResult();
            string exception = "";
            using (var context = new MyIdeaDataDBEntities())
            {  
                //سکوریتی ثبت میتونه اینجا هم چک بشه
                //itemResult.ID = item.ID;
                var dbItem = context.Letter.First(x => x.ID == letterID);
                BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
                if (!bizTableDrivedEntity.DataIsAccessable(requester, dbItem.MyDataItem.TableDrivedEntityID, new List<SecurityAction>() { SecurityAction.LetterEdit }))
                {
                    throw new Exception("عدم دسترسی ثبت نامه");
                }
                try
                {
                    if (dbItem.Letter1.Any())
                    {
                        foreach (var item in dbItem.Letter1)
                        {
                            ResultDetail relatedItemException = new ResultDetail();
                            relatedItemException.Title = "دارای نامه مرتبط";
                            relatedItemException.Description = "نامه به شماره" + " " + item.LetterNumber + " " + "به تاریخ" + " " + item.LetterDate;
                            result.Details.Add(relatedItemException);
                        }
                        result.Message = "به علت وجود نامه های مرتبط عملیات حذف امکان پذیر نمی باشد";
                        return result;
                    }
                    context.Letter.Remove(dbItem);

                    context.SaveChanges();
                    result.Result = true;
                    result.Message = "عملیات حذف با موفقیت انجام شد";
                }
                catch (Exception ex)
                {
                    exception = ex.Message;
                    ResultDetail itemResult = new ResultDetail();
                    itemResult.Title = "خطا";
                    itemResult.Description = "حذف نامه با شناسه" + " " + letterID + " " + "با خطا همراه بود";
                    itemResult.TechnicalDescription = ex.Message;
                    result.Details.Add(itemResult);
                    result.Message = "عملیات حذف با خطا همراه بود";
                }
                finally
                {
                    var logResult = bizLogManager.AddLog(GetDeleteItemDataLog(dbItem, exception), requester);
                    if (!string.IsNullOrEmpty(logResult))
                    {
                        ResultDetail logException = new ResultDetail();
                        logException.Title = "خطای ثبت لاگ";
                        logException.Description = "ثبت لاگ برای حذف فایل با شناسه" + " " + dbItem.ID + " " + "با خطا همراه بود";
                        logException.TechnicalDescription = logResult;
                        result.Details.Add(logException);
                    }
                }
            }
            return result;
        }
        private DataLogDTO GetDeleteItemDataLog(Letter dbletter, string exceptionMessage)
        {
            var dataLog = new DataLogDTO();
            dataLog.LocationInfo = "";
            dataLog.Duration = 0;
            dataLog.DatItem = new DP_BaseData(dbletter.MyDataItemID);
            dataLog.MainType = DataLogType.LetterDelete;
            dataLog.RelatedItemID = dbletter.ID;
            if (!string.IsNullOrEmpty(exceptionMessage))
            {
                dataLog.MajorException = true;
                dataLog.MajorFunctionExceptionMessage = exceptionMessage;
                //dataLog.ArchiveItemLog = new ArchiveItemLogDTO();
                //dataLog.ArchiveItemLog.Exception = exceptionMessage;
            }
            return dataLog;
        }
        public List<LetterDTO> SearchLetters(DR_Requester dR_Requester, string generalFilter)
        {
            List<LetterDTO> result = new List<LetterDTO>();
            using (var letterModel = new MyIdeaDataDBEntities())
            {
                using (var projectContext = new MyProjectEntities())
                {
                    var listEntity = letterModel.Letter as IQueryable<Letter>;
                    if (generalFilter != "")
                        listEntity = listEntity.Where(x => x.ID.ToString() == generalFilter || x.Title.Contains(generalFilter) || x.LetterNumber.Contains(generalFilter) || x.ExternalSourceKey == generalFilter);
                    foreach (var item in listEntity)
                        result.Add(ToLetterDTO(dR_Requester,item, false, projectContext));
                }
            }
            return result;
        }
        public List<LetterTypeDTO> GetLetterTypes()
        {
            List<LetterTypeDTO> result = new List<LetterTypeDTO>();
            using (var letterModel = new MyProjectEntities())
            {
                var list = letterModel.LetterType;
                foreach (var item in list)
                {
                    result.Add(ToLetterTypeDTO(item));
                }

            }
            return result;
        }


        CodeFunctionHandler codeFunctionHandler = new CodeFunctionHandler();
        public LetterDTO GetLetter(DR_Requester requester, int letterID, bool withDetails)
        {
            using (var letterModel = new MyIdeaDataDBEntities())
            {
                using (var projectContext = new MyProjectEntities())
                {
                    var dbitem = letterModel.Letter.First(x => x.ID == letterID);
                    var letetr = ToLetterDTO(requester,dbitem, withDetails, projectContext);
                    //BizLetterTemplate bizLetterTemplate = new BizLetterTemplate();
                    if (withDetails)
                    {
                        var letterSetting = GetLetterSetting();
                        if (letterSetting != null)
                        {
                            if (letterSetting.BeforeLetterLoadCodeID != 0)
                            {
                                var result = codeFunctionHandler.GetCodeFunctionResult(requester, letterSetting.BeforeLetterLoadCodeID, letetr);
                                if (result.ExecutionException == null)
                                {

                                    //if (!string.IsNullOrEmpty(result.Message))
                                    //{
                                    //}

                                }
                                else
                                {

                                }
                            }
                        }
                    }
                    return letetr;
                }
            }

        }

        public bool ConvertLetterToExternal(int letterID, string externalCode)
        {
            using (var letterModel = new MyIdeaDataDBEntities())
            {
                var dbitem = letterModel.Letter.First(x => x.ID == letterID);
                dbitem.ExternalSourceKey = externalCode;
                //dbitem.FromExternalSource = true;
                letterModel.SaveChanges();

            }
            return true;
        }

        private LetterTypeDTO ToLetterTypeDTO(LetterType item)
        {
            LetterTypeDTO result = new LetterTypeDTO();
            result.ID = item.ID;
            result.Name = item.Name;
            return result;
        }



        private LetterDTO ToLetterDTO(DR_Requester requester, Letter item, bool withDetails, MyProjectEntities projectContext)
        {
            LetterDTO result = new LetterDTO();
            result.ID = item.ID;
            result.LetterNumber = item.LetterNumber;
            result.CreationDate = item.CreationDate;
            result.Desc = item.Desc;
            result.IsGeneratedOrSelected = item.IsGeneratedOrSelected;
            result.LetterDate = item.LetterDate;
            result.LetterNumber = item.LetterNumber;
            result.IsExternalOrInternal = item.FromExternalSource;
            result.ExternalCode = item.ExternalSourceKey;
            result.LetterTemplateID = item.LetterTemplateID == null ? 0 : item.LetterTemplateID.Value;
            result.LetterTypeID = item.LetterTypeID;
            result.vwLetterType = projectContext.LetterType.First(x => x.ID == result.LetterTypeID).Name;
            result.RelatedLetterID = item.RelatedLetterID == null ? 0 : item.RelatedLetterID.Value;
            //  result.DatItemID = item.MyDataItemID;
            //روش خوبی نیست که برای هر دیتا آیتم اطلاعات داده گرفته شود
            result.DataItem = bizDataItem.ToDataViewDTO(requester,item.MyDataItem, true);
            //////foreach (var dataitem in item.MyDataItem_Letter)
            //////    result.RelatedDataItems.Add(bizDataItem.ToDataItemDTO(dataitem.MyDataItem));
            result.Title = item.Title;
            result.UserID = item.UserID;
            result.AttechedFileID = item.FileRepositoryID == null ? 0 : item.FileRepositoryID.Value;
            result.HasAttechedFile = result.AttechedFileID != 0;
            if (withDetails)
            {
                if (item.FileRepository != null)
                    result.AttechedFile = bizFileRepository.ToFileRepository(item.FileRepository);
            }
            return result;
        }



        public LetterSettingDTO GetLetterSetting()
        {
            using (var projectContext = new MyProjectEntities())
            {
                var letterSetting = projectContext.LetterSetting.FirstOrDefault();
                if (letterSetting == null)
                    return null;
                else
                    return ToLetterSettingDTO(letterSetting);
            }
        }
        private LetterSettingDTO ToLetterSettingDTO(LetterSetting letterSetting)
        {
            LetterSettingDTO result = new LetterSettingDTO();

            result.AfterLetterSaveCodeID = letterSetting.AfterLetterSaveCodeID ?? 0;
            result.BeforeLetterLoadCodeID = letterSetting.BeforeLetterLoadCodeID ?? 0;
            result.BeforeLetterSaveCodeID = letterSetting.BeforeLetterSaveCodeID ?? 0;
            result.LetterExternalInfoCodeID = letterSetting.LetterExternalInfoCodeID ?? 0;
            result.LetterSendToExternalCodeID = letterSetting.LetterConvertToExternalCodeID ?? 0;

            return result;
        }
        BizLogManager bizLogManager = new BizLogManager();

        public LetterResult UpdateLetter(LetterDTO message, DR_Requester requester)
        {
            BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
            if (!bizTableDrivedEntity.DataIsAccessable(requester, message.DataItem.TargetEntityID, new List<SecurityAction>() { SecurityAction.LetterEdit }))
            {
                throw new Exception("عدم دسترسی ثبت نامه");
            }
            LetterResult result = new LetterResult();
            bool isNew = message.ID == 0;
            var letterSetting = GetLetterSetting();
            if (letterSetting != null)
            {
                if (letterSetting.BeforeLetterSaveCodeID != 0)
                {
                    var resultFunction = codeFunctionHandler.GetCodeFunctionResult(requester, letterSetting.BeforeLetterSaveCodeID, message);
                    if (resultFunction.ExecutionException != null)
                    {
                        var logResult = bizLogManager.AddLog(GetBeforeUpdateExceptionLog(message, resultFunction.ExecutionException, isNew ? DataLogType.LetterInsert : DataLogType.LetterUpdate), requester);
                        if (!string.IsNullOrEmpty(logResult))
                        {
                            ResultDetail logException = new ResultDetail();
                            logException.Title = "خطای ثبت لاگ";
                            logException.Description = "خطای لاگ برای اقدام قبل از ثبت نامه";
                            logException.TechnicalDescription = logResult;
                            result.Details.Add(logException);
                        }
                        result.Message = "اقدامات مرتبط قبل از ثبت نامه با خطا همراه بود";
                        return result;
                    }
                }
            }
            using (var letterModel = new MyIdeaDataDBEntities())
            {
                try
                {
                    var dbLetter = letterModel.Letter.FirstOrDefault(x => x.ID == message.ID);
                    if (dbLetter == null)
                    {
                        dbLetter = new Letter();
                        dbLetter.CreationDate = DateTime.Now;
                        dbLetter.UserID = requester.Identity;
                        //var dataItemID = bizDataItem.GetOrCreateDataItem(message.DataItem);
                        if (message.DataItem.DataItemID == 0)
                        {
                            dbLetter.MyDataItemID = bizDataItem.GetOrCreateDataItem(message.DataItem);
                            message.DataItem.DataItemID = dbLetter.MyDataItemID;
                        }
                        else
                        {
                            dbLetter.MyDataItemID = message.DataItem.DataItemID;
                        }
                    }
                    dbLetter.ID = message.ID;
                    dbLetter.Desc = message.Desc;
                    dbLetter.LetterNumber = message.LetterNumber;
                    dbLetter.IsGeneratedOrSelected = message.IsGeneratedOrSelected;
                    dbLetter.FromExternalSource = message.IsExternalOrInternal;
                    dbLetter.ExternalSourceKey = message.ExternalCode;
                    dbLetter.LetterNumber = message.LetterNumber;
                    dbLetter.LetterDate = message.LetterDate;
                    dbLetter.LetterTemplateID = message.LetterTemplateID == 0 ? (int?)null : message.LetterTemplateID;
                    dbLetter.LetterTypeID = message.LetterTypeID;
                    dbLetter.RelatedLetterID = message.RelatedLetterID == 0 ? (int?)null : message.RelatedLetterID;
                    dbLetter.Title = message.Title;

                    if (dbLetter.FileRepository == null)
                        if (message.AttechedFile != null && message.AttechedFile.Content != null)
                            dbLetter.FileRepository = bizFileRepository.ToFileRepository(letterModel, message.AttechedFile);

                    if (dbLetter.ID == 0)
                        letterModel.Letter.Add(dbLetter);
                    letterModel.SaveChanges();
                    message.ID = dbLetter.ID;
                    result.SavedID = dbLetter.ID;
                    result.Result = true;
                }
                catch (Exception ex)
                {
                    result.Result = false;
                    result.Message = ex.Message;
                }
                finally
                {
                    var logResult = bizLogManager.AddLog(GetUpdateLetterDataLog(message, result.Message, isNew ? DataLogType.LetterInsert : DataLogType.LetterUpdate), requester);
                    if (!string.IsNullOrEmpty(logResult))
                    {
                        ResultDetail logException = new ResultDetail();
                        logException.Title = "خطای ثبت لاگ";
                        logException.Description = "خطای لاگ برای ثبت نامه";
                        logException.TechnicalDescription = logResult;
                        result.Details.Add(logException);
                    }

                }
            }
            if (result.Result == false)
            {
                result.Message = "عملیات ثبت با خطا همراه بود";
                return result;
            }
            else
            {
                bool afterSaveException = false;
                if (letterSetting != null)
                {
                    if (letterSetting.AfterLetterSaveCodeID != 0)
                    {
                        var resultFunction = codeFunctionHandler.GetCodeFunctionResult(requester, letterSetting.AfterLetterSaveCodeID, message);
                        if (resultFunction.ExecutionException != null)
                        {
                            afterSaveException = true;
                            var logResult = bizLogManager.AddLog(GetAfterUpdateExceptionLog(message, resultFunction.ExecutionException, isNew ? DataLogType.LetterInsert : DataLogType.LetterUpdate), requester);
                            if (!string.IsNullOrEmpty(logResult))
                            {
                                ResultDetail logException = new ResultDetail();
                                logException.Title = "خطای ثبت لاگ";
                                logException.Description = "خطای لاگ برای اقدام بعد از ثبت نامه";
                                logException.TechnicalDescription = logResult;
                                result.Details.Add(logException);
                            }
                            return result;
                        }

                    }
                }
                if (afterSaveException)
                    result.Message = "عملیات ثبت با موفقیت انجام شد اما اقدامات بعد از ثبت با خطا همراه بود";
                else
                    result.Message = "عملیات ثبت با موفقیت انجام شد";

                return result;
            }
        }

        private DataLogDTO GetUpdateLetterDataLog(LetterDTO message, string exceptionMessage, DataLogType logType)
        {
            var dataLog = new DataLogDTO();
            dataLog.LocationInfo = "";
            dataLog.Duration = 0;
            dataLog.DatItem = message.DataItem;
            dataLog.MainType = logType;
            dataLog.RelatedItemID = message.ID;
            if (!string.IsNullOrEmpty(exceptionMessage))
            {
                dataLog.MajorException = true;
                dataLog.MajorFunctionExceptionMessage = exceptionMessage;
                //dataLog.ArchiveItemLog = new ArchiveItemLogDTO();
                //dataLog.ArchiveItemLog.Exception = exceptionMessage;
            }
            return dataLog;
        }
        private DataLogDTO GetBeforeUpdateExceptionLog(LetterDTO message, Exception executionException, DataLogType logType)
        {
            var dataLog = new DataLogDTO();
            dataLog.LocationInfo = "";
            dataLog.Duration = 0;
            dataLog.MajorException = true;
            dataLog.MainType = logType;
            dataLog.DatItem = message.DataItem;
            dataLog.MajorFunctionExceptionMessage = executionException.Message;
            return dataLog;
        }
        private DataLogDTO GetAfterUpdateExceptionLog(LetterDTO message, Exception executionException, DataLogType logType)
        {
            var dataLog = new DataLogDTO();
            dataLog.LocationInfo = "";
            dataLog.Duration = 0;
            dataLog.MinorException = true;
            dataLog.MainType = logType;
            dataLog.DatItem = message.DataItem;
            dataLog.LetterItemLog.AfterUpdateException = executionException.Message;
            return dataLog;
        }
    }
}
