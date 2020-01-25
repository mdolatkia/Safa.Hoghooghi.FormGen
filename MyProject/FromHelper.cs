using AutoMapper;
using DataAccess;
using MyDataManagerBusiness;
using MyRuleEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyProject
{
    public class FormHelper
    {
        public frmMain View { set; get; }
        public FormHelper(frmMain frmSQLServerGenerator)
        {
            View = frmSQLServerGenerator;
        }

        internal void GetTables(string serverName, string dbName)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                //projectContext.Configuration.LazyLoadingEnabled = false;
                string catalogName = GeneralHelper.GetCatalogName(serverName, dbName);
                var list = projectContext.Table.Where(x => x.Catalog == catalogName).ToList();
                View.SetTables(list);
            }
        }
        internal void UpdateTables(object dataSource)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                foreach (var table in (dataSource as List<Table>))
                {
                    var dbTable = projectContext.Table.First(x => x.ID == table.ID);
                    dbTable.Alias = table.Alias;
                    //  dbTable.BatchDataEntry = table.BatchDataEntry;
                    //  dbTable.IsAssociative = table.IsAssociative;
                    dbTable.IsInheritanceImplementation = table.IsInheritanceImplementation;
                    //   dbTable.IsDataReference = table.IsDataReference;
                    //   dbTable.IsStructurReferencee = table.IsStructurReferencee;
                }
                projectContext.SaveChanges();
            }
        }
        internal void GetColumns(int tableID)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                //projectContext.Configuration.LazyLoadingEnabled = false;
                var list = projectContext.Column.Where(x => x.TableID == tableID).ToList();
                View.SetColumns(list);
            }
        }
        internal void UpdateColumns(object dataSource)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                foreach (var column in (dataSource as List<Column>))
                {
                    var dbColumn = projectContext.Column.First(x => x.ID == column.ID);
                    dbColumn.Alias = column.Alias;
                    dbColumn.DataEntryEnabled = column.DataEntryEnabled;
                    dbColumn.DefaultValue = column.DefaultValue;
                    dbColumn.IsMandatory = column.IsMandatory;
                    dbColumn.Position = column.Position;
                    dbColumn.SearchEnabled = column.SearchEnabled;
                    //dbColumn.ViewAggregatedData = column.ViewAggregatedData;
                    dbColumn.ViewEnabled = column.ViewEnabled;
                }
                projectContext.SaveChanges();
            }
        }

        internal void GetKeyValue(int columnID)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                //projectContext.Configuration.LazyLoadingEnabled = false;
                var column = projectContext.Column.First(x => x.ID == columnID);
                View.SetColumnKeyValue(column.ColumnKeyValue);
            }
        }
        internal void UpdateColumnKeyValue(int columnID, bool ValueFromKeyOrValue, object dataSource)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var column = projectContext.Column.First(x => x.ID == columnID);
                if ((dataSource as List<ColumnKeyValueRange>).Count > 0)
                {
                    if (column.ColumnKeyValue == null)
                        column.ColumnKeyValue = new ColumnKeyValue();
                    column.ColumnKeyValue.ValueFromKeyOrValue = ValueFromKeyOrValue;
                    while (column.ColumnKeyValue.ColumnKeyValueRange.Any())
                        projectContext.ColumnKeyValueRange.Remove(column.ColumnKeyValue.ColumnKeyValueRange.First());
                    foreach (var keyValueRange in (dataSource as List<ColumnKeyValueRange>))
                    {
                        column.ColumnKeyValue.ColumnKeyValueRange.Add(new ColumnKeyValueRange() { Value = keyValueRange.Value, KeyTitle = keyValueRange.KeyTitle });
                    }
                }
                else
                {
                    if (column.ColumnKeyValue != null)
                    {
                        while (column.ColumnKeyValue.ColumnKeyValueRange.Any())
                            projectContext.ColumnKeyValueRange.Remove(column.ColumnKeyValue.ColumnKeyValueRange.First());
                        projectContext.ColumnKeyValue.Remove(column.ColumnKeyValue);

                    }
                }
                projectContext.SaveChanges();
            }
        }

        internal void ImportColumnKeyValues(string connectionString, int columnID, bool valueFromKey)
        {
            SQLServerHelper helper = new SQLServerHelper();
       
            //try
            //{
            var result = helper.GenerateKeyColumns(connectionString, columnID, valueFromKey);
            if (result)
            {
                GetKeyValue(columnID);
                MessageBox.Show("Operation is completed.");
            }
            else
                MessageBox.Show("Operation is not done!");

            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var column = projectContext.Column.First(x => x.ID == columnID);

            }

        }
        internal void GetColumnType(int columnID)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                //projectContext.Configuration.LazyLoadingEnabled = false;
                var column = projectContext.Column.First(x => x.ID == columnID);

                if (column != null)
                {
                    View.RemoveColumnType();
                    if (column.StringColumnType != null)
                    {
                        View.SetStringColumnType(GeneralHelper.CreateListFromSingleObject<StringColumnType>(column.StringColumnType));
                    }
                    else if (column.NumericColumnType != null)
                    {
                        View.SetNumericColumnType(GeneralHelper.CreateListFromSingleObject<NumericColumnType>(column.NumericColumnType));
                    }
                    else if (column.DateColumnType != null)
                    {
                        View.SetDateColumnType(GeneralHelper.CreateListFromSingleObject<DateColumnType>(column.DateColumnType));
                    }
                }


            }
        }
        internal enum_ColumnType GetTypeOfColumn(int columnID)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                //projectContext.Configuration.LazyLoadingEnabled = false;
                var column = projectContext.Column.First(x => x.ID == columnID);

                if (column != null)
                {
                    if (column.StringColumnType != null)
                    {
                        return enum_ColumnType.StringColumnType;
                    }
                    else if (column.NumericColumnType != null)
                    {
                        return enum_ColumnType.NumericColumnType;
                    }
                    else if (column.DateColumnType != null)
                    {
                        return enum_ColumnType.DataColumnType;
                    }
                }
                return enum_ColumnType.None;
            }
        }
        internal void UpdateNumericColumnType(object dataSource)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                foreach (var column in (dataSource as List<NumericColumnType>))
                {
                    var dbColumn = projectContext.NumericColumnType.First(x => x.ColumnID == column.ColumnID);
                    dbColumn.MaxValue = column.MaxValue;
                    dbColumn.MinValue = column.MinValue;
                    dbColumn.Precision = column.Precision;
                    dbColumn.Scale = column.Scale;
                }
                projectContext.SaveChanges();
            }
        }
        internal void UpdateDateColumnType(object dataSource)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                foreach (var column in (dataSource as List<DateColumnType>))
                {
                    var dbColumn = projectContext.DateColumnType.First(x => x.ColumnID == column.ColumnID);
                    dbColumn.IsPersianDate = column.IsPersianDate;
                }
                projectContext.SaveChanges();
            }
        }

        internal void UpdateStringColumnType(object dataSource)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                foreach (var column in (dataSource as List<StringColumnType>))
                {
                    var dbColumn = projectContext.StringColumnType.First(x => x.ColumnID == column.ColumnID);
                    dbColumn.MaxLength = column.MaxLength;
                    dbColumn.Format = column.Format;
                }
                projectContext.SaveChanges();
            }
        }

        internal void ConvertToStringColumnType(int columnID)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var dbColumn = projectContext.DateColumnType.First(x => x.ColumnID == columnID);
                if (dbColumn.Column.DateColumnType != null)
                    dbColumn.Column.DateColumnType = null;
                if (dbColumn.Column.NumericColumnType != null)
                    dbColumn.Column.NumericColumnType = null;
                if (dbColumn.Column.StringColumnType == null)
                    dbColumn.Column.StringColumnType = new StringColumnType();
                projectContext.SaveChanges();
            }
        }

        internal void ConvertToDateColumnType(int columnID)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var dbColumn = projectContext.StringColumnType.First(x => x.ColumnID == columnID);
                if (dbColumn.Column.StringColumnType != null)
                    dbColumn.Column.StringColumnType = null;
                if (dbColumn.Column.NumericColumnType != null)
                    dbColumn.Column.NumericColumnType = null;
                if (dbColumn.Column.DateColumnType == null)
                    dbColumn.Column.DateColumnType = new DateColumnType();
                projectContext.SaveChanges();
            }
        }
        internal void EditColumnColumnValue(int columnID)
        {
            frmColumnValue view = new frmColumnValue(columnID);
            view.ShowDialog();
        }


        internal void GetUniqueConstraints(string serverName, string dbName)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                //projectContext.Configuration.LazyLoadingEnabled = false;
                string catalogName = GeneralHelper.GetCatalogName(serverName, dbName);
                var listUniqueConstraint = projectContext.UniqueConstraint.Where(x => x.Table.Catalog == catalogName);
                List<UniqueConstraintDTO> list = new List<UniqueConstraintDTO>();
                foreach (var item in listUniqueConstraint)
                    list.Add(ToUniqueConstraintDTO(item));
                View.UniqueConstraints(list);
            }
        }

        private UniqueConstraintDTO ToUniqueConstraintDTO(UniqueConstraint item)
        {
            UniqueConstraintDTO result = new UniqueConstraintDTO();
            result.Name = item.Name;
            result.ID = item.ID;
            result.Table = item.Table.Name;
            result.Columns = "";
            foreach (var column in item.Column)
            {
                result.Columns += (result.Columns == "" ? "" : ",") + column.Name;
            }
            return result;
        }

        internal void GetEntities(string serverName, string dbName)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                //projectContext.Configuration.LazyLoadingEnabled = false;
                string catalogName = GeneralHelper.GetCatalogName(serverName, dbName);
                var listEntity = projectContext.TableDrivedEntity.Where(x => x.Table.Catalog == catalogName);
                List<EntityDTO> list = new List<EntityDTO>();
                foreach (var item in listEntity)
                    list.Add(ToEntityDTO(item));
                View.SetEntities(list);
            }
        }

        internal void ImposeEntitTableRule(string serverName, string dbName)
        {
            string catalogName = GeneralHelper.GetCatalogName(serverName, dbName);
            MyProjectEntities context = new MyProjectEntities();
            context.Configuration.LazyLoadingEnabled = true;
            var list = context.TableDrivedEntity.Where(x => x.Table.Catalog == catalogName);
            var count = list.Count();
            int index = 0;
            foreach (var entity in list)
            {
                index++;
                View.SetInfo(count, index, entity.Name);
                Biz_RuleSet myruleSet = new Biz_RuleSet("TableDrivedEntity_IsReference");
                myruleSet.Execute(entity);
            }
            //index = 0;
            //foreach (var entity in list)
            //{
            //    index++;
            //    View.SetInfo(count, index, entity.Name);
            //    Biz_RuleSet myruleSet = new Biz_RuleSet("TableDrivedEntity_IsAssociative");
            //    myruleSet.Execute(entity);
            //}
            //index = 0;
            //foreach (var entity in list)
            //{
            //    index++;
            //    View.SetInfo(count, index, entity.Name);
            //    Biz_RuleSet myruleSet = new Biz_RuleSet("TableDrivedEntity_IndependentDataEntry");
            //    myruleSet.Execute(entity);
            //}

            context.SaveChanges();
            View.SetInfo(count, index, "Operation is completed.");
            MessageBox.Show("Operation is completed.");
        }

        internal void GetRuleEntities(string serverName, string dbName)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                //projectContext.Configuration.LazyLoadingEnabled = false;
                string catalogName = GeneralHelper.GetCatalogName(serverName, dbName);
                var listEntity = projectContext.TableDrivedEntity.Where(x => x.Table.Catalog == catalogName);
                List<EntityDTO> list = new List<EntityDTO>();
                foreach (var item in listEntity)
                    list.Add(ToEntityDTO(item));

                View.SetRuleEntities(list);
            }
        }
        internal void UpdateRuleEntities(object dataSource)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                foreach (var entity in (dataSource as List<EntityDTO>))
                {
                    var dbEntity = projectContext.TableDrivedEntity.First(x => x.ID == entity.ID);
                    dbEntity.Alias = entity.Alias;
                    dbEntity.Name = entity.Name;
                    dbEntity.Criteria = entity.Criteria;
                    dbEntity.IndependentDataEntry = entity.IndependentDataEntry;
                    dbEntity.BatchDataEntry = entity.BatchDataEntry;
                    dbEntity.IsAssociative = entity.IsAssociative;
                    dbEntity.IsDataReference = entity.IsDataReference;
                    dbEntity.IsStructurReferencee = entity.IsStructurReferencee;
                }
                projectContext.SaveChanges();
            }
        }
        private EntityDTO ToEntityDTO(TableDrivedEntity item)
        {
            var result = new EntityDTO();
            result.ID = item.ID;
            result.Name = item.Name;
            result.TableID = item.TableID;
            result.Table = item.Table.Name;
            result.Alias = item.Alias; result.Criteria = item.Criteria;
            result.IndependentDataEntry = item.IndependentDataEntry;



            if (result.UnionTypeEntities == "")
                if (item.Relationship.Any(x => (x.RelationshipType == null && x.Relationship2 != null && x.TableDrivedEntity != x.TableDrivedEntity1 && !x.RelationshipColumns.All(y => y.Column.PrimaryKey == true))
                    || (x.Relationship2 == null && x.TableDrivedEntity != x.TableDrivedEntity1 && !x.RelationshipColumns.All(y => y.Column1.PrimaryKey == true))))
                    result.UnionTypeEntities = "Choose UnionType";

            result.BatchDataEntry = item.BatchDataEntry;
            result.IsAssociative = item.IsAssociative;
            result.IsDataReference = item.IsDataReference;
            result.IsStructurReferencee = item.IsStructurReferencee;
            return result;
        }



        internal void DefineArcRelationships(EntityDTO entity)
        {
            frmArcRelationships view = new frmArcRelationships(entity.ID);
            view.ShowDialog();
        }


        internal void ImposeRelationshipRule(string serverName, string dbName)
        {
            string catalogName = GeneralHelper.GetCatalogName(serverName, dbName);
            MyProjectEntities context = new MyProjectEntities();
            context.Configuration.LazyLoadingEnabled = true;
            var list = context.TableDrivedEntity.Where(x => x.Table.Catalog == catalogName);
            var count = list.Count();
            int index = 0;
            foreach (var entity in list)
            {
                index++;
                View.SetInfo(count, index, entity.Name);
                Biz_RuleSet myruleSet = new Biz_RuleSet("RuleSet3");
                //myruleSet.ActionEvent += View.myruleSet_ActionEvent;
                myruleSet.Execute(entity, context);

            }
            try
            {
                context.SaveChanges();
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
            View.SetInfo(count, index, "Operation is completed.");
            MessageBox.Show("Operation is completed.");
        }

        internal void GetRuleRelationships(string serverName, string dbName)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                //projectContext.Configuration.LazyLoadingEnabled = false;
                string catalogName = GeneralHelper.GetCatalogName(serverName, dbName);
                var list = projectContext.Relationship.Where(x => x.TableDrivedEntity.Table.Catalog == catalogName);
                List<RelationshipDTO> result = new List<RelationshipDTO>();
                foreach (var item in list)
                {
                    RelationshipDTO rItem = GeneralHelper.ToRelationshipDTO(item);
                    result.Add(rItem);
                }



                View.SetRuleRelationships(result);
            }
        }
        internal void UpdateRuleRelationships(object dataSource)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                foreach (var relationship in (dataSource as List<RelationshipDTO>))
                {
                    var dbRelationship = projectContext.Relationship.First(x => x.ID == relationship.ID);
                    dbRelationship.Alias = relationship.Alias;
                    dbRelationship.Name = relationship.Name;
                    dbRelationship.DataEntryEnabled = relationship.DataEntryEnabled;
                    dbRelationship.Enabled = relationship.Enabled;
                    dbRelationship.SearchEnabled = relationship.SearchEnabled;
                    dbRelationship.ViewEnabled = relationship.ViewEnabled;
                }
                projectContext.SaveChanges();
            }
        }


        internal void GetOneToManyRelationships(string serverName, string dbName)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                //projectContext.Configuration.LazyLoadingEnabled = false;
                string catalogName = GeneralHelper.GetCatalogName(serverName, dbName);
                var list = projectContext.Relationship.Where(x => x.TableDrivedEntity.Table.Catalog == catalogName
                    && x.RelationshipType != null && x.RelationshipType.OneToManyRelationshipType != null);

                List<OneToMany> OneToManyList = new List<OneToMany>();
                foreach (var item in list)
                {
                    RelationshipDTO rItem = GeneralHelper.ToRelationshipDTO(item);
                    OneToManyList.Add(ToOneToMany(rItem, item.RelationshipType.OneToManyRelationshipType));

                }
                View.SetRuleRelationships(OneToManyList);
            }
        }
        internal void UpdateOneToManyRelationships(object dataSource)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                foreach (var relationship in (dataSource as List<OneToMany>))
                {
                    var dbRelationship = projectContext.OneToManyRelationshipType.First(x => x.ID == relationship.ID);
                    dbRelationship.RelationshipType.IsOtherSideDirectlyCreatable = relationship.IsManySideDirectlyCreatable;
                    dbRelationship.RelationshipType.IsOtherSideCreatable = relationship.IsManySideCreatable;
                    dbRelationship.RelationshipType.IsOtherSideMandatory = relationship.IsManySideMadatory;
                    dbRelationship.MasterDetailState = relationship.MasterDetailState;
                    dbRelationship.ManySideCount = relationship.DetailsCount;
                    dbRelationship.RelationshipType.Relationship.Name = relationship.Name;
                    dbRelationship.RelationshipType.Relationship.Alias = relationship.Alias;
                    dbRelationship.RelationshipType.Relationship.Enabled = relationship.Enabled;
                }
                projectContext.SaveChanges();
            }
        }
        private OneToMany ToOneToMany(RelationshipDTO rItem, OneToManyRelationshipType oneToManyRelationshipType)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<RelationshipDTO, OneToMany>());

            var result = AutoMapper.Mapper.Map<RelationshipDTO, OneToMany>(rItem);
            result.MasterDetailState = oneToManyRelationshipType.MasterDetailState;
            result.DetailsCount = oneToManyRelationshipType.ManySideCount;
            result.IsManySideMadatory = oneToManyRelationshipType.RelationshipType.IsOtherSideMandatory;
            result.IsManySideCreatable = oneToManyRelationshipType.RelationshipType.IsOtherSideCreatable;
            result.IsManySideDirectlyCreatable = oneToManyRelationshipType.RelationshipType.IsOtherSideDirectlyCreatable;

            return result;
        }

        internal void UpdateManyToOneRelationships(object dataSource)
        {

            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                foreach (var relationship in (dataSource as List<ManyToOne>))
                {
                    var dbRelationship = projectContext.ManyToOneRelationshipType.First(x => x.ID == relationship.ID);
                    dbRelationship.RelationshipType.IsOtherSideCreatable = relationship.IsOneSideCreatable;
                    dbRelationship.RelationshipType.IsOtherSideDirectlyCreatable = relationship.IsOneSideDirectlyCreatable;
                    dbRelationship.RelationshipType.IsOtherSideMandatory = relationship.IsOneSideMadatory;
                    dbRelationship.RelationshipType.IsOtherSideTransferable = relationship.IsOneSideTransferable;
                    dbRelationship.RelationshipType.Relationship.Name = relationship.Name;
                    dbRelationship.RelationshipType.Relationship.Alias = relationship.Alias;
                    dbRelationship.RelationshipType.Relationship.Enabled = relationship.Enabled;
                }
                projectContext.SaveChanges();
            }

        }

        internal void GetManyToOneRelationships(string serverName, string dbName)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                //projectContext.Configuration.LazyLoadingEnabled = false;
                string catalogName = GeneralHelper.GetCatalogName(serverName, dbName);
                var list = projectContext.Relationship.Where(x => x.TableDrivedEntity.Table.Catalog == catalogName
                    && x.RelationshipType != null && x.RelationshipType.ManyToOneRelationshipType != null);

                List<ManyToOne> ManyToOneList = new List<ManyToOne>();
                foreach (var item in list)
                {
                    RelationshipDTO rItem = GeneralHelper.ToRelationshipDTO(item);
                    ManyToOneList.Add(GeneralHelper.ToManyToOne(rItem, item.RelationshipType.ManyToOneRelationshipType));
                }
                View.SetRuleRelationships(ManyToOneList);
            }
        }
      


        internal void UpdateManyToManyRelationships(object dataSource)
        {

            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                foreach (var relationship in (dataSource as List<ManyToMany>))
                {
                    ManyToManyRelationshipType dbRelationship = projectContext.ManyToManyRelationshipType.First(x => x.ID == relationship.ID);
                    dbRelationship.Name = relationship.Name;
                }
                projectContext.SaveChanges();
            }

        }

        internal void GetManyToManyRelationships(string serverName, string dbName)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                //projectContext.Configuration.LazyLoadingEnabled = false;
                string catalogName = GeneralHelper.GetCatalogName(serverName, dbName);
                var list = projectContext.ManyToManyRelationshipType.Where(x => x.ManyToOneRelationshipType.Any(y => y.RelationshipType.Relationship.TableDrivedEntity.Table.Catalog == catalogName));


                List<ManyToMany> ManyToManyList = new List<ManyToMany>();
                foreach (var item in list)
                {
                    ManyToManyList.Add(ToManyToMany(item));
                }
                View.SetManyToManyRelationships(ManyToManyList);
            }
        }
        private ManyToMany ToManyToMany(ManyToManyRelationshipType ManyToManyRelationshipType)
        {
            var result = new ManyToMany();
            result.Name = ManyToManyRelationshipType.Name;
            result.ID = ManyToManyRelationshipType.ID;
            return result;
        }

        internal void GetManyToMany_ManyToOneRelationships(int manyToManyId)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                //projectContext.Configuration.LazyLoadingEnabled = false;
                var manyToOneList = projectContext.ManyToOneRelationshipType.Where(x => x.ManyToManyRelationshipTypeID == manyToManyId);
                if (manyToOneList != null)
                {
                    List<ManyToOne> ManyToOneList = new List<ManyToOne>();
                    foreach (var item in manyToOneList)
                    {
                        RelationshipDTO rItem = GeneralHelper.ToRelationshipDTO(item.RelationshipType.Relationship);
                        ManyToOneList.Add(GeneralHelper.ToManyToOne(rItem, item));
                    }

                    View.SetManyToMany_ManyToOneRelationships(ManyToOneList);
                }

            }
        }
        internal void CreateManyToManyRelationships(string serverName, string dbName)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                string catalogName = GeneralHelper.GetCatalogName(serverName, dbName);
                var list = projectContext.Table.Where(x => x.Catalog == catalogName);
                list=list.Where(x=>x.TableDrivedEntity.Any(y=>y.Relationship.Count(z=>z.RelationshipType!=null && z.RelationshipType.ManyToOneRelationshipType!=null)>0));
                var selectView = new frmChooseItem<Table>(list.ToList(), false);
                selectView.ItemSelected += selectView_ItemSelected;
                selectView.Text = "Select Associative Table";
                selectView.ShowDialog();
            }

        }

        void selectView_ItemSelected(object sender, SelectedItemArg<Table> e)
        {
            frmCreateManyToManyRelationship view = new frmCreateManyToManyRelationship(e.Items.First().ID);
            view.ManyToManyCreated += view_ManyToManyCreated;
            view.ShowDialog();
        }

        void view_ManyToManyCreated(object sender, ManyToManyCreatedArg e)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                ManyToManyRelationshipType manyToManyRelationshipType = new ManyToManyRelationshipType();
                manyToManyRelationshipType.Name = e.Name;
                foreach (var id in e.ManyToOneIDs)
                    manyToManyRelationshipType.ManyToOneRelationshipType.Add(projectContext.ManyToOneRelationshipType.First(x => x.ID == id));

                var dbTable = projectContext.Table.First(x => x.ID == e.TableID);
                dbTable.IsAssociative = true;

                projectContext.ManyToManyRelationshipType.Add(manyToManyRelationshipType);
                projectContext.SaveChanges();

                var srvdb=GeneralHelper.GetServerNameDatabaseName( dbTable.Catalog);
                GetManyToManyRelationships(srvdb.Item1, srvdb.Item2);
            }
        }


        void selectView_ManyToOneSelected(object sender, SelectedItemArg<ManyToOne> e, ManyToMany manyToMany)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var manyToManyDB = projectContext.ManyToManyRelationshipType.First(x => x.ID == manyToMany.ID);
                foreach (var item in e.Items)
                {
                    if (!manyToManyDB.ManyToOneRelationshipType.Any(x => x.ID == item.ID))
                    {
                        var manyToOneDB = projectContext.ManyToOneRelationshipType.First(x => x.ID == item.ID);
                        manyToManyDB.ManyToOneRelationshipType.Add(manyToOneDB);
                    }
                }
                projectContext.SaveChanges();
            }
        }

        internal void RemoveManyToManyRelationships(int manyToManyID)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var manyToOneDB = projectContext.ManyToManyRelationshipType.First(x => x.ID == manyToManyID);
                while (manyToOneDB.ManyToOneRelationshipType.Any())
                    manyToOneDB.ManyToOneRelationshipType.Remove(manyToOneDB.ManyToOneRelationshipType.First());
                projectContext.ManyToManyRelationshipType.Remove(manyToOneDB);
                projectContext.SaveChanges();
                GetManyToManyRelationships(View.ServerName, View.DatabaseName);
            }
        }

        //internal void RemoveManyToOneFromManyToManyRelationship(ManyToOne entity)
        //{
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var manyToOneDB = projectContext.ManyToOneRelationshipType.First(x => x.ID == entity.ID);
        //        var manyToManyId = manyToOneDB.ManyToManyRelationshipTypeID;
        //        manyToOneDB.ManyToManyRelationshipType = null;
        //        projectContext.SaveChanges();
        //        GetManyToMany_ManyToOneRelationships(manyToManyId.Value);
        //    }
        //}

        internal void UpdateExplicitRelationships(object dataSource)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                foreach (var relationship in (dataSource as List<ExplicitOneToOne>))
                {
                    var dbRelationship = projectContext.ExplicitOneToOneRelationshipType.First(x => x.ID == relationship.ID);
                    dbRelationship.RelationshipType.IsOtherSideCreatable = relationship.IsOtherSideCreatable;
                    dbRelationship.RelationshipType.IsOtherSideDirectlyCreatable = relationship.IsOtherSideDirectlyCreatable;
                    dbRelationship.RelationshipType.IsOtherSideMandatory = relationship.IsOtherSideMandatory;
                    dbRelationship.RelationshipType.IsOtherSideTransferable = relationship.IsOtherSideTransferable;
                    dbRelationship.RelationshipType.Relationship.Name = relationship.Name;
                    dbRelationship.RelationshipType.Relationship.Alias = relationship.Alias;
                    dbRelationship.RelationshipType.Relationship.Enabled = relationship.Enabled;
                }
                projectContext.SaveChanges();
            }
        }
        internal void GetExplicitOneToOneRelationships(string serverName, string dbName)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                //projectContext.Configuration.LazyLoadingEnabled = false;
                string catalogName = GeneralHelper.GetCatalogName(serverName, dbName);
                var list = projectContext.Relationship.Where(x => x.TableDrivedEntity.Table.Catalog == catalogName
                    && x.RelationshipType != null && x.RelationshipType.ExplicitOneToOneRelationshipType != null);

                List<ExplicitOneToOne> ExplicitOneToOneList = new List<ExplicitOneToOne>();
                foreach (var item in list)
                {
                    RelationshipDTO rItem = GeneralHelper.ToRelationshipDTO(item);
                    ExplicitOneToOneList.Add(ToExplicitOneToOne(rItem, item.RelationshipType.ExplicitOneToOneRelationshipType));
                }
                View.SetRuleRelationships(ExplicitOneToOneList);
            }
        }
        private ExplicitOneToOne ToExplicitOneToOne(RelationshipDTO rItem, ExplicitOneToOneRelationshipType explicitOneToOneRelationshipType)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<RelationshipDTO, ExplicitOneToOne>());
            var result = AutoMapper.Mapper.Map<RelationshipDTO, ExplicitOneToOne>(rItem);
            result.IsOtherSideTransferable = explicitOneToOneRelationshipType.RelationshipType.IsOtherSideTransferable;
            result.IsOtherSideCreatable = explicitOneToOneRelationshipType.RelationshipType.IsOtherSideCreatable;
            result.IsOtherSideMandatory = explicitOneToOneRelationshipType.RelationshipType.IsOtherSideMandatory;
            result.IsOtherSideDirectlyCreatable = explicitOneToOneRelationshipType.RelationshipType.IsOtherSideDirectlyCreatable;

            return result;
        }


        internal void GetImplicitOneToOneRelationships(string serverName, string dbName)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                //projectContext.Configuration.LazyLoadingEnabled = false;
                string catalogName = GeneralHelper.GetCatalogName(serverName, dbName);
                var list = projectContext.Relationship.Where(x => x.TableDrivedEntity.Table.Catalog == catalogName
                    && x.RelationshipType != null && x.RelationshipType.ImplicitOneToOneRelationshipType != null);

                List<ImplicitOneToOne> ImplicitOneToOneList = new List<ImplicitOneToOne>();
                foreach (var item in list)
                {
                    RelationshipDTO rItem = GeneralHelper.ToRelationshipDTO(item);
                    ImplicitOneToOneList.Add(ToImplicitOneToOne(rItem, item.RelationshipType.ImplicitOneToOneRelationshipType));
                }
                View.SetRuleRelationships(ImplicitOneToOneList);
            }
        }




        internal void UpdateImplicitRelationships(object dataSource)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                foreach (var relationship in (dataSource as List<ImplicitOneToOne>))
                {
                    var dbRelationship = projectContext.ImplicitOneToOneRelationshipType.First(x => x.ID == relationship.ID);
                    dbRelationship.RelationshipType.IsOtherSideCreatable = relationship.IsOtherSideCreatable;
                    dbRelationship.RelationshipType.IsOtherSideDirectlyCreatable = relationship.IsOtherSideDirectlyCreatable;
                    dbRelationship.RelationshipType.IsOtherSideMandatory = relationship.IsOtherSideMandatory;
                    dbRelationship.RelationshipType.IsOtherSideTransferable = relationship.IsOtherSideTransferable;
                    dbRelationship.RelationshipType.Relationship.Name = relationship.Name;
                    dbRelationship.RelationshipType.Relationship.Alias = relationship.Alias;
                    dbRelationship.RelationshipType.Relationship.Enabled = relationship.Enabled;

                }
                projectContext.SaveChanges();
            }
        }
        private ImplicitOneToOne ToImplicitOneToOne(RelationshipDTO rItem, ImplicitOneToOneRelationshipType ImplicitOneToOneRelationshipType)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<RelationshipDTO, ImplicitOneToOne>());
            var result = AutoMapper.Mapper.Map<RelationshipDTO, ImplicitOneToOne>(rItem);
            result.IsOtherSideTransferable = ImplicitOneToOneRelationshipType.RelationshipType.IsOtherSideTransferable;
            result.IsOtherSideCreatable = ImplicitOneToOneRelationshipType.RelationshipType.IsOtherSideCreatable;
            result.IsOtherSideMandatory = ImplicitOneToOneRelationshipType.RelationshipType.IsOtherSideMandatory;
            result.IsOtherSideDirectlyCreatable = ImplicitOneToOneRelationshipType.RelationshipType.IsOtherSideDirectlyCreatable;

            return result;
        }





        internal void GetISARelationships(string serverName, string dbName)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                string catalogName = GeneralHelper.GetCatalogName(serverName, dbName);
                List<ISARelationshipDTO> ISARelationshipList = new List<ISARelationshipDTO>();
                foreach (var item in projectContext.ISARelationship.Where(x => x.SuperToSubRelationshipType.Any(y => y.RelationshipType.Relationship.TableDrivedEntity.Table.Catalog == catalogName)))
                {
                    ISARelationshipDTO rItem = new ISARelationshipDTO();
                    rItem.SuperTypeEntities = "";
                    foreach (var superType in item.SuperToSubRelationshipType)
                    {
                        if (!rItem.SuperTypeEntities.Contains(superType.RelationshipType.Relationship.TableDrivedEntity.Name))
                            rItem.SuperTypeEntities += (rItem.SuperTypeEntities == "" ? "" : ",") + superType.RelationshipType.Relationship.TableDrivedEntity.Name;
                    }
                    rItem.SubTypeEntities = "";
                    foreach (var subType in item.SubToSuperRelationshipType)
                    {
                        rItem.SubTypeEntities += (rItem.SubTypeEntities == "" ? "" : ",") + subType.RelationshipType.Relationship.TableDrivedEntity.Name;
                    }
                    rItem.IsDisjoint = item.IsDisjoint;
                    rItem.IsGeneralization = item.IsGeneralization;
                    rItem.IsSpecialization = item.IsSpecialization;
                    rItem.IsTolatParticipation = item.IsTolatParticipation;
                    rItem.ID = item.ID;
                    rItem.Name = item.Name;
                    ISARelationshipList.Add(rItem);
                }
                View.SetRuleRelationships(ISARelationshipList);
            }
        }
        internal void UpdateISARelationships(object dataSource)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                foreach (var relationship in (dataSource as List<ISARelationshipDTO>))
                {
                    var dbRelationship = projectContext.ISARelationship.First(x => x.ID == relationship.ID);
                    dbRelationship.IsDisjoint = relationship.IsDisjoint;
                    dbRelationship.IsGeneralization = relationship.IsGeneralization;
                    dbRelationship.IsSpecialization = relationship.IsSpecialization;
                    dbRelationship.IsTolatParticipation = relationship.IsTolatParticipation;
                    dbRelationship.Name = relationship.Name;
                }
                projectContext.SaveChanges();
            }
        }

        internal void GetSubSuperRelationshipTypes(int iSARelationshipID)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                //projectContext.Configuration.LazyLoadingEnabled = false;
                var iSaRelationship = projectContext.ISARelationship.FirstOrDefault(x => x.ID == iSARelationshipID);
                if (iSaRelationship != null)
                {
                    List<SuperToSub> SuperToSubList = new List<SuperToSub>();
                    List<SubToSuper> SubToSuperList = new List<SubToSuper>();
                    foreach (var item in iSaRelationship.SuperToSubRelationshipType)
                    {
                        var rItem = GeneralHelper.ToRelationshipDTO(item.RelationshipType.Relationship);
                        SuperToSubList.Add(ToSuperToSub(rItem, item));
                    }
                    foreach (var item in iSaRelationship.SubToSuperRelationshipType)
                    {
                        var rItem = GeneralHelper.ToRelationshipDTO(item.RelationshipType.Relationship);
                        SubToSuperList.Add(ToSubToSuper(rItem, item));
                    }

                    View.SetSubSuperRelationships(SuperToSubList, SubToSuperList);
                }

            }
        }

        private SuperToSub ToSuperToSub(RelationshipDTO rItem, SuperToSubRelationshipType item)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<RelationshipDTO, SuperToSub>());
            var result = AutoMapper.Mapper.Map<RelationshipDTO, SuperToSub>(rItem);
            result.IsOtherSideTransferable = item.RelationshipType.IsOtherSideTransferable;
            result.IsOtherSideCreatable = item.RelationshipType.IsOtherSideCreatable;
            result.IsOtherSideDirectlyCreatable = item.RelationshipType.IsOtherSideDirectlyCreatable;

            return result;
        }

        private SubToSuper ToSubToSuper(RelationshipDTO rItem, SubToSuperRelationshipType item)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<RelationshipDTO, SubToSuper>());
            var result = AutoMapper.Mapper.Map<RelationshipDTO, SubToSuper>(rItem);
            result.IsOtherSideTransferable = item.RelationshipType.IsOtherSideTransferable;
            result.IsOtherSideCreatable = item.RelationshipType.IsOtherSideCreatable;
            result.IsOtherSideDirectlyCreatable = item.RelationshipType.IsOtherSideDirectlyCreatable;

            return result;
        }
        internal void UpdateSuperToSubRelationships(object dataSource)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                foreach (var relationship in (dataSource as List<SuperToSub>))
                {
                    var dbRelationship = projectContext.SuperToSubRelationshipType.First(x => x.ID == relationship.ID);
                    dbRelationship.RelationshipType.IsOtherSideCreatable = relationship.IsOtherSideCreatable;
                    dbRelationship.RelationshipType.IsOtherSideDirectlyCreatable = relationship.IsOtherSideDirectlyCreatable;
                    dbRelationship.RelationshipType.IsOtherSideTransferable = relationship.IsOtherSideTransferable;
                    dbRelationship.RelationshipType.Relationship.Name = relationship.Name;
                    dbRelationship.RelationshipType.Relationship.Alias = relationship.Alias;
                    dbRelationship.RelationshipType.Relationship.Enabled = relationship.Enabled;
                }
                projectContext.SaveChanges();
            }
        }

        internal void UpdateSubToSuperRelationships(object dataSource)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                foreach (var relationship in (dataSource as List<SubToSuper>))
                {
                    var dbRelationship = projectContext.SubToSuperRelationshipType.First(x => x.ID == relationship.ID);
                    dbRelationship.RelationshipType.IsOtherSideCreatable = relationship.IsOtherSideCreatable;
                    dbRelationship.RelationshipType.IsOtherSideDirectlyCreatable = relationship.IsOtherSideDirectlyCreatable;
                    dbRelationship.RelationshipType.IsOtherSideTransferable = relationship.IsOtherSideTransferable;
                    dbRelationship.RelationshipType.Relationship.Name = relationship.Name;
                    dbRelationship.RelationshipType.Relationship.Alias = relationship.Alias;
                    dbRelationship.RelationshipType.Relationship.Enabled = relationship.Enabled;
                }
                projectContext.SaveChanges();
            }
        }










        internal void GetUnionRelationships(string serverName, string dbName)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                string catalogName = GeneralHelper.GetCatalogName(serverName, dbName);
                List<UnionRelationship> UnionRelationshipTypeList = new List<UnionRelationship>();
                foreach (var item in projectContext.UnionRelationshipType.Where(x => x.UnionToSubUnionRelationshipType.Any(y => y.RelationshipType.Relationship.TableDrivedEntity.Table.Catalog == catalogName)))
                {
                    UnionRelationship rItem = new UnionRelationship();
                    rItem.UnionTypeEntities = "";
                    foreach (var unionType in item.UnionToSubUnionRelationshipType)
                    {
                        if (!rItem.UnionTypeEntities.Contains(unionType.RelationshipType.Relationship.TableDrivedEntity.Name))
                            rItem.UnionTypeEntities += (rItem.UnionTypeEntities == "" ? "" : ",") + unionType.RelationshipType.Relationship.TableDrivedEntity.Name;
                    }
                    rItem.SubUnionTypeEntities = "";
                    foreach (var subUnionType in item.SubUnionToUnionRelationshipType)
                    {
                        rItem.SubUnionTypeEntities += (rItem.SubUnionTypeEntities == "" ? "" : ",") + subUnionType.RelationshipType.Relationship.TableDrivedEntity.Name;
                    }
                    rItem.IsTolatParticipation = item.IsTolatParticipation;
                    rItem.ID = item.ID;
                    rItem.Name = item.Name;
                    rItem.UnionHoldsKeys = item.UnionHoldsKeys;
                    UnionRelationshipTypeList.Add(rItem);
                }
                View.SetRuleRelationships(UnionRelationshipTypeList);
            }

        }
        internal void UpdateUnionRelationships(object dataSource)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                foreach (var relationship in (dataSource as List<UnionRelationship>))
                {
                    var dbRelationship = projectContext.UnionRelationshipType.First(x => x.ID == relationship.ID);
                    dbRelationship.IsTolatParticipation = relationship.IsTolatParticipation;
                    dbRelationship.Name = relationship.Name;
                }
                projectContext.SaveChanges();
            }
        }


        internal void GetUnionRelationshipTypes(int unionRelationshipID)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {



                //projectContext.Configuration.LazyLoadingEnabled = false;
                var unionRelationship = projectContext.UnionRelationshipType.FirstOrDefault(x => x.ID == unionRelationshipID);
                if (unionRelationship != null)
                {
                    List<UnionToSubUnion> UnionToList = new List<UnionToSubUnion>();
                    List<SubUnionToUnion> SubUnionToUnionList = new List<SubUnionToUnion>();
                    foreach (var item in unionRelationship.UnionToSubUnionRelationshipType)
                    {
                        var rItem = GeneralHelper.ToRelationshipDTO(item.RelationshipType.Relationship);
                        UnionToList.Add(ToUnionToSubUnion(rItem, item));
                    }
                    foreach (var item in unionRelationship.SubUnionToUnionRelationshipType)
                    {
                        var rItem = GeneralHelper.ToRelationshipDTO(item.RelationshipType.Relationship);
                        SubUnionToUnionList.Add(ToSubUnionToUnion(rItem, item));
                    }
                    View.SetUnionRelationships(UnionToList, SubUnionToUnionList);
                }

            }
        }

        private UnionToSubUnion ToUnionToSubUnion(RelationshipDTO rItem, UnionToSubUnionRelationshipType item)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<RelationshipDTO, UnionToSubUnion>());
            var result = AutoMapper.Mapper.Map<RelationshipDTO, UnionToSubUnion>(rItem);
            result.IsOtherSideTransferable = item.RelationshipType.IsOtherSideTransferable;
            result.IsOtherSideCreatable = item.RelationshipType.IsOtherSideCreatable;
            result.IsOtherSideDirectlyCreatable = item.RelationshipType.IsOtherSideDirectlyCreatable;
            result.UnionHoldsKeys = item.UnionRelationshipType.UnionHoldsKeys;
            return result;
        }

        private SubUnionToUnion ToSubUnionToUnion(RelationshipDTO rItem, SubUnionToUnionRelationshipType item)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<RelationshipDTO, SubUnionToUnion>());
            var result = AutoMapper.Mapper.Map<RelationshipDTO, SubUnionToUnion>(rItem);
            result.IsOtherSideTransferable = item.RelationshipType.IsOtherSideTransferable;
            result.IsOtherSideCreatable = item.RelationshipType.IsOtherSideCreatable;
            result.IsOtherSideDirectlyCreatable = item.RelationshipType.IsOtherSideDirectlyCreatable;
            result.UnionHoldsKeys = item.UnionRelationshipType.UnionHoldsKeys;
            return result;
        }

        internal void UpdateUnionToSubUnionRelationships(object dataSource)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                foreach (var relationship in (dataSource as List<UnionToSubUnion>))
                {
                    var dbRelationship = projectContext.UnionToSubUnionRelationshipType.First(x => x.ID == relationship.ID);
                    dbRelationship.RelationshipType.IsOtherSideCreatable = relationship.IsOtherSideCreatable;
                    dbRelationship.RelationshipType.IsOtherSideDirectlyCreatable = relationship.IsOtherSideDirectlyCreatable;
                    dbRelationship.RelationshipType.IsOtherSideTransferable = relationship.IsOtherSideTransferable;
                    dbRelationship.RelationshipType.Relationship.Name = relationship.Name;
                    dbRelationship.RelationshipType.Relationship.Alias = relationship.Alias;
                    dbRelationship.RelationshipType.Relationship.Enabled = relationship.Enabled;
                }
                projectContext.SaveChanges();
            }
        }

        internal void UpdateSubUnionToUnionRelationships(object dataSource)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                foreach (var relationship in (dataSource as List<SubUnionToUnion>))
                {
                    var dbRelationship = projectContext.SubUnionToUnionRelationshipType.First(x => x.ID == relationship.ID);
                    dbRelationship.RelationshipType.IsOtherSideCreatable = relationship.IsOtherSideCreatable;
                    dbRelationship.RelationshipType.IsOtherSideDirectlyCreatable = relationship.IsOtherSideDirectlyCreatable;
                    dbRelationship.RelationshipType.IsOtherSideTransferable = relationship.IsOtherSideTransferable;
                    dbRelationship.RelationshipType.Relationship.Name = relationship.Name;
                    dbRelationship.RelationshipType.Relationship.Alias = relationship.Alias;
                    dbRelationship.RelationshipType.Relationship.Enabled = relationship.Enabled;
                }
                projectContext.SaveChanges();
            }
        }




        internal void ConvertRelationship(string convertType, RelationshipDTO relationship)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                if (convertType == "OneToMany_ImplicitOneToOne"
                      || convertType == "OneToMany_SuperToSub"
                      || convertType == "OneToMany_SubUnionToUnion"
                      || convertType == "OneToMany_UnionToSubUnion"
                      || convertType == "ManyToOne_ExplicitOneToOne"
                      || convertType == "ManyToOne_SuperToSub"
                      || convertType == "ManyToOne_SubUnionToUnion"
                      || convertType == "ManyToOne_UnionToSubUnion")
                {
                    Relationship dbRelationship = null;
                    if (convertType == "ManyToOne_ExplicitOneToOne"
                     || convertType == "ManyToOne_SuperToSub"
                     || convertType == "ManyToOne_SubUnionToUnion"
                     || convertType == "ManyToOne_UnionToSubUnion")
                    {
                        if (convertType == "ManyToOne_ExplicitOneToOne")
                            convertType = "OneToMany_ImplicitOneToOne";
                        else if (convertType == "ManyToOne_SuperToSub")
                            convertType = "OneToMany_SuperToSub";
                        else if (convertType == "ManyToOne_SubUnionToUnion")
                            convertType = "OneToMany_UnionToSubUnion";
                        else if (convertType == "ManyToOne_UnionToSubUnion")
                            convertType = "OneToMany_SubUnionToUnion";
                        dbRelationship = projectContext.Relationship.First(x => x.ID == relationship.ID).Relationship2;
                    }
                    else
                        dbRelationship = projectContext.Relationship.First(x => x.ID == relationship.ID);
                    //OneToMany oneToMany = relationship as OneToMany;
                    //dbRelationship = projectContext.Relationship.First(x => x.ID == dbRelationship.ID);

                    var entityInfo = DataHelper.GetEntityRelationshipsInfo(dbRelationship.TableDrivedEntity, dbRelationship);
                    if (entityInfo.RelationInfos.Any(x => x.RelationType == RelationType.ManyDataItems && x.FKHasData))
                    {
                        MessageBox.Show("بعلت وجود ارتباط یک به چند بین داده های دو جدول امکان تبدیل وجود ندارد");
                        return;
                    }

                    var dbReverseRelationship = projectContext.Relationship.First(x => x.RelationshipID == dbRelationship.ID);
                    if (convertType == "OneToMany_ImplicitOneToOne")
                    {
                        var relationInfo = entityInfo.RelationInfos.First(x => x.RelationType == RelationType.ManyDataItems);
                        projectContext.OneToManyRelationshipType.Remove(dbRelationship.RelationshipType.OneToManyRelationshipType);
                        projectContext.ManyToOneRelationshipType.Remove(dbReverseRelationship.RelationshipType.ManyToOneRelationshipType);


                        dbRelationship.RelationshipType.ImplicitOneToOneRelationshipType = new ImplicitOneToOneRelationshipType();
                        dbRelationship.RelationshipType.IsOtherSideCreatable = true;
                        dbRelationship.RelationshipType.IsOtherSideMandatory = relationInfo.AllPrimarySideHasFkSideData;

                        dbReverseRelationship.RelationshipType.ExplicitOneToOneRelationshipType = new ExplicitOneToOneRelationshipType();
                        dbReverseRelationship.RelationshipType.IsOtherSideCreatable = true;
                        dbReverseRelationship.RelationshipType.IsOtherSideMandatory = relationInfo.FKColumnIsMandatory;
                        projectContext.SaveChanges();

                    }
                    else if (convertType == "OneToMany_SuperToSub")
                    {
                        var existingISA = projectContext.ISARelationship.Where(x => x.SuperToSubRelationshipType.Any(y => y.RelationshipType.Relationship.TableDrivedEntity.ID == dbRelationship.TableDrivedEntity.ID));
                        ISARelationshipCreateOrSelect(existingISA.ToList(), convertType, dbRelationship.ID, dbReverseRelationship.ID);
                    }//union standard
                    else if (convertType == "OneToMany_SubUnionToUnion")
                    {
                        var existingUnionRelarionship = projectContext.UnionRelationshipType.Where(x => x.UnionToSubUnionRelationshipType.Any(y => y.RelationshipType.Relationship.TableDrivedEntity.ID == dbRelationship.TableDrivedEntity1.ID));
                        UnionRelationshipCreateOrSelect(existingUnionRelarionship.ToList(), convertType, dbRelationship.ID, dbReverseRelationship.ID);
                    }
                    else if (convertType == "OneToMany_UnionToSubUnion")
                    {
                        var existingUnionRelarionship = projectContext.UnionRelationshipType.Where(x => x.UnionToSubUnionRelationshipType.Any(y => y.RelationshipType.Relationship.TableDrivedEntity.ID == dbRelationship.TableDrivedEntity.ID));
                        UnionRelationshipCreateOrSelect(existingUnionRelarionship.ToList(), convertType, dbRelationship.ID, dbReverseRelationship.ID);
                    }
                }
                else if (convertType == "ImplicitOneToOne_OneToMany"
                     || convertType == "ImplicitOneToOne_SuperToSub"
                     || convertType == "ImplicitOneToOne_SubUnionToUnion"
                     || convertType == "ImplicitOneToOne_UnionToSubUnion"
                     || convertType == "ExplicitOneToOne_ManyToOne"
                     || convertType == "ExplicitOneToOne_SuperToSub"
                     || convertType == "ExplicitOneToOne_SubUnionToUnion"
                     || convertType == "ExplicitOneToOne_UnionToSubUnion"
                    )
                {
                    Relationship dbRelationship = null;
                    if (convertType == "ExplicitOneToOne_ManyToOne"
                     || convertType == "ExplicitOneToOne_SuperToSub"
                     || convertType == "ExplicitOneToOne_SubUnionToUnion"
                     || convertType == "ExplicitOneToOne_UnionToSubUnion")
                    {
                        if (convertType == "ExplicitOneToOne_ManyToOne")
                            convertType = "ImplicitOneToOne_OneToMany";
                        else if (convertType == "ExplicitOneToOne_SuperToSub")
                            convertType = "ImplicitOneToOne_SuperToSub";
                        else if (convertType == "ExplicitOneToOne_SubUnionToUnion")
                            convertType = "ImplicitOneToOne_UnionToSubUnion";
                        else if (convertType == "ExplicitOneToOne_UnionToSubUnion")
                            convertType = "ImplicitOneToOne_SubUnionToUnion";
                        dbRelationship = projectContext.Relationship.First(x => x.ID == relationship.ID).Relationship2;
                    }
                    else
                        dbRelationship = projectContext.Relationship.First(x => x.ID == relationship.ID);


                    //////var entityInfo = DataHelper.GetEntityRelationshipsInfo(dbRelationship.TableDrivedEntity, dbRelationship);
                    //////if (entityInfo.RelationInfos.Any(x => x.RelationType == RelationType.ManyDataItems && x.FKHasData))
                    //////{
                    //////    MessageBox.Show("بعلت وجود ارتباط یک به چند بین داده های دو جدول امکان تبدیل وجود ندارد");
                    //////    return;
                    //////}
                    //////var relationInfo = entityInfo.RelationInfos.First(x => x.RelationType == RelationType.ManyDataItems);
                    var dbReverseRelationship = projectContext.Relationship.First(x => x.RelationshipID == dbRelationship.ID);
                    if (convertType == "ImplicitOneToOne_OneToMany")
                    {
                        var entityInfo = DataHelper.GetEntityRelationshipsInfo(dbRelationship.TableDrivedEntity, dbRelationship);
                        var relationInfo = entityInfo.RelationInfos.FirstOrDefault(x => x.RelationType == RelationType.OneDataItems);
                        if (relationInfo == null)
                            relationInfo = entityInfo.RelationInfos.First(x => x.RelationType == RelationType.ManyDataItems && !x.FKHasData);

                        projectContext.ImplicitOneToOneRelationshipType.Remove(dbRelationship.RelationshipType.ImplicitOneToOneRelationshipType);
                        projectContext.ExplicitOneToOneRelationshipType.Remove(dbReverseRelationship.RelationshipType.ExplicitOneToOneRelationshipType);


                        dbRelationship.RelationshipType.OneToManyRelationshipType = new OneToManyRelationshipType();
                        dbRelationship.RelationshipType.IsOtherSideMandatory = relationInfo.AllPrimarySideHasFkSideData;

                        dbReverseRelationship.RelationshipType.ManyToOneRelationshipType = new ManyToOneRelationshipType();
                        dbReverseRelationship.RelationshipType.IsOtherSideCreatable = true;
                        dbReverseRelationship.RelationshipType.IsOtherSideMandatory = relationInfo.FKColumnIsMandatory;
                        projectContext.SaveChanges();

                    }
                    else if (convertType == "ImplicitOneToOne_SuperToSub")
                    {
                        var existingISA = projectContext.ISARelationship.Where(x => x.SuperToSubRelationshipType.Any(y => y.RelationshipType.Relationship.TableDrivedEntity.ID == dbRelationship.TableDrivedEntity.ID));
                        ISARelationshipCreateOrSelect(existingISA.ToList(), convertType, dbRelationship.ID, dbReverseRelationship.ID);
                    }//union standard
                    else if (convertType == "ImplicitOneToOne_SubUnionToUnion")
                    {
                        var existingUnionRelarionship = projectContext.UnionRelationshipType.Where(x => x.UnionToSubUnionRelationshipType.Any(y => y.RelationshipType.Relationship.TableDrivedEntity.ID == dbRelationship.TableDrivedEntity1.ID));
                        UnionRelationshipCreateOrSelect(existingUnionRelarionship.ToList(), convertType, dbRelationship.ID, dbReverseRelationship.ID);
                    }
                    else if (convertType == "ImplicitOneToOne_UnionToSubUnion")
                    {
                        var existingUnionRelarionship = projectContext.UnionRelationshipType.Where(x => x.UnionToSubUnionRelationshipType.Any(y => y.RelationshipType.Relationship.TableDrivedEntity.ID == dbRelationship.TableDrivedEntity.ID));
                        UnionRelationshipCreateOrSelect(existingUnionRelarionship.ToList(), convertType, dbRelationship.ID, dbReverseRelationship.ID);
                    }
                }
                else if (convertType == "SuperToSub_OneToMany"
                      || convertType == "SuperToSub_ImplicitOneToOne"
                      || convertType == "SuperToSub_SubUnionToUnion"
                      || convertType == "SuperToSub_UnionToSubUnion"
                      || convertType == "SubToSuper_ManyToOne"
                      || convertType == "SubToSuper_ExplicitOneToOne"
                      || convertType == "SubToSuper_SubUnionToUnion"
                      || convertType == "SubToSuper_UnionToSubUnion"
                      || convertType == "SuperToSub_SuperToSub"
                      || convertType == "SubToSuper_SubToSuper"
                   )
                {
                    Relationship dbRelationship = null;
                    if (convertType == "SubToSuper_ManyToOne"
                     || convertType == "SubToSuper_ExplicitOneToOne"
                     || convertType == "SubToSuper_SubUnionToUnion"
                     || convertType == "SubToSuper_UnionToSubUnion"
                     || convertType == "SubToSuper_SubToSuper")
                    {
                        if (convertType == "SubToSuper_ManyToOne")
                            convertType = "SuperToSub_OneToMany";
                        else if (convertType == "SubToSuper_ExplicitOneToOne")
                            convertType = "SuperToSub_ImplicitOneToOne";
                        else if (convertType == "SubToSuper_SubUnionToUnion")
                            convertType = "SuperToSub_UnionToSubUnion";
                        else if (convertType == "SubToSuper_UnionToSubUnion")
                            convertType = "SuperToSub_SubUnionToUnion";
                        else if (convertType == "SubToSuper_SubToSuper")
                            convertType = "SuperToSub_SuperToSub";
                        dbRelationship = projectContext.Relationship.First(x => x.ID == relationship.ID).Relationship2;
                    }
                    else
                        dbRelationship = projectContext.Relationship.First(x => x.ID == relationship.ID);

                    var dbReverseRelationship = projectContext.Relationship.First(x => x.RelationshipID == dbRelationship.ID);

                    if (convertType != "SuperToSub_SuperToSub")
                    {
                        var entityInfo = DataHelper.GetEntityRelationshipsInfo(dbRelationship.TableDrivedEntity, dbRelationship);
                        if (entityInfo.RelationInfos.Any(x => x.RelationType == RelationType.OneDataItems && x.FKRelatesOnPrimaryKey))
                        {
                            MessageBox.Show("بعلت وجود ارتباط بروی کلیدهای اصلی تنها رابطه ارث بری معنی دارد و تبدیل میسر نمیباشد");
                            return;
                        }
                        var relationInfo = entityInfo.RelationInfos.FirstOrDefault(x => x.RelationType == RelationType.OneDataItems);
                        if (relationInfo == null)
                            relationInfo = entityInfo.RelationInfos.First(x => x.RelationType == RelationType.ManyDataItems && !x.FKHasData);



                        if (convertType == "SuperToSub_OneToMany")
                        {


                            projectContext.SuperToSubRelationshipType.Remove(dbRelationship.RelationshipType.SuperToSubRelationshipType);
                            projectContext.SubToSuperRelationshipType.Remove(dbReverseRelationship.RelationshipType.SubToSuperRelationshipType);

                            dbRelationship.RelationshipType.OneToManyRelationshipType = new OneToManyRelationshipType();
                            dbRelationship.RelationshipType.IsOtherSideMandatory = relationInfo.AllPrimarySideHasFkSideData;

                            dbReverseRelationship.RelationshipType.ManyToOneRelationshipType = new ManyToOneRelationshipType();
                            dbReverseRelationship.RelationshipType.IsOtherSideCreatable = true;
                            dbReverseRelationship.RelationshipType.IsOtherSideMandatory = relationInfo.FKColumnIsMandatory;

                            projectContext.SaveChanges();

                        }
                        else if (convertType == "SuperToSub_ImplicitOneToOne")
                        {
                            projectContext.SuperToSubRelationshipType.Remove(dbRelationship.RelationshipType.SuperToSubRelationshipType);
                            projectContext.SubToSuperRelationshipType.Remove(dbReverseRelationship.RelationshipType.SubToSuperRelationshipType);

                            dbRelationship.RelationshipType.ImplicitOneToOneRelationshipType = new ImplicitOneToOneRelationshipType();
                            dbRelationship.RelationshipType.IsOtherSideCreatable = true;
                            dbRelationship.RelationshipType.IsOtherSideMandatory = relationInfo.AllPrimarySideHasFkSideData;

                            dbReverseRelationship.RelationshipType.ExplicitOneToOneRelationshipType = new ExplicitOneToOneRelationshipType();
                            dbReverseRelationship.RelationshipType.IsOtherSideCreatable = true;
                            dbReverseRelationship.RelationshipType.IsOtherSideMandatory = relationInfo.FKColumnIsMandatory;

                            projectContext.SaveChanges();
                        }//union standard
                        else if (convertType == "SuperToSub_SubUnionToUnion")
                        {
                            var existingUnionRelarionship = projectContext.UnionRelationshipType.Where(x => x.UnionToSubUnionRelationshipType.Any(y => y.RelationshipType.Relationship.TableDrivedEntity.ID == dbRelationship.TableDrivedEntity1.ID));
                            UnionRelationshipCreateOrSelect(existingUnionRelarionship.ToList(), convertType, dbRelationship.ID, dbReverseRelationship.ID);
                        }
                        else if (convertType == "SuperToSub_UnionToSubUnion")
                        {
                            var existingUnionRelarionship = projectContext.UnionRelationshipType.Where(x => x.UnionToSubUnionRelationshipType.Any(y => y.RelationshipType.Relationship.TableDrivedEntity.ID == dbRelationship.TableDrivedEntity.ID));
                            UnionRelationshipCreateOrSelect(existingUnionRelarionship.ToList(), convertType, dbRelationship.ID, dbReverseRelationship.ID);
                        }
                    }
                    else
                    {
                        var existingISA = projectContext.ISARelationship.Where(x => x.ID != dbRelationship.RelationshipType.SuperToSubRelationshipType.ISARelationshipID && x.SuperToSubRelationshipType.Any(y => y.RelationshipType.Relationship.TableDrivedEntity.ID == dbRelationship.TableDrivedEntity.ID));
                        ISARelationshipCreateOrSelect(existingISA.ToList(), convertType, dbRelationship.ID, dbReverseRelationship.ID);
                    }//un
                }


                else if (convertType == "UnionToSubUnion_!UnionHoldsKeys_OneToMany"
                      || convertType == "UnionToSubUnion_!UnionHoldsKeys_ImplicitOneToOne"
                      || convertType == "UnionToSubUnion_!UnionHoldsKeys_SuperToSub"
                      || convertType == "UnionToSubUnion_!UnionHoldsKeys_SubUnionToUnion"
                      || convertType == "UnionToSubUnion_!UnionHoldsKeys_UnionToSubUnion"
                      || convertType == "SubUnionToUnion_!UnionHoldsKeys_ManyToOne"
                      || convertType == "SubUnionToUnion_!UnionHoldsKeys_ExplicitOneToOne"
                      || convertType == "SubUnionToUnion_!UnionHoldsKeys_SubToSuper"
                      || convertType == "SubUnionToUnion_!UnionHoldsKeys_UnionToSubUnion"
                      || convertType == "SubUnionToUnion_!UnionHoldsKeys_SubUnionToUnion"
                   )
                {


                    Relationship dbRelationship = null;
                    if (convertType == "SubUnionToUnion_!UnionHoldsKeys_ManyToOne"
                    || convertType == "SubUnionToUnion_!UnionHoldsKeys_ExplicitOneToOne"
                    || convertType == "SubUnionToUnion_!UnionHoldsKeys_SubToSuper"
                    || convertType == "SubUnionToUnion_!UnionHoldsKeys_UnionToSubUnion"
                        || convertType == "SubUnionToUnion_!UnionHoldsKeys_SubUnionToUnion"
                     )
                    {
                        if (convertType == "SubUnionToUnion_!UnionHoldsKeys_ManyToOne")
                            convertType = "UnionToSubUnion_!UnionHoldsKeys_OneToMany";
                        else if (convertType == "SubUnionToUnion_!UnionHoldsKeys_ExplicitOneToOne")
                            convertType = "UnionToSubUnion_!UnionHoldsKeys_ImplicitOneToOne";
                        else if (convertType == "SubUnionToUnion_!UnionHoldsKeys_SubToSuper")
                            convertType = "UnionToSubUnion_!UnionHoldsKeys_SuperToSub";
                        else if (convertType == "SubUnionToUnion_!UnionHoldsKeys_UnionToSubUnion")
                            convertType = "UnionToSubUnion_!UnionHoldsKeys_SubUnionToUnion";
                        else if (convertType == "SubUnionToUnion_!UnionHoldsKeys_SubUnionToUnion")
                            convertType = "UnionToSubUnion_!UnionHoldsKeys_UnionToSubUnion";
                        dbRelationship = projectContext.Relationship.First(x => x.ID == relationship.ID).Relationship2;
                    }
                    else
                        dbRelationship = projectContext.Relationship.First(x => x.ID == relationship.ID);



                    var dbReverseRelationship = projectContext.Relationship.First(x => x.RelationshipID == dbRelationship.ID);


                    if (convertType != "UnionToSubUnion_!UnionHoldsKeys_UnionToSubUnion")
                    {
                        var entityInfo = DataHelper.GetEntityRelationshipsInfo(dbRelationship.TableDrivedEntity, dbRelationship);
                        //if (entityInfo.RelationInfos.Any(x => x.RelationType == RelationType.OneDataItems && x.FKRelatesOnPrimaryKey))
                        //{
                        //    MessageBox.Show("بعلت وجود ارتباط بروی کلیدهای اصلی تنها رابطه ارث بری معنی دارد و تبدیل میسر نمیباشد");
                        //    return;
                        //}
                        var relationInfo = entityInfo.RelationInfos.FirstOrDefault(x => x.RelationType == RelationType.OneDataItems);
                        if (relationInfo == null)
                            relationInfo = entityInfo.RelationInfos.First(x => x.RelationType == RelationType.ManyDataItems && !x.FKHasData);
                        if (convertType == "UnionToSubUnion_!UnionHoldsKeys_OneToMany")
                        {


                            if (dbRelationship.RelationshipType.UnionToSubUnionRelationshipType != null)
                                projectContext.UnionToSubUnionRelationshipType.Remove(dbRelationship.RelationshipType.UnionToSubUnionRelationshipType);
                            //if (dbRelationship.RelationshipType.SubUnionToUnionRelationshipType != null)
                            //    projectContext.SubUnionToUnionRelationshipType.Remove(dbRelationship.RelationshipType.SubUnionToUnionRelationshipType);
                            if (dbReverseRelationship.RelationshipType.SubUnionToUnionRelationshipType != null)
                                projectContext.SubUnionToUnionRelationshipType.Remove(dbReverseRelationship.RelationshipType.SubUnionToUnionRelationshipType);
                            //if (dbReverseRelationship.RelationshipType.UnionToSubUnionRelationshipType != null)
                            //    projectContext.UnionToSubUnionRelationshipType.Remove(dbReverseRelationship.RelationshipType.UnionToSubUnionRelationshipType);

                            dbRelationship.RelationshipType.OneToManyRelationshipType = new OneToManyRelationshipType();
                            dbRelationship.RelationshipType.IsOtherSideMandatory = relationInfo.AllPrimarySideHasFkSideData;

                            dbReverseRelationship.RelationshipType.ManyToOneRelationshipType = new ManyToOneRelationshipType();
                            dbReverseRelationship.RelationshipType.IsOtherSideCreatable = true;
                            dbReverseRelationship.RelationshipType.IsOtherSideMandatory = relationInfo.FKColumnIsMandatory;

                            projectContext.SaveChanges();

                        }
                        else if (convertType == "UnionToSubUnion_!UnionHoldsKeys_ImplicitOneToOne")
                        {
                            if (dbRelationship.RelationshipType.UnionToSubUnionRelationshipType != null)
                                projectContext.UnionToSubUnionRelationshipType.Remove(dbRelationship.RelationshipType.UnionToSubUnionRelationshipType);
                            //if (dbRelationship.RelationshipType.SubUnionToUnionRelationshipType != null)
                            //    projectContext.SubUnionToUnionRelationshipType.Remove(dbRelationship.RelationshipType.SubUnionToUnionRelationshipType);
                            if (dbReverseRelationship.RelationshipType.SubUnionToUnionRelationshipType != null)
                                projectContext.SubUnionToUnionRelationshipType.Remove(dbReverseRelationship.RelationshipType.SubUnionToUnionRelationshipType);
                            //if (dbReverseRelationship.RelationshipType.UnionToSubUnionRelationshipType != null)
                            //    projectContext.UnionToSubUnionRelationshipType.Remove(dbReverseRelationship.RelationshipType.UnionToSubUnionRelationshipType);

                            dbRelationship.RelationshipType.ImplicitOneToOneRelationshipType = new ImplicitOneToOneRelationshipType();
                            dbRelationship.RelationshipType.IsOtherSideCreatable = true;
                            dbRelationship.RelationshipType.IsOtherSideMandatory = relationInfo.AllPrimarySideHasFkSideData;

                            dbReverseRelationship.RelationshipType.ExplicitOneToOneRelationshipType = new ExplicitOneToOneRelationshipType();
                            dbReverseRelationship.RelationshipType.IsOtherSideCreatable = true;
                            dbReverseRelationship.RelationshipType.IsOtherSideMandatory = relationInfo.FKColumnIsMandatory;

                            projectContext.SaveChanges();
                        }//union standard
                        else if (convertType == "UnionToSubUnion_!UnionHoldsKeys_SuperToSub")
                        {
                            var existingISA = projectContext.ISARelationship.Where(x => x.SuperToSubRelationshipType.Any(y => y.RelationshipType.Relationship.TableDrivedEntity.ID == dbRelationship.TableDrivedEntity.ID));
                            ISARelationshipCreateOrSelect(existingISA.ToList(), convertType, dbRelationship.ID, dbReverseRelationship.ID);
                        }
                        else if (convertType == "UnionToSubUnion_!UnionHoldsKeys_SubUnionToUnion")
                        {
                            var existingUnionRelarionship = projectContext.UnionRelationshipType.Where(x => x.UnionToSubUnionRelationshipType.Any(y => y.RelationshipType.Relationship.TableDrivedEntity.ID == dbRelationship.TableDrivedEntity.ID));
                            UnionRelationshipCreateOrSelect(existingUnionRelarionship.ToList(), convertType, dbRelationship.ID, dbReverseRelationship.ID);
                        }
                    }
                    else
                    {
                        var existingUnionRelarionship = projectContext.UnionRelationshipType.Where(x => x.UnionHoldsKeys == false && x.ID != dbRelationship.RelationshipType.UnionToSubUnionRelationshipType.UnionRelationshipTypeID && x.UnionToSubUnionRelationshipType.Any(y => y.RelationshipType.Relationship.TableDrivedEntity.ID == dbRelationship.TableDrivedEntity.ID));
                        UnionRelationshipCreateOrSelect(existingUnionRelarionship.ToList(), convertType, dbRelationship.ID, dbReverseRelationship.ID);

                    }

                }
                else if (convertType == "UnionToSubUnion_UnionHoldsKeys_ManyToOne"
                         || convertType == "UnionToSubUnion_UnionHoldsKeys_ExplicitOneToOne"
                         || convertType == "UnionToSubUnion_UnionHoldsKeys_SubToSuper"
                         || convertType == "UnionToSubUnion_UnionHoldsKeys_SubUnionToUnion"
                    || convertType == "UnionToSubUnion_UnionHoldsKeys_UnionToSubUnion"
                         || convertType == "SubUnionToUnion_UnionHoldsKeys_OneToMany"
                         || convertType == "SubUnionToUnion_UnionHoldsKeys_ImplicitOneToOne"
                         || convertType == "SubUnionToUnion_UnionHoldsKeys_SuperToSub"
                         || convertType == "SubUnionToUnion_UnionHoldsKeys_UnionToSubUnion"
                     || convertType == "SubUnionToUnion_UnionHoldsKeys_SubUnionToUnion"
                  )
                {
                    Relationship dbRelationship = null;
                    if (convertType == "UnionToSubUnion_UnionHoldsKeys_ManyToOne"
                    || convertType == "UnionToSubUnion_UnionHoldsKeys_ExplicitOneToOne"
                    || convertType == "UnionToSubUnion_UnionHoldsKeys_SubToSuper"
                    || convertType == "UnionToSubUnion_UnionHoldsKeys_SubUnionToUnion"
                       || convertType == "UnionToSubUnion_UnionHoldsKeys_UnionToSubUnion"
                        )
                    {
                        if (convertType == "UnionToSubUnion_UnionHoldsKeys_ManyToOne")
                            convertType = "SubUnionToUnion_UnionHoldsKeys_OneToMany";
                        else if (convertType == "UnionToSubUnion_UnionHoldsKeys_ExplicitOneToOne")
                            convertType = "SubUnionToUnion_UnionHoldsKeys_ImplicitOneToOne";
                        else if (convertType == "UnionToSubUnion_UnionHoldsKeys_SubToSuper")
                            convertType = "SubUnionToUnion_UnionHoldsKeys_SuperToSub";
                        else if (convertType == "UnionToSubUnion_UnionHoldsKeys_SubUnionToUnion")
                            convertType = "SubUnionToUnion_UnionHoldsKeys_UnionToSubUnion";
                        else if (convertType == "UnionToSubUnion_UnionHoldsKeys_UnionToSubUnion")
                            convertType = "SubUnionToUnion_UnionHoldsKeys_SubUnionToUnion";
                        dbRelationship = projectContext.Relationship.First(x => x.ID == relationship.ID).Relationship2;
                    }
                    else
                        dbRelationship = projectContext.Relationship.First(x => x.ID == relationship.ID);

                    var dbReverseRelationship = projectContext.Relationship.First(x => x.RelationshipID == dbRelationship.ID);
                    if (convertType != "SubUnionToUnion_UnionHoldsKeys_SubUnionToUnion")
                    {
                        var entityInfo = DataHelper.GetEntityRelationshipsInfo(dbRelationship.TableDrivedEntity, dbRelationship);
                        //if (entityInfo.RelationInfos.Any(x => x.RelationType == RelationType.OneDataItems && x.FKRelatesOnPrimaryKey))
                        //{
                        //    MessageBox.Show("بعلت وجود ارتباط بروی کلیدهای اصلی تنها رابطه ارث بری معنی دارد و تبدیل میسر نمیباشد");
                        //    return;
                        //}
                        var relationInfo = entityInfo.RelationInfos.FirstOrDefault(x => x.RelationType == RelationType.OneDataItems);
                        if (relationInfo == null)
                            relationInfo = entityInfo.RelationInfos.First(x => x.RelationType == RelationType.ManyDataItems && !x.FKHasData);




                        if (convertType == "SubUnionToUnion_UnionHoldsKeys_OneToMany")
                        {


                            //if (dbRelationship.RelationshipType.UnionToSubUnionRelationshipType != null)
                            //    projectContext.UnionToSubUnionRelationshipType.Remove(dbRelationship.RelationshipType.UnionToSubUnionRelationshipType);
                            if (dbRelationship.RelationshipType.SubUnionToUnionRelationshipType != null)
                                projectContext.SubUnionToUnionRelationshipType.Remove(dbRelationship.RelationshipType.SubUnionToUnionRelationshipType);
                            //if (dbReverseRelationship.RelationshipType.SubUnionToUnionRelationshipType != null)
                            //    projectContext.SubUnionToUnionRelationshipType.Remove(dbReverseRelationship.RelationshipType.SubUnionToUnionRelationshipType);
                            if (dbReverseRelationship.RelationshipType.UnionToSubUnionRelationshipType != null)
                                projectContext.UnionToSubUnionRelationshipType.Remove(dbReverseRelationship.RelationshipType.UnionToSubUnionRelationshipType);

                            dbRelationship.RelationshipType.OneToManyRelationshipType = new OneToManyRelationshipType();
                            dbRelationship.RelationshipType.IsOtherSideMandatory = relationInfo.AllPrimarySideHasFkSideData;

                            dbReverseRelationship.RelationshipType.ManyToOneRelationshipType = new ManyToOneRelationshipType();
                            dbReverseRelationship.RelationshipType.IsOtherSideCreatable = true;
                            dbReverseRelationship.RelationshipType.IsOtherSideMandatory = relationInfo.FKColumnIsMandatory;

                            projectContext.SaveChanges();

                        }
                        else if (convertType == "SubUnionToUnion_UnionHoldsKeys_ImplicitOneToOne")
                        {
                            //if (dbRelationship.RelationshipType.UnionToSubUnionRelationshipType != null)
                            //    projectContext.UnionToSubUnionRelationshipType.Remove(dbRelationship.RelationshipType.UnionToSubUnionRelationshipType);
                            if (dbRelationship.RelationshipType.SubUnionToUnionRelationshipType != null)
                                projectContext.SubUnionToUnionRelationshipType.Remove(dbRelationship.RelationshipType.SubUnionToUnionRelationshipType);
                            //if (dbReverseRelationship.RelationshipType.SubUnionToUnionRelationshipType != null)
                            //    projectContext.SubUnionToUnionRelationshipType.Remove(dbReverseRelationship.RelationshipType.SubUnionToUnionRelationshipType);
                            if (dbReverseRelationship.RelationshipType.UnionToSubUnionRelationshipType != null)
                                projectContext.UnionToSubUnionRelationshipType.Remove(dbReverseRelationship.RelationshipType.UnionToSubUnionRelationshipType);

                            dbRelationship.RelationshipType.ImplicitOneToOneRelationshipType = new ImplicitOneToOneRelationshipType();
                            dbRelationship.RelationshipType.IsOtherSideCreatable = true;
                            dbRelationship.RelationshipType.IsOtherSideMandatory = relationInfo.AllPrimarySideHasFkSideData;

                            dbReverseRelationship.RelationshipType.ExplicitOneToOneRelationshipType = new ExplicitOneToOneRelationshipType();
                            dbReverseRelationship.RelationshipType.IsOtherSideCreatable = true;
                            dbReverseRelationship.RelationshipType.IsOtherSideMandatory = relationInfo.FKColumnIsMandatory;

                            projectContext.SaveChanges();
                        }//union standard
                        else if (convertType == "SubUnionToUnion_UnionHoldsKeys_SuperToSub")
                        {
                            var existingISA = projectContext.ISARelationship.Where(x => x.SuperToSubRelationshipType.Any(y => y.RelationshipType.Relationship.TableDrivedEntity.ID == dbRelationship.TableDrivedEntity.ID));
                            ISARelationshipCreateOrSelect(existingISA.ToList(), convertType, dbRelationship.ID, dbReverseRelationship.ID);
                        }
                        else if (convertType == "SubUnionToUnion_UnionHoldsKeys_UnionToSubUnion")
                        {
                            var existingUnionRelarionship = projectContext.UnionRelationshipType.Where(x => x.SubUnionToUnionRelationshipType.Any(y => y.RelationshipType.Relationship.TableDrivedEntity.ID == dbRelationship.TableDrivedEntity.ID));
                            UnionRelationshipCreateOrSelect(existingUnionRelarionship.ToList(), convertType, dbRelationship.ID, dbReverseRelationship.ID);
                        }
                    }
                    else
                    {
                        var existingUnionRelarionship = projectContext.UnionRelationshipType.Where(x => x.UnionHoldsKeys == true && x.ID != dbRelationship.RelationshipType.SubUnionToUnionRelationshipType.UnionRelationshipTypeID && x.SubUnionToUnionRelationshipType.Any(y => y.RelationshipType.Relationship.TableDrivedEntity.ID == dbRelationship.TableDrivedEntity.ID));
                        UnionRelationshipCreateOrSelect(existingUnionRelarionship.ToList(), convertType, dbRelationship.ID, dbReverseRelationship.ID);
                    }
                }

            }
        }
        private void UnionRelationshipCreateOrSelect(List<DataAccess.UnionRelationshipType> list, string convertType, int dbRelationship, int dbReverseRelationship)
        {
            string defaultName = "";
            bool? uninoHoldsKeys = null;
            if (convertType == "OneToMany_SubUnionToUnion"
             || convertType == "ImplicitOneToOne_SubUnionToUnion"
             || convertType == "SuperToSub_SubUnionToUnion"
             || convertType == "UnionToSubUnion_!UnionHoldsKeys_SubUnionToUnion"
                 || convertType == "SubUnionToUnion_UnionHoldsKeys_SubUnionToUnion")
            {
                uninoHoldsKeys = true;
                using (var projectContext = new DataAccess.MyProjectEntities())
                {
                    defaultName = projectContext.Relationship.First(x => x.ID == dbRelationship).TableDrivedEntity1.Name;
                }
            }
            else if (convertType == "OneToMany_UnionToSubUnion"
                  || convertType == "ImplicitOneToOne_UnionToSubUnion"
                  || convertType == "SuperToSub_UnionToSubUnion"
                || convertType == "SubUnionToUnion_UnionHoldsKeys_UnionToSubUnion"
                || convertType == "UnionToSubUnion_!UnionHoldsKeys_UnionToSubUnion")
            {
                uninoHoldsKeys = false;
                using (var projectContext = new DataAccess.MyProjectEntities())
                {
                    defaultName = projectContext.Relationship.First(x => x.ID == dbRelationship).TableDrivedEntity.Name;
                }
            }

            frmUnionRelationshipCreateSelect frm = new frmUnionRelationshipCreateSelect(list, uninoHoldsKeys, defaultName);
            frm.UnionRelationshipSelected += (sender, e) => frm_UnionRelationshipSelected(sender, e, convertType, dbRelationship, dbReverseRelationship);
            frm.ShowDialog();
        }

        void frm_UnionRelationshipSelected(object sender, UnionRelationshipSelectedArg e, string convertType, int dbRelationshipID, int dbReverseRelationshipID)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                //dbrelationship 
                //در ادامه در حالت عادی طرف ساب پونیون تو یونیون ساخته میشود 
                if (convertType == "OneToMany_SubUnionToUnion"
                    || convertType == "OneToMany_UnionToSubUnion"
                    || convertType == "ImplicitOneToOne_SubUnionToUnion"
                    || convertType == "ImplicitOneToOne_UnionToSubUnion"
                    || convertType == "SuperToSub_SubUnionToUnion"
                    || convertType == "SuperToSub_UnionToSubUnion"
                    || convertType == "UnionToSubUnion_!UnionHoldsKeys_SubUnionToUnion"
                    || convertType == "SubUnionToUnion_UnionHoldsKeys_UnionToSubUnion"
                    || convertType == "UnionToSubUnion_!UnionHoldsKeys_UnionToSubUnion"
                    || convertType == "SubUnionToUnion_UnionHoldsKeys_SubUnionToUnion")
                {

                    var dbRelationship = projectContext.Relationship.First(x => x.ID == dbRelationshipID);
                    var dbReverseRelationship = projectContext.Relationship.First(x => x.ID == dbReverseRelationshipID);


                    if (convertType == "OneToMany_SubUnionToUnion"
                    || convertType == "ImplicitOneToOne_SubUnionToUnion"
                    || convertType == "SuperToSub_SubUnionToUnion"
                    || convertType == "UnionToSubUnion_!UnionHoldsKeys_SubUnionToUnion"
                    || convertType == "UnionToSubUnion_!UnionHoldsKeys_UnionToSubUnion")
                    {
                        if (convertType == "OneToMany_SubUnionToUnion")
                        {
                            projectContext.OneToManyRelationshipType.Remove(dbRelationship.RelationshipType.OneToManyRelationshipType);
                            projectContext.ManyToOneRelationshipType.Remove(dbReverseRelationship.RelationshipType.ManyToOneRelationshipType);
                        }
                        else if (convertType == "ImplicitOneToOne_SubUnionToUnion")
                        {
                            projectContext.ImplicitOneToOneRelationshipType.Remove(dbRelationship.RelationshipType.ImplicitOneToOneRelationshipType);
                            projectContext.ExplicitOneToOneRelationshipType.Remove(dbReverseRelationship.RelationshipType.ExplicitOneToOneRelationshipType);
                        }
                        else if (convertType == "SuperToSub_SubUnionToUnion")
                        {
                            projectContext.SuperToSubRelationshipType.Remove(dbRelationship.RelationshipType.SuperToSubRelationshipType);
                            projectContext.SubToSuperRelationshipType.Remove(dbReverseRelationship.RelationshipType.SubToSuperRelationshipType);
                        }
                        else if (convertType == "UnionToSubUnion_!UnionHoldsKeys_SubUnionToUnion")
                        {
                            if (dbRelationship.RelationshipType.UnionToSubUnionRelationshipType != null)
                                projectContext.UnionToSubUnionRelationshipType.Remove(dbRelationship.RelationshipType.UnionToSubUnionRelationshipType);
                            //if (dbRelationship.RelationshipType.SubUnionToUnionRelationshipType != null)
                            //    projectContext.SubUnionToUnionRelationshipType.Remove(dbRelationship.RelationshipType.SubUnionToUnionRelationshipType);
                            if (dbReverseRelationship.RelationshipType.SubUnionToUnionRelationshipType != null)
                                projectContext.SubUnionToUnionRelationshipType.Remove(dbReverseRelationship.RelationshipType.SubUnionToUnionRelationshipType);
                            //if (dbReverseRelationship.RelationshipType.UnionToSubUnionRelationshipType != null)
                            //    projectContext.UnionToSubUnionRelationshipType.Remove(dbReverseRelationship.RelationshipType.UnionToSubUnionRelationshipType);

                        }
                        if (convertType == "UnionToSubUnion_!UnionHoldsKeys_UnionToSubUnion")
                        {
                            dbRelationship.RelationshipType.UnionToSubUnionRelationshipType.UnionRelationshipTypeID = e.UnionRelationship.ID;
                            dbReverseRelationship.RelationshipType.SubUnionToUnionRelationshipType.UnionRelationshipTypeID = e.UnionRelationship.ID;
                        }
                        else
                        {
                            dbRelationship.RelationshipType.SubUnionToUnionRelationshipType = new SubUnionToUnionRelationshipType();
                            dbRelationship.RelationshipType.IsOtherSideCreatable = true;
                            dbRelationship.RelationshipType.SubUnionToUnionRelationshipType.UnionRelationshipTypeID = e.UnionRelationship.ID;

                            dbReverseRelationship.RelationshipType.UnionToSubUnionRelationshipType = new UnionToSubUnionRelationshipType();
                            dbReverseRelationship.RelationshipType.IsOtherSideCreatable = true;
                            dbReverseRelationship.RelationshipType.UnionToSubUnionRelationshipType.UnionRelationshipTypeID = e.UnionRelationship.ID;

                        }


                    }
                    else if (convertType == "OneToMany_UnionToSubUnion"
                          || convertType == "ImplicitOneToOne_UnionToSubUnion"
                          || convertType == "SuperToSub_UnionToSubUnion"
                          || convertType == "SubUnionToUnion_UnionHoldsKeys_UnionToSubUnion"
                        || convertType == "SubUnionToUnion_UnionHoldsKeys_SubUnionToUnion")
                    {
                        //int id = dbRelationshipID;
                        //dbRelationshipID = dbReverseRelationshipID;
                        //dbReverseRelationshipID = id;


                        if (convertType == "OneToMany_UnionToSubUnion")
                        {
                            projectContext.OneToManyRelationshipType.Remove(dbRelationship.RelationshipType.OneToManyRelationshipType);
                            projectContext.ManyToOneRelationshipType.Remove(dbReverseRelationship.RelationshipType.ManyToOneRelationshipType);
                        }
                        else if (convertType == "ImplicitOneToOne_UnionToSubUnion")
                        {
                            projectContext.ImplicitOneToOneRelationshipType.Remove(dbRelationship.RelationshipType.ImplicitOneToOneRelationshipType);
                            projectContext.ExplicitOneToOneRelationshipType.Remove(dbReverseRelationship.RelationshipType.ExplicitOneToOneRelationshipType);
                        }
                        else if (convertType == "SuperToSub_UnionToSubUnion")
                        {
                            projectContext.SuperToSubRelationshipType.Remove(dbRelationship.RelationshipType.SuperToSubRelationshipType);
                            projectContext.SubToSuperRelationshipType.Remove(dbReverseRelationship.RelationshipType.SubToSuperRelationshipType);
                        }
                        else if (convertType == "SubUnionToUnion_UnionHoldsKeys_UnionToSubUnion")
                        {
                            //if (dbRelationship.RelationshipType.UnionToSubUnionRelationshipType != null)
                            //    projectContext.UnionToSubUnionRelationshipType.Remove(dbRelationship.RelationshipType.UnionToSubUnionRelationshipType);
                            if (dbRelationship.RelationshipType.SubUnionToUnionRelationshipType != null)
                                projectContext.SubUnionToUnionRelationshipType.Remove(dbRelationship.RelationshipType.SubUnionToUnionRelationshipType);
                            //if (dbReverseRelationship.RelationshipType.SubUnionToUnionRelationshipType != null)
                            //    projectContext.SubUnionToUnionRelationshipType.Remove(dbReverseRelationship.RelationshipType.SubUnionToUnionRelationshipType);
                            if (dbReverseRelationship.RelationshipType.UnionToSubUnionRelationshipType != null)
                                projectContext.UnionToSubUnionRelationshipType.Remove(dbReverseRelationship.RelationshipType.UnionToSubUnionRelationshipType);

                        }
                        if (convertType == "SubUnionToUnion_UnionHoldsKeys_SubUnionToUnion")
                        {
                            dbRelationship.RelationshipType.SubUnionToUnionRelationshipType.UnionRelationshipTypeID = e.UnionRelationship.ID;
                            dbReverseRelationship.RelationshipType.UnionToSubUnionRelationshipType.UnionRelationshipTypeID = e.UnionRelationship.ID;
                        }
                        else
                        {
                            dbRelationship.RelationshipType.UnionToSubUnionRelationshipType = new UnionToSubUnionRelationshipType();
                            dbRelationship.RelationshipType.IsOtherSideCreatable = true;
                            dbRelationship.RelationshipType.UnionToSubUnionRelationshipType.UnionRelationshipTypeID = e.UnionRelationship.ID;

                            dbReverseRelationship.RelationshipType.SubUnionToUnionRelationshipType = new SubUnionToUnionRelationshipType();
                            dbReverseRelationship.RelationshipType.IsOtherSideCreatable = true;
                            dbReverseRelationship.RelationshipType.SubUnionToUnionRelationshipType.UnionRelationshipTypeID = e.UnionRelationship.ID;
                        }

                    }





                    //////else if (convertType == "UnionToSubUnion_!UnionHoldsKeys_SubUnionToUnion")
                    //////{
                    //////    if (dbRelationship.RelationshipType.UnionToSubUnionRelationshipType != null)
                    //////        projectContext.UnionToSubUnionRelationshipType.Remove(dbRelationship.RelationshipType.UnionToSubUnionRelationshipType);
                    //////    if (dbRelationship.RelationshipType.SubUnionToUnionRelationshipType != null)
                    //////        projectContext.SubUnionToUnionRelationshipType.Remove(dbRelationship.RelationshipType.SubUnionToUnionRelationshipType);
                    //////    if (dbReverseRelationship.RelationshipType.SubUnionToUnionRelationshipType != null)
                    //////        projectContext.SubUnionToUnionRelationshipType.Remove(dbReverseRelationship.RelationshipType.SubUnionToUnionRelationshipType);
                    //////    if (dbReverseRelationship.RelationshipType.UnionToSubUnionRelationshipType != null)
                    //////        projectContext.UnionToSubUnionRelationshipType.Remove(dbReverseRelationship.RelationshipType.UnionToSubUnionRelationshipType);

                    //////    // projectContext.UnionToSubUnionRelationshipType.Remove(dbRelationship.RelationshipType.UnionToSubUnionRelationshipType);
                    //////    //   projectContext.SubUnionToUnionRelationshipType.Remove(dbReverseRelationship.RelationshipType.SubUnionToUnionRelationshipType);
                    //////}



                }
                projectContext.SaveChanges();
            }
        }

        private void ISARelationshipCreateOrSelect(List<DataAccess.ISARelationship> list, string convertType, int dbRelationship, int dbReverseRelationship)
        {
            frmISARelationshipCreateSelect frm = new frmISARelationshipCreateSelect(list);
            frm.ISARelationshipSelected += (sender, e) => frm_ISARelationshipSelected(sender, e, convertType, dbRelationship, dbReverseRelationship);
            frm.ShowDialog();
        }

        void frm_ISARelationshipSelected(object sender, ISARelationshipSelectedArg e, string convertType, int dbRelationshipID, int dbReverseRelationshipID)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                if (convertType == "OneToMany_SuperToSub"
                 || convertType == "ImplicitOneToOne_SuperToSub"
                 || convertType == "UnionToSubUnion_!UnionHoldsKeys_SuperToSub"
                 || convertType == "SubUnionToUnion_UnionHoldsKeys_SuperToSub"
                 || convertType == "SuperToSub_SuperToSub")
                {
                    var dbRelationship = projectContext.Relationship.First(x => x.ID == dbRelationshipID);
                    var dbReverseRelationship = projectContext.Relationship.First(x => x.ID == dbReverseRelationshipID);
                    if (convertType == "OneToMany_SuperToSub")
                    {
                        projectContext.OneToManyRelationshipType.Remove(dbRelationship.RelationshipType.OneToManyRelationshipType);
                        projectContext.ManyToOneRelationshipType.Remove(dbReverseRelationship.RelationshipType.ManyToOneRelationshipType);
                    }
                    else if (convertType == "ImplicitOneToOne_SuperToSub")
                    {
                        projectContext.ImplicitOneToOneRelationshipType.Remove(dbRelationship.RelationshipType.ImplicitOneToOneRelationshipType);
                        projectContext.ExplicitOneToOneRelationshipType.Remove(dbReverseRelationship.RelationshipType.ExplicitOneToOneRelationshipType);
                    }
                    else if (convertType == "UnionToSubUnion_!UnionHoldsKeys_SuperToSub")
                    {
                        if (dbRelationship.RelationshipType.UnionToSubUnionRelationshipType != null)
                            projectContext.UnionToSubUnionRelationshipType.Remove(dbRelationship.RelationshipType.UnionToSubUnionRelationshipType);
                        //if (dbRelationship.RelationshipType.SubUnionToUnionRelationshipType != null)
                        //    projectContext.SubUnionToUnionRelationshipType.Remove(dbRelationship.RelationshipType.SubUnionToUnionRelationshipType);
                        if (dbReverseRelationship.RelationshipType.SubUnionToUnionRelationshipType != null)
                            projectContext.SubUnionToUnionRelationshipType.Remove(dbReverseRelationship.RelationshipType.SubUnionToUnionRelationshipType);
                        //if (dbReverseRelationship.RelationshipType.UnionToSubUnionRelationshipType != null)
                        //    projectContext.UnionToSubUnionRelationshipType.Remove(dbReverseRelationship.RelationshipType.UnionToSubUnionRelationshipType);


                        //projectContext.UnionToSubUnionRelationshipType.Remove(dbRelationship.RelationshipType.UnionToSubUnionRelationshipType);
                        //projectContext.SubUnionToUnionRelationshipType.Remove(dbReverseRelationship.RelationshipType.SubUnionToUnionRelationshipType);
                    }
                    else if (convertType == "SubUnionToUnion_UnionHoldsKeys_SuperToSub")
                    {
                        //if (dbRelationship.RelationshipType.UnionToSubUnionRelationshipType != null)
                        //    projectContext.UnionToSubUnionRelationshipType.Remove(dbRelationship.RelationshipType.UnionToSubUnionRelationshipType);
                        if (dbRelationship.RelationshipType.SubUnionToUnionRelationshipType != null)
                            projectContext.SubUnionToUnionRelationshipType.Remove(dbRelationship.RelationshipType.SubUnionToUnionRelationshipType);
                        //if (dbReverseRelationship.RelationshipType.SubUnionToUnionRelationshipType != null)
                        //    projectContext.SubUnionToUnionRelationshipType.Remove(dbReverseRelationship.RelationshipType.SubUnionToUnionRelationshipType);
                        if (dbReverseRelationship.RelationshipType.UnionToSubUnionRelationshipType != null)
                            projectContext.UnionToSubUnionRelationshipType.Remove(dbReverseRelationship.RelationshipType.UnionToSubUnionRelationshipType);


                        //projectContext.UnionToSubUnionRelationshipType.Remove(dbRelationship.RelationshipType.UnionToSubUnionRelationshipType);
                        //projectContext.SubUnionToUnionRelationshipType.Remove(dbReverseRelationship.RelationshipType.SubUnionToUnionRelationshipType);
                    }


                    if (convertType == "SuperToSub_SuperToSub")
                    {
                        dbRelationship.RelationshipType.SuperToSubRelationshipType.ISARelationshipID = e.ISARelationship.ID;
                        dbReverseRelationship.RelationshipType.SubToSuperRelationshipType.ISARelationshipID = e.ISARelationship.ID;
                    }
                    else
                    {
                        dbRelationship.RelationshipType.SuperToSubRelationshipType = new SuperToSubRelationshipType();
                        dbRelationship.RelationshipType.IsOtherSideCreatable = true;
                        dbRelationship.RelationshipType.SuperToSubRelationshipType.ISARelationshipID = e.ISARelationship.ID;

                        dbReverseRelationship.RelationshipType.SubToSuperRelationshipType = new SubToSuperRelationshipType();
                        dbReverseRelationship.RelationshipType.IsOtherSideCreatable = true;
                        dbReverseRelationship.RelationshipType.SubToSuperRelationshipType.ISARelationshipID = e.ISARelationship.ID;
                    }

                }
                projectContext.SaveChanges();
            }
        }

        internal void MergeISARelationships(string p, List<ISARelationshipDTO> relationships, ISARelationshipDTO selectedOne)
        {
            if (relationships.GroupBy(x => x.SuperTypeEntities).Count() > 1)
            {
                MessageBox.Show("برای ادغام چند رابطه ارث بری تمامی موجودیتهای پدر میباست از یک نوع باشند");
                return;
            }
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                int isaRelationID = 0;
                foreach (var relationship in relationships)
                {

                    //if (relationship == selectedOne)
                    //    isaRelationID = relationship.ID;
                    //else
                    //{
                    var dbRelationship = projectContext.ISARelationship.First(x => x.ID == relationship.ID);
                    foreach (var detail in dbRelationship.SuperToSubRelationshipType)
                    {
                        detail.ISARelationshipID = selectedOne.ID;
                    }
                    foreach (var detail in dbRelationship.SubToSuperRelationshipType)
                    {
                        detail.ISARelationshipID = selectedOne.ID;
                    }
                    //}

                }
                projectContext.SaveChanges();
            }

        }

        internal void MergeUnionRelationships(string p, List<UnionRelationship> relationships, UnionRelationship selectedOne)
        {
            if (relationships.GroupBy(x => new { x.UnionTypeEntities, x.UnionHoldsKeys }).Count() > 1)
            {
                MessageBox.Show("برای ادغام چند رابطه اتحاد تمامی موجودیتهای پدر میباست از یک نوع باشند");
                return;
            }
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                //   int isaRelationID = 0;
                foreach (var relationship in relationships)
                {

                    //if (relationship == selectedOne)
                    //    isaRelationID = relationship.ID;
                    //else
                    //{
                    var dbRelationship = projectContext.UnionRelationshipType.First(x => x.ID == relationship.ID);
                    foreach (var detail in dbRelationship.UnionToSubUnionRelationshipType)
                    {
                        detail.UnionRelationshipTypeID = selectedOne.ID;
                    }
                    foreach (var detail in dbRelationship.SubUnionToUnionRelationshipType)
                    {
                        detail.UnionRelationshipTypeID = selectedOne.ID;
                    }
                    //}

                }
                projectContext.SaveChanges();
            }
        }

        internal void DuplicateEntity(EntityDTO entity, bool inheritance)
        {
            frmEditEntity frm = new frmEditEntity(entity.ID, inheritance);
            frm.ShowDialog();
            //using (var projectContext = new DataAccess.MyProjectEntities())
            //{

            //}
        }

        //internal void EditEntity(EntityDTO entity)
        //{
        //    frmEditEntity frm = new frmEditEntity(entity, false);
        //    frm.ShowDialog();
        //}









       
    }
    public enum enum_ColumnType
    {
        None,
        StringColumnType,
        NumericColumnType,
        DataColumnType
    }
    public class ColumnDTO
    {
        public int ID { set; get; }
        public string Name { set; get; }
        public bool IsMandatory { set; get; }
        public bool IsNull { set; get; }
        public string Alias { set; get; }
        //public bool Select { set; get; }
    }

    public class EntityDTO
    {
        public EntityDTO()
        {
            //SuperTypeEntityIDs = new List<int>();
            //UnionTypeEntityIDs = new List<int>();
        }
        public int ID { set; get; }
        public string Name { set; get; }
        public int TableID { set; get; }
        public string Table { set; get; }
        public string Alias { set; get; }
        public string Criteria { set; get; }
        public bool? IndependentDataEntry { set; get; }
        public string SuperTypeEntities { set; get; }
        public string SubTypeEntities { set; get; }
        //public List<int> SuperTypeEntityIDs { set; get; }
        public string UnionTypeEntities { set; get; }
        public string SubUnionTypeEntities { set; get; }
        //public List<int> UnionTypeEntityIDs { set; get; }
        public bool? BatchDataEntry { set; get; }
        public bool? IsDataReference { set; get; }
        public bool? IsStructurReferencee { set; get; }
        public bool? IsAssociative { set; get; }
    }

    public class SimpleEntityDTO
    {
        public SimpleEntityDTO()
        {
            //SuperTypeEntityIDs = new List<int>();
            //UnionTypeEntityIDs = new List<int>();
        }
        public int ID { set; get; }
        public string Name { set; get; }
        public int TableID { set; get; }
        public string Table { set; get; }
        public string Alias { set; get; }
        public string Criteria { set; get; }
        public bool? IndependentDataEntry { set; get; }
        public bool? IsDataReference { set; get; }
        public bool? IsStructurReferencee { set; get; }
        public bool? IsAssociative { set; get; }
        public bool? BatchDataEntry { set; get; }

    }
    public class UniqueConstraintDTO
    {
        public int ID { set; get; }
        public string Table { set; get; }
        public string Name { set; get; }
        public string Columns { set; get; }
    }

    public class RelationshipDTO
    {
        public int ID { set; get; }
        public string Name { set; get; }
        public string PairRelationship { set; get; }
        public int? PairRelationshipID { set; get; }
        public string Alias { set; get; }
        public string Entity1 { set; get; }
        public string Entity2 { set; get; }
        public string RelationshipColumns { set; get; }
        public string TypeStr { set; get; }
        public Enum_RelationshipType TypeEnum { set; get; }
        public bool? DataEntryEnabled { set; get; }
        public bool? SearchEnabled { set; get; }
        public bool? ViewEnabled { set; get; }
        public bool? Enabled { set; get; }

        public bool Select { set; get; }
    }
    public class OneToMany : RelationshipDTO
    {
        public short? MasterDetailState { set; get; }
        public int? DetailsCount { set; get; }
        public bool IsManySideMadatory { set; get; }
        public bool IsManySideCreatable { set; get; }
        public bool? IsManySideDirectlyCreatable { set; get; }

    }

    public class ManyToOne : RelationshipDTO
    {
        public bool? IsOneSideTransferable { set; get; }
        public bool IsOneSideCreatable { set; get; }
        public bool IsOneSideMadatory { set; get; }
        public bool? IsOneSideDirectlyCreatable { set; get; }
    }

    public class ExplicitOneToOne : RelationshipDTO
    {
        public bool? IsOtherSideTransferable { set; get; }
        public bool IsOtherSideCreatable { set; get; }
        public bool IsOtherSideMandatory { set; get; }
        public bool? IsOtherSideDirectlyCreatable { set; get; }
    }
    public class ImplicitOneToOne : RelationshipDTO
    {
        public bool? IsOtherSideTransferable { set; get; }
        public bool IsOtherSideCreatable { set; get; }
        public bool IsOtherSideMandatory { set; get; }
        public bool? IsOtherSideDirectlyCreatable { set; get; }
    }
    public class SuperToSub : RelationshipDTO
    {
        public bool? IsOtherSideTransferable { set; get; }
        public bool IsOtherSideCreatable { set; get; }
        public bool? IsOtherSideDirectlyCreatable { set; get; }
    }
    public class SubToSuper : RelationshipDTO
    {
        public bool? IsOtherSideTransferable { set; get; }
        public bool IsOtherSideCreatable { set; get; }
        public bool? IsOtherSideDirectlyCreatable { set; get; }
    }
    public class ManyToMany
    {
        public int ID { set; get; }
        public string Name { set; get; }
    }
    public class ISARelationshipDTO
    {
        public int ID { set; get; }
        public string Name { set; get; }
        public string SuperTypeEntities { set; get; }
        public string SubTypeEntities { set; get; }
        public bool? IsGeneralization { set; get; }
        public bool? IsSpecialization { set; get; }
        public bool IsTolatParticipation { set; get; }
        public bool IsDisjoint { set; get; }

    }

    public class UnionRelationship
    {
        public int ID { set; get; }
        public string Name { set; get; }
        public string UnionTypeEntities { set; get; }
        public string SubUnionTypeEntities { set; get; }
        public bool IsTolatParticipation { set; get; }
        public bool? UnionHoldsKeys { set; get; }
    }
    public class UnionToSubUnion : RelationshipDTO
    {
        public bool? IsOtherSideTransferable { set; get; }
        public bool IsOtherSideCreatable { set; get; }
        public bool? IsOtherSideDirectlyCreatable { set; get; }
        public bool UnionHoldsKeys { set; get; }
    }
    public class SubUnionToUnion : RelationshipDTO
    {
        public bool? IsOtherSideTransferable { set; get; }
        public bool IsOtherSideCreatable { set; get; }
        public bool? IsOtherSideDirectlyCreatable { set; get; }
        public bool UnionHoldsKeys { set; get; }
    }

    public enum Enum_RelationshipType
    {
        None,
        ManyToOne,
        OneToMany,
        ExplicitOneToOne,
        ImplicitOneToOne,
        SubToSuper,
        SuperToSub,
        SubUnionToUnion_UnionHoldsKeys,
        UnionToSubUnion_UnionHoldsKeys,
        SubUnionToUnion_SubUnionHoldsKeys,
        UnionToSubUnion_SubUnionHoldsKeys
    }
}
