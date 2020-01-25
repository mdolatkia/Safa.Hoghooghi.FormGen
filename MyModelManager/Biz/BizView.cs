//using DataAccess;
//using ModelEntites;
//using MyGeneralLibrary;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace MyModelManager
//{
//    public class BizView
//    {
//        public List<ViewDTO> GetViews(int databaseID)
//        {
//            List<ViewDTO> result = new List<ViewDTO>();
//            using (var projectContext = new DataAccess.MyProjectEntities())
//            {
//                //projectContext.Configuration.LazyLoadingEnabled = false;
//                //string catalogName = GeneralHelper.GetCatalogName(serverName, dbName);
//                var listView = projectContext.View.Where(x => x.DBSchema.DatabaseInformationID == databaseID);
//                foreach (var item in listView)
//                    result.Add(ToViewDTO(item));

//            }
//            return result;
//        }
//        public ViewDTO GetView(int ViewsID)
//        {
//            List<ViewDTO> result = new List<ViewDTO>();
//            using (var projectContext = new DataAccess.MyProjectEntities())
//            {
//                var Views = projectContext.View.First(x => x.ID == ViewsID);
//                return ToViewDTO(Views);
//            }
//        }
//        private ViewDTO ToViewDTO(View item)
//        {
//            ViewDTO result = new ViewDTO();
//            result.Name = item.Name;
//            result.DatabaseID = item.DBSchema.DatabaseInformationID;
//            result.RelatedSchema = item.DBSchema.Name;
//            result.ID = item.ID;
//            result.Enable = item.Enable;
//            foreach (var column in item.ViewColumns)
//            {
//                var mColumn = new ViewColumnsDTO();
//                mColumn.ColumnID = column.ColumnID;
//                //   mColumn.ColumnName = column.Column.Name;
//                mColumn.ID = column.ID;
//                mColumn.Name = column.Column.Name;
//                mColumn.TableName = column.Column.Table.Name;
//                result.Columns.Add(mColumn);
//            }
//            return result;
//        }

//        public bool OrginalViewExists(string name, int databaseID)
//        {
//            using (var projectContext = new DataAccess.MyProjectEntities())
//            {
//                return projectContext.View.Any(x => x.Name == name && x.DBSchema.DatabaseInformationID == databaseID);

//            }
//        }
//        public ViewDTO GetOrginalView(string name, int databaseID)
//        {
//            using (var projectContext = new DataAccess.MyProjectEntities())
//            {
//                return ToViewDTO(projectContext.View.First(x => x.Name == name && x.DBSchema.DatabaseInformationID == databaseID));

//            }
//        }

//        public List<ViewDTO> GetEnableOrginalViews(int databaseID)
//        {
//            List<ViewDTO> result = new List<ViewDTO>();
//            using (var projectContext = new DataAccess.MyProjectEntities())
//            {
//                var list = projectContext.View.Where(x => x.Enable == true && x.DBSchema.DatabaseInformationID == databaseID);
//                foreach (var item in list)
//                    result.Add(ToViewDTO(item));
//            }
//            return result;
//        }

//        public void UpdateModel(int databaseID, List<ViewDTO> listNew, List<ViewDTO> listEdit, List<ViewDTO> listDeleted)
//        {
//            using (var projectContext = new DataAccess.MyProjectEntities())
//            {
//                foreach (var newEntity in listNew)
//                {
//                    UpdateViewInModel(projectContext, databaseID, newEntity);
//                }
//                foreach (var editEntity in listEdit)
//                {
//                    UpdateViewInModel(projectContext, databaseID, editEntity);
//                }
//                foreach (var deleteView in listDeleted)
//                {
//                    var dbEntity = projectContext.View.FirstOrDefault(x => x.ID == deleteView.ID);
//                    dbEntity.Enable = false;
//                }
//                projectContext.SaveChanges();
//            }
//        }

//        private void UpdateViewInModel(MyProjectEntities projectContext, int databaseID, ViewDTO view)
//        {
//            var schema = view.RelatedSchema;
//            var dbSchema = projectContext.DBSchema.FirstOrDefault(x => x.DatabaseInformationID == databaseID && x.Name == schema);
//            if (dbSchema == null)
//            {
//                dbSchema = new DBSchema() { DatabaseInformationID = databaseID, Name = schema };
//                dbSchema.SecurityObject = new SecurityObject();
//                dbSchema.SecurityObject.Type = (int)DatabaseObjectCategory.Schema;
//                projectContext.DBSchema.Add(dbSchema);
//            }

//            var dbView = projectContext.View.FirstOrDefault(x => x.Name == view.Name && x.DBSchema.DatabaseInformationID == databaseID);
//            if (dbView == null)
//            {
//                dbView = new View();
//                projectContext.View.Add(dbView);
//                dbView.Name = view.Name;
//                dbView.Enable = true;
//            }
//            dbView.DBSchema = dbSchema;
//            foreach (var dbColumn in dbView.ViewColumns.ToList())
//            {
//                if (!view.Columns.Any(x => x.TableName == dbColumn.Column.Table.Name && x.Name == dbColumn.Column.Name));
//                dbColumn.Enabled = false;
//            }
//            foreach (var column in view.Columns)
//            {
//                var dbTable = projectContext.Table.FirstOrDefault(x => x.Name == column.TableName && x.DBSchema.DatabaseInformationID == databaseID);
//                if (dbTable == null)
//                    throw new Exception("جدول" + " " + dbTable.Name + " " + "موجود نمی باشد");
//                var dbColumn = projectContext.Column.FirstOrDefault(x => x.Table.Name == dbTable.Name && x.Name == column.Name && x.Table.DBSchema.DatabaseInformationID == databaseID);
//                if (dbColumn == null)
//                    throw new Exception("ستون" + " " + dbColumn.Name + " " + "در جدول" + " " + dbTable.Name + " " + "موجود نمی باشد");
//                var dbViewColumn = dbView.ViewColumns.Where(x => x.ColumnID == dbColumn.ID).FirstOrDefault();
//                if (dbViewColumn == null)
//                {
//                    dbView.ViewColumns.Add(new ViewColumns() { ColumnID = dbColumn.ID });
//                }
//            }
           
//        }
//        //public void UpdateViews(List<ViewDTO> Views)
//        //{
//        //    using (var projectContext = new DataAccess.MyProjectEntities())
//        //    {
//        //        foreach (var item in Views)
//        //        {
//        //            var dbViews = projectContext.View.First(x => x.ID == Views.ID);
//        //            dbViews.Name = item.Name;

//        //            //  dbViews.BatchDataEntry = Views.BatchDataEntry;
//        //            //  dbViews.IsAssociative = Views.IsAssociative;

//        //            //   dbViews.IsDataReference = Views.IsDataReference;
//        //            //   dbViews.IsStructurReferencee = Views.IsStructurReferencee;
//        //        }
//        //        projectContext.SaveChanges();
//        //    }
//        //}
//    }

//}
