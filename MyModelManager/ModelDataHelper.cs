using DataAccess;
using ModelEntites;
using MyConnectionManager;
using MyModelManager;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MyModelManager
{
    public class ModelDataHelper
    {
        //public List<DataAccess.Column> GetColumnList(TableDrivedEntity template)
        //{
        //    if (template.Column == null || template.Column.Count == 0)
        //    {
        //        return template.Table.Column.ToList();
        //    }
        //    else
        //        return template.Column.ToList();
        //}
        BizTableDrivedEntity bizEntity = new BizTableDrivedEntity();
        BizRelationship bizRelationship = new BizRelationship();
        ConnectionManager connectionManager = new ConnectionManager();

        //بهتر نوشته بشه.خروجی تغییر کنه و ریلیشن تایپ رو نیز در نظر بگیره
        //public EntityRelationInfo GetEntityRelationshipsInfo(int entityID, bool asPrimary, bool asForeign)
        //{
        //    EntityRelationInfo result = new EntityRelationInfo();
        //    result.TableDrivedEntity = bizEntity.GetTableDrivedEntity(entityID, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithoutRelationships);
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var entity = projectContext.TableDrivedEntity.First(x => x.ID == entityID);
        //        if (asPrimary)
        //        {
        //            foreach (var relationship in entity.Relationship.Where(x => x.Relationship2 == null && x.RelationshipType == null))
        //            {
        //                var relationInfo = GetOtherSideRelationshipInfo(relationship);
        //                result.RelationInfos.Add(relationInfo);
        //            }
        //        }
        //        if (asForeign)
        //        {
        //            foreach (var relationship in entity.Relationship.Where(x => x.Relationship2 != null && x.RelationshipType == null))
        //            {
        //                var relationInfo = GetOtherSideRelationshipInfo(relationship);
        //                result.RelationInfos.Add(relationInfo);
        //            }
        //        }
        //    }
        //    return result;
        //}

        //باید روابط اصلی به فرعی فرستاده شود
        //public RelationInfo GetRelationshipsInfo(int relationshipID)
        //{
        //    //using (var projectContext = new DataAccess.MyProjectEntities())
        //    //{
        //    var relationship = bizRelationship.GetRelationship(relationshipID);
        //    return GetRelationshipsInfoWithEntityIds(relationship);
        //    //}
        //}

        public bool CountDataIsMore(int databaseID, TableDrivedEntityDTO entity, int limitCount)
        {
            var commandStr = GetSingleEntityBaseSelectFromQuery(entity, "", "top" + " " + limitCount + 1);
            //   string commandStr = @"select top" + " " + limitCount + 1 + " " + "1 from " + (string.IsNullOrEmpty(entity.RelatedSchema) ? "" : entity.RelatedSchema + ".") + entity.Name;
            var fkDBHelper = ConnectionManager.GetDBHelper(databaseID);
            var countData = (int?)fkDBHelper.ExecuteScalar(commandStr);
            if (countData != null && countData.Value > limitCount)
                return true;
            else
                return false;
        }

        //public RelationInfo GetRelationshipsInfo(RelationshipDTO relationship)
        //{
        //    return GetOtherSideRelationshipInfo(relationship);

        //}
        //public RelationInfo GetRelationshipsInfoWithEntityNames(int databaseID, RelationshipDTO relationship)
        //{
        //    TableDrivedEntityDTO pkEntity = null;
        //    TableDrivedEntityDTO fkEntity = null;
        //    pkEntity = bizEntity.GetTableDrivedEntity(databaseID, relationship.Entity1, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithoutRelationships);
        //    fkEntity = bizEntity.GetTableDrivedEntity(databaseID, relationship.Entity2, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithoutRelationships);
        //    //foreach(var item in relationship.RelationshipColumns)
        //    //{
        //    //    var 
        //    //}
        //    return GetRelationshipsInfo(relationship, pkEntity, fkEntity);

        //}
        public RelationInfo GetRelationshipsInfoWithEntityIds(DR_Requester requester, RelationshipDTO relationship)
        {
            TableDrivedEntityDTO pkEntity = null;
            TableDrivedEntityDTO fkEntity = null;
            pkEntity = bizEntity.GetTableDrivedEntity(requester, relationship.EntityID1, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithoutRelationships);
            fkEntity = bizEntity.GetTableDrivedEntity(requester, relationship.EntityID2, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithoutRelationships);
            return GetRelationshipsInfo(relationship, pkEntity, fkEntity);
        }

        public RelationInfo GetRelationshipsInfo(RelationshipDTO relationship, TableDrivedEntityDTO pkEntity, TableDrivedEntityDTO fkEntity)
        {
            var relationInfo = new RelationInfo();
            //BizRelationship bizRelationship = new BizRelationship();
            ////RelationshipDTO relationshipDTO = bizRelationship.ToRelationshipDTO(relationship);
            //if (relationship.Relationship2 == null)
            //{
            //    relationIsFromPkToFK = true;
            //    PKEntity = relationship.TableDrivedEntity;
            //    FKEntity = relationship.TableDrivedEntity1;
            //}
            //else
            //{
            //    relationIsFromPkToFK = false;
            //    PKEntity = relationship.TableDrivedEntity1;
            //    FKEntity = relationship.TableDrivedEntity;
            //}

            //relationInfo.FKCoumnIsNullable = relationship.RelationshipColumns.All(x => x.SecondSideColumn.IsNull);
            //if (relationInfo.FKRelatesOnPrimaryKey)
            //    relationInfo.RelationType = RelationType.OnePKtoOneFK;
            //if (!relationInfo.FKCoumnIsNullable)
            //{
            //    relationInfo.AllFKSidesHavePKSide = true;
            //}
            //relationInfo.IsPrecise = database.DBHasData;
            relationInfo.FKHasData = FKEntityHasDataQuery(fkEntity, relationship);


            if (relationInfo.FKHasData == true)
            {
                relationInfo.AllFKSidesHavePKSide = FKEntityHasNotNullDataQuery(fkEntity, relationship);
                relationInfo.AllPrimarySideHasFkSideData = PKEntityExistWithoutRelationQuerty(pkEntity, fkEntity, relationship);
                relationInfo.MoreThanOneFkForEachPK = FKMoreThanOnceQuery(fkEntity, relationship);
                //relationInfo.RelationType = fkMoreThanOnceQuery ? RelationType.OnePKtoManyFK : RelationType.OnePKtoOneFK;
            }















            //if (hasManyFkSides == true)
            //{
            //    if (relationIsFromPkToFK == true)
            //        relationInfo.RelationType = RelationType.OnePKtoManyFK;
            //    else if (relationIsFromPkToFK == false)
            //        relationInfo.RelationType = RelationType.ManyFKtoOnePK;
            //}
            //else
            //{
            //    if (relationIsFromPkToFK == true)
            //        relationInfo.RelationType = RelationType.OnePKtoOneFK;
            //    else if (relationIsFromPkToFK == false)
            //        relationInfo.RelationType = RelationType.OneFKtoOnePK;
            //}




            return relationInfo;
            //}
            //catch (Exception ex) //error occurred
            //{
            //    throw (ex);
            //}

            //}
        }




        public QueryDepthOptions GetQueryDepthOptions(TableDrivedEntityDTO mainEntity, TableDrivedEntityDTO targetEntity)
        {
            QueryDepthOptions result = new QueryDepthOptions();


            result.LinkedServer = "";
            //result.SecondSideLinkedServer = "";
            if (mainEntity.ServerID != targetEntity.ServerID)
            {

                BizDatabase bizDatabase = new MyModelManager.BizDatabase();

                //if (defaulConnectionOnFirstSide == null)
                //    defaulConnectionOnFirstSide = true;

                //if (defaulConnectionOnFirstSide == true)
                //{
                var linkedServer = bizDatabase.GetLinkedServer(mainEntity.ServerID, targetEntity.ServerID);
                if (linkedServer != null)
                {
                    result.LinkedServer = linkedServer.Name;
                    result.DatabaseName = true;
                    result.SchemaName = true;
                }
                else
                {
                    throw new Exception("Lenked Server from " + mainEntity.Name + " " + "to" + " " + targetEntity.Name + " not found!");
                    return null;
                }
                //}
                //else if (defaulConnectionOnFirstSide == false)
                //{
                //    linkedServer = bizDatabase.GetLinkedServer(targetEntity.ServerID, mainEntity.ServerID);
                //    result.ConnectionTarget = QueryDeterminerConnection.SecondSide;

                //}
                //if (linkedServer != null)
                //    result.LinkedServer = linkedServer.Name;
                //else
                //    return null;


            }
            else
            {
                if (mainEntity.DatabaseID != targetEntity.DatabaseID)
                {
                    result.DatabaseName = true;
                    result.SchemaName = true;
                }
                else
                    result.SchemaName = true;
            }
            return result;
        }

        //private bool FKColumnIsNull(TableDrivedEntity FKEntity, Relationship relationship, bool? relationIsFromPkToFK)
        //{
        //    var columns2 = GetFKRelationshipColumns(FKEntity, relationship, relationIsFromPkToFK);
        //    foreach (var column in columns2)
        //    {
        //        if (column.IsNull == true)
        //            return true;
        //    }
        //    return false;
        //}

        //private IEnumerable<Column> GetFKRelationshipColumns(TableDrivedEntity FKEntity, Relationship relationship, bool? relationIsFromPkToFK)
        //{
        //    var columns2 = FKEntity.Table.Column as IEnumerable<Column>;

        //    if (relationIsFromPkToFK == true)
        //        columns2 = columns2.Where(x => relationship.RelationshipColumns.Any(y => x.ID == y.SecondSideColumnID));
        //    else if (relationIsFromPkToFK == false)
        //        columns2 = columns2.Where(x => relationship.RelationshipColumns.Any(y => x.ID == y.FirstSideColumnID));
        //    return columns2;
        //}



        //private IEnumerable<Column> GetPKRelationshipColumns(TableDrivedEntity PKEntity, Relationship relationship, bool? relationIsFromPkToFK)
        //{
        //    var columns2 = PKEntity.Table.Column as IEnumerable<Column>;
        //    if (relationIsFromPkToFK == true)
        //        columns2 = columns2.Where(x => relationship.RelationshipColumns.Any(y => x.ID == y.FirstSideColumnID));
        //    else if (relationIsFromPkToFK == false)
        //        columns2 = columns2.Where(x => relationship.RelationshipColumns.Any(y => x.ID == y.SecondSideColumnID));
        //    return columns2;
        //}

        // سنجش رابطه

        private bool FKEntityHasDataQuery(TableDrivedEntityDTO fkEntity, RelationshipDTO relationship)
        {
            var fkDBHelper = ConnectionManager.GetDBHelper(fkEntity.DatabaseID);
            var condition = "";
            foreach (var relcolumn in relationship.RelationshipColumns)
            {
                // if (relcolumn.FirstSideColumnID != null)
                condition += (condition == "" ? "" : " and ") + relcolumn.SecondSideColumn.Name + " is not null";
                //else
                //    condition += (condition == "" ? "" : " and ") + (relcolumn.SecondSideColumn.Name + " = " + "'" + relcolumn.PrimarySideFixedValue + "'");
            }
            var fkEntityBaseSelectFromQuery = GetSingleEntityBaseSelectFromQuery(fkEntity);
            var query = fkEntityBaseSelectFromQuery + (fkEntityBaseSelectFromQuery.Contains(" where ") ? " and " : " where ") + condition;
            var res1 = fkDBHelper.ExecuteScalar(query);
            return Convert.ToInt32(res1) != 0;
        }
        private bool EntityHasData(TableDrivedEntityDTO entity)
        {
            var fkDBHelper = ConnectionManager.GetDBHelper(entity.DatabaseID);

            var query = GetSingleEntityBaseSelectFromQuery(entity);
       
            var res1 = fkDBHelper.ExecuteScalar(query);
            return Convert.ToInt32(res1) != 0;
        }
        private bool FKEntityHasNotNullDataQuery(TableDrivedEntityDTO fkEntity, RelationshipDTO relationship)
        {
            var fkDBHelper = ConnectionManager.GetDBHelper(fkEntity.DatabaseID);
            var condition = "";
            foreach (var relcolumn in relationship.RelationshipColumns)
            {
                condition += (condition == "" ? "" : " and ") + relcolumn.SecondSideColumn.Name + " is null";
            }
            var fkEntityBaseSelectFromQuery = GetSingleEntityBaseSelectFromQuery(fkEntity);
            var query = fkEntityBaseSelectFromQuery + (fkEntityBaseSelectFromQuery.Contains(" where ") ? " and " : " where ") + condition;
            var resCheckFKIsNull = fkDBHelper.ExecuteScalar(query);
            return (Convert.ToInt32(resCheckFKIsNull) == 0);
        }
        private bool PKEntityExistWithoutRelationQuerty(TableDrivedEntityDTO pkEntity, TableDrivedEntityDTO fkEntity, RelationshipDTO relationship)
        {

            TableDrivedEntityDTO mainEntity = null;
            TableDrivedEntityDTO relatedEntity = null;
            if (pkEntity.ServerID != fkEntity.ServerID)
            {
                BizDatabase bizDatabase = new BizDatabase();
                if (bizDatabase.LinkedServerExists(pkEntity.ServerID, fkEntity.ServerID))
                {
                    mainEntity = pkEntity;
                    relatedEntity = fkEntity;
                }
                else if (bizDatabase.LinkedServerExists(fkEntity.ServerID, pkEntity.ServerID))
                {
                    mainEntity = fkEntity;
                    relatedEntity = pkEntity;
                }
                else
                {
                    throw (new Exception("sdfsdf1"));
                }
            }
            else
            {
                mainEntity = pkEntity;
                relatedEntity = fkEntity;
            }
            I_DBHelper dbHelper = ConnectionManager.GetDBHelper(mainEntity.DatabaseID);
            var pkEntityBaseSelectFromQuery = GetRelationEntityBaseSelectFromQuery(mainEntity, pkEntity, "pk", "");
            var fkEntityBaseSelectFromQuery = GetRelationEntityBaseSelectFromQuery(mainEntity, fkEntity, "fk", "");

            var condition = "";
            foreach (var relcolumn in relationship.RelationshipColumns)
            {
                //if (relcolumn.FirstSideColumnID != null)
                condition += (condition == "" ? "" : " and ") + "pk." + relcolumn.FirstSideColumn.Name + "="
                    + "fk." + relcolumn.SecondSideColumn.Name;
                //else
                //    condition += (condition == "" ? "" : " and ") + "fk." + relcolumn.SecondSideColumn.Name + "='" + relcolumn.PrimarySideFixedValue + "'";
            }

            var query = pkEntityBaseSelectFromQuery + (pkEntityBaseSelectFromQuery.Contains(" where ") ? " and " : " where ") + "not exists (" + fkEntityBaseSelectFromQuery + (fkEntityBaseSelectFromQuery.Contains(" where ") ? " and " : " where ") + condition + ")";
            var resCheckExists = dbHelper.ExecuteScalar(query);
            return (Convert.ToInt32(resCheckExists) == 0);
        }
        public long GetRelationDataCount(TableDrivedEntityDTO fkEntity, RelationshipDTO relationship)
        {
            var fkDBHelper = ConnectionManager.GetDBHelper(relationship.DatabaseID2);
            var condition = "";
            foreach (var relcolumn in relationship.RelationshipColumns)
            {
                // if (relcolumn.FirstSideColumnID != null)
                condition += (condition == "" ? "" : " and ") + relcolumn.SecondSideColumn.Name + " is not null";
                //else
                //    condition += (condition == "" ? "" : " and ") + (relcolumn.SecondSideColumn.Name + " = " + "'" + relcolumn.PrimarySideFixedValue + "'");
            }
            var fkEntityBaseSelectFromQuery = GetSingleEntityBaseSelectFromQuery(fkEntity, "", "Count(1)");
            var query = fkEntityBaseSelectFromQuery + (fkEntityBaseSelectFromQuery.Contains(" where ") ? " and " : " where ") + condition;
            var val = fkDBHelper.ExecuteScalar(query);
            if (val == null)
                return 0;
            else
                return Convert.ToInt64(val);
        }
        public long GetDataCount(TableDrivedEntityDTO fkEntity)
        {
            var fkDBHelper = ConnectionManager.GetDBHelper(fkEntity.DatabaseID);
            var fkEntityBaseSelectFromQuery = GetSingleEntityBaseSelectFromQuery(fkEntity, "", "Count(1)");
            var val = fkDBHelper.ExecuteScalar(fkEntityBaseSelectFromQuery);
            if (val == null)
                return 0;
            else
                return Convert.ToInt64(val);
        }

        private bool FKMoreThanOnceQuery(TableDrivedEntityDTO fkEntity, RelationshipDTO relationship)
        {

            var fkDBHelper = ConnectionManager.GetDBHelper(fkEntity.DatabaseID);
            var fkEntityBaseSelectFromQuery = GetSingleEntityBaseSelectFromQuery(fkEntity);
            var condition = "";
            var groupBy = "";
            foreach (var relcolumn in relationship.RelationshipColumns)
            {
                groupBy += (groupBy == "" ? "" : ",") + relcolumn.SecondSideColumn.Name;
                condition += (condition == "" ? "" : " and ") + relcolumn.SecondSideColumn.Name + " is not null";
            }

            var query = fkEntityBaseSelectFromQuery + (fkEntityBaseSelectFromQuery.Contains(" where ") ? " and " : " where ") + condition
                + " group by " + groupBy + " having count(*)>1";
            var res = fkDBHelper.ExecuteScalar(query);
            return Convert.ToInt32(res) > 0;
        }



        /// ////////////////////////////////////////////////////

        ////////////////////ارث بری
        public ISARelationshipDetail GetISARelationshipDetail(DR_Requester requester, List<RelationshipDTO> relationships)
        {
            if (relationships.Any(x => x.DatabaseID1 != x.DatabaseID2))
                return null;
            ISARelationshipDetail result = new ISARelationshipDetail();
            string subTypesStr = "";
            foreach (var rel in relationships)
            {
                subTypesStr += (subTypesStr == "" ? "" : ",") + rel.Entity2;
            }
            result.Name = relationships.First().Entity1 + ">" + subTypesStr;

            var pkEntity = bizEntity.GetTableDrivedEntity(requester, relationships.First().EntityID1, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithoutRelationships);

            if (EntityHasData(pkEntity))
            {
                var pkDBHelper = ConnectionManager.GetDBHelper(relationships.First().DatabaseID1);
                var participationQuery = ISATolatParticipationQuery(requester, relationships);
                var resCheckExists = pkDBHelper.ExecuteScalar(participationQuery);
                if (Convert.ToInt32(resCheckExists) > 0)
                    result.IsTotalParticipation = false;
                else
                    result.IsTotalParticipation = true;

                string commandExistInBoth = ISADisjoinQuery(requester, relationships);
                //pkDBHelper.CommandTimeout = 60;
                var resExistInBoth = pkDBHelper.ExecuteScalar(commandExistInBoth);
                if (Convert.ToInt32(resExistInBoth) > 0)
                    result.IsDisjoint = false;
                else
                    result.IsDisjoint = true;
            }

            return result;
        }
        private string ISATolatParticipationQuery(DR_Requester requester, List<RelationshipDTO> relationships)
        {
            //باید روابط واقعی و در سطح یک پایگاه داده باشند
            //منظور از واقعی اینه که با primaryfixedvalue نباید باشد
            //اونها خودشون بصورت دستی ایجاد و نعیین نوع رابطه میشوند

            var pkEntity = bizEntity.GetTableDrivedEntity(requester, relationships.First().EntityID1, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithoutRelationships);
            var superTypeBaseSelectFromQuery = GetSingleEntityBaseSelectFromQuery(pkEntity, "pk", "");
            var whereClaus = "";
            foreach (var relationship in relationships)
            {
                var fkEntity = bizEntity.GetTableDrivedEntity(requester, relationship.EntityID2, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithoutRelationships);
                var condition = "";
                foreach (var column in relationship.RelationshipColumns)
                {
                    var columnEqual1 = "pk." + column.FirstSideColumn.Name
                                      + "=" + "fk." + column.SecondSideColumn.Name;
                    condition += (condition == "" ? "" : " and ") + columnEqual1;
                }
                var subTypeBaseSelectFromQuery = GetSingleEntityBaseSelectFromQuery(fkEntity, "fk", "");
                whereClaus += (whereClaus == "" ? "" : " and ") + "not exists (" + subTypeBaseSelectFromQuery + (subTypeBaseSelectFromQuery.Contains(" where ") ? " and " : " where ") + condition + ")";
            }
            return superTypeBaseSelectFromQuery + (superTypeBaseSelectFromQuery.Contains(" where ") ? " and " : " where ") + whereClaus;
        }

        private string ISADisjoinQuery(DR_Requester requester, List<RelationshipDTO> relationships)
        {
            var pkEntity = bizEntity.GetTableDrivedEntity(requester, relationships.First().EntityID1, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithoutRelationships);

            var superTypeBaseSelectFromQuery = GetSingleEntityBaseSelectFromQuery(pkEntity);
            var UnionQuery = "";
            foreach (var relationship in relationships)
            {
                var fkEntity = bizEntity.GetTableDrivedEntity(requester, relationship.EntityID2, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithoutRelationships);
                int col = 0;
                string unionColumns = "";
                foreach (var relcolumn in relationship.RelationshipColumns)
                {
                    col++;
                    unionColumns += (unionColumns == "" ? "" : ",") + relcolumn.SecondSideColumn.Name + " as col" + col;
                }
                var subTypeBaseSelectFromQuery = GetSingleEntityBaseSelectFromQuery(fkEntity, "", unionColumns);
                UnionQuery += (UnionQuery == "" ? "" : " Union All ") + subTypeBaseSelectFromQuery;
            }

            var sampleSuperRelationship = relationships.First();
            var groupBy = "";
            int col1 = 0;
            string columnEqual = "";
            foreach (var relcolumn in sampleSuperRelationship.RelationshipColumns)
            {
                col1++;
                columnEqual = (columnEqual == "" ? "" : ",") + relcolumn.FirstSideColumn.Name
                            + "=" + "col" + col1;
                groupBy += (groupBy == "" ? "" : ",") + relcolumn.FirstSideColumn.Name;
            }

            Tuple<string, string> splitedQuery = splitQuery(superTypeBaseSelectFromQuery);

            return splitedQuery.Item1 + " inner join (" + UnionQuery + ") as subUnions on " + columnEqual + (splitedQuery.Item2 == "" ? "" : " where " + splitedQuery.Item2) + " group by " + groupBy + " having count(*)>1";
        }


        /////////////اتحاد 

        public UnionRelationshipDetail GetUnionRelationshipDetail(DR_Requester requester, List<RelationshipDTO> relationships)
        {
            if (relationships.Any(x => x.DatabaseID1 != x.DatabaseID2))
                return null;
            UnionRelationshipDetail result = new UnionRelationshipDetail();

            var superType = relationships.First().Entity2;

            string subTypesStr = "";
            foreach (var rel in relationships)
            {
                subTypesStr += (subTypesStr == "" ? "" : ",") + rel.Entity1;
            }
            result.Name = superType + ">" + subTypesStr;
            var unionEntity = bizEntity.GetTableDrivedEntity(requester, relationships.First().EntityID2, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithoutRelationships);

            if (EntityHasData(unionEntity))
            {
                var pkDBHelper = ConnectionManager.GetDBHelper(relationships.First().DatabaseID1);
                result.IsTotalParticipation = UnionTolatParticipationQuery(requester, pkDBHelper, relationships);

            }
            //string commandExistInBoth = UnionDisjoinQuery(superType, relationships);
            ////pkDBHelper.CommandTimeout = 60;
            //var resExistInBoth = pkDBHelper.ExecuteScalar(commandExistInBoth);
            //if (Convert.ToInt32(resExistInBoth) > 0)
            //    result.IsDisjoint = false;
            //else
            //    result.IsDisjoint = true;

            return result;
        }
        private bool UnionTolatParticipationQuery(DR_Requester requester, I_DBHelper dbHelper, List<RelationshipDTO> relationships)
        {
            //باید روابط واقعی و در سطح یک پایگاه داده باشند
            //منظور از واقعی اینه که با primaryfixedvalue نباید باشد
            //اونها خودشون بصورت دستی ایجاد و نعیین نوع رابطه میشوند



            foreach (var relationship in relationships)
            {
                var pkEntity = bizEntity.GetTableDrivedEntity(requester, relationship.EntityID1, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithoutRelationships);
                var fkEntity = bizEntity.GetTableDrivedEntity(requester, relationship.EntityID2, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithoutRelationships);

                var condition = "";
                foreach (var column in relationship.RelationshipColumns)
                {
                    var columnEqual1 = "pk." + column.FirstSideColumn.Name
                                      + "=" + "fk." + column.SecondSideColumn.Name;
                    condition += (condition == "" ? "" : " and ") + columnEqual1;
                }
                var subTypeBaseSelectFromQuery = GetSingleEntityBaseSelectFromQuery(fkEntity, "fk", "");
                var whereClaus = "not exists (" + subTypeBaseSelectFromQuery + (subTypeBaseSelectFromQuery.Contains(" where ") ? " and " : " where ") + condition + ")";
                var pkBaseSelectFromQuery = GetSingleEntityBaseSelectFromQuery(pkEntity, "pk", "");
                var query = pkBaseSelectFromQuery + (pkBaseSelectFromQuery.Contains(" where ") ? " and " : " where ") + whereClaus;
                var resCheckExists = dbHelper.ExecuteScalar(query);
                if (Convert.ToInt32(resCheckExists) > 0)
                    return false;

            }
            return true;
            //var superTypeBaseSelectFromQuery = GetSingleEntityBaseSelectFromQuery(superType, "", "pk");
            //var whereClaus = "";
            //foreach (var subType in relationships)
            //{



            //    foreach (var column in subType.RelationshipColumns)
            //    {
            //        var columnEqual1 = "pk." + column.FirstSideColumn.Name + " is Null";
            //        whereClaus += (whereClaus == "" ? "" : " and ") + columnEqual1;
            //        //                  + "=" + "fk." + column.SecondSideColumn.Name;
            //        //condition += (condition == "" ? "" : " and ") + columnEqual1;
            //    }
            //    // var subTypeBaseSelectFromQuery = GetSingleEntityBaseSelectFromQuery(subType.Item2, "", "fk");
            //    // whereClaus += (whereClaus == "" ? "" : " and ") + "not exists (" + subTypeBaseSelectFromQuery + (subTypeBaseSelectFromQuery.Contains(" where ") ? " and " : " where ") + condition + ")";
            //}
            //return superTypeBaseSelectFromQuery + (superTypeBaseSelectFromQuery.Contains(" where ") ? " and " : " where ") + whereClaus;
        }

        //private string UnionDisjoinQuery(TableDrivedEntityDTO superType, List<RelationshipDTO> relationships)
        //{


        //    var UnionQuery = "";
        //    foreach (var subType in relationships)
        //    {
        //        string unionColumns = "";
        //        foreach (var pkColumn in superType.Columns.Where(x => x.PrimaryKey))
        //        {
        //            unionColumns += (unionColumns == "" ? "" : ",") + pkColumn.Name;
        //        }
        //        string whereColumns = "";
        //        foreach (var relcolumn in subType.RelationshipColumns)
        //        {
        //            whereColumns += (whereColumns == "" ? "" : " and ") + relcolumn.FirstSideColumn.Name + " is not NULL";
        //        }
        //        var unionAllSelectFromQuery = GetSingleEntityBaseSelectFromQuery(superType, "", "unAll", unionColumns);
        //        unionAllSelectFromQuery += (unionAllSelectFromQuery.Contains(" where ") ? " and " : " where ") + whereColumns;
        //        UnionQuery += (UnionQuery == "" ? "" : " Union All ") + unionAllSelectFromQuery;
        //    }

        //    var superTypeBaseSelectFromQuery = GetSingleEntityBaseSelectFromQuery(superType, "", "main");
        //    var groupBy = "";
        //    int col1 = 0;
        //    string columnEqual = "";
        //    foreach (var pkColumn in superType.Columns.Where(x => x.PrimaryKey))
        //    {
        //        col1++;
        //        columnEqual = (columnEqual == "" ? "" : ",") + "main." + pkColumn.Name
        //                    + "=" + "unAll." + pkColumn.Name;
        //        groupBy += (groupBy == "" ? "" : ",") + "main." + pkColumn.Name;
        //    }

        //    Tuple<string, string> splitedQuery = splitQuery(superTypeBaseSelectFromQuery);

        //    return splitedQuery.Item1 + " inner join (" + UnionQuery + ") as subUnions on " + columnEqual + (splitedQuery.Item2 == "" ? "" : " where " + splitedQuery.Item2) + " group by " + groupBy + " having count(*)>1";
        //}


        /// <summary>
        /// /////////////////////////////////////
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        //public string GetCriteria1(TableDrivedEntityDTO entity)
        //{
        //    return string.IsNullOrEmpty(entity.Criteria) ? "" : " where " + entity.Criteria;
        //}
        //public string GetCriteria(TableDrivedEntity entity)
        //{
        //    return string.IsNullOrEmpty(entity.Criteria) ? "" : " where " + entity.Criteria;
        //}
        //public string GetEntityBaseSelectFromQuery1(TableDrivedEntityDTO entity, bool isPK, string columns = "")
        //{
        //    var tableName = (string.IsNullOrEmpty(entity.RelatedSchema) ? "" : "[" + entity.RelatedSchema + "].") + "[" + entity.TableName + "]";
        //    var selectFrom = "";
        //    if (columns == "")
        //        selectFrom = "select top 1 1 from " + tableName;
        //    else
        //        selectFrom = "select " + columns + " from " + tableName;
        //    if (isPK)
        //        return selectFrom + " as pk";
        //    else
        //        return selectFrom + " as fk";
        //}
        public string GetRelationEntityBaseSelectFromQuery(TableDrivedEntityDTO connectionEntity, TableDrivedEntityDTO entity, string alias = "", string columns = "")
        {
            var depthOptions = GetQueryDepthOptions(connectionEntity, entity);
            // var tableName = GetTableNameFromDepthOption(entity, depthOptions);
            return GetSingleEntityBaseSelectFromQuery(entity, alias, columns, depthOptions);
        }

        public string GetSingleEntityBaseSelectFromQuery(TableDrivedEntityDTO entity, string alias = "", string columns = "", QueryDepthOptions queryDepthOptions = null)
        {
            //if (tableName == "")
            //    tableName = "[" + entity.TableName + "]";
            var selectFrom = "";
            string tableName = "";
            if (queryDepthOptions == null)
                tableName = (string.IsNullOrEmpty(entity.RelatedSchema) ? "" : "[" + entity.RelatedSchema + "]" + ".") + "[" + entity.TableName + "]";
            else
                tableName = GetTableNameFromDepthOption(entity, queryDepthOptions);
            if (columns == "")
                selectFrom = "select top 1 1 from " + tableName;
            else
                selectFrom = "select " + columns + " from " + tableName;

            if (alias != "")
                selectFrom += " as " + alias;

            //if (!string.IsNullOrEmpty(entity.Criteria))
            //{
            //    selectFrom += " where " + entity.Criteria;
            //    //?????    var fkEntityCriteria = GetCriteria(FKEntity);
            //}
            return selectFrom;
        }

        private string GetTableNameFromDepthOption(TableDrivedEntityDTO entity, QueryDepthOptions queryDepthOption)
        {
            var tableName = "";
            if (!string.IsNullOrEmpty(queryDepthOption.LinkedServer))
            {
                tableName += (tableName == "" ? "" : ".") + "[" + queryDepthOption.LinkedServer + "]";
            }
            if (queryDepthOption.DatabaseName)
                tableName += (tableName == "" ? "" : ".") + "[" + entity.DatabaseName + "]";
            if (queryDepthOption.SchemaName)
                tableName += (tableName == "" ? "" : ".") + "[" + entity.RelatedSchema + "]";

            tableName += (tableName == "" ? "" : ".") + "[" + entity.TableName + "]";
            return tableName;
        }





        ////////////////////////////////////جستجو

        public string GetOnClause(string parentTableAlias, string targetTableAlias, RelationshipDTO relationship)
        {
            var onClause = "";
            //var index = 0;
            foreach (var rCol in relationship.RelationshipColumns)
            {
                var ffield = "";
                var sfield = "";
                if (relationship.MastertTypeEnum == Enum_MasterRelationshipType.FromPrimartyToForeign)
                {

                    ffield = parentTableAlias + "." + rCol.FirstSideColumn.Name;
                    //}
                    //else
                    //    ffield = rCol.PrimarySideFixedValue;
                    sfield = targetTableAlias + "." + rCol.SecondSideColumn.Name;
                }
                else
                {
                    ffield = parentTableAlias + "." + rCol.FirstSideColumn.Name;
                    //if (rCol.SecondSideColumnID != null)
                    //{
                    sfield = targetTableAlias + "." + rCol.SecondSideColumn.Name;
                    //}
                    //else
                    //    sfield = rCol.PrimarySideFixedValue;
                }
                onClause += (onClause == "" ? "" : " and ") + ffield + "=" + sfield;

            }
            return onClause;
        }
        public Tuple<string, string> GetInnerjoinOnClause(string parentTableAlias, string targetTableAlias, RelationshipDTO relationship, TableDrivedEntityDTO connectionEntity, TableDrivedEntityDTO targetEntity)
        {

            var onClause = GetOnClause(parentTableAlias, targetTableAlias, relationship);
            var queryOption = GetQueryDepthOptions(connectionEntity, targetEntity);
            if (queryOption == null)
            {
                return null;
            }
            else
            {
                var tableName = GetTableNameFromDepthOption(targetEntity, queryOption);
                // var innerStr = " inner join " + tableName + " as " + targetTableAlias + " on " + onClause;
                return new Tuple<string, string>(tableName, onClause);
            }
        }

        //public string GetExistsClauseFull(string parentSearchTableName, RelationshipDTO relatinship,  string targetSearchTableName, TableDrivedEntityDTO targetEntity, bool positive)
        //{

        //    //var existsClause = GetExistsClause(parentSearchTableName, relatinship, targetSearchTableName, targetEntity, positive);
        //    //return (positive ? "Exists" : "Not Exists") + "(" + select + " where " +where + ")";
        //}
        //public string GetExistsClauseFull(string parentSearchTableName, RelationshipDTO relationship, string targetSearchTableName, TableDrivedEntityDTO connectionEntity, TableDrivedEntityDTO targetEntity, bool positive)
        //{
        //    //var queryOption = GetQueryDepthOptions(connectionEntity, targetEntity);
        //    //if (queryOption == null)
        //    //{
        //    //    return null;
        //    //}
        //    //else
        //    //{
        //    var existsselect = GetRelationEntityBaseSelectFromQuery(connectionEntity, targetEntity, targetSearchTableName, "");
        //    string existswhere = GetOnClause(parentSearchTableName, targetSearchTableName, relationship);
        //    return (positive ? "Exists" : "Not Exists") + "(" + existsselect + " where " + existswhere + ")";
        //    //}

        //}
        //public string GetForignKeyNullClause(string parentSearchTableName, List<RelationshipColumnDTO> relationshipColumns, TableDrivedEntityDTO parentEntity, bool positive)
        //{
        //    //ذاتا از فارن به پرایمری هست

        //    var resutl = "";
        //    foreach (var fCol in relationshipColumns)
        //    {
        //        var column = parentEntity.Columns.First(x => x.ID == fCol.FirstSideColumnID);
        //        //if (fCol.SecondSideColumnID != null)
        //        //{

        //        resutl += (resutl == "" ? "" : " and ") + parentSearchTableName + "." + column.Name + (positive ? " is not " : " is ") + " Null";
        //        // }
        //        //else
        //        //{//قشنگه
        //        //    if (positive)
        //        //        resutl += (resutl == "" ? "" : " and ") + parentSearchTableName + "." + column.Name + " = " + fCol.PrimarySideFixedValue;
        //        //    else
        //        //        resutl += (resutl == "" ? "" : " and ") + "(" + (parentSearchTableName + "." + column.Name + " <> " + fCol.PrimarySideFixedValue + " or " +
        //        //            parentSearchTableName + "." + column.Name + " is null") + ")";

        //        //    //اونایی که با یک پرایمری خاص از نوع فیکس رابطه ندارد
        //        //}

        //    }
        //    return resutl;
        //}



        //////////////////////////////


        /// <summary>
        /// ///////////////////////////
        /// </summary>
        /// <param name="relationship"></param>
        /// <returns></returns>
        public string GetKeyColumns(TableDrivedEntityDTO entity)
        {
            var result = "";
            foreach (var column in entity.Columns.Where(x => x.PrimaryKey == true))
            {
                result += (result == "" ? "" : ",") + column.Name;
            }
            return result;
        }

        //private bool GetFKRelatesOnPartOfPrimaryKey(TableDrivedEntity FKEntity, Relationship relationship, bool? relationIsFromPkToFK)
        //{
        //    var columns2 = FKEntity.Table.Column; //Helper.GetColumnList(FKEntity);
        //    if (relationIsFromPkToFK == true)
        //    {
        //        if (columns2.Where(x => x.PrimaryKey == true).Any(x =>
        //           relationship.TableDrivedEntity != relationship.TableDrivedEntity1 && relationship.RelationshipColumns.Any(y => x.ID == y.SecondSideColumnID)))
        //            if (!columns2.Where(x => x.PrimaryKey == true).All(x =>
        //               relationship.TableDrivedEntity != relationship.TableDrivedEntity1 && relationship.RelationshipColumns.Any(y => x.ID == y.SecondSideColumnID)))
        //                return true;


        //    }
        //    else if (relationIsFromPkToFK == false)
        //    {
        //        if (columns2.Where(x => x.PrimaryKey == true).Any(x =>
        //           relationship.TableDrivedEntity != relationship.TableDrivedEntity1 && relationship.RelationshipColumns.Any(y => x.ID == y.FirstSideColumnID)))
        //            if (!columns2.Where(x => x.PrimaryKey == true).All(x =>
        //               relationship.TableDrivedEntity != relationship.TableDrivedEntity1 && relationship.RelationshipColumns.Any(y => x.ID == y.FirstSideColumnID)))
        //                return true;
        //    }
        //    return false;
        //}


        //public void ClearRelationshipType(RelationshipType relationshipType)
        //{
        //    if (relationshipType.ManyToOneRelationshipType != null)
        //        relationshipType.ManyToOneRelationshipType = null;

        //    if (relationshipType.OneToManyRelationshipType != null)
        //        relationshipType.OneToManyRelationshipType = null;

        //    if (relationshipType.ExplicitOneToOneRelationshipType != null)
        //        relationshipType.ExplicitOneToOneRelationshipType = null;

        //    if (relationshipType.ImplicitOneToOneRelationshipType != null)
        //        relationshipType.ImplicitOneToOneRelationshipType = null;

        //    if (relationshipType.SuperToSubRelationshipType != null)
        //        relationshipType.SuperToSubRelationshipType = null;

        //    if (relationshipType.SubToSuperRelationshipType != null)
        //        relationshipType.SuperToSubRelationshipType = null;

        //    if (relationshipType.UnionToSubUnionRelationshipType != null)
        //        relationshipType.UnionToSubUnionRelationshipType = null;

        //    if (relationshipType.SubUnionToUnionRelationshipType != null)
        //        relationshipType.SubUnionToUnionRelationshipType = null;
        //}

        //public Relationship GetReverseRelationship(MyProjectEntities context, Relationship relationship)
        //{
        //    return context.Relationship.FirstOrDefault(x => (x.RelationshipID != null && x.RelationshipID == relationship.ID) || (relationship.RelationshipID != null && relationship.RelationshipID == x.ID));
        //}
        //public Relationship GetReverseRelationship(Relationship relationship)
        //{
        //    if (relationship.Relationship2 != null)
        //        return relationship;
        //    else
        //        return relationship.Relationship1.First();
        //}

        public Tuple<string, string> splitQuery(string query)
        {
            if (query.Contains(" where "))
            {
                var split = query.Split(" where ".ToCharArray());
                return new Tuple<string, string>(split[0], split[1]);
            }
            else
                return new Tuple<string, string>(query, "");
        }

        //public void SetUnion_RelationshipProperties(Tuple<UnionRelationshipType, TableDrivedEntity, List<TableDrivedEntity>, UnionKeyType> unionRelationship)
        //{
        //    var ConnectionString = unionRelationship.Item2.Table.DBSchema.DatabaseInformation.ConnectionString;
        //    var unionType = unionRelationship.Item2;

        //    List<TableDrivedEntity> subTypes = unionRelationship.Item3;
        //    string subUnionStr = "";
        //    foreach (var entity in subTypes)
        //    {
        //        subUnionStr += (subUnionStr == "" ? "" : ",") + entity.Name;
        //    }
        //    unionRelationship.Item1.Name = unionType.Name + ">" + subUnionStr;

        //    //دراینجا از روش دیگری استفاده شده و بجای یک کوئری هر ساب تایپ مجزا چک میشود
        //    //زیرا مفهوم توتال پارتیشن با ارث بری متفاوت است
        //    if (unionRelationship.Item4 == UnionKeyType.SubUnionHoldsKeys)
        //    {
        //        unionRelationship.Item1.IsTolatParticipation = true;
        //        foreach (var entity in subTypes)
        //        {
        //            var relationship = unionType.Relationship.FirstOrDefault(x => x.RelationshipType != null && x.RelationshipType.UnionToSubUnionRelationshipType != null && x.RelationshipType.UnionToSubUnionRelationshipType.UnionRelationshipType == unionRelationship.Item1
        //               && x.TableDrivedEntity1 == entity);
        //            using (SqlConnection testConn = new SqlConnection(ConnectionString))
        //            {
        //                testConn.Open();
        //                SqlTransaction trans;
        //                //try
        //                //{
        //                trans = testConn.BeginTransaction();
        //                SqlCommand commandCheckNull = new SqlCommand(CheckSubUnionIsNullQuery(entity, relationship), testConn, trans);
        //                var resCheckFKIsNull = commandCheckNull.ExecuteScalar();
        //                if (Convert.ToInt32(resCheckFKIsNull) != 0)
        //                {
        //                    unionRelationship.Item1.IsTolatParticipation = false;
        //                    testConn.Close();
        //                    break;
        //                }

        //                //}
        //                //catch (Exception ex)
        //                //{
        //                //    throw ex;
        //                //}
        //            }
        //        }
        //    }
        //    else
        //    {
        //        unionRelationship.Item1.IsTolatParticipation = true;
        //        foreach (var subUnion in subTypes)
        //        {
        //            var relationship = unionType.Relationship.FirstOrDefault(x => x.RelationshipType != null && x.RelationshipType.UnionToSubUnionRelationshipType != null && x.RelationshipType.UnionToSubUnionRelationshipType.UnionRelationshipType == unionRelationship.Item1
        //               && x.TableDrivedEntity1 == subUnion);
        //            using (SqlConnection testConn = new SqlConnection(ConnectionString))
        //            {
        //                testConn.Open();
        //                SqlTransaction trans;
        //                //try
        //                //{
        //                trans = testConn.BeginTransaction();
        //                SqlCommand commandCheckNull = new SqlCommand(CheckSubUnionWithoutUnionQuery(unionType, subUnion, relationship), testConn, trans);
        //                var resCheckFKIsNull = commandCheckNull.ExecuteScalar();
        //                if (Convert.ToInt32(resCheckFKIsNull) != 0)
        //                {
        //                    unionRelationship.Item1.IsTolatParticipation = false;
        //                    testConn.Close();
        //                    break;
        //                }
        //                //}
        //                //catch (Exception ex)
        //                //{
        //                //    throw ex;
        //                //}
        //            }
        //        }
        //    }
        //}

        //private string CheckSubUnionWithoutUnionQuery(TableDrivedEntity union, TableDrivedEntity subUniob, Relationship relationship)
        //{
        //    var columns2 = GetFKRelationshipColumns(union, relationship, false);
        //    var condition = "";
        //    foreach (var column in columns2)
        //    {
        //        string columnEqual1 = "";
        //        if (relationship.TableDrivedEntity == union && relationship.RelationshipColumns.Any(y => column.ID == y.FirstSideColumnID))
        //        {
        //            columnEqual1 = "fk." + relationship.RelationshipColumns.First(y => column.ID == y.FirstSideColumnID).Column.Name
        //                + "=" + "pk." + relationship.RelationshipColumns.First(y => column.ID == y.FirstSideColumnID).Column1.Name;
        //        }
        //        else if (relationship.TableDrivedEntity1 == union && relationship.RelationshipColumns.Any(y => column.ID == y.SecondSideColumnID))
        //        {
        //            columnEqual1 = "fk." + relationship.RelationshipColumns.First(y => column.ID == y.SecondSideColumnID).Column1.Name
        //                     + "=" + "pk." + relationship.RelationshipColumns.First(y => column.ID == y.SecondSideColumnID).Column.Name;
        //        }

        //        condition += (condition == "" ? "" : " and ") + columnEqual1;
        //    }
        //    var subUnionBaseSelectFromQuery = GetEntityBaseSelectFromQuery(subUniob, true);
        //    var subUnionCriteria = GetCriteria(subUniob);


        //    var unionBaseSelectFromQuery = GetEntityBaseSelectFromQuery(union, false);
        //    var unionCriteria = GetCriteria(union);

        //    string where = "not exists (" + unionBaseSelectFromQuery + unionCriteria + (unionCriteria.Contains(" where ") ? " and " : " where ") + condition + ")";

        //    return subUnionBaseSelectFromQuery + subUnionCriteria + (subUnionCriteria.Contains(" where ") ? " and " : " where ") + where;

        //    //return   unionBaseSelectFromQuery + unionCriteria + (unionCriteria.Contains(" where ") ? " and " : " where ") + where;

        //}

        //private string CheckSubUnionIsNullQuery(TableDrivedEntity subUniob, Relationship relationship)
        //{
        //    var columns2 = GetFKRelationshipColumns(subUniob, relationship, true);
        //    var condition = "";
        //    foreach (var column in columns2)
        //    {
        //        condition += (condition == "" ? "" : " and ") + column.Name + " is null";
        //    }
        //    var subUnionBaseSelectFromQuery = GetEntityBaseSelectFromQuery(subUniob, false);
        //    var subUnionCriteria = GetCriteria(subUniob);
        //    return subUnionBaseSelectFromQuery + subUnionCriteria + (subUnionCriteria.Contains(" where ") ? " and " : " where ") + condition;
        //}

    }

    //public class EntityInfo
    //{
    //    public EntityInfo()
    //    {
    //        ManyDataItems = new List<Tuple<TableDrivedEntity, bool>>();
    //        OneDataItems = new List<Tuple<TableDrivedEntity, bool>>();
    //        ManyToOneDataItems = new List<Tuple<TableDrivedEntity, bool>>();
    //        OneToOneDataItems = new List<Tuple<TableDrivedEntity, bool>>();
    //    }
    //    public TableDrivedEntity TableDrivedEntity { set; get; }
    //    public bool? EntityHasDate { set; get; }
    //    public List<Tuple<TableDrivedEntity, bool>> ManyDataItems { set; get; }
    //    public List<Tuple<TableDrivedEntity, bool>> OneDataItems { set; get; }

    //    public List<Tuple<TableDrivedEntity, bool>> ManyToOneDataItems { set; get; }
    //    public List<Tuple<TableDrivedEntity, bool>> OneToOneDataItems { set; get; }
    //}
    //public class PrimaryEntityInfo
    //{
    //    public PrimaryEntityInfo()
    //    {
    //        ManyDataItems = new List<Tuple<TableDrivedEntity, bool>>();
    //        OneDataItems = new List<Tuple<TableDrivedEntity, bool>>();

    //    }
    //    public TableDrivedEntity TableDrivedEntity { set; get; }
    //    public bool? EntityHasDate { set; get; }
    //    public List<Tuple<TableDrivedEntity, bool>> ManyDataItems { set; get; }
    //    public List<Tuple<TableDrivedEntity, bool>> OneDataItems { set; get; }

    //}
    //public class SecondaryEntityInfo
    //{
    //    public SecondaryEntityInfo()
    //    {

    //        ManyToOneDataItems = new List<Tuple<TableDrivedEntity, bool>>();
    //        OneToOneDataItems = new List<Tuple<TableDrivedEntity, bool>>();
    //    }
    //    public TableDrivedEntity TableDrivedEntity { set; get; }
    //    public bool? EntityHasDate { set; get; }
    //    public List<Tuple<TableDrivedEntity, bool>> ManyToOneDataItems { set; get; }
    //    public List<Tuple<TableDrivedEntity, bool>> OneToOneDataItems { set; get; }
    //}
    public enum UnionKeyType
    {
        UnionHoldsKeys,
        SubUnionHoldsKeys
    }
    public enum QueryDeterminerConnection
    {
        FirstSide,
        SecondSide,
        NoDifference
    }
    public class QueryDeterminer
    {
        public QueryDeterminerConnection ConnectionTarget { set; get; }
        public string query { set; get; }

    }
    public class QueryDepthOptions
    {
        //public QueryDeterminerConnection ConnectionTarget { set; get; }
        public string LinkedServer { set; get; }
        //public string SecondSideLinkedServer { set; get; }
        public bool DatabaseName { set; get; }
        public bool SchemaName { set; get; }
    }
    public class ISARelationshipDetail
    {
        public string Name { set; get; }
        public bool? IsDisjoint { set; get; }
        public bool? IsTotalParticipation { set; get; }
    }
    public class UnionRelationshipDetail
    {
        public string Name { set; get; }
        //    public bool IsDisjoint { set; get; }
        public bool? IsTotalParticipation { set; get; }
    }
}
