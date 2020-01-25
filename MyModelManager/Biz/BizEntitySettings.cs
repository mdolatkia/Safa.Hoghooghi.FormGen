using ModelEntites;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyModelManager
{
    public class BizEntitySettings
    {
        BizEntityListView bizEntityListView = new BizEntityListView();
        BizEntitySearch bizEntitySearch = new BizEntitySearch();
        BizEntityUIComposition bizEntityUIComposition = new BizEntityUIComposition();
        public event EventHandler<ItemImportingStartedArg> ItemImportingStarted;
        private void BizTableDrivedEntity_ItemImportingStarted(object sender, ItemImportingStartedArg e)
        {
            if (ItemImportingStarted != null)
                ItemImportingStarted(this, e);
        }
        public void UpdateDefaultSettingsInModel(DR_Requester requester, List<int> entityIDs)
        {
            BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
            bizTableDrivedEntity.ItemImportingStarted += BizTableDrivedEntity_ItemImportingStarted;
            List<TableDrivedEntityDTO> listEntities = new List<TableDrivedEntityDTO>();
            foreach (var id in entityIDs)
            {
                listEntities.Add(bizTableDrivedEntity.GetTableDrivedEntity(requester, id, EntityColumnInfoType.WithFullColumns, EntityRelationshipInfoType.WithRelationships));
            }
            var allEntities = bizTableDrivedEntity.GetOrginalEntities(listEntities.First().DatabaseID, EntityColumnInfoType.WithFullColumns, EntityRelationshipInfoType.WithRelationships, false);

            UpdateDefaultSettingsInModel(listEntities, listEntities, listEntities, allEntities);
        }
        public void UpdateDefaultSettingsInModel(int databaseID)
        {
            //try
            //{
            BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
            bizTableDrivedEntity.ItemImportingStarted += BizTableDrivedEntity_ItemImportingStarted;

            var allEntities = bizTableDrivedEntity.GetOrginalEntities(databaseID, EntityColumnInfoType.WithFullColumns, EntityRelationshipInfoType.WithRelationships, false);

            var uiCompositionEntities = bizTableDrivedEntity.GetOrginalEntitiesWithoutUIComposition(databaseID, EntityColumnInfoType.WithFullColumns, EntityRelationshipInfoType.WithRelationships, false);
            var listViewEntities = allEntities.Where(x => x.EntityListViewID == 0).ToList();

            var searchEntities = allEntities.Where(x => x.EntitySearchID == 0).ToList();

            UpdateDefaultSettingsInModel(uiCompositionEntities, listViewEntities, searchEntities, allEntities);



        }
        public void UpdateDefaultSettingsInModel(List<TableDrivedEntityDTO> uiCompositionEntities, List<TableDrivedEntityDTO> listViewEntities, List<TableDrivedEntityDTO> searchEntities, List<TableDrivedEntityDTO> allEntities)
        {

            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                List<Tuple<TableDrivedEntityDTO, List<EntityUICompositionDTO>>> listEntityUIComposition = new List<Tuple<TableDrivedEntityDTO, List<EntityUICompositionDTO>>>();

                foreach (var entity in uiCompositionEntities)
                {
                    if (ItemImportingStarted != null)
                        ItemImportingStarted(this, new ItemImportingStartedArg() { ItemName = "Fetching entity" + " " + entity.Name, TotalProgressCount = uiCompositionEntities.Count(), CurrentProgress = uiCompositionEntities.IndexOf(entity) + 1 });
                    var uiComposition = bizEntityUIComposition.GenerateUIComposition(entity);
                    listEntityUIComposition.Add(new Tuple<TableDrivedEntityDTO, List<EntityUICompositionDTO>>(entity, uiComposition));

                }
                foreach (var item in listEntityUIComposition)
                {
                    if (ItemImportingStarted != null)
                        ItemImportingStarted(this, new ItemImportingStartedArg() { ItemName = "Setting UI for entity" + " " + item.Item1.Name, TotalProgressCount = listEntityUIComposition.Count(), CurrentProgress = listEntityUIComposition.IndexOf(item) + 1 });
                    bizEntityUIComposition.SaveItem(projectContext, item.Item1.ID, item.Item2);
                }




                List<Tuple<TableDrivedEntityDTO, EntityListViewDTO>> listEntityAndView = new List<Tuple<TableDrivedEntityDTO, EntityListViewDTO>>();
                foreach (var entity in listViewEntities)
                {
                    if (ItemImportingStarted != null)
                        ItemImportingStarted(this, new ItemImportingStartedArg() { ItemName = "Fetching entity" + " " + entity.Name, TotalProgressCount = listViewEntities.Count(), CurrentProgress = listViewEntities.IndexOf(entity) + 1 });
                    var viewItem = bizEntityListView.GenerateDefaultListView(entity, allEntities);
                    listEntityAndView.Add(new Tuple<TableDrivedEntityDTO, EntityListViewDTO>(entity, viewItem));
                }

                foreach (var item in listEntityAndView)
                {
                    if (ItemImportingStarted != null)
                        ItemImportingStarted(this, new ItemImportingStartedArg() { ItemName = "Setting default view for entity" + " " + item.Item1.Name, TotalProgressCount = listEntityAndView.Count(), CurrentProgress = listEntityAndView.IndexOf(item) + 1 });
                    var dbListView = bizEntityListView.SaveItem(projectContext, item.Item2);
                    var dbentity = projectContext.TableDrivedEntity.First(x => x.ID == item.Item1.ID);
                    dbentity.EntityListView1 = dbListView;
                }


                List<Tuple<TableDrivedEntityDTO, EntitySearchDTO>> listEntityAndSearch = new List<Tuple<TableDrivedEntityDTO, EntitySearchDTO>>();
                foreach (var entity in searchEntities)
                {
                    if (ItemImportingStarted != null)
                        ItemImportingStarted(this, new ItemImportingStartedArg() { ItemName = "Fetching entity" + " " + entity.Name, TotalProgressCount = searchEntities.Count(), CurrentProgress = searchEntities.IndexOf(entity) + 1 });
                    var searchItem = bizEntitySearch.GenerateDefaultSearchList(entity, allEntities);
                    listEntityAndSearch.Add(new Tuple<TableDrivedEntityDTO, EntitySearchDTO>(entity, searchItem));
                }

                foreach (var item in listEntityAndSearch)
                {
                    if (ItemImportingStarted != null)
                        ItemImportingStarted(this, new ItemImportingStartedArg() { ItemName = "Setting default search for entity" + " " + item.Item1.Name, TotalProgressCount = listEntityAndSearch.Count(), CurrentProgress = listEntityAndSearch.IndexOf(item) + 1 });
                    var dbSearch = bizEntitySearch.SaveItem(projectContext, item.Item2);
                    var dbentity = projectContext.TableDrivedEntity.First(x => x.ID == item.Item1.ID);
                    dbentity.EntitySearch = dbSearch;
                }


                if (ItemImportingStarted != null)
                    ItemImportingStarted(this, new ItemImportingStartedArg() { ItemName = "Saving changes" });
                projectContext.SaveChanges();

            }
        }
        public bool EntityWithoutSetting(int databaseID)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                return projectContext.TableDrivedEntity.Any(x =>
                x.IsOrginal == true && x.IsView == false && x.IsDisabled == false && x.Table.DBSchema.DatabaseInformationID == databaseID
                && (x.EntitySearchID == null || x.EntityListViewID == null || !x.EntityUIComposition.Any()));
            }
        }


    }
}
