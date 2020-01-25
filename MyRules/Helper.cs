using DataAccess;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRules
{
    public class Helper
    {
        //internal static List<DataAccess.Column> GetColumnList(Entity template)
        //{
        //    if (template.Column == null || template.Column.Count == 0)
        //    {
        //        return template.TableDrivedEntity.Table.Column.ToList();
        //    }
        //    else
        //        return template.Column.ToList();
        //}

      
        //internal static EntityRelationInfo GetEntityAsForeignRelationships(Entity entity)
        //{
        //    EntityRelationInfo result = new EntityRelationInfo();
        //    result.Entity = entity;
        //    var columns = GetColumnList(entity);
        //    foreach (var relationship in entity.Relationship1)
        //    {
        //        //if (relationship.RelationshipID != null)
        //        //{
        //        //    throw (new Exception("asdsdf"));
        //        //}
        //        if (columns.Where(x => x.PrimaryKey == true).All(x => relationship.Entity == entity && relationship.RelationshipColumns.Any(y => x.ID == y.ColumnID1)))
        //        {
        //            GetOtherFkSideInfo(result, relationship, relationship.Entity1);
        //        }
        //        //else if (columns.Where(x => x.PrimaryKey == true).All(x => relationship.Entity1 == entity && relationship.RelationshipColumns.Any(y => x.ID == y.ColumnID2)))
        //        //{
        //        //    GetOtherFkSideInfo(result, relationship, relationship.Entity);
        //        //}

        //        //if(relationship.RelationshipColumns.)
        //    }
        //    return result;
        //}
        //////internal static void GetEntityAsPrimaryInfo(List<EntityInfo> listEntitInfo, Entity entity)
        //////{
        //////    //List<Tuple<PrimaryEntityInfo, SecondaryEntityInfo>> result = new List<Tuple<PrimaryEntityInfo, SecondaryEntityInfo>>();
        //////    PrimaryEntityInfo primaryEntityInfo = new PrimaryEntityInfo();
        //////    primaryEntityInfo.Entity = entity;
        //////    var columns = GetColumnList(entity);
        //////    foreach (var relationship in entity.Relationship.Where(x => x.RelationshipID == null))
        //////    {
        //////        //if (relationship.RelationshipID != null)
        //////        //{
        //////        //    throw (new Exception("asdsdf"));
        //////        //}
        //////        SecondaryEntityInfo secondaryEntityInfos = new SecondaryEntityInfo();
        //////        secondaryEntityInfos.Entity = relationship.Entity1;
        //////        if (columns.Where(x => x.PrimaryKey == true).All(x => relationship.Entity == entity && relationship.RelationshipColumns.Any(y => x.ID == y.ColumnID1)))
        //////        {
        //////            GetOtherFkSideInfo(primaryEntityInfo, secondaryEntityInfos, relationship);
        //////        }
        //////        CheckListEntityInfo(listEntitInfo, secondaryEntityInfos);
        //////        //else if (columns.Where(x => x.PrimaryKey == true).All(x => relationship.Entity1 == entity && relationship.RelationshipColumns.Any(y => x.ID == y.ColumnID2)))
        //////        //{
        //////        //    GetOtherFkSideInfo(result, relationship, relationship.Entity);
        //////        //}

        //////        //if(relationship.RelationshipColumns.)
        //////    }
        //////    CheckListEntityInfo(listEntitInfo, primaryEntityInfo);

        //////}

        private static void CheckListEntityInfo(List<EntityInfo> listEntitInfo, PrimaryEntityInfo primaryEntityInfo)
        {
            EntityInfo entityInfo = null;
            entityInfo = listEntitInfo.FirstOrDefault(x => x.Entity == primaryEntityInfo.Entity);
            if (entityInfo == null)
            {
                entityInfo = new EntityInfo();
                entityInfo.Entity = primaryEntityInfo.Entity;
                listEntitInfo.Add(entityInfo);
            }
            foreach (var item in primaryEntityInfo.ManyDataItems)
                entityInfo.ManyDataItems.Add(item);
            foreach (var item in primaryEntityInfo.OneDataItems)
                entityInfo.OneDataItems.Add(item);
        }

        private static void CheckListEntityInfo(List<EntityInfo> listEntitInfo, SecondaryEntityInfo secondaryEntityInfos)
        {
            EntityInfo entityInfo = null;
            entityInfo = listEntitInfo.FirstOrDefault(x => x.Entity == secondaryEntityInfos.Entity);
            if (entityInfo == null)
            {
                entityInfo = new EntityInfo();
                entityInfo.Entity = secondaryEntityInfos.Entity;
                listEntitInfo.Add(entityInfo);
            }
            foreach (var item in secondaryEntityInfos.ManyToOneDataItems)
                entityInfo.ManyToOneDataItems.Add(item);
            foreach (var item in secondaryEntityInfos.OneToOneDataItems)
                entityInfo.OneToOneDataItems.Add(item);
        }


        //internal static EntityInfo GetEntityAsForeignInfo(Entity entity)
        //{
        //    EntityInfo result = new EntityInfo();
        //    result.Entity = entity;
        //    var columns = GetColumnList(entity);
        //    foreach (var relationship in entity.Relationship.Where(x => x.RelationshipID == null))
        //    {
        //        //if (relationship.RelationshipID != null)
        //        //{
        //        //    throw (new Exception("asdsdf"));
        //        //}
        //        if (columns.Where(x => x.PrimaryKey == true).All(x => relationship.Entity == entity && relationship.RelationshipColumns.Any(y => x.ID == y.ColumnID1)))
        //        {
        //            GetOtherFkSideInfo(result, relationship, relationship.Entity1);
        //        }
        //        //else if (columns.Where(x => x.PrimaryKey == true).All(x => relationship.Entity1 == entity && relationship.RelationshipColumns.Any(y => x.ID == y.ColumnID2)))
        //        //{
        //        //    GetOtherFkSideInfo(result, relationship, relationship.Entity);
        //        //}

        //        //if(relationship.RelationshipColumns.)
        //    }
        //    return result;
        //}

        private static void GetOtherSideRelationshipInfo(EntityRelationInfo mainEntityInfo, Relationship relationship)
        {
            bool? mainEntityIsPrimary = null;
            Entity PKEntity = null;
            Entity FKEntity = null;

            if (relationship.Relationship2 == null)
            {
                mainEntityIsPrimary = true;
                PKEntity = mainEntityInfo.Entity;
                FKEntity = relationship.Entity1;
            }
            else
            {
                mainEntityIsPrimary = false;
                PKEntity = relationship.Entity1;
                FKEntity = relationship.Entity;
            }

            bool fkRelatesOnPrimaryKey = false;
            bool fkRelatesOnPartOfPrimaryKey = false;
            var columns2 = FKEntity.TableDrivedEntity.Table.Column; //Helper.GetColumnList(FKEntity);
            if (columns2.Where(x => x.PrimaryKey == true).All(x =>
                (relationship.Entity != relationship.Entity1 && (relationship.Entity1 == FKEntity && relationship.RelationshipColumns.Any(y => x.ID == y.ColumnID2) ||
               relationship.Entity == FKEntity && relationship.RelationshipColumns.Any(y => x.ID == y.ColumnID1))
               )
               )
               )
            {
                fkRelatesOnPrimaryKey = true;
            }
            else
            {
                if (columns2.Where(x => x.PrimaryKey == true).All(x =>
               (relationship.Entity != relationship.Entity1 && (relationship.Entity1 == FKEntity && relationship.RelationshipColumns.Any(y => x.ID == y.ColumnID2) ||
              relationship.Entity == FKEntity && relationship.RelationshipColumns.Any(y => x.ID == y.ColumnID1))
              )
              )
              )
                {
                    fkRelatesOnPartOfPrimaryKey = true;
                }
            }

            var baseQueryFKEntity = "select top 1 1 from [" + FKEntity.TableDrivedEntity.Table.Name + "] as fk";
            var fkEntityCriteriaWhere = (string.IsNullOrEmpty(FKEntity.TableDrivedEntity.Criteria) ? "" : " where " + FKEntity.TableDrivedEntity.Criteria);
            var baseQueryPKEntity = "select top 1 1 from [" + PKEntity.TableDrivedEntity.Table.Name + "] as main";
            var pkEntityCriteriaWhere = (string.IsNullOrEmpty(PKEntity.TableDrivedEntity.Criteria) ? "" : " where " + PKEntity.TableDrivedEntity.Criteria);
            string conditionCheckFKNull = "";
            string conditionCheckExists = "";
            string condition = "";
            string groupBy = "";
            bool fkColumnIsMandatory = true;

            if (relationship.Entity == relationship.Entity1)
            {

            }
            foreach (var column in columns2.Where(x => (relationship.Entity == FKEntity && relationship.RelationshipColumns.Any(y => x.ID == y.ColumnID1)) ||
               (relationship.Entity1 == FKEntity && relationship.RelationshipColumns.Any(y => x.ID == y.ColumnID2))
               ))
            {
                if (relationship.Entity == relationship.Entity1)
                {
                    if (column.PrimaryKey == true)
                        continue;
                }
                if (column.IsNull == true)
                    fkColumnIsMandatory = false;

                string columnEqual = "";
                if (relationship.Entity == FKEntity && relationship.RelationshipColumns.Any(y => column.ID == y.ColumnID1))
                {
                    columnEqual = "fk." + relationship.RelationshipColumns.First(y => column.ID == y.ColumnID1).Column.Name
                        + "=" + "main." + relationship.RelationshipColumns.First(y => column.ID == y.ColumnID1).Column1.Name;
                }
                else if (relationship.Entity1 == FKEntity && relationship.RelationshipColumns.Any(y => column.ID == y.ColumnID2))
                {
                    columnEqual = "fk." + relationship.RelationshipColumns.First(y => column.ID == y.ColumnID2).Column1.Name
                             + "=" + "main." + relationship.RelationshipColumns.First(y => column.ID == y.ColumnID2).Column.Name;
                }
                conditionCheckExists += (conditionCheckExists == "" ? "" : " and ") + columnEqual;

                conditionCheckFKNull += (conditionCheckFKNull == "" ? "" : " and ") + column.Name + " is null";

                condition += (condition == "" ? "" : " and ") + column.Name + " is not null";
                groupBy += (groupBy == "" ? "" : ",") + column.Name;
            }

            groupBy = " group by " + groupBy;
            var query = baseQueryFKEntity + fkEntityCriteriaWhere + (fkEntityCriteriaWhere.Contains(" where ") ? " and " : " where ") + condition;
            var queryGroupBy = query + groupBy + " having count(*)>1";
            var queryCheckFKIsNull = baseQueryFKEntity + fkEntityCriteriaWhere + (fkEntityCriteriaWhere.Contains(" where ") ? " and " : " where ") + conditionCheckFKNull;
            var queryCheckExist = baseQueryPKEntity + pkEntityCriteriaWhere + (pkEntityCriteriaWhere.Contains(" where ") ? " and " : " where ") + "not exists (" + baseQueryFKEntity + fkEntityCriteriaWhere + (fkEntityCriteriaWhere.Contains(" where ") ? "" : " where ") + conditionCheckExists + ")";
            var ConnectionString = SQLHelper.GetConnectionString(PKEntity.TableDrivedEntity.Table.Catalog);
            using (SqlConnection testConn = new SqlConnection(ConnectionString))
            {
                testConn.Open();
                SqlTransaction trans;

                //try
                //{

                trans = testConn.BeginTransaction();


                SqlCommand command1 = new SqlCommand(query, testConn, trans);
                var res1 = command1.ExecuteScalar();
                bool FKEntityHasData = Convert.ToInt32(res1) != 0;


                //SqlCommand command2 = new SqlCommand(baseQueryFKEntity, testConn, trans);
                //var res2 = command2.ExecuteScalar();
                //bool FKEntityHasData = Convert.ToInt32(res2) != 0;

                bool allPrimarySideHasFkSideData = false;
                if (FKEntityHasData)
                {
                    if (!fkColumnIsMandatory)
                    {
                        SqlCommand commandCheckNull = new SqlCommand(queryCheckFKIsNull, testConn, trans);
                        var resCheckFKIsNull = commandCheckNull.ExecuteScalar();
                        if (Convert.ToInt32(resCheckFKIsNull) == 0)
                            fkColumnIsMandatory = true;
                    }

                    SqlCommand commandCheckExists = new SqlCommand(queryCheckExist, testConn, trans);
                    var resCheckExists = commandCheckExists.ExecuteScalar();
                    if (Convert.ToInt32(resCheckExists) == 0)
                        allPrimarySideHasFkSideData = true;
                }
                bool? hasManyFkSides = null;
                if (fkRelatesOnPrimaryKey)
                    hasManyFkSides = false;
                if (hasManyFkSides == null)
                {
                    if (FKEntityHasData)
                    {
                        SqlCommand command19 = new SqlCommand(queryGroupBy, testConn, trans);
                        var res = command19.ExecuteScalar();
                        hasManyFkSides = Convert.ToInt32(res) > 0;
                    }
                    else
                        hasManyFkSides = true;
                }

                Entity otherSideEntity = null;
                if (mainEntityIsPrimary == true)
                    otherSideEntity = FKEntity;
                else if (mainEntityIsPrimary == false)
                    otherSideEntity = PKEntity;
                var relationInfo = new RelationInfo(relationship, otherSideEntity, allPrimarySideHasFkSideData, fkColumnIsMandatory, fkRelatesOnPrimaryKey, fkRelatesOnPartOfPrimaryKey, FKEntityHasData);

                if (hasManyFkSides == true)
                {
                    if (mainEntityIsPrimary == true)
                        relationInfo.RelationType = RelationType.ManyDataItems;
                    else if (mainEntityIsPrimary == false)
                        relationInfo.RelationType = RelationType.ManyToOneDataItems;
                }
                else
                {
                    if (mainEntityIsPrimary == true)
                        relationInfo.RelationType = RelationType.OneDataItems;
                    else if (mainEntityIsPrimary == false)
                        relationInfo.RelationType = RelationType.OneToOneDataItems;
                }
                mainEntityInfo.RelationInfos.Add(relationInfo);
                //}
                //catch (Exception ex) //error occurred
                //{
                //    throw (ex);
                //}

            }


        }

        //private static void GetOtherPKSideInfo(EntityRelationInfo mainEntityInfo, Relationship relationship, Entity entity2)
        //{

        //    bool fkRelatesOnPrimaryKey = false;
        //    bool fkRelatesOnPartOfPrimaryKey = false;
        //    var columns2 = Helper.GetColumnList(entity2);
        //    if (columns2.Where(x => x.PrimaryKey == true).All(x =>
        //        (relationship.Entity != relationship.Entity1 && (relationship.Entity1 == entity2 && relationship.RelationshipColumns.Any(y => x.ID == y.ColumnID2) ||
        //       relationship.Entity == entity2 && relationship.RelationshipColumns.Any(y => x.ID == y.ColumnID1))
        //       )
        //       )
        //       )
        //    {
        //        fkRelatesOnPrimaryKey = true;
        //    }
        //    else
        //    {
        //        if (columns2.Where(x => x.PrimaryKey == true).All(x =>
        //       (relationship.Entity != relationship.Entity1 && (relationship.Entity1 == entity2 && relationship.RelationshipColumns.Any(y => x.ID == y.ColumnID2) ||
        //      relationship.Entity == entity2 && relationship.RelationshipColumns.Any(y => x.ID == y.ColumnID1))
        //      )
        //      )
        //      )
        //        {
        //            fkRelatesOnPartOfPrimaryKey = true;
        //        }
        //    }

        //    var baseQueryEntity2 = "select top 1 1 from [" + entity2.Name + "]";
        //    var baseQueryEntity1 = "select top 1 1 from [" + mainEntityInfo.Entity.Name + "]";

        //    string conditionCheckFKNull = "";
        //    string conditionCheckExists = "";
        //    string condition = "";
        //    string groupBy = "";
        //    bool fkColumnIsMandatory = true;

        //    if (relationship.Entity == relationship.Entity1)
        //    {

        //    }
        //    foreach (var column in columns2.Where(x => (relationship.Entity == entity2 && relationship.RelationshipColumns.Any(y => x.ID == y.ColumnID1)) ||
        //       (relationship.Entity1 == entity2 && relationship.RelationshipColumns.Any(y => x.ID == y.ColumnID2))
        //       ))
        //    {
        //        if (relationship.Entity == relationship.Entity1)
        //        {
        //            if (column.PrimaryKey == true)
        //                continue;
        //        }
        //        if (column.IsNull == true)
        //            fkColumnIsMandatory = false;

        //        string columnEqual = "";
        //        if (relationship.Entity == entity2 && relationship.RelationshipColumns.Any(y => column.ID == y.ColumnID1))
        //        {
        //            columnEqual = "fk." + relationship.RelationshipColumns.First(y => column.ID == y.ColumnID1).Column.Name
        //                + "=" + "main." + relationship.RelationshipColumns.First(y => column.ID == y.ColumnID1).Column1.Name;
        //        }
        //        else if (relationship.Entity1 == entity2 && relationship.RelationshipColumns.Any(y => column.ID == y.ColumnID2))
        //        {
        //            columnEqual = "fk." + relationship.RelationshipColumns.First(y => column.ID == y.ColumnID2).Column1.Name
        //                     + "=" + "main." + relationship.RelationshipColumns.First(y => column.ID == y.ColumnID2).Column.Name;
        //        }
        //        conditionCheckExists += (conditionCheckExists == "" ? "" : " and ") + columnEqual;

        //        conditionCheckFKNull += (conditionCheckFKNull == "" ? "" : " and ") + column.Name + " is null";

        //        condition += (condition == "" ? "" : " and ") + column.Name + " is not null";
        //        groupBy += (groupBy == "" ? "" : ",") + column.Name;
        //    }

        //    groupBy = " group by " + groupBy;
        //    var query = baseQueryEntity2 + " where " + condition;
        //    var queryGroupBy = query + groupBy + " having count(*)>1";
        //    //var queryTest = baseQuery + " where " + condition;
        //    var queryCheckFKIsNull = baseQueryEntity2 + " where " + conditionCheckFKNull;
        //    var queryCheckExist = baseQueryEntity1 + " as main where " + "not exists (select * from [" + entity2.Name + "] as fk where " + conditionCheckExists + ")";
        //    var ConnectionString = SQLHelper.GetConnectionString(mainEntityInfo.Entity.TableDrivedEntity.Table.Catalog);
        //    using (SqlConnection testConn = new SqlConnection(ConnectionString))
        //    {
        //        testConn.Open();
        //        SqlTransaction trans;

        //        //try
        //        //{
        //        trans = testConn.BeginTransaction();

        //        if (mainEntityInfo.EntityHasDate == null)
        //        {
        //            SqlCommand command1 = new SqlCommand(query, testConn, trans);
        //            var res1 = command1.ExecuteScalar();
        //            mainEntityInfo.EntityHasDate = Convert.ToInt32(res1) != 0;
        //        }

        //        SqlCommand command2 = new SqlCommand(baseQueryEntity2, testConn, trans);
        //        var res2 = command2.ExecuteScalar();
        //        bool entity2HasData = Convert.ToInt32(res2) != 0;

        //        bool allPrimarySideHasFkSideData = false;
        //        if (mainEntityInfo.EntityHasDate == true && entity2HasData)
        //        {
        //            if (!fkColumnIsMandatory)
        //            {
        //                SqlCommand commandCheckNull = new SqlCommand(queryCheckFKIsNull, testConn, trans);
        //                var resCheckFKIsNull = commandCheckNull.ExecuteScalar();
        //                if (Convert.ToInt32(resCheckFKIsNull) == 0)
        //                    fkColumnIsMandatory = true;
        //            }

        //            SqlCommand commandCheckExists = new SqlCommand(queryCheckExist, testConn, trans);
        //            var resCheckExists = commandCheckExists.ExecuteScalar();
        //            if (Convert.ToInt32(resCheckExists) == 0)
        //                allPrimarySideHasFkSideData = true;
        //        }
        //        bool? hasManyFkSides = null;
        //        if (fkRelatesOnPrimaryKey)
        //            hasManyFkSides = false;
        //        if (hasManyFkSides == null)
        //        {
        //            if (mainEntityInfo.EntityHasDate == true && entity2HasData)
        //            {
        //                SqlCommand command1 = new SqlCommand(queryGroupBy, testConn, trans);
        //                var res = command1.ExecuteScalar();
        //                hasManyFkSides = Convert.ToInt32(res) > 0;
        //            }
        //            else
        //                hasManyFkSides = true;
        //        }
        //        if (hasManyFkSides == true)
        //        {
        //            mainEntityInfo.ManyDataItems.Add(new Tuple<Relationship, Entity, bool, bool, bool>(relationship, entity2, allPrimarySideHasFkSideData, fkColumnIsMandatory, fkRelatesOnPartOfPrimaryKey));
        //        }
        //        else
        //        {
        //            mainEntityInfo.OneDataItems.Add(new Tuple<Relationship, Entity, bool, bool, bool>(relationship, entity2, allPrimarySideHasFkSideData, fkColumnIsMandatory, fkRelatesOnPrimaryKey));
        //        }

        //        //}
        //        //catch (Exception ex) //error occurred
        //        //{
        //        //    throw (ex);
        //        //}

        //    }


        //}
        //private static void GetOtherFkSideInfo(PrimaryEntityInfo primaryEntityInfo, SecondaryEntityInfo secondaryEntityInfo, Relationship relationship)
        //{
        //    //   PrimaryEntityInfo primaryEntityInfo = new PrimaryEntityInfo();

        //    // primaryEntityInfo.Entity = entity1;


        //    bool fkRelatesOnPrimaryKey = false;
        //    bool fkRelatesOnPartOfPrimaryKey = false;
        //    var columns2 = Helper.GetColumnList(secondaryEntityInfo.Entity);
        //    if (columns2.Where(x => x.PrimaryKey == true).All(x =>
        //        (relationship.Entity != relationship.Entity1 && (relationship.RelationshipColumns.Any(y => x.ID == y.ColumnID2))
        //            //relationship.Entity == entity2 && relationship.RelationshipColumns.Any(y => x.ID == y.ColumnID1))
        //       )
        //       )
        //       )
        //    {
        //        fkRelatesOnPrimaryKey = true;
        //    }
        //    else
        //    {
        //        if (columns2.Where(x => x.PrimaryKey == true).All(x =>
        //       (relationship.Entity != relationship.Entity1 && (relationship.RelationshipColumns.Any(y => x.ID == y.ColumnID2))
        //           //relationship.Entity == entity2 && relationship.RelationshipColumns.Any(y => x.ID == y.ColumnID1))
        //      )
        //      )
        //      )
        //        {
        //            fkRelatesOnPartOfPrimaryKey = true;
        //        }
        //    }

        //    var baseQueryEntity2 = "select top 1 1 from [" + secondaryEntityInfo.Entity.Name + "]";
        //    var baseQueryEntity1 = "select top 1 1 from [" + primaryEntityInfo.Entity.Name + "]";

        //    //string conditionCheckFKNull = "";
        //    //string conditionCheckExists = "";
        //    string condition = "";
        //    string groupBy = "";
        //    //bool fkColumnIsMandatory = true;

        //    if (relationship.Entity == relationship.Entity1)
        //    {

        //    }
        //    foreach (var column in columns2.Where(x => (relationship.Entity == secondaryEntityInfo.Entity && relationship.RelationshipColumns.Any(y => x.ID == y.ColumnID1)) ||
        //       (relationship.Entity1 == secondaryEntityInfo.Entity && relationship.RelationshipColumns.Any(y => x.ID == y.ColumnID2))
        //       ))
        //    {
        //        if (relationship.Entity == relationship.Entity1)
        //        {
        //            if (column.PrimaryKey == true)
        //                continue;
        //        }
        //        //if (column.IsNull == true)
        //        //    fkColumnIsMandatory = false;

        //        //string columnEqual = "";
        //        //if (relationship.Entity == entity2 && relationship.RelationshipColumns.Any(y => column.ID == y.ColumnID1))
        //        //{
        //        //    columnEqual = "fk." + relationship.RelationshipColumns.First(y => column.ID == y.ColumnID1).Column.Name
        //        //        + "=" + "main." + relationship.RelationshipColumns.First(y => column.ID == y.ColumnID1).Column1.Name;
        //        //}
        //        //else if (relationship.Entity1 == entity2 && relationship.RelationshipColumns.Any(y => column.ID == y.ColumnID2))
        //        //{
        //        //    columnEqual = "fk." + relationship.RelationshipColumns.First(y => column.ID == y.ColumnID2).Column1.Name
        //        //             + "=" + "main." + relationship.RelationshipColumns.First(y => column.ID == y.ColumnID2).Column.Name;
        //        //}
        //        //conditionCheckExists += (conditionCheckExists == "" ? "" : " and ") + columnEqual;

        //        //conditionCheckFKNull += (conditionCheckFKNull == "" ? "" : " and ") + column.Name + " is null";

        //        condition += (condition == "" ? "" : " and ") + column.Name + " is not null";
        //        groupBy += (groupBy == "" ? "" : ",") + column.Name;
        //    }

        //    groupBy = " group by " + groupBy;
        //    var query = baseQueryEntity2 + " where " + condition;
        //    var queryGroupBy = query + groupBy + " having count(*)>1";
        //    //var queryTest = baseQuery + " where " + condition;
        //    //var queryCheckFKIsNull = baseQueryEntity2 + " where " + conditionCheckFKNull;
        //    //var queryCheckExist = "select top 1 count(*) from [" + entity1.Name + "] as main where " + "not exists (select * from [" + entity2.Name + "] as fk where " + conditionCheckExists + ")";
        //    var ConnectionString = SQLHelper.GetConnectionString(primaryEntityInfo.Entity.TableDrivedEntity.Table.Catalog);
        //    using (SqlConnection testConn = new SqlConnection(ConnectionString))
        //    {
        //        testConn.Open();
        //        SqlTransaction trans;

        //        //try
        //        //{
        //        trans = testConn.BeginTransaction();

        //        if (primaryEntityInfo.EntityHasDate == null)
        //        {
        //            SqlCommand command1 = new SqlCommand(query, testConn, trans);
        //            var res1 = command1.ExecuteScalar();
        //            primaryEntityInfo.EntityHasDate = Convert.ToInt32(res1) != 0;
        //        }

        //        if (secondaryEntityInfo.EntityHasDate == null)
        //        {
        //            SqlCommand command2 = new SqlCommand(baseQueryEntity2, testConn, trans);
        //            var res2 = command2.ExecuteScalar();
        //            secondaryEntityInfo.EntityHasDate = Convert.ToInt32(res2) != 0;
        //        }
        //        //bool allPrimarySideHasFkSideData = false;
        //        //if (entity1HasDate == true && entity2HasData)
        //        //{
        //        //    if (!fkColumnIsMandatory)
        //        //    {
        //        //        SqlCommand commandCheckNull = new SqlCommand(queryCheckFKIsNull, testConn, trans);
        //        //        var resCheckFKIsNull = commandCheckNull.ExecuteScalar();
        //        //        if (Convert.ToInt32(resCheckFKIsNull) == 0)
        //        //            fkColumnIsMandatory = true;
        //        //    }

        //        //    SqlCommand commandCheckExists = new SqlCommand(queryCheckExist, testConn, trans);
        //        //    var resCheckExists = commandCheckExists.ExecuteScalar();
        //        //    if (Convert.ToInt32(resCheckExists) == 0)
        //        //        allPrimarySideHasFkSideData = true;
        //        //}
        //        bool? hasManyFkSides = null;
        //        if (fkRelatesOnPrimaryKey)
        //            hasManyFkSides = false;
        //        if (hasManyFkSides == null)
        //        {
        //            if (primaryEntityInfo.EntityHasDate == true && secondaryEntityInfo.EntityHasDate == true)
        //            {
        //                SqlCommand command1 = new SqlCommand(queryGroupBy, testConn, trans);
        //                var res = command1.ExecuteScalar();
        //                hasManyFkSides = Convert.ToInt32(res) > 0;
        //            }
        //            else
        //                hasManyFkSides = true;
        //        }
        //        if (hasManyFkSides == true)
        //        {
        //            primaryEntityInfo.ManyDataItems.Add(new Tuple<Entity, bool>(secondaryEntityInfo.Entity, fkRelatesOnPartOfPrimaryKey));
        //            secondaryEntityInfo.ManyToOneDataItems.Add(new Tuple<Entity, bool>(primaryEntityInfo.Entity, fkRelatesOnPartOfPrimaryKey));
        //        }
        //        else
        //        {
        //            primaryEntityInfo.OneDataItems.Add(new Tuple<Entity, bool>(secondaryEntityInfo.Entity, fkRelatesOnPrimaryKey));
        //            secondaryEntityInfo.OneToOneDataItems.Add(new Tuple<Entity, bool>(primaryEntityInfo.Entity, fkRelatesOnPrimaryKey));
        //        }
        //        //return secondaryEntityInfo;
        //        //}
        //        //catch (Exception ex) //error occurred
        //        //{
        //        //    throw (ex);
        //        //}

        //    }


        //}


        //private static void GetOtherPKSideInfo(EntityInfo mainEntityInfo, Relationship relationship, Entity entity2)
        //{

        //    bool fkRelatesOnPrimaryKey = false;
        //    bool fkRelatesOnPartOfPrimaryKey = false;
        //    var columns2 = Helper.GetColumnList(entity2);
        //    if (columns2.Where(x => x.PrimaryKey == true).All(x =>
        //        (relationship.Entity != relationship.Entity1 && (relationship.Entity1 == entity2 && relationship.RelationshipColumns.Any(y => x.ID == y.ColumnID2) ||
        //       relationship.Entity == entity2 && relationship.RelationshipColumns.Any(y => x.ID == y.ColumnID1))
        //       )
        //       )
        //       )
        //    {
        //        fkRelatesOnPrimaryKey = true;
        //    }
        //    else
        //    {
        //        if (columns2.Where(x => x.PrimaryKey == true).All(x =>
        //       (relationship.Entity != relationship.Entity1 && (relationship.Entity1 == entity2 && relationship.RelationshipColumns.Any(y => x.ID == y.ColumnID2) ||
        //      relationship.Entity == entity2 && relationship.RelationshipColumns.Any(y => x.ID == y.ColumnID1))
        //      )
        //      )
        //      )
        //        {
        //            fkRelatesOnPartOfPrimaryKey = true;
        //        }
        //    }

        //    var baseQueryEntity2 = "select top 1 count(*) from [" + entity2.Name + "]";
        //    var baseQueryEntity1 = "select top 1 count(*) from [" + mainEntityInfo.Entity.Name + "]";

        //    //string conditionCheckFKNull = "";
        //    //string conditionCheckExists = "";
        //    string condition = "";
        //    string groupBy = "";
        //    //bool fkColumnIsMandatory = true;

        //    if (relationship.Entity == relationship.Entity1)
        //    {

        //    }
        //    foreach (var column in columns2.Where(x => (relationship.Entity == entity2 && relationship.RelationshipColumns.Any(y => x.ID == y.ColumnID1)) ||
        //       (relationship.Entity1 == entity2 && relationship.RelationshipColumns.Any(y => x.ID == y.ColumnID2))
        //       ))
        //    {
        //        if (relationship.Entity == relationship.Entity1)
        //        {
        //            if (column.PrimaryKey == true)
        //                continue;
        //        }
        //        //if (column.IsNull == true)
        //        //    fkColumnIsMandatory = false;

        //        //string columnEqual = "";
        //        //if (relationship.Entity == entity2 && relationship.RelationshipColumns.Any(y => column.ID == y.ColumnID1))
        //        //{
        //        //    columnEqual = "fk." + relationship.RelationshipColumns.First(y => column.ID == y.ColumnID1).Column.Name
        //        //        + "=" + "main." + relationship.RelationshipColumns.First(y => column.ID == y.ColumnID1).Column1.Name;
        //        //}
        //        //else if (relationship.Entity1 == entity2 && relationship.RelationshipColumns.Any(y => column.ID == y.ColumnID2))
        //        //{
        //        //    columnEqual = "fk." + relationship.RelationshipColumns.First(y => column.ID == y.ColumnID2).Column1.Name
        //        //             + "=" + "main." + relationship.RelationshipColumns.First(y => column.ID == y.ColumnID2).Column.Name;
        //        //}
        //        //conditionCheckExists += (conditionCheckExists == "" ? "" : " and ") + columnEqual;

        //        //conditionCheckFKNull += (conditionCheckFKNull == "" ? "" : " and ") + column.Name + " is null";

        //        condition += (condition == "" ? "" : " and ") + column.Name + " is not null";
        //        groupBy += (groupBy == "" ? "" : ",") + column.Name;
        //    }

        //    groupBy = " group by " + groupBy;
        //    var query = baseQueryEntity2 + " where " + condition;
        //    var queryGroupBy = query + groupBy + " having count(*)>1";
        //    //var queryTest = baseQuery + " where " + condition;
        //    //var queryCheckFKIsNull = baseQueryEntity2 + " where " + conditionCheckFKNull;
        //    //var queryCheckExist = "select top 1 count(*) from [" + mainEntityInfo.Entity.Name + "] as main where " + "not exists (select * from [" + entity2.Name + "] as fk where " + conditionCheckExists + ")";
        //    var ConnectionString = SQLHelper.GetConnectionString(mainEntityInfo.Entity.TableDrivedEntity.Table.Catalog);
        //    using (SqlConnection testConn = new SqlConnection(ConnectionString))
        //    {
        //        testConn.Open();
        //        SqlTransaction trans;

        //        try
        //        {
        //            trans = testConn.BeginTransaction();

        //            if (mainEntityInfo.EntityHasDate == null)
        //            {
        //                SqlCommand command1 = new SqlCommand(query, testConn, trans);
        //                var res1 = command1.ExecuteScalar();
        //                mainEntityInfo.EntityHasDate = Convert.ToInt32(res1) != 0;
        //            }


        //            SqlCommand command2 = new SqlCommand(baseQueryEntity2, testConn, trans);
        //            var res2 = command2.ExecuteScalar();
        //            bool entity2HasData = Convert.ToInt32(res2) != 0;
        //            //bool allPrimarySideHasFkSideData = false;
        //            //if (mainEntityInfo.EntityHasDate == true && entity2HasData)
        //            //{
        //            //    if (!fkColumnIsMandatory)
        //            //    {
        //            //        SqlCommand commandCheckNull = new SqlCommand(queryCheckFKIsNull, testConn, trans);
        //            //        var resCheckFKIsNull = commandCheckNull.ExecuteScalar();
        //            //        if (Convert.ToInt32(resCheckFKIsNull) == 0)
        //            //            fkColumnIsMandatory = true;
        //            //    }

        //            //    SqlCommand commandCheckExists = new SqlCommand(queryCheckExist, testConn, trans);
        //            //    var resCheckExists = commandCheckExists.ExecuteScalar();
        //            //    if (Convert.ToInt32(resCheckExists) == 0)
        //            //        allPrimarySideHasFkSideData = true;
        //            //}
        //            bool? hasManyFkSides = null;
        //            if (fkRelatesOnPrimaryKey)
        //                hasManyFkSides = false;
        //            if (hasManyFkSides == null)
        //            {
        //                if (mainEntityInfo.EntityHasDate == true && entity2HasData)
        //                {
        //                    SqlCommand command1 = new SqlCommand(queryGroupBy, testConn, trans);
        //                    var res = command1.ExecuteScalar();
        //                    hasManyFkSides = Convert.ToInt32(res) > 0;
        //                }
        //                else
        //                    hasManyFkSides = true;
        //            }
        //            if (hasManyFkSides == true)
        //            {
        //                mainEntityInfo.ManyDataItems.Add(new Tuple<Entity, bool>(entity2, fkRelatesOnPartOfPrimaryKey));
        //            }
        //            else
        //            {
        //                mainEntityInfo.OneDataItems.Add(new Tuple<Entity, bool>(entity2, fkRelatesOnPrimaryKey));
        //            }

        //        }
        //        catch (Exception ex) //error occurred
        //        {
        //            throw (ex);
        //        }

        //    }


        //}
        internal static void ClearRelationshipType(RelationshipType relationshipType)
        {
            if (relationshipType.ManyToOneRelationshipType != null)
                relationshipType.ManyToOneRelationshipType = null;

            if (relationshipType.OneToManyRelationshipType != null)
                relationshipType.OneToManyRelationshipType = null;

            if (relationshipType.ExplicitOneToOneRelationshipType != null)
                relationshipType.ExplicitOneToOneRelationshipType = null;

            if (relationshipType.ImplicitOneToOneRelationshipType != null)
                relationshipType.ImplicitOneToOneRelationshipType = null;

            if (relationshipType.SuperToSubRelationshipType != null)
                relationshipType.SuperToSubRelationshipType = null;

            if (relationshipType.SubToSuperRelationshipType != null)
                relationshipType.SuperToSubRelationshipType = null;

            if (relationshipType.UnionToSubUnionRelationshipType != null)
                relationshipType.UnionToSubUnionRelationshipType = null;

            if (relationshipType.SubUnionToUnionRelationshipType != null)
                relationshipType.SubUnionToUnionRelationshipType = null;
        }

        internal static Relationship GetReverseRelationship(MyProjectEntities context, Relationship relationship)
        {
            return context.Relationship.FirstOrDefault(x => (x.RelationshipID != null && x.RelationshipID == relationship.ID) || (relationship.RelationshipID != null && relationship.RelationshipID == x.ID));
        }

        internal static void SetISA_RelationshipProperties(Tuple<ISARelationship, Entity, List<Entity>> iSARelationship)
        {
            var ConnectionString = SQLHelper.GetConnectionString(iSARelationship.Item2.TableDrivedEntity.Table.Catalog);
            var superType = iSARelationship.Item2;
            List<Entity> subTypes = iSARelationship.Item3;
            string subTypesStr = "";
            foreach (var entity in subTypes)
            {
                subTypesStr += (subTypesStr == "" ? "" : ",") + entity.Name;
            }
            iSARelationship.Item1.Name = superType.Name + ">" + subTypesStr;


            var whereClaus = "";
            var UnionQuery = "";
            foreach (var entity in subTypes)
            {
                var relationship = superType.Relationship.FirstOrDefault(x => x.RelationshipType != null && x.RelationshipType.SuperToSubRelationshipType != null && x.RelationshipType.SuperToSubRelationshipType.ISARelationship == iSARelationship.Item1
                   && x.Entity1 == entity);
                var columns2 = entity.TableDrivedEntity.Table.Column;// Helper.GetColumnList(entity);
                string conditionCheckExists = "";
                string unionColumns = "";
                int col = 0;
                foreach (var column in columns2.Where(x => (relationship.Entity == entity && relationship.RelationshipColumns.Any(y => x.ID == y.ColumnID1)) ||
                (relationship.Entity1 == entity && relationship.RelationshipColumns.Any(y => x.ID == y.ColumnID2))
                ))
                {
                    col++;
                    unionColumns += (unionColumns == "" ? "" : ",") + column.Name + " as col" + col;



                    string columnEqual1 = "";
                    if (relationship.Entity == entity && relationship.RelationshipColumns.Any(y => column.ID == y.ColumnID1))
                    {
                        columnEqual1 = "fk." + relationship.RelationshipColumns.First(y => column.ID == y.ColumnID1).Column.Name
                            + "=" + "main." + relationship.RelationshipColumns.First(y => column.ID == y.ColumnID1).Column1.Name;
                    }
                    else if (relationship.Entity1 == entity && relationship.RelationshipColumns.Any(y => column.ID == y.ColumnID2))
                    {
                        columnEqual1 = "fk." + relationship.RelationshipColumns.First(y => column.ID == y.ColumnID2).Column1.Name
                                 + "=" + "main." + relationship.RelationshipColumns.First(y => column.ID == y.ColumnID2).Column.Name;
                    }

                    conditionCheckExists += (conditionCheckExists == "" ? "" : " and ") + columnEqual1;
                }
                var baseQuerySub = "[" + entity.TableDrivedEntity.Table.Name + "] as fk";
                var subCriteriaWehere = (string.IsNullOrEmpty(entity.TableDrivedEntity.Criteria) ? "" : " where " + entity.TableDrivedEntity.Criteria);
                UnionQuery += (UnionQuery == "" ? "" : " Union All ") + "select " + unionColumns + " from " + baseQuerySub + subCriteriaWehere;
                whereClaus += (whereClaus == "" ? "" : " and ") + "not exists (select * from " + baseQuerySub + subCriteriaWehere + (subCriteriaWehere.Contains(" where ") ? " and " : " where ") + conditionCheckExists + ")";
            }

            var superRelationship = superType.Relationship.FirstOrDefault(x => x.RelationshipType != null && x.RelationshipType.SuperToSubRelationshipType != null && x.RelationshipType.SuperToSubRelationshipType.ISARelationship == iSARelationship.Item1);
            var columns = superType.TableDrivedEntity.Table.Column; //Helper.GetColumnList(superType);
            var groupBy = "";
            int col1 = 0;
            string columnEqual = "";
            foreach (var column in columns.Where(x => (superRelationship.Entity == superType && superRelationship.RelationshipColumns.Any(y => x.ID == y.ColumnID1)) ||
              (superRelationship.Entity1 == superType && superRelationship.RelationshipColumns.Any(y => x.ID == y.ColumnID2))
              ))
            {
                col1++;
                columnEqual = (columnEqual == "" ? "" : ",") + column.Name
                            + "=" + "col" + col1;
                groupBy += (groupBy == "" ? "" : ",") + column.Name;
            }



            using (SqlConnection testConn = new SqlConnection(ConnectionString))
            {
                testConn.Open();
                SqlTransaction trans;

                //try
                //{
                trans = testConn.BeginTransaction();
                var superCriteriaWhere = (string.IsNullOrEmpty(superType.TableDrivedEntity.Criteria) ? "" : " where " + superType.TableDrivedEntity.Criteria);
                var baseQuerySuper = "select top 1 1 from " + "[" + superType.TableDrivedEntity.Table.Name + "] as main" + superCriteriaWhere;
                var queryCheckExist = baseQuerySuper + superCriteriaWhere + (superCriteriaWhere.Contains(" where ") ? " and " : " where ") + whereClaus;

                bool ExistOneWithoutRelation = false;
                SqlCommand commandCheckExists = new SqlCommand(queryCheckExist, testConn, trans);
                var resCheckExists = commandCheckExists.ExecuteScalar();
                if (Convert.ToInt32(resCheckExists) > 0)
                {
                    ExistOneWithoutRelation = true;
                }
                iSARelationship.Item1.IsTolatParticipation = !ExistOneWithoutRelation;



                var query = baseQuerySuper + " inner join (" + UnionQuery + ") as subUnions on " + columnEqual + superCriteriaWhere + " group by " + groupBy + " having count(*)>1";
                bool ExistInBoth = false;
                SqlCommand commandExistInBoth = new SqlCommand(query, testConn, trans);
                commandExistInBoth.CommandTimeout = 60;
                var resExistInBoth = commandExistInBoth.ExecuteScalar();
                if (Convert.ToInt32(resExistInBoth) > 0)
                {
                    ExistInBoth = true;
                }
                iSARelationship.Item1.IsDisjoint = !ExistInBoth;



                //}
                //catch (Exception ex)
                //{
                //    throw ex;
                //}
            }
        }




        internal static void SetUnion_RelationshipProperties(Tuple<UnionRelationshipType, Entity, List<Entity>, TableRules.TableDrivedEntity_Relationships.Action.UnionKeyType> unionRelationship)
        {
            var ConnectionString = SQLHelper.GetConnectionString(unionRelationship.Item2.TableDrivedEntity.Table.Catalog);
            var unionType = unionRelationship.Item2;

            List<Entity> subTypes = unionRelationship.Item3;
            string subUnionStr = "";
            foreach (var entity in subTypes)
            {
                subUnionStr += (subUnionStr == "" ? "" : ",") + entity.Name;
            }
            unionRelationship.Item1.Name = unionType.Name + ">" + subUnionStr;

            //دراینجا از روش دیگری استفاده شده و بجای یک کوئری هر ساب تایپ مجزا چک میشود
            if (unionRelationship.Item4 == TableRules.TableDrivedEntity_Relationships.Action.UnionKeyType.SubUnionHoldsKeys)
            {
                unionRelationship.Item1.IsTolatParticipation = true;
                foreach (var entity in subTypes)
                {
                    var relationship = unionType.Relationship.FirstOrDefault(x => x.RelationshipType != null && x.RelationshipType.UnionToSubUnionRelationshipType != null && x.RelationshipType.UnionToSubUnionRelationshipType.UnionRelationshipType == unionRelationship.Item1
                       && x.Entity1 == entity);

                    var baseQuerySub = "[" + entity.TableDrivedEntity.Table.Name + "]";
                    var subCriteriaWehere = (string.IsNullOrEmpty(entity.TableDrivedEntity.Criteria) ? "" : " where " + entity.TableDrivedEntity.Criteria);


                    var columns2 = entity.TableDrivedEntity.Table.Column;// Helper.GetColumnList(entity);
                    var conditionCheckFKNull = "";


                    foreach (var column in columns2.Where(x => (relationship.Entity == entity && relationship.RelationshipColumns.Any(y => x.ID == y.ColumnID1)) ||
                    (relationship.Entity1 == entity && relationship.RelationshipColumns.Any(y => x.ID == y.ColumnID2))))
                    {
                        conditionCheckFKNull += (conditionCheckFKNull == "" ? "" : " and ") + column.Name + " is null";
                    }
                    using (SqlConnection testConn = new SqlConnection(ConnectionString))
                    {
                        testConn.Open();
                        SqlTransaction trans;

                        //try
                        //{
                        trans = testConn.BeginTransaction();
                        var queryCheckFKIsNull = baseQuerySub + subCriteriaWehere + (subCriteriaWehere.Contains(" where ") ? " and " : " where ") + " where " + conditionCheckFKNull;
                        SqlCommand commandCheckNull = new SqlCommand(queryCheckFKIsNull, testConn, trans);
                        var resCheckFKIsNull = commandCheckNull.ExecuteScalar();
                        if (Convert.ToInt32(resCheckFKIsNull) != 0)
                        {
                            unionRelationship.Item1.IsTolatParticipation = false;
                            testConn.Close();
                            break;
                        }


                        //}
                        //catch (Exception ex)
                        //{
                        //    throw ex;
                        //}
                    }
                }
            }
            else
            {
                unionRelationship.Item1.IsTolatParticipation = true;
                var whereClaus = "";
                foreach (var entity in subTypes)
                {
                    var relationship = unionType.Relationship.FirstOrDefault(x => x.RelationshipType != null && x.RelationshipType.UnionToSubUnionRelationshipType != null && x.RelationshipType.UnionToSubUnionRelationshipType.UnionRelationshipType == unionRelationship.Item1
                       && x.Entity1 == entity);

                    //var baseQuery = "select top 1 1 from " + entity.Name;


                    var columns2 = entity.TableDrivedEntity.Table.Column;// Helper.GetColumnList(entity);
                    var conditionCheckExists = "";

                    foreach (var column in columns2.Where(x => (relationship.Entity == entity && relationship.RelationshipColumns.Any(y => x.ID == y.ColumnID1)) ||
                         (relationship.Entity1 == entity && relationship.RelationshipColumns.Any(y => x.ID == y.ColumnID2))))
                    {
                        string columnEqual1 = "";
                        if (relationship.Entity == entity && relationship.RelationshipColumns.Any(y => column.ID == y.ColumnID1))
                        {
                            columnEqual1 = "main." + relationship.RelationshipColumns.First(y => column.ID == y.ColumnID1).Column.Name
                                + "=" + "fk." + relationship.RelationshipColumns.First(y => column.ID == y.ColumnID1).Column1.Name;
                        }
                        else if (relationship.Entity1 == entity && relationship.RelationshipColumns.Any(y => column.ID == y.ColumnID2))
                        {
                            columnEqual1 = "main." + relationship.RelationshipColumns.First(y => column.ID == y.ColumnID2).Column1.Name
                                     + "=" + "fk." + relationship.RelationshipColumns.First(y => column.ID == y.ColumnID2).Column.Name;
                        }

                        conditionCheckExists += (conditionCheckExists == "" ? "" : " and ") + columnEqual1;
                    }
                    var baseQuerySub = "[" + entity.TableDrivedEntity.Table.Name + "] as fk";
                    var subCriteriaWehere = (string.IsNullOrEmpty(entity.TableDrivedEntity.Criteria) ? "" : " where " + entity.TableDrivedEntity.Criteria);

                    whereClaus += (whereClaus == "" ? "" : " and ") + "not exists (select * from " + baseQuerySub + subCriteriaWehere + (subCriteriaWehere.Contains(" where ") ? " and " : " where ") + conditionCheckExists + ")";
                  
                }


                using (SqlConnection testConn = new SqlConnection(ConnectionString))
                {
                    testConn.Open();
                    SqlTransaction trans;

                    //try
                    //{
                    var superCriteriaWhere = (string.IsNullOrEmpty(unionRelationship.Item2.TableDrivedEntity.Criteria) ? "" : " where " + unionRelationship.Item2.TableDrivedEntity.Criteria);
                    var baseQuerySuper = "select top 1 1 from " + "[" + unionRelationship.Item2.TableDrivedEntity.Table.Name + "] as main" + superCriteriaWhere;

                    trans = testConn.BeginTransaction();
                    var queryCheckExist = baseQuerySuper + superCriteriaWhere + (superCriteriaWhere.Contains(" where ") ? " and " : " where ") + whereClaus;

                    SqlCommand commandCheckExists = new SqlCommand(queryCheckExist, testConn, trans);
                    var resCheckExists = commandCheckExists.ExecuteScalar();
                    if (Convert.ToInt32(resCheckExists) > 0)
                    {
                        unionRelationship.Item1.IsTolatParticipation = false;
                        testConn.Close();
                        break;
                    }
                    //}
                    //catch (Exception ex)
                    //{
                    //    throw ex;
                    //}
                }
            }
        }
    }
    public class EntityRelationInfo
    {
        public EntityRelationInfo()
        {
            RelationInfos = new List<RelationInfo>();
        }
        public Entity Entity { set; get; }
        //public bool? EntityHasDate { set; get; }
        //public List<Tuple<Relationship, Entity, bool, bool, bool>> ManyDataItems { set; get; }
        //public List<Tuple<Relationship, Entity, bool, bool, bool>> OneDataItems { set; get; }
        //public List<Tuple<Relationship, Entity, bool, bool, bool>> ManyToOneDataItems { set; get; }
        //public List<Tuple<Relationship, Entity, bool, bool, bool>> OneToOneDataItems { set; get; }
        public List<RelationInfo> RelationInfos { set; get; }
    }

    public class RelationInfo
    {
        public RelationInfo(Relationship relationship, Entity otherSideEntity, bool allPrimarySideHasFkSideData
            , bool fKColumnIsMandatory, bool fKRelatesOnPrimaryKey, bool fKRelatesOnPartOfPrimaryKey, bool fKHasData)
        {
            Relationship = relationship;
            OtherSideEntity = otherSideEntity;
            AllPrimarySideHasFkSideData = allPrimarySideHasFkSideData;
            FKColumnIsMandatory = fKColumnIsMandatory;
            FKRelatesOnPrimaryKey = fKRelatesOnPrimaryKey;
            FKRelatesOnPartOfPrimaryKey = fKRelatesOnPartOfPrimaryKey;
            FKHasData = fKHasData;
        }
        public Relationship Relationship { set; get; }
        public Entity OtherSideEntity { set; get; }
        public bool AllPrimarySideHasFkSideData { set; get; }
        public bool FKColumnIsMandatory { set; get; }
        public bool FKRelatesOnPrimaryKey { set; get; }

        public bool FKRelatesOnPartOfPrimaryKey { set; get; }

        public bool FKHasData { set; get; }

        public RelationType RelationType { set; get; }
    }
    public enum RelationType
    {
        ManyDataItems,
        OneDataItems,
        ManyToOneDataItems,
        OneToOneDataItems
    }
    public class EntityInfo
    {
        public EntityInfo()
        {
            ManyDataItems = new List<Tuple<Entity, bool>>();
            OneDataItems = new List<Tuple<Entity, bool>>();
            ManyToOneDataItems = new List<Tuple<Entity, bool>>();
            OneToOneDataItems = new List<Tuple<Entity, bool>>();
        }
        public Entity Entity { set; get; }
        public bool? EntityHasDate { set; get; }
        public List<Tuple<Entity, bool>> ManyDataItems { set; get; }
        public List<Tuple<Entity, bool>> OneDataItems { set; get; }

        public List<Tuple<Entity, bool>> ManyToOneDataItems { set; get; }
        public List<Tuple<Entity, bool>> OneToOneDataItems { set; get; }
    }
    public class PrimaryEntityInfo
    {
        public PrimaryEntityInfo()
        {
            ManyDataItems = new List<Tuple<Entity, bool>>();
            OneDataItems = new List<Tuple<Entity, bool>>();

        }
        public Entity Entity { set; get; }
        public bool? EntityHasDate { set; get; }
        public List<Tuple<Entity, bool>> ManyDataItems { set; get; }
        public List<Tuple<Entity, bool>> OneDataItems { set; get; }

    }
    public class SecondaryEntityInfo
    {
        public SecondaryEntityInfo()
        {

            ManyToOneDataItems = new List<Tuple<Entity, bool>>();
            OneToOneDataItems = new List<Tuple<Entity, bool>>();
        }
        public Entity Entity { set; get; }
        public bool? EntityHasDate { set; get; }

        public List<Tuple<Entity, bool>> ManyToOneDataItems { set; get; }
        public List<Tuple<Entity, bool>> OneToOneDataItems { set; get; }
    }

    //public class EntityInfo
    //{
    //    public EntityInfo()
    //    {
    //        ManyDataItems = new List<Tuple<Relationship, Entity, bool>>();
    //        OneDataItems = new List<Tuple<Relationship, Entity, bool>>();
    //    }
    //    public Entity Entity { set; get; }
    //    public bool? EntityHasDate { set; get; }
    //    public List<Tuple<Relationship, Entity, bool>> ManyDataItems { set; get; }
    //    public List<Tuple<Relationship, Entity, bool>> OneDataItems { set; get; }
    //}
}
