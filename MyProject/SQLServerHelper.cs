//using CommonBusiness;

using DataAccess;
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

namespace MyProject
{
    class SQLServerHelper
    {
        public static string GetConnectionString(string serverName, string dbName, string userName, string password)
        {
            //Server=.;Database=ccc;User Id=sa;Password=1;MultipleActiveResultSets=True;
            var str = "Server={0};Database={1};User Id={2};Password={3};MultipleActiveResultSets=True;";
            return string.Format(str, serverName, dbName, userName, password);
        }
        public event EventHandler<SimpleGenerationInfoArg> ItemGenerationEvent;
        //public event EventHandler<SimpleGenerationInfoArg> RelationshipGenerationEvent;
        public bool GenerateTablesAndColumns(string connectionString, string serverName, string dbName)
        {

            //try
            //{


            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                bool dataBaseInforationExists = false;
                using (SqlConnection testConn = new SqlConnection(connectionString))
                {
                    testConn.Open();
                    int count = 0;
                    using (SqlCommand commandCount = new SqlCommand("SELECT count (*) FROM information_schema.tables where table_type='BASE TABLE'", testConn))
                    {
                        count = (int)commandCount.ExecuteScalar();
                    }
                    var counter = 0;
                    using (SqlCommand command = new SqlCommand("SELECT * FROM information_schema.tables where table_type='BASE TABLE'", testConn))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            counter++;
                            var tableName = reader["TABLE_Name"].ToString();

                            if (ItemGenerationEvent != null)
                                ItemGenerationEvent(this, new SimpleGenerationInfoArg() { Title = tableName, TotalProgressCount = count, CurrentProgress = counter });

                            var catalog = GeneralHelper.GetCatalogName(serverName, reader["TABLE_Catalog"].ToString());

                            if (!dataBaseInforationExists)
                                if (!projectContext.DatabaseInformation.Any(x => x.Name == catalog))
                                {
                                    projectContext.DatabaseInformation.Add(new DatabaseInformation() { Name = catalog, ConnectionString = connectionString });
                                    dataBaseInforationExists = true;
                                }
                                else
                                    dataBaseInforationExists = true;

                            Table table = projectContext.Table.Where(x => x.Name == tableName && x.Catalog == catalog).FirstOrDefault();
                            if (table == null)
                                table = new Table();

                            table.Name = tableName;
                            table.Catalog = catalog;
                            table.RelatedSchema = reader["TABLE_Schema"].ToString();


                            List<string> keyColumns = new List<string>();
                            using (SqlCommand commandFields = new SqlCommand("SELECT column_name FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE OBJECTPROPERTY(OBJECT_ID(CONSTRAINT_SCHEMA+'.'+constraint_name), 'IsPrimaryKey') = 1 AND table_name = '" + tableName + "'", testConn))
                            using (SqlDataReader readerFields = commandFields.ExecuteReader())
                            {
                                while (readerFields.Read())
                                {
                                    keyColumns.Add(readerFields["column_name"].ToString());
                                }
                            }

                            string queryColumns = "Select *, COLUMNPROPERTY(object_id(TABLE_SCHEMA+'.'+TABLE_NAME), COLUMN_NAME, 'IsIdentity') as IsIdentity From INFORMATION_SCHEMA.COLUMNS";
                            using (SqlCommand commandFields = new SqlCommand(queryColumns + " Where TABLE_Name = '" + tableName + "' ", testConn))
                            using (SqlDataReader readerFields = commandFields.ExecuteReader())
                            {
                                while (readerFields.Read())
                                {

                                    var columnName = readerFields["Column_Name"].ToString();
                                    Column column = table.Column.Where(x => x.Name.ToLower() == columnName.ToLower()).FirstOrDefault();

                                    if (column == null)
                                    {
                                        column = new Column();
                                        table.Column.Add(column);
                                    }

                                    column.Name = columnName;
                                    column.DataType = readerFields["DATA_TYPE"].ToString();
                                    column.PrimaryKey = keyColumns.Contains(column.Name);
                                    column.DataEntryEnabled = true;
                                    column.SearchEnabled = true;
                                    column.ViewEnabled = true;
                                    column.IsNull = readerFields["is_nullable"].ToString() == "YES";
                                    column.IsIdentity = readerFields["IsIdentity"].ToString() == "1";
                                    if (column.Position == null)
                                        column.Position = Convert.ToInt32(readerFields["ORDINAL_POSITION"].ToString());
                                    if (column.DefaultValue == null)
                                        column.DefaultValue = (readerFields["COLUMN_DEFAULT"] == null ? null : readerFields["COLUMN_DEFAULT"].ToString());

                                    if (IsStringType(column))
                                    {

                                        if (column.DateColumnType != null)
                                            column.DateColumnType = null;
                                        if (column.NumericColumnType != null)
                                            column.NumericColumnType = null;

                                        if (column.StringColumnType == null)
                                            column.StringColumnType = new StringColumnType();
                                        column.StringColumnType.MaxLength = (readerFields["CHARACTER_MAXIMUM_LENGTH"] == null ? 0 : Convert.ToInt32(readerFields["CHARACTER_MAXIMUM_LENGTH"]));
                                    }
                                    else if (IsNumericType(column))
                                    {

                                        if (column.DateColumnType != null)
                                            column.DateColumnType = null;
                                        if (column.StringColumnType != null)
                                            column.StringColumnType = null;

                                        if (column.NumericColumnType == null)
                                            column.NumericColumnType = new NumericColumnType();
                                        if (readerFields["NUMERIC_PRECISION"] != null)
                                            column.NumericColumnType.Precision = Convert.ToInt32(readerFields["NUMERIC_PRECISION"]);
                                        if (readerFields["NUMERIC_SCALE"] != null)
                                            column.NumericColumnType.Scale = Convert.ToInt32(readerFields["NUMERIC_SCALE"]);
                                    }
                                    else if (IsDateType(column))
                                    {

                                        if (column.StringColumnType != null)
                                            column.StringColumnType = null;
                                        if (column.NumericColumnType != null)
                                            column.NumericColumnType = null;

                                        if (column.DateColumnType == null)
                                            column.DateColumnType = new DateColumnType();
                                    }
                                    //table.Column.Add(column);
                                }
                            }
                            if (table.ID == 0)
                                projectContext.Table.Add(table);

                        }

                    }
                }
                projectContext.SaveChanges();
                if (ItemGenerationEvent != null)
                    ItemGenerationEvent(this, new SimpleGenerationInfoArg() { Title = "Operation is completed." });
                return true;
            }

            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        public bool GenerateDefaultEntities()
        {
            GenericResult<OperationResult> result = new GenericResult<OperationResult>();
            try
            {
                using (var projectContext = new DataAccess.MyProjectEntities())
                {
                    var list = projectContext.Table.Where(x => !x.TableDrivedEntity.Any(y => y.Criteria == "" || y.Criteria == null));
                    int count = list.Count();
                    var counter = 0;
                    foreach (var table in list)
                    {
                        TableDrivedEntity tdEntity = new TableDrivedEntity();
                        tdEntity.IndependentDataEntry = true;
                        tdEntity.Alias = table.Alias;
                        tdEntity.Name = table.Name;
                        table.TableDrivedEntity.Add(tdEntity);

                        counter++;
                        if (ItemGenerationEvent != null)
                            ItemGenerationEvent(this, new SimpleGenerationInfoArg() { Title = table.Name, TotalProgressCount = count, CurrentProgress = counter });

                    }
                    projectContext.SaveChanges();
                    if (ItemGenerationEvent != null)
                        ItemGenerationEvent(this, new SimpleGenerationInfoArg() { Title = "Operation is completed." });
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        private bool IsStringType(Column column)
        {
            return (column.DataType == "char" || column.DataType == "nvarchar" || column.DataType == "varchar" || column.DataType == "text");
        }
        private bool IsNumericType(Column column)
        {
            return (column.DataType == "bigint" || column.DataType == "numeric" || column.DataType == "smallint"
                || column.DataType == "decimal" || column.DataType == "smallmoney" || column.DataType == "int"
                || column.DataType == "tinyint" || column.DataType == "money");
        }
        private bool IsDateType(Column column)
        {
            return (column.DataType == "date" || column.DataType == "datetime");
        }
        public bool GenerateRelationships(string connectionString, string serverName, string dbName)
        {
            GenericResult<OperationResult> result = new GenericResult<OperationResult>();
            //try
            //{

            using (SqlConnection testConn = new SqlConnection(connectionString))
            {
                using (var projectContext = new DataAccess.MyProjectEntities())
                {
                    testConn.Open();
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

                    DataTable relationDataTable = null;
                    using (SqlCommand command = new SqlCommand(commandStr, testConn))
                    {
                        using (SqlDataReader dr = command.ExecuteReader())
                        {
                            relationDataTable = new DataTable();
                            relationDataTable.Load(dr);
                        }
                    }
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

                            var relationName = reader["Constraint_Name"].ToString();
                            var rows = FindRelationDataRow(relationDataTable, relationName);

                            counter++;
                            if (ItemGenerationEvent != null)
                                ItemGenerationEvent(this, new SimpleGenerationInfoArg() { Title = relationName, TotalProgressCount = count, CurrentProgress = counter });

                            string PKTable = reader["PK_Table"].ToString();

                            //  string PKColumn = reader["PK_Column"].ToString();
                            string FKTable = reader["FK_Table"].ToString();
                            // string FKColumn = reader["FK_Column"].ToString();
                            var catalogName = GeneralHelper.GetCatalogName(serverName, dbName);

                            //var pkColumn = projectContext.Column.Where(x => x.Table.Name == PKTable && x.Table.Catalog == catalogName && x.Name == PKColumn).FirstOrDefault();
                            //if (pkColumn == null)
                            //    throw (new Exception("Column " + PKColumn + " in " + PKTable + " is not found!"));

                            //var fkColumn = projectContext.Column.Where(x => x.Table.Name == FKTable && x.Table.Catalog == catalogName && x.Name == FKColumn).FirstOrDefault();
                            //if (fkColumn == null)
                            //    throw (new Exception("Column " + FKColumn + " in " + FKTable + " is not found!"));

                            var entity1 = projectContext.TableDrivedEntity.Where(x => x.Table.Name == PKTable && x.Table.Catalog == catalogName && (x.Criteria == null || x.Criteria == "")).FirstOrDefault();
                            if (entity1 == null)
                                throw (new Exception("There is no entity defined for table " + PKTable));

                            var entity2 = projectContext.TableDrivedEntity.Where(x => x.Table.Name == FKTable && x.Table.Catalog == catalogName && (x.Criteria == null || x.Criteria == "")).FirstOrDefault();
                            if (entity2 == null)
                                throw (new Exception("There is no entity defined for table " + FKTable));

                            var relations = projectContext.Relationship.Where(x => x.TableDrivedEntity.ID == entity1.ID && x.TableDrivedEntity1.ID == entity2.ID);
                            string PKColumns = "";
                            string FKColumns = "";
                            foreach (var row in rows)
                            {
                                string PKColumn = row["PK_Column"].ToString();
                                string FKColumn = row["FK_Column"].ToString();
                                PKColumns += (PKColumns == "" ? "" : ",") + PKColumn;
                                FKColumns += (FKColumns == "" ? "" : ",") + FKColumn;
                                var pkColumn = projectContext.Column.Where(x => x.Table.Name == PKTable && x.Table.Catalog == catalogName && x.Name == PKColumn).FirstOrDefault();
                                if (pkColumn == null)
                                    throw (new Exception("Column " + PKColumn + " in " + PKTable + " is not found!"));

                                var fkColumn = projectContext.Column.Where(x => x.Table.Name == FKTable && x.Table.Catalog == catalogName && x.Name == FKColumn).FirstOrDefault();
                                if (fkColumn == null)
                                    throw (new Exception("Column " + FKColumn + " in " + FKTable + " is not found!"));

                                relations = relations.Where(x => x.RelationshipColumns.Any(y => y.Column.Name == PKColumn && y.Column1.Name == FKColumn));
                            }

                            Relationship reverseRelation = null;
                            var relation = relations.FirstOrDefault();
                            if (relation != null)
                                reverseRelation = projectContext.Relationship.FirstOrDefault(x => x.RelationshipID == relation.ID);
                            if (relation == null)
                                relation = new Relationship();
                            if (reverseRelation == null)
                                reverseRelation = new Relationship();

                            relation.Name = relationName + "=" + "(PK)" + PKTable + "." + PKColumns + ">(FK)" + FKTable + "." + FKColumns;
                            reverseRelation.Relationship2 = relation;
                            reverseRelation.Name = relationName + "=(FK)" + FKTable + "." + FKColumns + ">" + "(PK)" + PKTable + "." + PKColumns;

                            if (relation.ID == 0)
                            {
                                relation.TableDrivedEntity = entity1;
                                relation.TableDrivedEntity1 = entity2;

                                reverseRelation.TableDrivedEntity1 = entity1;
                                reverseRelation.TableDrivedEntity = entity2;

                                foreach (var row in rows)
                                {
                                    string PKColumn = row["PK_Column"].ToString();
                                    string FKColumn = row["FK_Column"].ToString();
                                    var pkColumn = projectContext.Column.Where(x => x.Table.Name == PKTable && x.Table.Catalog == catalogName && x.Name == PKColumn).FirstOrDefault();
                                    var fkColumn = projectContext.Column.Where(x => x.Table.Name == FKTable && x.Table.Catalog == catalogName && x.Name == FKColumn).FirstOrDefault();

                                    relation.RelationshipColumns.Add(new RelationshipColumns() { Column = pkColumn, Column1 = fkColumn });
                                    reverseRelation.RelationshipColumns.Add(new RelationshipColumns() { Column = fkColumn, Column1 = pkColumn });
                                }
                            }
                            if (relation.ID == 0)
                            {
                                projectContext.Relationship.Add(relation);
                                projectContext.Relationship.Add(reverseRelation);
                            }
                        }

                    }
                    try
                    {
                        // Your code...
                        // Could also be before try if you know the exception occurs in SaveChanges
                        if (ItemGenerationEvent != null)
                            ItemGenerationEvent(this, new SimpleGenerationInfoArg() { Title = "Operation is completed." });
                        projectContext.SaveChanges();
                        return true;
                    }
                    catch (System.Data.Entity.Validation.DbEntityValidationException e)
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

        private List<DataRow> FindRelationDataRow(DataTable relationDataTable, string relationName)
        {
            List<DataRow> result = new List<DataRow>();
            foreach (DataRow row in relationDataTable.Rows)
            {
                if (row != null)
                    if (row["Constraint_Name"].ToString() == relationName)
                        result.Add(row);
            }
            return result;

        }
        //        internal bool CheckRelationTypesExits(string connectionString, string serverName, string dbName)
        //                {
        //                    try
        //                    {
        //                        using (IdeaEntities context = new IdeaEntities())
        //                        {
        //                            using (SqlConnection testConn = new SqlConnection(connectionString))
        //                            {
        //                                testConn.Open();
        //                                string commandStr=@"SELECT FK_Table = FK.TABLE_NAME,FK_Column = CU.COLUMN_NAME,PK_Table = PK.TABLE_NAME,PK_Column = PT.COLUMN_NAME,
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

        //                                using (SqlCommand command = new SqlCommand(commandStr, testConn))
        //                                using (SqlDataReader reader = command.ExecuteReader())
        //                                {
        //                                    while (reader.Read())
        //                                    {
        //                                        BizType

        //                                    }
        //                                }
        //                            }

        //                            return true;
        //                        }
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        throw (ex);
        //                    }
        //                }


        //public GenericResult<OperationResult> GenerateDefaultPackages()
        //{
        //    GenericResult<OperationResult> result = new GenericResult<OperationResult>();
        //    try
        //    {
        //        BizType bizType = new BizType();
        //        Dictionary<string, string> typeCategories = new Dictionary<string, string>();
        //        typeCategories.Add(SQLServerSetting.SQLGlobalVariables.TypeCategory.ServerName, serverName);
        //        typeCategories.Add(SQLServerSetting.SQLGlobalVariables.TypeCategory.DatabaseName, dbName);
        //        var typeList = bizType.GetTypes(typeCategories);
        //        foreach (var item in typeList)
        //        {

        //        }
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //در اسکیوئل یونیک کانسترنت و یونیک ایندکس درایم که خیلی متفاوت از هم نیستن.برای هر یونیک کانسترنت یک یونیک ایندکس میسازد
        internal bool GenerateUniqueConstraints(string connectionString, string serverName, string dbName)
        { //try
            //{

            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                using (SqlConnection testConn = new SqlConnection(connectionString))
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
                                var tableName = reader["TableName"].ToString();
                                var catalogName = GeneralHelper.GetCatalogName(serverName, dbName); ;
                                var columnName = reader["ColumnName"].ToString();

                                counter++;
                                if (ItemGenerationEvent != null)
                                    ItemGenerationEvent(this, new SimpleGenerationInfoArg() { Title = tableName, TotalProgressCount = count, CurrentProgress = counter });

                                Table table = projectContext.Table.Where(x => x.Name == tableName && x.Catalog == catalogName).FirstOrDefault();
                                Column column = projectContext.Column.Where(x => x.Table.Name == tableName && x.Table.Catalog == catalogName && x.Name == columnName).FirstOrDefault();
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

                            }

                        }
                    }
                }
                projectContext.SaveChanges();
                if (ItemGenerationEvent != null)
                    ItemGenerationEvent(this, new SimpleGenerationInfoArg() { Title = "Operation is completed." });
                return true;
            }
            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }
        }

        internal bool GenerateKeyColumns(string connectionString, int columnID, bool valueFromKey)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var column = projectContext.Column.First(x => x.ID == columnID);
                using (SqlConnection testConn = new SqlConnection(connectionString))
                {
                    testConn.Open();
                    string commandStr = @"select " + column.Name + " from " + (string.IsNullOrEmpty(column.Table.RelatedSchema) ? "" : column.Table.RelatedSchema + ".") + column.Table.Name;

                    if (column.ColumnKeyValue == null)
                        column.ColumnKeyValue = new ColumnKeyValue();

                    column.ColumnKeyValue.ValueFromKeyOrValue = valueFromKey;

                    while (column.ColumnKeyValue.ColumnKeyValueRange.Any())
                        projectContext.ColumnKeyValueRange.Remove(column.ColumnKeyValue.ColumnKeyValueRange.First());

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

                                if (valueFromKey)
                                {
                                    if (column.ColumnKeyValue.ColumnKeyValueRange.Any(x => x.KeyTitle == value))
                                        isrepeated = true;
                                }
                                else
                                {
                                    if (value == null)
                                    {
                                        if (column.ColumnKeyValue.ColumnKeyValueRange.Any(x => x.Value == null))
                                            isrepeated = true;
                                    }
                                    else
                                    {
                                        int val = 0;
                                        if (Int32.TryParse(value, out val))
                                        {
                                            if (column.ColumnKeyValue.ColumnKeyValueRange.Any(x => x.Value == val))
                                                isrepeated = true;
                                        }
                                    }
                                }

                                if (!isrepeated)
                                    if (valueFromKey)
                                        column.ColumnKeyValue.ColumnKeyValueRange.Add(new ColumnKeyValueRange() { Value = null, KeyTitle = value });
                                    else
                                    {
                                        int val = 0;
                                        if (Int32.TryParse(value, out val))
                                        {
                                            column.ColumnKeyValue.ColumnKeyValueRange.Add(new ColumnKeyValueRange() { Value = val, KeyTitle = "" });
                                        }
                                    }
                            }
                        }
                    }
                }
                //try
                //{
                projectContext.SaveChanges();
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
            return true;
        }
    }


    class SimpleGenerationInfoArg : EventArgs
    {
        public int TotalProgressCount;
        public int CurrentProgress;
        public string Title;
    }

    public class GenericResult<T>
    {
        public T Result { set; get; }
        public BaseResult BaseResult { set; get; }
        public GenericResult()
        {
            BaseResult = new BaseResult();
        }
    }
    public class BaseResult
    {
        public BaseResult()
        {
            Messages = new List<string>();
        }
        public Guid ID { set; get; }
        //    public bool Result { set; get; }
        public bool Exception { set; get; }
        public List<string> Messages { set; get; }
    }

    public enum OperationResult
    {
        Done = 1,
        Failed = 2
    }

}
