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
    public class BizArchiveManager
    {

        BizDataItem bizDataItem = new BizDataItem();
        BizFileRepository bizFileRepository = new BizFileRepository();

        public List<ArchiveItemDTO> GetArchiveItemsAllFolders(DR_Requester requester, List<int> dataitemIDS)
        {
            List<ArchiveItemDTO> result = new List<ArchiveItemDTO>();
            using (var context = new MyIdeaDataDBEntities())
            {
                var archivedItems = context.ArchiveItem.Where(x => dataitemIDS.Contains(x.MyDataItemID));
                foreach (var item in archivedItems)
                    result.Add(ToArchiveItemDTO(requester, item, false));
                return result;
            }
        }

        public List<ArchiveItemDTO> GetArchiveItems(DR_Requester requester, List<int> dataitemIDS, int? folderID)
        {
            List<ArchiveItemDTO> result = new List<ArchiveItemDTO>();
            using (var context = new MyIdeaDataDBEntities())
            {
                var archivedItems = context.ArchiveItem.Where(x => dataitemIDS.Contains(x.MyDataItemID)
                    && x.FolderID == folderID);
                foreach (var item in archivedItems)
                    result.Add(ToArchiveItemDTO(requester, item, false));
                return result;
            }
        }
        //private ArchiveItemDataItemDTO ToArchiveItemDataItemDTO(MyDataItem_ArchiveItem item, DataItemDTO messageDataItem)
        //{
        //    ArchiveItemDataItemDTO result = new ArchiveItemDataItemDTO();
        //    result.ArchiveItemID = item.ArchiveItemID;
        //    result.DatItemID = item.MyDataItemID;
        //    result.ID = item.ID;
        //    result.FolderID = item.FolderID;
        //    result.DatItem = messageDataItem;
        //    result.ArchiveItem = ToArchiveItemDTO(item.ArchiveItem, false);
        //    return result;
        //}
        //BizArchive bizArchive = new BizArchive();
        public List<int> GetArchiveItemsTags(List<int> dataitemIDS)
        {
            List<int> result = new List<int>();
            using (var context = new MyIdeaDataDBEntities())
            {

                var archivedItems = context.ArchiveItem.Where(x => dataitemIDS.Contains(x.MyDataItemID));
                List<int> tagIds = new List<int>();
                foreach (var item in archivedItems)
                {
                    tagIds.AddRange(item.ArchiveItem_Tag.Select(x => x.TagID));
                }
                foreach (var id in tagIds.Distinct())
                {
                    result.Add(id);
                }
                return result;
            }
        }

        public ArchiveItemDTO GetArchiveItem(DR_Requester requester, int id)
        {
            using (var context = new MyIdeaDataDBEntities())
            {
                var dbitem = context.ArchiveItem.First(x => x.ID == id);
                var letetr = ToArchiveItemDTO(requester, dbitem, true);
                return letetr;
            }
        }

        public List<Tuple<string, List<string>>> GetArchiveExtentions()
        {
            var extentions = new List<Tuple<string, List<string>>>();
            extentions.Add(new Tuple<string, List<string>>("All", new List<string>() { "*.jpg", "*.bmp", "*.png", "*.tif", "*.tiff", "*.doc", "*.docx", "*.pdf" }));
            extentions.Add(new Tuple<string, List<string>>("Images", new List<string>() { "*.jpg", "*.bmp", "*.png", "*.tif", "*.tiff" }));
            extentions.Add(new Tuple<string, List<string>>("MS Word", new List<string>() { "*.doc", "*.docx" }));
            extentions.Add(new Tuple<string, List<string>>("Pdf", new List<string>() { "*.pdf" }));
            return extentions;
        }

        public bool UpdateMultipleArchiveItemInfo(DR_Requester requester, List<int> IDs, bool changeFolder, int? folderID, bool changeTagIDs, List<int> selectedTagIds)
        {
            using (var context = new MyIdeaDataDBEntities())
            {
                //سکوریتی ثبت میتونه اینجا هم چک بشه
                //BizArchive bizArchive = new BizArchive();
                foreach (var iD in IDs)
                {
                    var item = context.ArchiveItem.First(x => x.ID == iD);
                    BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
                    if (!bizTableDrivedEntity.DataIsAccessable(requester, item.MyDataItem.TableDrivedEntityID, new List<SecurityAction>() { SecurityAction.ArchiveEdit }))
                    {
                        throw new Exception("عدم دسترسی ثبت آرشیو");
                    }
                    if (changeFolder)
                        item.FolderID = folderID;
                    if (changeTagIDs)
                    {
                        //var entityArchiveTags = bizArchive.GetArchiveTags(item.MyDataItem.TableDrivedEntityID, true);
                        //var entityTagIds = entityArchiveTags.Select(x => x.ID).ToList();
                        while (item.ArchiveItem_Tag.Any())
                            context.ArchiveItem_Tag.Remove(item.ArchiveItem_Tag.First());
                        foreach (var tagid in selectedTagIds)
                        {
                            item.ArchiveItem_Tag.Add(new ArchiveItem_Tag() { TagID = tagid });
                        }
                    }
                }
                context.SaveChanges();
                return true;
            }
        }

        public bool UpdateArchiveItemInfo(DR_Requester requester, int iD, string name, int? folderID, List<int> tagIDs)
        {
            using (var context = new MyIdeaDataDBEntities())
            {
                //سکوریتی ثبت میتونه اینجا هم چک بشه
                var item = context.ArchiveItem.First(x => x.ID == iD);
                BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
                if (!bizTableDrivedEntity.DataIsAccessable(requester, item.MyDataItem.TableDrivedEntityID, new List<SecurityAction>() { SecurityAction.ArchiveEdit }))
                {
                    throw new Exception("عدم دسترسی ثبت آرشیو");
                }

                item.Name = name;
                item.FolderID = folderID;

                //BizArchive bizArchive = new BizArchive();
                //var entityArchiveTags = bizArchive.GetArchiveTags(item.MyDataItem.TableDrivedEntityID, true);
                //var entityTagIds = entityArchiveTags.Select(x => x.ID).ToList();
                while (item.ArchiveItem_Tag.Any())
                    context.ArchiveItem_Tag.Remove(item.ArchiveItem_Tag.First());
                foreach (var tagid in tagIDs)
                {
                    item.ArchiveItem_Tag.Add(new ArchiveItem_Tag() { TagID = tagid });
                }
                context.SaveChanges();
                return true;
            }

        }

        public ArchiveDeleteResult DeleteArchiveItemDataItems(List<int> items, DR_Requester requester)
        {
            ArchiveDeleteResult result = new ArchiveDeleteResult();

            using (var context = new MyIdeaDataDBEntities())
            {
                bool saved = false;
                bool exceptioned = false;
                foreach (var item in items)
                {
                    string exception = "";
                    //itemResult.ID = item.ID;
                    var dbItem = context.ArchiveItem.First(x => x.ID == item);
                 
                    try
                    {
                        BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
                        if (!bizTableDrivedEntity.DataIsAccessable(requester, dbItem.MyDataItem.TableDrivedEntityID, new List<SecurityAction>() { SecurityAction.ArchiveEdit }))
                        {
                            throw new Exception("عدم دسترسی ثبت نامه");
                        }
                        while (dbItem.ArchiveItem_Tag.Any())
                            context.ArchiveItem_Tag.Remove(dbItem.ArchiveItem_Tag.First());
                        context.ArchiveItem.Remove(dbItem);
                        //سکوریتی حذف میتونه اینجا هم چک بشه
                        context.SaveChanges();
                        saved = true;
                        //itemResult.Result = true;
                    }
                    catch (Exception ex)
                    {
                        exception = ex.Message;
                        ResultDetail itemResult = new ResultDetail();
                        exceptioned = true;
                        itemResult.Title = "خطا";
                        itemResult.Description = "حذف فایل با شناسه" + " " + dbItem.ID + " " + "با خطا همراه بود";
                        itemResult.TechnicalDescription = ex.Message;
                        result.Details.Add(itemResult);
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
                if (saved && exceptioned)
                    result.Message = "حذف برخی از فایلها با خطا همراه بود";
                else if (saved)
                    result.Message = "عملیات حذف با موفقیت انجام شد";
                else if (exceptioned)
                    result.Message = "عملیات حذف با خطا همراه بود";
                else
                    result.Message = "عملیات حذف انجام نشد";

            }
            return result;
        }

        private DataLogDTO GetDeleteItemDataLog(ArchiveItem dbArchiveItem, string exceptionMessage)
        {
            var dataLog = new DataLogDTO();
            dataLog.LocationInfo = "";
            dataLog.Duration = 0;
            dataLog.DatItem = new DP_BaseData(dbArchiveItem.MyDataItemID);
            dataLog.MainType = DataLogType.ArchiveDelete;
            dataLog.RelatedItemID = dbArchiveItem.ID;
            if (!string.IsNullOrEmpty(exceptionMessage))
            {
                dataLog.MajorException = true;
                dataLog.MajorFunctionExceptionMessage = exceptionMessage;
                //dataLog.ArchiveItemLog = new ArchiveItemLogDTO();
                //dataLog.ArchiveItemLog.Exception = exceptionMessage;
            }
            return dataLog;
        }

        public List<int> GetArchiveItemTagIds(int iD)
        {
            using (var context = new MyIdeaDataDBEntities())
            {
                var item = context.ArchiveItem.First(x => x.ID == iD);
                return item.ArchiveItem_Tag.Select(x => x.TagID).ToList();
            }
        }

        //public bool UpdateArhiveItemTags(int iD, List<int> selectedTags)
        //{
        //    using (var context = new MyIdeaDataDBEntities())
        //    {
        //        var item = context.ArchiveItem.First(x => x.ID == iD);
        //        while (item.ArchiveItem_Tag.Any())
        //            context.ArchiveItem_Tag.Remove(item.ArchiveItem_Tag.First());
        //        foreach (var tagid in selectedTags)
        //        {
        //            item.ArchiveItem_Tag.Add(new ArchiveItem_Tag() { TagID = tagid });
        //        }
        //        context.SaveChanges();
        //        return true;
        //    }
        //}

        public bool UpdateArchiveItemFileBinary(DR_Requester requester,int iD, byte[] file)
        {
           
            using (var context = new MyIdeaDataDBEntities())
            {   //سکوریتی ثبت میتونه اینجا هم چک بشه
                var item = context.ArchiveItem.First(x => x.ID == iD);
                BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
                if (!bizTableDrivedEntity.DataIsAccessable(requester, item.MyDataItem.TableDrivedEntityID, new List<SecurityAction>() { SecurityAction.ArchiveEdit }))
                {
                    throw new Exception("عدم دسترسی ثبت آرشیو");
                }
                item.FileRepository1.Content = file;
                SetPossibleThumbnail(context, item, (Enum_ArchiveItemMainType)item.MainType, (Enum_ArchiveItemFileType)item.FileType, item.FileRepository1.Content, item.FileRepository1.FileName);
                context.SaveChanges();
                return true;
            }
        }

        public FileRepositoryDTO GetAttachedFile(int iD)
        {
            using (var context = new MyIdeaDataDBEntities())
            {
                var item = context.ArchiveItem.First(x => x.ID == iD);
                return bizFileRepository.ToFileRepository(item.FileRepository1);
            }
        }

        public List<Tuple<int?, int>> GetArchivedItemsFolderIDs(List<int> dataItemIds, List<int> tagIDs)
        {
            List<Tuple<int?, int>> result = new List<Tuple<int?, int>>();
            using (var context = new MyIdeaDataDBEntities())
            {
                var list = context.ArchiveItem.Where(x => dataItemIds.Contains(x.MyDataItemID));
                if (tagIDs.Any())
                    list = list.Where(x => x.ArchiveItem_Tag.Any(y => tagIDs.Contains(y.TagID)));
                var group = list.GroupBy(x => x.FolderID);
                foreach (var item in group)
                    result.Add(new Tuple<int?, int>(item.Key, item.Count()));
            }
            return result;
        }

        private ArchiveItemDTO ToArchiveItemDTO(DR_Requester requester, ArchiveItem item, bool withDetails)
        {
            ArchiveItemDTO result = new ArchiveItemDTO();
            result.ID = item.ID;
            result.Name = item.Name;
            result.UserID = item.UserID;
            result.FolderID = item.FolderID;
            result.FileType = (Enum_ArchiveItemFileType)item.FileType;
            result.MainType = (Enum_ArchiveItemMainType)item.MainType;
            result.CreationDate = item.CreationDate;
            foreach (var tag in item.ArchiveItem_Tag)
            {
                result.TagIDs.Add(tag.TagID);
            }

            //result.vwDatItemID = item.MyDataItemID;
            //روش خوبی نیست که برای هر دیتا آیتم اطلاعات داده گرفته شود
            result.DatItem = bizDataItem.ToDataViewDTO(requester, item.MyDataItem, true);

            result.AttechedFileID = item.FileRepositoryID;
            if (item.ThumbnailFileRepositoryID != null)
                result.ThumbnailFile = bizFileRepository.ToFileRepository(item.FileRepository);
            if (withDetails)
            {
                if (item.FileRepository1 != null)
                    result.AttechedFile = bizFileRepository.ToFileRepository(item.FileRepository1);
            }
            return result;
        }
        BizLogManager bizLogManager = new BizLogManager();

        public ArchiveResult CreateArchiveItems(ArchiveItemDTO message, DR_Requester requester)
        {
            ArchiveResult result = new ArchiveResult();
            try
            {

                // افزوده شدن نامه ها به آرشیو بعدا بررسی شود

                if (message.ID != 0)
                {
                    throw new Exception();
                }
                BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
                if (!bizTableDrivedEntity.DataIsAccessable(requester, message.DatItem.TargetEntityID, new List<SecurityAction>() { SecurityAction.ArchiveEdit }))
                {
                    throw new Exception("عدم دسترسی ثبت آرشیو");
                }
                using (var context = new MyIdeaDataDBEntities())
                {
                    var dbArchiveItem = new ArchiveItem();
                    if (message.DatItem.DataItemID == 0)
                    {
                        dbArchiveItem.MyDataItemID = bizDataItem.GetOrCreateDataItem(message.DatItem);
                        message.DatItem.DataItemID = dbArchiveItem.MyDataItemID;
                    }
                    else
                    {
                        dbArchiveItem.MyDataItemID = message.DatItem.DataItemID;

                    }
                    //var dataItem = bizDataItem.GetOrCreateDataItem(message.DatItem);
                    //var archiveItemDataItem = new MyDataItem_ArchiveItem();

                    dbArchiveItem.FolderID = message.FolderID;
                    dbArchiveItem.CreationDate = DateTime.Now;
                    dbArchiveItem.UserID = requester.Identity;
                    dbArchiveItem.Name = message.Name;
                    var type = GetArchiveItemType(message);
                    message.FileType = type.Item2;
                    message.MainType = type.Item1;
                    dbArchiveItem.FileType = (short)message.FileType;
                    dbArchiveItem.MainType = (short)message.MainType;
                    if (message.AttechedFile != null && message.AttechedFile.Content != null)
                    {
                        dbArchiveItem.FileSize = message.AttechedFile.Content.Count();
                        dbArchiveItem.FileRepository1 = bizFileRepository.ToFileRepository(context, message.AttechedFile);
                    }
                    SetPossibleThumbnail(context, dbArchiveItem, message.MainType, message.FileType, message.AttechedFile.Content, message.AttechedFile.FileName);
                    //if (!hasThumbnail)
                    //{
                    //    dbArchiveItem.ThumbnailFileRepositoryID = null;
                    //    //    dbArchiveItem.FileRepository1 = null;
                    //}
                    foreach (var tagid in message.TagIDs)
                    {
                        dbArchiveItem.ArchiveItem_Tag.Add(new ArchiveItem_Tag() { TagID = tagid });
                    }
                    context.ArchiveItem.Add(dbArchiveItem);
                    context.SaveChanges();
                    message.ID = dbArchiveItem.ID;

                    result.Result = true;


                    //انتیتی فرمورک قاطی داره رابطه فایل را برای تامبنیل هم میزاره

                    //context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
            }
            finally
            {
                var logResult = bizLogManager.AddLog(GetCreateItemDataLog(message, result.Message), requester);
                if (!string.IsNullOrEmpty(logResult))
                    result.Message += (!string.IsNullOrEmpty(result.Message) ? Environment.NewLine : "") + "خطا در ثبت لاگ" + " , " + logResult;
            }
            return result;
        }

        private DataLogDTO GetCreateItemDataLog(ArchiveItemDTO archiveItem, string exceptionMessage)
        {
            var dataLog = new DataLogDTO();
            dataLog.LocationInfo = "";
            dataLog.Duration = 0;
            dataLog.DatItem = archiveItem.DatItem;
            dataLog.MainType = DataLogType.ArchiveInsert;
            dataLog.RelatedItemID = archiveItem.ID;
            if (!string.IsNullOrEmpty(exceptionMessage))
            {
                dataLog.MajorException = true;
                dataLog.MajorFunctionExceptionMessage = exceptionMessage;
                //dataLog.ArchiveItemLog = new ArchiveItemLogDTO();
                //dataLog.ArchiveItemLog.Exception = exceptionMessage;
            }
            return dataLog;
        }

        private bool SetPossibleThumbnail(MyIdeaDataDBEntities context, ArchiveItem dbArchiveItem, Enum_ArchiveItemMainType mainType, Enum_ArchiveItemFileType fileType, byte[] fileContent, string fileName)
        {
            byte[] thumbNail = null;
            if (mainType == Enum_ArchiveItemMainType.Image)
            {
                thumbNail = GetJpegThumbnail(mainType, fileType, fileContent);
            }
            else if (mainType == Enum_ArchiveItemMainType.Pdf)
            {
                var pdfToImage = PdfToImage(mainType, fileType, fileContent);
                if (pdfToImage != null)
                    thumbNail = GetJpegThumbnail(Enum_ArchiveItemMainType.Image, Enum_ArchiveItemFileType.BMP, pdfToImage.ToArray());
            }


            if (thumbNail != null)
            {

                FileRepositoryDTO attechedFile = new FileRepositoryDTO();
                attechedFile.Content = thumbNail;
                if (dbArchiveItem.ThumbnailFileRepositoryID != null)
                    attechedFile.ID = dbArchiveItem.ThumbnailFileRepositoryID.Value;
                attechedFile.FileName = fileName;
                attechedFile.FileExtension = "jpg";
                dbArchiveItem.FileRepository = bizFileRepository.ToFileRepository(context, attechedFile);
                return true;
            }

            return false;
        }

        private byte[] PdfToImage(Enum_ArchiveItemMainType mainType, Enum_ArchiveItemFileType fileType, byte[] fileContent)
        {
            System.IO.MemoryStream myMemStream = new System.IO.MemoryStream(fileContent);
            var pdfFile = O2S.Components.PDFRender4NET.PDFFile.Open(myMemStream);
            var bitmap = pdfFile.GetPageImage(0, 100);
            System.IO.MemoryStream myResult = new System.IO.MemoryStream();
            bitmap.Save(myResult, System.Drawing.Imaging.ImageFormat.Jpeg);
            return myResult.ToArray();
        }

        private byte[] GetJpegThumbnail(Enum_ArchiveItemMainType mainType, Enum_ArchiveItemFileType fileType, byte[] fileContent)
        {
            if (mainType == Enum_ArchiveItemMainType.Image)
            {
                System.IO.MemoryStream myMemStream = new System.IO.MemoryStream(fileContent);
                System.Drawing.Image fullsizeImage = System.Drawing.Image.FromStream(myMemStream);

                float nPercent = 0;
                float nPercentW = 0;
                float nPercentH = 0;
                var sourceWidth = fullsizeImage.Width;
                var sourceHeight = fullsizeImage.Height;
                var width = 120;
                var height = 160;
                nPercentW = ((float)width / (float)sourceWidth);
                nPercentH = ((float)height / (float)sourceHeight);
                if (nPercentH < nPercentW)
                {
                    nPercent = nPercentH;
                }
                else
                {
                    nPercent = nPercentW;
                }

                int destWidth = (int)(sourceWidth * nPercent);
                int destHeight = (int)(sourceHeight * nPercent);

                System.Drawing.Image newImage = fullsizeImage.GetThumbnailImage(destWidth, destHeight, null, IntPtr.Zero);
                System.IO.MemoryStream myResult = new System.IO.MemoryStream();
                //System.Drawing.Imaging.ImageFormat format;
                //if (type == Enum_ArchiveItemType.JPG)
                //    format = System.Drawing.Imaging.ImageFormat.Jpeg;
                //همه jpg بشن
                newImage.Save(myResult, System.Drawing.Imaging.ImageFormat.Jpeg);  //Or whatever format you want.
                return myResult.ToArray();
            }

            return null;
        }

        private Tuple<Enum_ArchiveItemMainType, Enum_ArchiveItemFileType> GetArchiveItemType(ArchiveItemDTO message)
        {
            if (message.AttechedFile != null && message.AttechedFile.FileExtension != null)
            {
                var extention = message.AttechedFile.FileExtension.ToLower();
                if (extention == "jpg")
                    return new Tuple<Enum_ArchiveItemMainType, Enum_ArchiveItemFileType>(Enum_ArchiveItemMainType.Image, Enum_ArchiveItemFileType.JPEG);
                else if (extention == "bmp")
                    return new Tuple<Enum_ArchiveItemMainType, Enum_ArchiveItemFileType>(Enum_ArchiveItemMainType.Image, Enum_ArchiveItemFileType.BMP);
                else if (extention == "pdf")
                    return new Tuple<Enum_ArchiveItemMainType, Enum_ArchiveItemFileType>(Enum_ArchiveItemMainType.Pdf, Enum_ArchiveItemFileType.PDF);
                else if (extention == "doc")
                    return new Tuple<Enum_ArchiveItemMainType, Enum_ArchiveItemFileType>(Enum_ArchiveItemMainType.MsWord, Enum_ArchiveItemFileType.DOC);
                else if (extention == "docx")
                    return new Tuple<Enum_ArchiveItemMainType, Enum_ArchiveItemFileType>(Enum_ArchiveItemMainType.MsWord, Enum_ArchiveItemFileType.DOCX);

            }
            return new Tuple<Enum_ArchiveItemMainType, Enum_ArchiveItemFileType>(Enum_ArchiveItemMainType.UnKnown, Enum_ArchiveItemFileType.UnKnown);
        }
    }
}
