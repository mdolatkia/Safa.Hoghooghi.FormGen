using ModelEntites;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
namespace ProxyLibrary
{

    public class ChildRelationshipInfo
    {
        public event EventHandler<System.Collections.Specialized.NotifyCollectionChangedEventArgs> CollectionChanged;
        public ChildRelationshipInfo()
        {
            RelatedData = new ObservableCollection<ProxyLibrary.DP_DataRepository>();
            RelatedData.CollectionChanged += RelatedData_CollectionChanged;
            OriginalRelatedData = new ObservableCollection<ProxyLibrary.DP_DataRepository>();
            RemovedDataForUpdate = new ObservableCollection<ProxyLibrary.DP_DataRepository>();
            //RemovedItems = new List<ProxyLibrary.DP_DataRepository>();
            ReadonlyStateFromTails = new List<string>();
        }

        private void RelatedData_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (CollectionChanged != null)
                CollectionChanged(sender, e);
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (DP_DataRepository item in e.NewItems)
                {
                    //    item.PropertyValueChanged += Item_PropertyValueChanged;
                    foreach (var changeMonitor in ChangeMonitorItems)//.Where(x => !string.IsNullOrEmpty(x.RestTail)))
                    {
                        item.AddChangeMonitor(changeMonitor.GeneralKey, changeMonitor.UsageKey, changeMonitor.RestTail, changeMonitor.columnID, changeMonitor.DataToCall);
                    }
                }

            }

            //if (ChangeMonitorItems.Any(x => x.columnID == 0))
            //{
            foreach (var item in ChangeMonitorItems)//.Where(x => x.columnID == 0))
            {
                item.DataToCall.OnRelatedDataOrColumnChanged(item);
            }

