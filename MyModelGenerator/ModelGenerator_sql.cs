//using CommonBusiness;

using DataAccess;
using ModelEntites;
using MyGeneralLibrary;
using MyModelManager;
//using CommonDefinitions;
//using CommonDefinitions.CommonDTOs;
//using DataMaster.EntityDefinition;
//using DataMaster.EntityRelations;
//using DataMasterBusiness;
//using DataMasterBusiness.EntityDefinition;
//using DataMasterBusiness.EntityRelations;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyModelGenerator
{
    public class ModelGenerator_sql : I_DatabaseImportHelper
    {

        //private string ConnectionString { set; get; }
        //private string ServerName { set; get; }
        //private string DBName { set; get; }
        //private int databaseID { set; get; }
        DatabaseDTO Database { set; get; }
        public ModelGenerator_sql(DatabaseDTO database)
        {
            Database = database;
        }
        //public void SetDatabaseID(int databaseID)
        //{
        //    if (databaseID == 0)
        //        databaseID = 1;
        //    //ServerName = serverName;
        //    //DBName = dbName;
        //    //ConnectionString = GeneralHelper.GetSQLConnectionString(ServerName, DBName, userName, password);
        //    BizDatabase bizDatabase = new BizDatabase();
        //    Database = bizDatabase.GetDatabase(databaseID);
        //    //ConnectionString = database.ConnectionString;
        //}


        //public event EventHandler<SimpleGenerationInfoArg> ItemGenerationEvent;
        public event EventHandler<ItemImportingStartedArg> ItemImportingStarted;
        //public event EventHandler<ImportCompletedArg> ImportCompleted;

        //public event EventHandler<SimpleGenerationInfoArg> RelationshipGenerationEvent;
        public List<TableImportItem> GetTablesAndColumnInfo()
        {
            //یوزینگ کانتکست باید بره داخل داخل تا خطا باعث جلوگیری از سیو چنج نشه
            List<TableImportItem> result = new List<TableImportItem>();

            //using (var projectContext = new DataAccess.MyProjectEntities())
            //{
            //bool dataBaseInforationExists = false;
            //   var database = projectContext.DatabaseInformation.First(x => x.ID == Database.ID);
            using (SqlConnection testConn = new SqlConnection(Database.ConnectionString))
            {
                testConn.Open();
                int count = 0;
                using (SqlCommand commandCount = new SqlCommand("SELECT count (*) FROM information_schema.tables where table_type='BASE TABLE'", testConn))
                {
                    count = (int)commandCount.ExecuteScalar();
                }

                var tableExtendedPropertyQuery = @"SELECT major_id, minor_id, t.name AS [TableName],ep.name as  [Tag] , value AS [Value]
                                                        FROM sys.extended_properties AS ep INNER JOIN sys.tables AS t ON ep.major_id = t.object_id 
                                                        AND ep.minor_id = 0
                                                        WHERE class = 1 ";// and t.name=" + "'" + table.Name + "'";

                SqlDataAdapter adapterTableDesc = new SqlDataAdapter(tableExtendedPropertyQuery, testConn);
                var datatableTableDes = new DataTable();
                adapterTableDesc.Fill(datatableTableDes);
                var tablesDescEnumerator = datatableTableDes.AsEnumerable();

                var keyColumnsQuery = @"SELECT * FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE OBJECTPROPERTY(OBJECT_ID(CONSTRAINT_SCHEMA + '.' + constraint_name), 'IsPrimaryKey') = 1";// AND table_name = '" + table.Name + "'"";// and t.name=" + "'" + table.Name + "'";
                SqlDataAdapter adapterKeyColumns = new SqlDataAdapter(keyColumnsQuery, testConn);
                var keyColumnsTableDes = new DataTable();
                adapterKeyColumns.Fill(keyColumnsTableDes);
                var keyColumnsEnumerator = keyColumnsTableDes.AsEnumerable();


                var columnExtendedPropertyQuery = @"SELECT major_id, minor_id, t.name AS [TableName], c.name AS [ColumnName],ep.name as [Tag], value AS [Value]
                                                        FROM sys.extended_properties AS ep
                                                        INNER JOIN sys.tables AS t ON ep.major_id = t.object_id 
                                                        INNER JOIN sys.columns AS c ON ep.major_id = c.object_id AND ep.minor_id = c.column_id
                                                        WHERE class = 1 ";// and t.name=" + "'" + table.Name + "'";

                SqlDataAdapter adapterColumnsDesc = new SqlDataAdapter(columnExtendedPropertyQuery, testConn);
                var columnExtendedPropertyDataTable = new DataTable();
                adapterColumnsDesc.Fill(columnExtendedPropertyDataTable);
                var columnDescEnumerator = columnExtendedPropertyDataTable.AsEnumerable();


                var columnsQuery = @"Select *, COLUMNPROPERTY(object_id(TABLE_SCHEMA+'.'+TABLE_NAME), COLUMN_NAME, 'IsIdentity') as IsIdentity 
                            ,(select definition from  sys.computed_columns where object_id(TABLE_SCHEMA+'.'+TABLE_NAME)= sys.computed_columns.object_id and sys.computed_columns.name=Column_Name ) as Formula, COLUMNPROPERTY(object_id(TABLE_SCHEMA+'.'+TABLE_NAME), COLUMN_NAME, 'iscomputed') as IsComputed from INFORMATION_SCHEMA.COLUMNS";

                SqlDataAdapter adaperColumns = new SqlDataAdapter(columnsQuery, testConn);
                var columnsDataTable = new DataTable();
                adaperColumns.Fill(columnsDataTable);
                var columnsEnumerator = columnsDataTable.AsEnumerable();

                var counter = 0;
                using (SqlCommand command = new SqlCommand("SELECT * FROM information_schema.tables where table_type='BASE TABLE'", testConn))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TableDrivedEntityDTO table = new TableDrivedEntityDTO();
                        table.Name = reader["TABLE_Name"].ToString();
                        try
                        {
                            counter++;
                            if (ItemImportingStarted != null)
                                ItemImportingStarted(this, new ItemImportingStartedArg() { ItemName = table.Name, TotalProgressCount = count, CurrentProgress = counter });
                            table.RelatedSchema = reader["TABLE_Schema"].ToString();

                            ////عنوان جدول
                            foreach (var tableDescRow in tablesDescEnumerator.Where(x => x.Field<String>("TableName").ToLower() == table.Name.ToLower()))
                            {
                                if (tableDescRow != null && tableDescRow["Tag"] != null && tableDescRow["Value"] != null)
                                    table.DatabaseDescriptions.Add(new Tuple<string, string>(tableDescRow["Tag"].ToString(), tableDescRow["Value"].ToString()));
                            }
                            ////////

                            //عنوان های ستونها

                            //////

                            List<string> keyColumns = new List<string>();
                            foreach (var row in keyColumnsEnumerator.Where(x => x.Field<String>("TABLE_NAME").ToLower() == table.Name.ToLower()))
                                keyColumns.Add(row["column_name"].ToString());


                            foreach (var columnRow in columnsEnumerator.Where(x => x.Field<String>("TABLE_Name").ToLower() == table.Name.ToLower()).OrderBy(x => x.Field<int>("ORDINAL_POSITION")))
                            {
                                ColumnDTO column = new ColumnDTO();
                                column.Name = columnRow["Column_Name"].ToString();
                                foreach (var columnDescRow in columnDescEnumerator.Where(x => x.Field<String>("TableName").ToLower() == table.Name.ToLower() && x.Field<String>("ColumnName").ToLower() == column.Name.ToLower()))
                                {
                                    if (columnDescRow != null && columnDescRow["Tag"] != null && columnDescRow["Value"] != null)
                                        column.DatabaseDescriptions.Add(new ColumnDescriptionDTO() { Key = columnDescRow["Tag"].ToString(), Value = columnDescRow["Value"].ToString() });
                                }
                                //if (columnDescRow != null && columnDescRow["Value"] != null)
                                //    column.Alias = columnDescRow["Value"].ToString();

                                //if (columnExtendedPropertyDataTable.Rows.Count > 0)
                                //{
                                //    foreach (DataRow row in columnExtendedPropertyDataTable.Rows)
                                //    {
                                //        if (column.Name == row["ColumnName"].ToString())
                                //            columnDesc = row["Value"].ToString();
                                //    }
                                //}
                                //if (!string.IsNullOrEmpty(columnDesc))
                                //    column.Alias = columnDesc;

                                column.DataType = columnRow["DATA_TYPE"].ToString();
                                column.PrimaryKey = keyColumns.Contains(column.Name);
                                column.IsNull = columnRow["is_nullable"].ToString() == "YES";
                                column.IsIdentity = columnRow["IsIdentity"].ToString() == "1";
                                column.Position = Convert.ToInt32(columnRow["ORDINAL_POSITION"].ToString());
                                column.DefaultValue = (columnRow["COLUMN_DEFAULT"] == null ? null : columnRow["COLUMN_DEFAULT"].ToString());





                                if (IsStringType(column))
                                {
                                    //column. = Convert.ToByte(Enum_ColumnType.String);
                                    //if (column.DateColumnType != null)
                                    //    column.DateColumnType = null;
                                    //if (column.NumericColumnType != null)
                                    //    column.NumericColumnType = null;

                                    if (column.StringColumnType == null)
                                        column.StringColumnType = new StringColumnTypeDTO();
                                    column.StringColumnType.MaxLength = (columnRow["CHARACTER_MAXIMUM_LENGTH"] == null ? 0 : Convert.ToInt32(columnRow["CHARACTER_MAXIMUM_LENGTH"]));
                                }
                                else if (IsNumericType(column))
                                {
                                    //       column.TypeEnum = Convert.ToByte(Enum_ColumnType.Numeric);
                                    //if (column.DateColumnType != null)
                                    //    column.DateColumnType = null;
                                    //if (column.StringColumnType != null)
                                    //    column.StringColumnType = null;

                                    if (column.NumericColumnType == null)
                                        column.NumericColumnType = new NumericColumnTypeDTO();
                                    if (columnRow["NUMERIC_PRECISION"] != null)
                                        column.NumericColumnType.Precision = Convert.ToInt32(columnRow["NUMERIC_PRECISION"]);
                                    if (columnRow["NUMERIC_SCALE"] != null)
                                        column.NumericColumnType.Scale = Convert.ToInt32(columnRow["NUMERIC_SCALE"]);
                                }
                                else if (IsDateType(column))
                                {
                                    //   column.TypeEnum = Convert.ToByte(Enum_ColumnType.Date);
                                    //if (column.StringColumnType != null)
                                    //    column.StringColumnType = null;
                                    //if (column.NumericColumnType != null)
                                    //    column.NumericColumnType = null;

                                    if (column.DateColumnType == null)
                                        column.DateColumnType = new DateColumnTypeDTO();
                                }
                                else if (IsBooleanType(column))
                                {
                                    column.IsBoolean = true;
                                    //   column.TypeEnum = Convert.ToByte(Enum_ColumnType.Boolean);
                                    //if (column.StringColumnType != null)
                                    //    column.StringColumnType = null;
                                    //if (column.NumericColumnType != null)
                                    //    column.NumericColumnType = null;
                                    //if (column.DateColumnType == null)
                                    //    column.DateColumnType = null;

                                }

                                var IsComputed = columnRow["IsComputed"];
                                //     column.IsDBCalculatedColumn = Convert.ToBoolean(IsComputed);
                                if (IsComputed != null && Convert.ToBoolean(IsComputed))
                                {
                                    if ((columnRow["Formula"]) != null)
                                        column.DBFormula = columnRow["Formula"].ToString();
                                }
                                else
                                {
                                    column.DBFormula = "";
                                    //column.IsDBCalculatedColumn = false;
                                    //if (column.DBCalculatedColumn != null)
                                    //    projectContext.DBCalculatedColumn.Remove(column.DBCalculatedColumn);
                                }
                                table.Columns.Add(column);
                            }
                            //    string queryColumns = @"Select *, COLUMNPROPERTY(object_id(TABLE_SCHEMA+'.'+TABLE_NAME), COLUMN_NAME, 'IsIdentity') as IsIdentity 
                            //,(select definition from  sys.computed_columns where object_id(TABLE_SCHEMA+'.'+TABLE_NAME)= sys.computed_columns.object_id and sys.computed_columns.name=Column_Name ) as Formula, COLUMNPROPERTY(object_id(TABLE_SCHEMA+'.'+TABLE_NAME), COLUMN_NAME, 'iscomputed') as IsComputed from INFORMATION_SCHEMA.COLUMNS";
                            //    using (SqlCommand commandFields = new SqlCommand(queryColumns + " Where TABLE_Name = '" + table.Name + "' ", testConn))
                            //    using (SqlDataReader readerFields = commandFields.ExecuteReader())
                            //    {
                            //while (readerFields.Read())
                            //{

                            //}

                            //if (table.ID == 0)
                            //    projectContext.Table.Add(table);
                            //projectContext.SaveChanges();
                            //result.SuccessfulItems.Add(resultitem);






                            result.Add(new TableImportItem(table, false, ""));
                        }
                        catch (Exception ex)
                        {
                            result.Add(new TableImportItem(table, true, ex.Message));
                        }

                    }

                    //    }
                }

                //if (ImportCompleted != null)
                //    ImportCompleted(this, new  ImportCompletedArg() { TaskName =  });
                return result;
            }

            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }



        //public bool GenerateDefaultEntities()
        //{
        //    GenericResult<OperationResult> result = new GenericResult<OperationResult>();
        //    try
        //    {
        //        using (var projectContext = new DataAccess.MyProjectEntities())
        //        {
        //            var list = projectContext.Table.Where(x => !x.TableDrivedEntity.Any(y => y.Criteria == "" || y.Criteria == null));
        //            int count = list.Count();
        //            var counter = 0;
        //            foreach (var table in list)
        //            {
        //                TableDrivedEntity tdEntity = new TableDrivedEntity();
        //                tdEntity.IndependentDataEntry = true;
        //                tdEntity.Alias = table.Alias;
        //                tdEntity.Name = table.Name;
        //                table.TableDrivedEntity.Add(tdEntity);

        //                counter++;
        //                if (ItemGenerationEvent != null)
        //                    ItemGenerationEvent(this, new SimpleGenerationInfoArg() { Title = table.Name, TotalProgressCount = count, CurrentProgress = counter });

        //            }
        //            projectContext.SaveChanges();
        //            if (ItemGenerationEvent != null)
        //                ItemGenerationEvent(this, new SimpleGenerationInfoArg() { Title = "Operation is completed." });
        //            return true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;

        //    }
        //}
        private bool IsStringType(ColumnDTO column)
        {
            return (column.DataType == "char" || column.DataType == "nvarchar" || column.DataType == "varchar" || column.DataType == "text");
        }
        private bool IsBooleanType(ColumnDTO column)
        {
            return (column.DataType == "bit");
        }
        private bool IsNumericType(ColumnDTO column)
        {
            return (column.DataType == "bigint" || column.DataType == "numeric" || column.DataType == "smallint"
                || column.DataType == "decimal" || column.DataType == "smallmoney" || column.DataType == "int"
                || column.DataType == "tinyint" || column.DataType == "money");
        }
        private bool IsDateType(ColumnDTO column)
        {
            return (column.DataType == "date" || column.DataType == "datetime");
        }
        public List<RelationshipImportItem> GetRelationships()
        {
            List<RelationshipImportItem> result = new List<RelationshipImportItem>();
            //try
            //{

            using (SqlConnection testConn = new SqlConnection(Database.ConnectionString))
            {
                //using (var projectContext = new DataAccess.MyProjectEntities())
                //{
                testConn.Open();

                //نام رابطه و جداول مرتبط
                string groupStr = @"SELECT
                                             fk.name 'Constraint_Name',tp.name 'FK_Table',  tr.name 'PK_Table',count(*) as count
                                        FROM 
                                            sys.foreign_keys fk
                                        INNER JOIN 
                                            sys.tables tp ON fk.parent_object_id = tp.object_id
                                        INNER JOIN 
                                            sys.tables tr ON fk.referenced_object_id = tr.object_id
                                        INNER JOIN 
                                            sys.foreign_key_columns fkc ON fkc.constraint_object_id = fk.object_id
                                        INNER JOIN 
                                            sys.columns cp ON fkc.parent_column_id = cp.column_id AND fkc.parent_object_id = cp.object_id
                                        INNER JOIN 
                                            sys.columns cr ON fkc.referenced_column_id = cr.column_id AND fkc.referenced_object_id = cr.object_id
        			group by  fk.name,tp.name,tr.name";


                //گرفتن تعداد
                string groupStrCount = @"select count(*) from (SELECT
                                             fk.name 'Constraint_Name',tp.name 'FK_Table',  tr.name 'PK_Table',count(*) as count
                                        FROM 
                                            sys.foreign_keys fk
                                        INNER JOIN 
                                            sys.tables tp ON fk.parent_object_id = tp.object_id
                                        INNER JOIN 
                                            sys.tables tr ON fk.referenced_object_id = tr.object_id
                                        INNER JOIN 
                                            sys.foreign_key_columns fkc ON fkc.constraint_object_id = fk.object_id
                                        INNER JOIN 
                                            sys.columns cp ON fkc.parent_column_id = cp.column_id AND fkc.parent_object_id = cp.object_id
                                        INNER JOIN 
                                            sys.columns cr ON fkc.referenced_column_id = cr.column_id AND fkc.referenced_object_id = cr.object_id
        			group by  fk.name,tp.name,tr.name) as innerQuery";

                //به همراه ستونها
                string commandStr = @"SELECT
                                            fk.name 'Constraint_Name',
                                            tp.name 'FK_Table',
                                            cp.name 'FK_Column',
                                            tr.name 'PK_Table',
                                            cr.name 'PK_Column'
                                        FROM 
                                            sys.foreign_keys fk
                                        INNER JOIN 
                                            sys.tables tp ON fk.parent_object_id = tp.object_id
                                        INNER JOIN 
                                            sys.tables tr ON fk.referenced_object_id = tr.object_id
                                        INNER JOIN 
                                            sys.foreign_key_columns fkc ON fkc.constraint_object_id = fk.object_id
                                        INNER JOIN 
                                            sys.columns cp ON fkc.parent_column_id = cp.column_id AND fkc.parent_object_id = cp.object_id
                                        INNER JOIN 
                                            sys.columns cr ON fkc.referenced_column_id = cr.column_id AND fkc.referenced_object_id = cr.object_id
                                        ORDER BY
                                            tp.name, cp.column_id";


                SqlDataAdapter adapterRelationDesc = new SqlDataAdapter(commandStr, testConn);
                var datatableRelation = new DataTable();
                adapterRelationDesc.Fill(datatableRelation);
                var relationEnumerator = datatableRelation.AsEnumerable();


                //برای اینکه ببینیم فارن کی خودش کلید هست
                var keyColumnsQuery = @"SELECT * FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE OBJECTPROPERTY(OBJECT_ID(CONSTRAINT_SCHEMA + '.' + constraint_name), 'IsPrimaryKey') = 1";// AND table_name = '" + table.Name + "'"";// and t.name=" + "'" + table.Name + "'";
                SqlDataAdapter adapterKeyColumns = new SqlDataAdapter(keyColumnsQuery, testConn);
                var keyColumnsTableDes = new DataTable();
                adapterKeyColumns.Fill(keyColumnsTableDes);
                var keyColumnsEnumerator = keyColumnsTableDes.AsEnumerable();

                //DataTable relationDataTable = null;
                //using (SqlCommand command = new SqlCommand(commandStr, testConn))
                //{
                //    using (SqlDataReader dr = command.ExecuteReader())
                //    {
                //        relationDataTable = new DataTable();
                //        relationDataTable.Load(dr);
                //    }
                //}
                int count = 0;
                using (SqlCommand commandCount = new SqlCommand(groupStrCount, testConn))
                {
                    count = (int)commandCount.ExecuteScalar();
                }

                var counter = 0;
                using (SqlCommand command = new SqlCommand(groupStr, testConn))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string tooltip = "";
                        var relation = new RelationshipDTO();
                        relation.Name = reader["Constraint_Name"].ToString();
                        try
                        {

                            counter++;
                            if (ItemImportingStarted != null)
                                ItemImportingStarted(this, new ItemImportingStartedArg() { ItemName = relation.Name, TotalProgressCount = count, CurrentProgress = counter });

                            relation.Entity1 = reader["PK_Table"].ToString();


                            relation.Entity2 = reader["FK_Table"].ToString();

                            ////var entity1 = projectContext.TableDrivedEntity.FirstOrDefault(x => x.DeteminerColumnID == null && x.Table.Name == PKTable && x.Table.DBSchema.DatabaseInformationID == Database.ID);
                            ////if (entity1 == null)
                            ////    throw (new Exception("There is no entity defined for table " + PKTable));

                            ////var entity2 = projectContext.TableDrivedEntity.FirstOrDefault(x => x.DeteminerColumnID == null && x.Table.Name == FKTable && x.Table.DBSchema.DatabaseInformationID == Database.ID);
                            ////if (entity2 == null)
                            ////    throw (new Exception("There is no entity defined for table " + FKTable));

                            //var relations = projectContext.Relationship.Where(x => x.TableDrivedEntity.ID == entity1.ID && x.TableDrivedEntity1.ID == entity2.ID);
                            var rows = relationEnumerator.Where(x => x.Field<string>("Constraint_Name").ToLower() == relation.Name.ToLower());
                            string PKColumns = "";
                            string FKColumns = "";
                            foreach (var row in rows)
                            {
                                string PKColumn = row["PK_Column"].ToString();
                                string FKColumn = row["FK_Column"].ToString();
                                PKColumns += (PKColumns == "" ? "" : ",") + PKColumn;
                                FKColumns += (FKColumns == "" ? "" : ",") + FKColumn;
                                //var pkColumn = projectContext.Column.Where(x => x.Table.Name == PKTable && x.Table.DBSchema.DatabaseInformationID == Database.ID && x.Name == PKColumn).FirstOrDefault();
                                //if (pkColumn == null)
                                //    throw (new Exception("Column " + PKColumn + " in " + PKTable + " is not found!"));
                                //var fkColumn = projectContext.Column.Where(x => x.Table.Name == FKTable && x.Table.DBSchema.DatabaseInformationID == Database.ID && x.Name == FKColumn).FirstOrDefault();
                                //if (fkColumn == null)
                                //    throw (new Exception("Column " + FKColumn + " in " + FKTable + " is not found!"));
                                //relations = relations.Where(x => x.RelationshipColumns.Any(y => y.Column.Name == PKColumn && y.Column1.Name == FKColumn));
                                relation.RelationshipColumns.Add(new RelationshipColumnDTO() { FirstSideColumn = new ColumnDTO() { Name = PKColumn }, SecondSideColumn = new ColumnDTO() { Name = FKColumn } });
                                tooltip += (tooltip == "" ? "" : Environment.NewLine) + relation.Entity1 + "." + PKColumn + " = " + relation.Entity2 + "." + FKColumn;
                            }

                            relation.FKSidePKColumnsAreFkColumns = FKSidePKColumnsAreFkColumns(relation, keyColumnsEnumerator);
                            if (relation.FKSidePKColumnsAreFkColumns)
                            {

                            }
                                //Relationship reverseRelation = null;
                                //var relation = relations.FirstOrDefault();
                                //if (relation != null)
                                //    reverseRelation = projectContext.Relationship.FirstOrDefault(x => x.RelationshipID == relation.ID);
                                //if (relation == null)
                                //    relation = new Relationship();
                                //if (reverseRelation == null)
                                //    reverseRelation = new Relationship();
                                //     relation.Info = "(PK)" + relation.Entity1 + "." + PKColumns + ">(FK)" + relation.Entity2 + "." + FKColumns;
                                //reverseRelation.Name = relation.Name + "=(FK)" + FKTable + "." + FKColumns + ">" + "(PK)" + PKTable + "." + PKColumns;

                                //if (relation.ID == 0)
                                //{
                                //    relation.MasterTypeEnum = (int)Enum_MasterRelationshipType.FromPrimartyToForeign;
                                //    reverseRelation.MasterTypeEnum = (int)Enum_MasterRelationshipType.FromForeignToPrimary;
                                //    relation.TableDrivedEntity = entity1;
                                //    relation.TableDrivedEntity1 = entity2;
                                //    reverseRelation.TableDrivedEntity1 = entity1;
                                //    reverseRelation.TableDrivedEntity = entity2;

                                //    foreach (var row in rows)
                                //    {
                                //        string PKColumn = row["PK_Column"].ToString();
                                //        string FKColumn = row["FK_Column"].ToString();
                                //        var pkColumn = projectContext.Column.Where(x => x.Table.Name == PKTable && x.Table.DBSchema.DatabaseInformationID == Database.ID && x.Name == PKColumn).FirstOrDefault();
                                //        var fkColumn = projectContext.Column.Where(x => x.Table.Name == FKTable && x.Table.DBSchema.DatabaseInformationID == Database.ID && x.Name == FKColumn).FirstOrDefault();
                                //        relation.RelationshipColumns.Add(new RelationshipColumns() { Column = pkColumn, Column1 = fkColumn });
                                //        reverseRelation.RelationshipColumns.Add(new RelationshipColumns() { Column = fkColumn, Column1 = pkColumn });
                                //    }
                                //}
                                //if (relation.ID == 0)
                                //{

                                //    relation.SecurityObject = new SecurityObject();
                                //    relation.SecurityObject.Type = (int)DatabaseObjectCategory.Relationship;
                                //    projectContext.Relationship.Add(relation);
                                //    reverseRelation.SecurityObject = new SecurityObject();
                                //    reverseRelation.SecurityObject.Type = (int)DatabaseObjectCategory.Relationship;
                                //    projectContext.Relationship.Add(reverseRelation);
                                //    projectContext.SaveChanges();
                                //    relation.RelationshipID = reverseRelation.ID;
                                //    reverseRelation.RelationshipID = relation.ID;
                                //    projectContext.SaveChanges();
                                //}
                                //else
                                //    projectContext.SaveChanges();


                                //    result.SuccessfulItems.Add(resultitem);
                                result.Add(new RelationshipImportItem(relation, false, tooltip));
                        }
                        catch (Exception ex)
                        {
                            result.Add(new RelationshipImportItem(relation, true, ex.Message));

                            //   resultitem.Message = MyGeneralLibrary.GeneralExceptionManager.GetExceptionMessage(ex);
                            //  result.FailedItems.Add(resultitem);
                        }
                    }
                }
                //}
                //catch (System.Data.Entity.Validation.DbEntityValidationException e)
                //{
                //    foreach (var eve in e.EntityValidationErrors)
                //    {
                //        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                //            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                //        foreach (var ve in eve.ValidationErrors)
                //        {
                //            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                //                ve.PropertyName, ve.ErrorMessage);
                //        }
                //    }
                //    throw;
                //}


                //}
            }
            return result;
            //////                if (done && faild)
            //////                    result.Result = OperationResult.PartiallyDone;
            //////                else if (done)
            //////                    result.Result = OperationResult.Done;
            //////                else if (faild)
            //////                    result.Result = OperationResult.Failed;

            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

        }

        private bool FKSidePKColumnsAreFkColumns(RelationshipDTO relation, EnumerableRowCollection<DataRow> keyColumnsEnumerator)
        {
            var fkSidePkColumns = new List<string>();
            foreach (var row in keyColumnsEnumerator.Where(x => x["TABLE_NAME"].ToString() == relation.Entity2))
            {
                fkSidePkColumns.Add(row["column_name"].ToString());
            }
            var fkColumns = new List<string>();
            foreach (var row in relation.RelationshipColumns)
            {
                fkColumns.Add(row.SecondSideColumn.Name);
            }
            return fkSidePkColumns.All(y => fkColumns.Any(z => y == z)) &&
           fkColumns.All(x => fkSidePkColumns.Any(z => z == x));
        }



        //private List<DataRow> FindRelationDataRow(DataTable relationDataTable, string relationName)
        //{
        //    List<DataRow> result = new List<DataRow>();
        //    foreach (DataRow row in relationDataTable.Rows)
        //    {
        //        if (row != null)
        //            if (row["Constraint_Name"].ToString() == relationName)
        //                result.Add(row);
        //    }
        //    return result;

        //}
        //public bool CheckRelationTypesExits()
        //{
        //    try
        //    {
        //        using (IdeaEntities context = new IdeaEntities())
        //        {
        //            using (SqlConnection testConn = new SqlConnection(Database.ConnectionString))
        //            {
        //                testConn.Open();
        //                string commandStr = @"SELECT FK_Table = FK.TABLE_NAME,FK_Column = CU.COLUMN_NAME,PK_Table = PK.TABLE_NAME,PK_Column = PT.COLUMN_NAME,
        //                                                Constraint_Name = C.CONSTRAINT_NAME
        //                                                FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS C
        //                                                INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS FK ON C.CONSTRAINT_NAME = FK.CONSTRAINT_NAME
        //                                                INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS PK ON C.UNIQUE_CONSTRAINT_NAME = PK.CONSTRAINT_NAME
        //                                                INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE CU ON C.CONSTRAINT_NAME = CU.CONSTRAINT_NAME
        //                                                INNER JOIN (
        //                                                SELECT i1.TABLE_NAME, i2.COLUMN_NAME
        //                                                FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS i1
        //                                                INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE i2 ON i1.CONSTRAINT_NAME = i2.CONSTRAINT_NAME
        //                                                WHERE i1.CONSTRAINT_TYPE = 'PRIMARY KEY'
        //                                                ) PT ON PT.TABLE_NAME = PK.TABLE_NAME";

        //                using (SqlCommand command = new SqlCommand(commandStr, testConn))
        //                using (SqlDataReader reader = command.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        BizType

        //                                    }
        //                }
        //            }

        //            return true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw (ex);
        //    }
        //}


        ////////public GenericResult<OperationResult> GenerateDefaultPackages()
        ////////{
        ////////    GenericResult<OperationResult> result = new GenericResult<OperationResult>();
        ////////    try
        ////////    {
        ////////        BizType bizType = new BizType();
        ////////        Dictionary<string, string> typeCategories = new Dictionary<string, string>();
        ////////        typeCategories.Add(SQLServerSetting.SQLGlobalVariables.TypeCategory.ServerName, ServerName);
        ////////        typeCategories.Add(SQLServerSetting.SQLGlobalVariables.TypeCategory.DatabaseName, DBName);
        ////////        var typeList = bizType.GetTypes(typeCategories);
        ////////        foreach (var item in typeList)
        ////////        {

        ////////        }
        ////////        return result;
        ////////    }
        ////////    catch (Exception ex)
        ////////    {
        ////////        throw ex;
        ////////    }
        ////////}

        //در اسکیوئل یونیک کانسترنت و یونیک ایندکس درایم که خیلی متفاوت از هم نیستن.برای هر یونیک کانسترنت یک یونیک ایندکس میسازد
        public ImportResult GenerateUniqueConstraints()
        { //try
          //{
            ImportResult result = new MyModelGenerator.ImportResult();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                using (SqlConnection testConn = new SqlConnection(Database.ConnectionString))
                {
                    testConn.Open();
                    string commandStr = @"SELECT TableName = t.name,IndexName = ind.name,ind.is_unique,ind.is_unique_constraint,ColumnName = col.name,ind.[type],ind.type_desc
                                        FROM sys.indexes ind INNER JOIN sys.index_columns ic ON  ind.object_id = ic.object_id and ind.index_id = ic.index_id 
                                        INNER JOIN sys.columns col ON ic.object_id = col.object_id and ic.column_id = col.column_id 
                                        INNER JOIN sys.tables t ON ind.object_id = t.object_id 
                                        WHERE ind.is_primary_key = 0 AND (ind.is_unique = 1 or ind.is_unique_constraint = 1) AND t.is_ms_shipped = 0 
                                        ORDER BY t.name, ind.name, ind.index_id, ic.index_column_id ";
                    string commandCountStr = @"SELECT count (*)
                                        FROM sys.indexes ind INNER JOIN sys.index_columns ic ON  ind.object_id = ic.object_id and ind.index_id = ic.index_id 
                                        INNER JOIN sys.columns col ON ic.object_id = col.object_id and ic.column_id = col.column_id 
                                        INNER JOIN sys.tables t ON ind.object_id = t.object_id 
                                        WHERE ind.is_primary_key = 0 AND (ind.is_unique = 1 or ind.is_unique_constraint = 1) AND t.is_ms_shipped = 0 ";
                    int count = 0;
                    using (SqlCommand commandCount = new SqlCommand(commandCountStr, testConn))
                    {
                        count = (int)commandCount.ExecuteScalar();
                    }
                    var counter = 0;

                    using (SqlCommand command = new SqlCommand(commandStr, testConn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {


                            while (reader.Read())
                            {
                                var resultitem = new MyModelGenerator.ImportResultItem();
                                resultitem.Title = reader["TableName"].ToString();
                                try
                                {
                                    var tableName = reader["TableName"].ToString();
                                    //var catalogName = GeneralHelper.GetCatalogName(ServerName, DBName); ;
                                    var columnName = reader["ColumnName"].ToString();

                                    counter++;

                                    if (ItemImportingStarted != null)
                                        ItemImportingStarted(this, new ItemImportingStartedArg() { ItemName = tableName, TotalProgressCount = count, CurrentProgress = counter });
                                    //if (ItemGenerationEvent != null)
                                    //    ItemGenerationEvent(this, new SimpleGenerationInfoArg() { Title = tableName, TotalProgressCount = count, CurrentProgress = counter });

                                    Table table = projectContext.Table.Where(x => x.Name == tableName && x.DBSchema.DatabaseInformationID == Database.ID).FirstOrDefault();
                                    Column column = projectContext.Column.Where(x => x.Table.Name == tableName && x.Table.DBSchema.DatabaseInformationID == Database.ID && x.Name == columnName).FirstOrDefault();
                                    if (table == null)
                                        throw (new Exception("Table " + tableName + " is not found!"));
                                    if (column == null)
                                        throw (new Exception("Column " + columnName + " in " + tableName + " is not found!"));
                                    var indexName = reader["IndexName"].ToString();
                                    var index = table.UniqueConstraint.Where(x => x.Name == indexName).FirstOrDefault();

                                    if (index == null)
                                    {
                                        index = new DataAccess.UniqueConstraint();
                                        table.UniqueConstraint.Add(index);
                                    }
                                    index.Name = indexName;
                                    if (!index.Column.Any(x => x == column))
                                        index.Column.Add(column);

                                    //if (index.ID == 0)
                                    //    projectContext.UniqueConstraint.Add(index);
                                    projectContext.SaveChanges();
                                    result.SuccessfulItems.Add(resultitem);
                                }

                                catch (Exception ex)
                                {
                                    resultitem.Message = MyGeneralLibrary.GeneralExceptionManager.GetExceptionMessage(ex);
                                    result.FailedItems.Add(resultitem);
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }

        public List<TableImportItem> GetViewsInfo()
        { //try
          //{
            List<TableImportItem> result = new List<TableImportItem>();
            //using (var projectContext = new DataAccess.MyProjectEntities())
            //{
            using (SqlConnection testConn = new SqlConnection(Database.ConnectionString))
            {
                testConn.Open();

                string commandCountStr = @"SELECT Count(*) FROM sys.views";
                int count = 0;
                using (SqlCommand commandCount = new SqlCommand(commandCountStr, testConn))
                {
                    count = (int)commandCount.ExecuteScalar();
                }
                var counter = 0;

                var tableExtendedPropertyQuery = @"SELECT major_id, minor_id, t.name AS [TableName],ep.name as  [Tag] , value AS [Value]
                                                        FROM sys.extended_properties AS ep INNER JOIN sys.views AS t ON ep.major_id = t.object_id 
                                                        AND ep.minor_id = 0
                                                        WHERE class = 1 ";// and t.name=" + "'" + table.Name + "'";

                var columnExtendedPropertyQuery = @"SELECT major_id, minor_id, t.name AS [TableName], c.name AS [ColumnName],ep.name as [Tag], value AS [Value]
                                                        FROM sys.extended_properties AS ep
                                                        INNER JOIN sys.views AS t ON ep.major_id = t.object_id 
                                                        INNER JOIN sys.columns AS c ON ep.major_id = c.object_id AND ep.minor_id = c.column_id
                                                        WHERE class = 1 ";// and t.name=" + "'" + table.Name + "'";

                SqlDataAdapter adapterColumnsDesc = new SqlDataAdapter(columnExtendedPropertyQuery, testConn);
                var columnExtendedPropertyDataTable = new DataTable();
                adapterColumnsDesc.Fill(columnExtendedPropertyDataTable);
                var columnDescEnumerator = columnExtendedPropertyDataTable.AsEnumerable();


                SqlDataAdapter adapterTableDesc = new SqlDataAdapter(tableExtendedPropertyQuery, testConn);
                var datatableTableDes = new DataTable();
                adapterTableDesc.Fill(datatableTableDes);
                var tablesDescEnumerator = datatableTableDes.AsEnumerable();

                string commandStr = @"SELECT *,SCHEMA_NAME(schema_id) as ViewSchema FROM sys.views";
                using (SqlCommand command = new SqlCommand(commandStr, testConn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {


                        while (reader.Read())
                        {
                            var table = new TableDrivedEntityDTO();

                            table.Name = reader["Name"].ToString();
                            try
                            {

                                //var catalogName = GeneralHelper.GetCatalogName(ServerName, DBName);


                                counter++;
                                if (ItemImportingStarted != null)
                                    ItemImportingStarted(this, new ItemImportingStartedArg() { ItemName = table.Name, TotalProgressCount = count, CurrentProgress = counter });

                                table.RelatedSchema = reader["ViewSchema"].ToString();

                                ////عنوان جدول
                                foreach (var tableDescRow in tablesDescEnumerator.Where(x => x.Field<String>("TableName").ToLower() == table.Name.ToLower()))
                                {
                                    if (tableDescRow != null && tableDescRow["Tag"] != null && tableDescRow["Value"] != null)
                                        table.DatabaseDescriptions.Add(new Tuple<string, string>(tableDescRow["Tag"].ToString(), tableDescRow["Value"].ToString()));
                                }
                                ////////

                                //var dbSchema = projectContext.DBSchema.FirstOrDefault(x => x.DatabaseInformationID == Database.ID && x.Name == schema);
                                //if (dbSchema == null)
                                //{
                                //    dbSchema = new DBSchema() { DatabaseInformationID = Database.ID, Name = schema };
                                //    dbSchema.SecurityObject = new SecurityObject();
                                //    dbSchema.SecurityObject.Type = (int)DatabaseObjectCategory.Schema;
                                //    projectContext.DBSchema.Add(dbSchema);
                                //}

                                // View view = projectContext.View.Where(x => x.Name == view.Name && x.DBSchema.DatabaseInformationID == Database.ID).FirstOrDefault();

                                //if (view == null)
                                //{
                                //    view = new View();
                                //    view.Name = viewName;
                                //    //view.Catalog = catalogName;
                                //}
                                //   view.RelatedSchema = dbSchema;
                                //view.RelatedSchema = reader["ViewSchema"].ToString();
                                string queryColumns = @"SELECT * FROM information_schema.columns WHERE 
                                    table_name = '" + table.Name + "'";
                                using (SqlCommand commandFields = new SqlCommand(queryColumns, testConn))
                                using (SqlDataReader readerFields = commandFields.ExecuteReader())
                                {
                                    while (readerFields.Read())
                                    {
                                        ColumnDTO column = new ColumnDTO();
                                        column.Name = readerFields["Column_Name"].ToString();
                                        foreach (var columnDescRow in columnDescEnumerator.Where(x => x.Field<String>("TableName").ToLower() == table.Name.ToLower() && x.Field<String>("ColumnName").ToLower() == column.Name.ToLower()))
                                        {
                                            if (columnDescRow != null && columnDescRow["Tag"] != null && columnDescRow["Value"] != null)
                                                column.DatabaseDescriptions.Add(new ColumnDescriptionDTO() { Key = columnDescRow["Tag"].ToString(), Value = columnDescRow["Value"].ToString() });
                                        }

                                        column.DataType = readerFields["DATA_TYPE"].ToString();
                                        // column.PrimaryKey = keyColumns.Contains(column.Name);
                                        column.IsNull = readerFields["is_nullable"].ToString() == "YES";
                                        //column.IsIdentity = readerFields["IsIdentity"].ToString() == "1";
                                        column.Position = Convert.ToInt32(readerFields["ORDINAL_POSITION"].ToString());
                                        column.DefaultValue = (readerFields["COLUMN_DEFAULT"] == null ? null : readerFields["COLUMN_DEFAULT"].ToString());

                                        table.Columns.Add(column);
                                    }
                                }
                                //if (view.ID == 0)
                                //    projectContext.View.Add(view);
                                //projectContext.SaveChanges();
                                //result.SuccessfulItems.Add(resultitem);

                                result.Add(new TableImportItem(table, false, ""));
                            }
                            catch (Exception ex)
                            {
                                result.Add(new TableImportItem(table, true, ex.Message));
                                //resultitem.Message = MyGeneralLibrary.GeneralExceptionManager.GetExceptionMessage(ex);
                                //result.FailedItems.Add(resultitem);
                            }
                        }
                    }
                    //}
                }
            }
            return result;
        }

        public List<FunctionImportItem> GetFunctions()
        { //try

            List<FunctionImportItem> result = new List<FunctionImportItem>();
            //using (var projectContext = new DataAccess.MyProjectEntities())
            //{
            using (SqlConnection testConn = new SqlConnection(Database.ConnectionString))
            {
                testConn.Open();
                string commandStr = @"SELECT name AS function_name,Parameter_Mode ,Is_Result,Parameter_Name,Data_Type
                        ,SCHEMA_NAME(schema_id) AS schema_name
                        ,type_desc
                        FROM sys.objects inner join INFORMATION_SCHEMA.PARAMETERS on sys.objects.name=specific_Name
                        WHERE type_desc LIKE '%FUNCTION%' and type_desc like '%scalar%' and Parameter_Mode='out';";
                string commandCountStr = @"SELECT Count(*) FROM   sys.objects inner join INFORMATION_SCHEMA.PARAMETERS on sys.objects.name=specific_Name
                        WHERE type_desc LIKE '%FUNCTION%' and type_desc like '%scalar%' and Parameter_Mode='out'";
                int count = 0;
                using (SqlCommand commandCount = new SqlCommand(commandCountStr, testConn))
                {
                    count = (int)commandCount.ExecuteScalar();
                }
                var counter = 0;

                using (SqlCommand command = new SqlCommand(commandStr, testConn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {


                        while (reader.Read())
                        {
                            var function = new DatabaseFunctionDTO();
                            function.FunctionName = reader["function_name"].ToString();
                            try
                            {


                                counter++;
                                if (ItemImportingStarted != null)
                                    ItemImportingStarted(this, new ItemImportingStartedArg() { ItemName = function.FunctionName, TotalProgressCount = count, CurrentProgress = counter });

                                //var DatabaseFunction = projectContext.DatabaseFunction.Where(x => x.FunctionName == function.Title && x.DBSchema.DatabaseInformationID == Database.ID && x.Type == 1).FirstOrDefault();

                                function.RelatedSchema = reader["schema_name"].ToString();
                                //var dbSchema = projectContext.DBSchema.FirstOrDefault(x => x.DatabaseInformationID == Database.ID && x.Name == schema);
                                //if (dbSchema == null)
                                //{
                                //    dbSchema = new DBSchema() { DatabaseInformationID = Database.ID, Name = schema };
                                //    dbSchema.SecurityObject = new SecurityObject();
                                //    dbSchema.SecurityObject.Type = (int)DatabaseObjectCategory.Schema;
                                //    projectContext.DBSchema.Add(dbSchema);
                                //}

                                //if (DatabaseFunction == null)
                                //{
                                //    DatabaseFunction = new DatabaseFunction();
                                //    DatabaseFunction.FunctionName = function.Title;

                                //}

                                //DatabaseFunction.DBSchema = dbSchema;
                                function.ReturnType = reader["Data_Type"].ToString().Trim();
                                function.Type = Enum_DatabaseFunctionType.Function;

                                var queryColumns = @"SELECT specific_Name,Parameter_Mode Is_Result,Parameter_Name,Data_Type
                                               FROM  INFORMATION_SCHEMA.PARAMETERS where  specific_Name='" + function.FunctionName + "' and Parameter_Mode='IN'";
                                using (SqlCommand commandFields = new SqlCommand(queryColumns, testConn))
                                using (SqlDataReader readerFields = commandFields.ExecuteReader())
                                {
                                    while (readerFields.Read())
                                    {
                                        var column = new DatabaseFunctionColumnDTO();
                                        function.DatabaseFunctionParameter.Add(column);
                                        column.ParameterName = readerFields["Parameter_Name"].ToString();
                                        column.DataType = readerFields["Data_Type"].ToString();
                                        //var column = DatabaseFunction.DatabaseFunctionParameter.Where(x => x.ParamName == paramName).FirstOrDefault();
                                        //if (column == null)
                                        //{
                                        //    column = new DatabaseFunctionParameter();
                                        //    column.ParamName = paramName;
                                        //    DatabaseFunction.DatabaseFunctionParameter.Add(column);
                                        //}
                                        //column.DataType = dataType;
                                    }
                                }

                                //if (DatabaseFunction.ID == 0)
                                //    projectContext.DatabaseFunction.Add(DatabaseFunction);
                                //projectContext.SaveChanges();
                                result.Add(new FunctionImportItem(function, false, ""));
                            }
                            catch (Exception ex)
                            {
                                result.Add(new FunctionImportItem(function, true, ex.Message));
                                //resultitem.Message = MyGeneralLibrary.GeneralExceptionManager.GetExceptionMessage(ex);
                                //result.FailedItems.Add(resultitem);
                            }
                        }
                    }
                }




                commandStr = @"select * 
                                  from information_schema.routines 
                                 where routine_type = 'PROCEDURE'";
                commandCountStr = @"select count(*)
                      from information_schema.routines 
                     where routine_type = 'PROCEDURE'";
                count = 0;
                using (SqlCommand commandCount = new SqlCommand(commandCountStr, testConn))
                {
                    count = (int)commandCount.ExecuteScalar();
                }
                counter = 0;

                using (SqlCommand command = new SqlCommand(commandStr, testConn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var function = new DatabaseFunctionDTO();
                            function.FunctionName = reader["specific_name"].ToString();
                            try
                            {

                                //var function.Title = reader["specific_name"].ToString();
                                counter++;
                                if (ItemImportingStarted != null)
                                    ItemImportingStarted(this, new ItemImportingStartedArg() { ItemName = function.FunctionName, TotalProgressCount = count, CurrentProgress = counter });

                                //var DatabaseFunction = projectContext.DatabaseFunction.Where(x => x.FunctionName == function.FunctionName && x.DBSchema.DatabaseInformationID == Database.ID && x.Type == 2).FirstOrDefault();

                                function.RelatedSchema = reader["specific_schema"].ToString();
                                //var dbSchema = projectContext.DBSchema.FirstOrDefault(x => x.DatabaseInformationID == Database.ID && x.Name == schema);
                                //if (dbSchema == null)
                                //{
                                //    dbSchema = new DBSchema() { DatabaseInformationID = Database.ID, Name = schema };
                                //    dbSchema.SecurityObject = new SecurityObject();
                                //    dbSchema.SecurityObject.Type = (int)DatabaseObjectCategory.Schema;
                                //    projectContext.DBSchema.Add(dbSchema);
                                //}

                                //if (DatabaseFunction == null)
                                //{
                                //    DatabaseFunction = new DatabaseFunction();
                                //    DatabaseFunction.FunctionName = function.Title;
                                //}

                                //DatabaseFunction.DBSchema = dbSchema;
                                function.ReturnType = "";
                                function.Type = Enum_DatabaseFunctionType.StoredProcedure;

                                var queryColumns = @"select * from 
                                       sys.objects inner join INFORMATION_SCHEMA.PARAMETERS on sys.objects.name=specific_Name
                                     WHERE (type = 'P')  and specific_Name='" + function.FunctionName + "' and Parameter_Mode='IN'";
                                using (SqlCommand commandFields = new SqlCommand(queryColumns, testConn))
                                using (SqlDataReader readerFields = commandFields.ExecuteReader())
                                {
                                    while (readerFields.Read())
                                    {
                                        var column = new DatabaseFunctionColumnDTO();
                                        function.DatabaseFunctionParameter.Add(column);
                                        column.ParameterName = readerFields["Parameter_Name"].ToString();
                                        column.DataType = readerFields["Data_Type"].ToString();
                                        //var column = DatabaseFunction.DatabaseFunctionParameter.Where(x => x.ParamName == paramName).FirstOrDefault();
                                        //if (column == null)
                                        //{
                                        //    column = new DatabaseFunctionParameter();
                                        //    column.ParamName = paramName;
                                        //    DatabaseFunction.DatabaseFunctionParameter.Add(column);
                                        //}
                                        //column.DataType = dataType;
                                    }
                                }

                                //if (DatabaseFunction.ID == 0)
                                //    projectContext.DatabaseFunction.Add(DatabaseFunction);
                                //projectContext.SaveChanges();
                                result.Add(new FunctionImportItem(function, false, ""));
                            }
                            catch (Exception ex)
                            {
                                result.Add(new FunctionImportItem(function, true, ex.Message));
                                //resultitem.Message = MyGeneralLibrary.GeneralExceptionManager.GetExceptionMessage(ex);
                                //result.FailedItems.Add(resultitem);
                            }
                        }
                    }
                }
                //}

            }


            return result;

        }
        //catch (Exception ex)
        //{
        //    throw ex;
        //}

        public ColumnValueRangeDTO GetColumnValueRange(int columnID)
        {
            ColumnValueRangeDTO result = new ColumnValueRangeDTO();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var column = projectContext.Column.First(x => x.ID == columnID);
                using (SqlConnection testConn = new SqlConnection(Database.ConnectionString))
                {
                    testConn.Open();
                    string commandStr = @"select " + column.Name + " from " + (string.IsNullOrEmpty(column.Table.DBSchema.Name) ? "" : column.Table.DBSchema.Name + ".") + column.Table.Name;
                    using (SqlCommand command = new SqlCommand(commandStr, testConn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string value = "";
                                bool isrepeated = false;
                                if (reader[column.Name] == DBNull.Value)
                                    value = null;
                                else
                                    value = reader[column.Name].ToString();
                                //if (valueFromTitle)
                                //{
                                //    if (result.Details.Any(x => x.KeyTitle == value))
                                //        isrepeated = true;
                                //}
                                //else
                                //{
                                if (value == null)
                                {
                                    if (result.Details.Any(x => x.Value == null))
                                        isrepeated = true;
                                }
                                else
                                {
                                    if (result.Details.Any(x => x.Value == value))
                                        isrepeated = true;
                                }
                                //}

                                if (!isrepeated)
                                    result.Details.Add(new ColumnValueRangeDetailsDTO() { Value = value, KeyTitle = "" });

                            }
                        }
                    }
                }
                //try
                //{
                //   projectContext.SaveChanges();
                //}
                //catch (System.Data.Entity.Validation.DbEntityValidationException e)
                //{
                //    foreach (var eve in e.EntityValidationErrors)
                //    {
                //        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                //            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                //        foreach (var ve in eve.ValidationErrors)
                //        {
                //            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                //                ve.PropertyName, ve.ErrorMessage);
                //        }
                //    }
                //    throw;
                //}
            }
            return result;
            // return true;
        }

    }



    //public class GenericResult<T>
    //{
    //    public T Result { set; get; }
    //    public BaseResult BaseResult { set; get; }
    //    public GenericResult()
    //    {
    //        BaseResult = new BaseResult();
    //    }
    //}
    //public class BaseResult
    //{
    //    public BaseResult()
    //    {
    //        Messages = new List<string>();
    //    }
    //    public Guid ID { set; get; }
    //    //    public bool Result { set; get; }
    //    public bool Exception { set; get; }
    //    public List<string> Messages { set; get; }
    //}
    public class ImportResult
    {
        public ImportResult()
        {
            SuccessfulItems = new List<MyModelGenerator.ImportResultItem>();
            FailedItems = new List<MyModelGenerator.ImportResultItem>();
        }
        public List<ImportResultItem> SuccessfulItems { set; get; }
        public List<ImportResultItem> FailedItems { set; get; }
    }
    public class ImportResultItem
    {
        public string Title { set; get; }
        public string Message { set; get; }
    }

    //public class ImportTablesResult
    //{
    //    public ImportTablesResult()
    //    {

    //    }
    //    public New
    //}
    //public class ImportItem
    //{
    //    public string Name { set; get; }
    //    public int ID { set; get; }
    //}
    //public enum OperationResult
    //{
    //    Done = 1,
    //    Failed = 2
    //}

}