            //}


        }



        //    RelationshipDeleteOption RelationshipDeleteOption { set; get; }
        public RelationshipDeleteOption RelationshipDeleteOption { set; get; }
        public ProxyLibrary.DP_DataRepository SourceData { set; get; }
        //public int SourceRelationID;
        //public int SourceEntityID;
        //public int SourceTableID;
        //public int TargetTableID;
        //public Enum_RelationshipType SourceToTargetRelationshipType;
        //public Enum_MasterRelationshipType SourceToTargetMasterRelationshipType;

        public void AddDataToChildRelationshipInfo(DP_DataRepository dataItem, bool fromDB)
        {
            dataItem.ParantChildRelationshipInfo = this;
            RelatedData.Add(dataItem);
            if (fromDB)
            {
                ProxyLibrary.DP_DataRepository orgData = new ProxyLibrary.DP_DataRepository(dataItem.TargetEntityID, dataItem.TargetEntityAlias);
                orgData.ParantChildRelationshipInfo = this;
                foreach (var item in dataItem.KeyProperties)
                    orgData.AddCopyProperty(item);
                OriginalRelatedData.Add(orgData);
            }
        }
        //public bool CheckRelationshipIsChanged(ChildRelationshipInfo relation)
        //{
        //    if (OriginalRelatedData.Any(x => x.KeyProperties.All(y => !RelatedData.Any(z => z.IsNewItem == false && z.KeyProperties.Any(u => u.ColumnID == y.ColumnID && u.Value == y.Value)))))
        //    {
        //        //داده ای حذف شده است
        //        return true;
        //    }
        //    if (RelatedData.Any(x => x.KeyProperties.All(y => !OriginalRelatedData.Any(z => z.KeyProperties.Any(u => u.ColumnID == y.ColumnID && u.Value == y.Value)))))
        //    {
        //        //داده ای اضافه شده است
        //        return true;
        //    }
        //    return false;
        //}

        public bool CheckRelationshipIsChanged()
        {
            foreach (var deleted in OriginalRelatedData.Where(x => !x.KeyProperties.All(y => RelatedData.Any(z => z.IsNewItem == false && z.KeyProperties.Any(u => u.ColumnID == y.ColumnID && u.Value == y.Value)))))
            {
                return true;
            }
            foreach (var added in RelatedData.Where(x => !x.KeyProperties.All(y => OriginalRelatedData.Any(z => z.KeyProperties.Any(u => u.ColumnID == y.ColumnID && u.Value == y.Value)))))
            {
                return true;
            }
            return false;
        }
        public bool DataItemIsAdded(DP_DataRepository item)
        {
            return item.IsNewItem || !item.KeyProperties.All(y => OriginalRelatedData.Any(z => z.KeyProperties.Any(u => u.ColumnID == y.ColumnID && u.Value == y.Value)));
        }
        //public bool OriginalDataItemIsRemoved(DP_DataRepository originalData)
        //{
        //}
        //public void CheckAddedRemovedRelationships()
        //{
        //    //if (IsReadonly)
        //    //    return;
        //    foreach (var deleted in OriginalRelatedData.Where(x => !x.KeyProperties.All(y => RelatedData.Any(z => z.IsNewItem == false && z.KeyProperties.Any(u => u.ColumnID == y.ColumnID && u.Value == y.Value)))))
        //    {
        //        if (deleted.IsHidden)
        //        {
        //            throw (new Exception("داده غیر فعال امکان حذف شدن را ندارد"));
        //        }
        //        deleted.RelationshipIsRemoved = true;
        //    }
        //    foreach (var added in RelatedData.Where(x => !x.KeyProperties.All(y => OriginalRelatedData.Any(z => z.KeyProperties.Any(u => u.ColumnID == y.ColumnID && u.Value == y.Value)))))
        //    {
        //        added.RelationshipIsAdded = true;
        //        if (added.IsHidden)
        //        {
        //            throw (new Exception("داده غیر فعال امکان اضافه شدن را ندارد"));
        //        }
        //    }
        //}
        public bool RelationshipIsChangedForUpdate
        {
            get
            {
                return RemovedDataForUpdate.Any() || RelatedData.Any(x => DataItemIsAdded(x));
            }
        }
        public List<DP_DataRepository> GetRelatedData(int relationshipID)
        {
            return RelatedData.ToList();
            //    else return new List<ProxyLibrary.DP_DataRepository>();
        }
        public void RemoveRelatedData(DP_DataRepository dP_DataRepository)
        {
            //var childRelationshipInfo = ChildRelationshipInfos.FirstOrDefault(x => x.Relationship.ID == relationshipID);
            //if (childRelationshipInfo != null)
            RelatedData.Remove(dP_DataRepository);

        }
        public void RemoveRelatedData()
        {
            //var childRelationshipInfo = ChildRelationshipInfos.FirstOrDefault(x => x.Relationship.ID == relationshipID);
            //if (childRelationshipInfo != null)
            RelatedData.Clear();

        }


        //internal void AddDataObserver(string key, string restTail, int columnID, DP_DataRepository targetDataItem, bool setSourceOrChilds)
        //{
        //    ObserverListForColumn.Add(new ObserverData()
        //    {
        //        Key = key,
        //        ColumnID = columnID,
        //        TargetDataItem = targetDataItem,
        //        SourceRalationshipTail = restTail
        //    });

        //    if (setSourceOrChilds)
        //    {
        //        SourceData.AddDataObserver(key, restTail, columnID, targetDataItem);
        //    }
        //    else
        //    {
        //        foreach (var relatedData in RelatedData)
        //        {
        //            relatedData.AddDataObserver(key, restTail, columnID, targetDataItem);
        //        }
        //    }
        //}


        //internal void RemoveDataObserver(string key, string restTail, DP_DataRepository targetDataItem)
        //{
        //    foreach (var item in ObserverList.ToList())
        //    {
        //        if (key == item.Key && item.TargetDataItem == targetDataItem)
        //        {
        //            ObserverList.Remove(item);
        //        }
        //    }

        //}
        public RelationshipDTO Relationship { set; get; }

        public ObservableCollection<DP_DataRepository> RemovedDataForUpdate { set; get; }
        public ObservableCollection<DP_DataRepository> RelatedData { set; get; }
        public ObservableCollection<DP_DataRepository> RealData
        {
            get
            {
                ObservableCollection<DP_DataRepository> result = new ObservableCollection<DP_DataRepository>();
                foreach (var item in RelatedData)
                {
                    if (item.ShoudBeCounted)
                    {
                        if (item.IsHiddenBecauseOfCreatorRelationshipOnState || this.IsReadonly || item.IsReadonlyBecauseOfCreatorRelationship)
                        {
                            if (!DataItemIsAdded(item))
                                result.Add(item);
                        }
                        else
                            result.Add(item);
                    }
                }
                foreach (var item in RemovedOriginalDatas)
                {
                    if (item.IsHiddenBecauseOfCreatorRelationshipOnState || this.IsReadonly || item.IsReadonlyBecauseOfCreatorRelationship)
                    {
                        result.Add(item);
                    }
                }
                return result;
            }
        }

        public bool SecurityIssue { get; set; }
        public ObservableCollection<DP_DataRepository> OriginalRelatedData { get; private set; }
        public List<DP_DataRepository> RemovedOriginalDatas
        {
            get
            {
                return OriginalRelatedData.Where(x => !x.KeyProperties.All(y => RelatedData.Any(z => z.KeyProperties.Any(u => u.ColumnID == y.ColumnID && u.Value == y.Value)))).ToList();
            }
        }
        public List<string> ReadonlyStateFromTails { set; get; }
        public bool IsHidden { get; set; }

        //میشه ریدونلی بودن ریلیشنشیپ رو هم داخل این گذاشت یا لازم نیست؟
        public bool IsReadonly { get; set; }


        List<ChangeMonitor> ChangeMonitorItems = new List<ChangeMonitor>();

        //اینجا وظیفه چک کردن هم داده ها و هم ستونهای داده را دارد
        internal void AddChangeMonitor(string generalKey, string usageKey, string restTail, int columnID, DP_DataRepository dataToCall)
        {
            ChangeMonitorItems.Add(new ChangeMonitor()
            {
                GeneralKey = generalKey,
                UsageKey = usageKey,
                DataToCall = dataToCall,
                columnID = columnID,
                RestTail = restTail

            });

            //if (!string.IsNullOrEmpty(restTail))
            //{
            foreach (var relatedData in RelatedData)
            {
                relatedData.AddChangeMonitor(generalKey, usageKey, restTail, columnID, dataToCall);
            }
            //}
        }



        internal void RemoveChangeMonitorByGenaralKey(string key)
        {
            foreach (var item in ChangeMonitorItems.Where(x => x.GeneralKey == key).ToList())
            {
                ChangeMonitorItems.Remove(item);
                foreach (var data in RelatedData)
                {
                    data.RemoveChangeMonitorByGenaralKey(key);
                }
            }
        }

        //public bool OriginalDataHasBecomeHidden(DP_DataRepository orginalData)
        //{
        //    var currentData = RelatedData.FirstOrDefault(z => z.IsNewItem == false && orginalData.KeyProperties.All(x => z.KeyProperties.Any(u => x.ColumnID == u.ColumnID && x.Value == u.Value)));
        //    if (currentData != null && currentData.IsHidden)
        //        return true;
        //    else
        //        return false;
        //}

        public DP_DataRepository GetOroginalDataOfOriginalData(DP_DataRepository data)
        {
            return OriginalRelatedData.First(z => data.KeyProperties.All(y => z.KeyProperties.Any(u => u.ColumnID == y.ColumnID && u.Value == y.Value)));
        }

        public DP_DataRepository GetRelatedDataOfOriginalData(DP_DataRepository orginalData)
        {
            return RelatedData.First(z => orginalData.KeyProperties.All(y => z.KeyProperties.Any(u => u.ColumnID == y.ColumnID && u.Value == y.Value)));
        }



        //public bool OriginalDataHasBecomeReadonlyAndNotExists(DP_DataRepository orginalRelationships)
        //{
        //    var currentData = RelatedData.FirstOrDefault(z => z.IsNewItem == false && orginalRelationships.KeyProperties.All(x => z.KeyProperties.Any(u => x.ColumnID == u.ColumnID && x.Value == u.Value)));
        //    if (currentData != null && currentData.IsReadonly)
        //        return true;
        //    else
        //        return false;
        //}
        //   public bool RelationshipIsChanged { get; set; }
    }
    public class ReadonlyStateFromTail
    {
        public ReadonlyStateFromTail(string relationshipTail)
        {
            RelationshipTail = relationshipTail;
        }
        public string RelationshipTail { set; get; }

    }
}
