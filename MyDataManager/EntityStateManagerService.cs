

using ModelEntites;
using MyModelManager;

using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MyDataManagerService
{
    public class EntityStateManagerService
    {
        BizEntityState bizEntityState = new BizEntityState();
        public List<EntityStateDTO> GetEntityStates(int entityID)
        {
            return bizEntityState.GetEntityStates(entityID, true);
        }

        //public DP_EntityActionActivitiesResult GetEntityActionActivities(DP_EntityActionActivitiesRequest request)
        //{
        //    BizActionActivity bizActionActivity = new BizActionActivity();
        //    DP_EntityActionActivitiesResult result = new DP_EntityActionActivitiesResult();
        //    result.ActionActivities = bizActionActivity.GetActionActivities(request.EntityID,
        //            new List<Enum_EntityActionActivityStep>() { Enum_EntityActionActivityStep.BeforeDelete, Enum_EntityActionActivityStep.BeforeLoad, Enum_EntityActionActivityStep.BeforeSave }, true);
        //    return result;
        //}
        public List<TableDrivedEntityDTO> SearchEntities(DP_SearchEntitiesRequest request)
        {
            //DP_EntityResult result = new DP_EntityResult();
            //BizTableDrivedEntity biz = new BizTableDrivedEntity();
            //result.Entity = biz.GetTableDrivedEntity(request.EntityID, entityColumnInfoType, entityRelationshipInfoType);
            //return result;
            return null;
        }
        public DP_EntityResult GetEntity(DP_EntityRequest request, EntityColumnInfoType entityColumnInfoType, EntityRelationshipInfoType entityRelationshipInfoType)
        {
            DP_EntityResult result = new DP_EntityResult();
            BizTableDrivedEntity biz = new BizTableDrivedEntity();
            result.Entity = biz.GetTableDrivedEntity(request.Requester, request.EntityID, entityColumnInfoType, entityRelationshipInfoType);
            return result;
        }
        //public DP_EntitySearchResult GetEntitySearch(DP_EntitySearchRequest request)
        //{
        //    DP_EntitySearchResult result = new DP_EntitySearchResult();
        //    BizEntitySearch biz = new BizEntitySearch();
        //    result.EntitySearch = biz.GetEntitySearch(request.EntitySearchID);
        //    return result;
        //}


        //public RR_ReportResult GetReport(RR_ReportRequest request, bool withDetails)
        //{
        //    RR_ReportResult result = new RR_ReportResult();
        //    BizEntityReport biz = new BizEntityReport();
        //    result.Report = biz.GetEntityReport(request.ReportID, withDetails);
        //    return result;
        //}
        //public DP_ResultDatabaseList GetDatabaseList(DP_DatabaseRequest request)
        //{
        //    DP_ResultDatabaseList result = new DP_ResultDatabaseList();
        //    BizDatabase biz = new BizDatabase();
        //    var list = biz.GetDatabases();
        //    foreach (var item in list)
        //    {
        //        if (SecurityHelper.PermissionGranted(request.Requester, "Database", item.Name))
        //        {
        //            DP_DatabaseListItem nitem = new DP_DatabaseListItem();
        //            nitem.Name = item.Name;
        //            result.Databases.Add(nitem);
        //        }
        //    }

        //    return result;
        //}

        //////private DP_PackageTreeStructure DP_PackageTreeStructureDBToND(DB_DPPackageTreeStructure dbItem)
        //////{
        //////    DP_PackageTreeStructure result = new DP_PackageTreeStructure();

        //////    result.ID = dbItem.ID;
        //////    if (dbItem.DB_DPPackage != null)
        //////        result.Package = DPPackageDBToND(dbItem.DB_DPPackage, false);
        //////    result.Name = dbItem.Name;

        //////    return result;
        //////}



        //////public GenericResult<OperationResult> UpdateAllDPPackages()
        //////{
        //////    GenericResult<OperationResult> result = new GenericResult<OperationResult>();

        //////    bool done = false;
        //////    bool faild = false;
        //////    List<DB_Package> listPackage = new List<DB_Package>();
        //////    using (IdeaEntities context = new IdeaEntities())
        //////    {
        //////        listPackage = context.DB_Package.Where(x => !x.DB_DPPackage.Any()).ToList();
        //////    }

        //////    foreach (var item in listPackage)
        //////    {
        //////        try
        //////        {
        //////            DP_Package package = new DP_Package();
        //////            package.NDPackageID = item.ID;
        //////            UpdateDPPackage(package);
        //////            done = true;
        //////        }
        //////        catch (Exception ex)
        //////        {
        //////            result.BaseResult.Messages.Add("خطا در بروزرسانی پکیج " + item.Name + Environment.NewLine + ex.Message);
        //////            faild = true;
        //////        }

        //////    }
        //////    if (done && faild)
        //////        result.Result = OperationResult.PartiallyDone;
        //////    else if (done)
        //////        result.Result = OperationResult.Done;
        //////    else if (faild)
        //////        result.Result = OperationResult.Failed;
        //////    return result;
        //////}
        ////////برای جنریت تستی
        //////public GenericResult<OperationResult> CreateDPPackagesByOneRelation(Guid mainNDTypeID, DataMaster.EntityRelations.ND_Relation ndRelation, List<DP_PackageTypeUISettings> packageTypeUISettings)
        //////{
        //////    GenericResult<OperationResult> result = new GenericResult<OperationResult>();

        //////    bool done = false;
        //////    bool faild = false;
        //////    DB_Relation dbRelation = null;



        //////    try
        //////    {


        //////        using (IdeaEntities context = new IdeaEntities())
        //////        {
        //////            DP_Package package = new DP_Package();
        //////            dbRelation = context.DB_Relation.Where(x => x.ID == ndRelation.ID).FirstOrDefault();
        //////            if (dbRelation != null)
        //////            {
        //////                DB_Type firstDBType = null;
        //////                DB_Type secondDBType = null;


        //////                if (dbRelation.FirstSide_DB_TypeID == mainNDTypeID)
        //////                {
        //////                    firstDBType = dbRelation.DB_Type;
        //////                    secondDBType = dbRelation.DB_Type2;
        //////                }
        //////                else if (dbRelation.SecondSide_DB_TypeID == mainNDTypeID)
        //////                {
        //////                    firstDBType = dbRelation.DB_Type2;
        //////                    secondDBType = dbRelation.DB_Type;
        //////                }


        //////                ND_Type firstNDType = new ND_Type();
        //////                firstNDType.ID = firstDBType.ID;

        //////                ND_Type secondNDType = new ND_Type();
        //////                secondNDType.ID = secondDBType.ID;
        //////                package.Name =3+"_"+ firstDBType.Name + "_" + secondDBType.Name;


        //////                package.TypeOrTypeConditions.Add(new ND_TypeOrTypeCondition() { Type = firstNDType });



        //////                package.TypeOrTypeConditions.Add(new ND_TypeOrTypeCondition() { Type = secondNDType });


        //////                package.Relations.Add(ndRelation);



        //////                package.PackageUITypeSettings = packageTypeUISettings;

        //////                //////DPUI_TypeSettings uiTypeSetting1 = new DPUI_TypeSettings();
        //////                //////uiTypeSetting1.TypeID = firstNDType.ID;
        //////                //////uiTypeSetting1.DataMode = Enum_DPUI_TypeDataMode.One;
        //////                //////uiTypeSetting1.IntractionMode = Enum_DPUI_TypeIntracionMode.Create;
        //////                //////uiTypeSetting1.IsMainType = true;
        //////                //////uiTypeSetting1.ItemSetting = GeneratePropertyItemSettingsFromNDTypeID(uiTypeSetting1.TypeID);
        //////                //////package.PackageUITypeSettings.Add(uiTypeSetting1);

        //////                //////DPUI_TypeSettings uiTypeSetting2 = new DPUI_TypeSettings();
        //////                //////uiTypeSetting2.TypeID = secondNDType.ID;
        //////                ////////اینجا باید از نوع رابطه خوانده شود
        //////                //////uiTypeSetting2.DataMode = Enum_DPUI_TypeDataMode.One;
        //////                //////uiTypeSetting2.IntractionMode = Enum_DPUI_TypeIntracionMode.Create;
        //////                //////uiTypeSetting2.IsMainType = faild;
        //////                //////uiTypeSetting2.ItemSetting = GeneratePropertyItemSettingsFromNDTypeID(uiTypeSetting2.TypeID);
        //////                //////package.PackageUITypeSettings.Add(uiTypeSetting2);

        //////                UpdateDPPackage(package);
        //////                done = true;
        //////            }
        //////        }



        //////    }
        //////    catch (Exception ex)
        //////    {
        //////        result.BaseResult.Messages.Add("خطا در بروزرسانی پکیج " + Environment.NewLine + ex.Message);
        //////        faild = true;
        //////    }


        //////    if (done && faild)
        //////        result.Result = OperationResult.PartiallyDone;
        //////    else if (done)
        //////        result.Result = OperationResult.Done;
        //////    else if (faild)
        //////        result.Result = OperationResult.Failed;
        //////    return result;
        //////}





        //////public Guid UpdateDPPackage(DP_Package ndItem)
        //////{
        //////    try
        //////    {
        //////        using (IdeaEntities context = new IdeaEntities())
        //////        {
        //////            var editItem = DPPackageNDToDB(ndItem, context); ;
        //////            if (editItem.DB_Package.ID == Guid.Empty)
        //////            {
        //////                editItem.DB_Package.ID = Guid.NewGuid();
        //////                context.DB_Package.Add(editItem.DB_Package);
        //////            }
        //////            if (editItem.ID == Guid.Empty)
        //////            {
        //////                editItem.ID = Guid.NewGuid();
        //////                context.DB_DPPackage.Add(editItem);
        //////            }


        //////            context.SaveChanges();
        //////            return editItem.ID;
        //////        }

        //////    }
        //////    catch (Exception ex)
        //////    {
        //////        throw ex;
        //////    }
        //////}

        //////private DB_DPPackage DPPackageNDToDB(DP_Package ndItem, IdeaEntities context)
        //////{
        //////    DB_DPPackage result;
        //////    if (ndItem.ID == Guid.Empty)
        //////    {
        //////        result = new DB_DPPackage();
        //////    }
        //////    else
        //////        result = context.DB_DPPackage.Where(x => x.ID == ndItem.ID).First();
        //////    if (ndItem.NDPackageID != Guid.Empty)
        //////        result.DB_PackageID = ndItem.NDPackageID;
        //////    else
        //////    {
        //////        DataMasterBusiness.EntityPackage.BizPackage bizPackage = new DataMasterBusiness.EntityPackage.BizPackage();
        //////        result.DB_Package = bizPackage.PackageNDToDB(ndItem, context);
        //////    }
        //////    //ادامه مخصوی این آبجکت نوشته شود

        //////    foreach (var item in ndItem.PackageUITypeSettings)
        //////    {
        //////        var editUITypeSettings = PackageUITypeSettingsNDToDB(item, context);
        //////        if (editUITypeSettings.ID == Guid.Empty)
        //////        {
        //////            editUITypeSettings.ID = Guid.NewGuid();
        //////            result.DB_DPPackage_UI_TypeSetting.Add(editUITypeSettings);
        //////        }

        //////    }
        //////    return result;


        //////}
        //////private DB_DPPackage_UI_TypeSetting PackageUITypeSettingsNDToDB(DP_PackageTypeUISettings ndItem, IdeaEntities context)
        //////{
        //////    DB_DPPackage_UI_TypeSetting result;
        //////    if (ndItem.ID == Guid.Empty)
        //////    {
        //////        result = new DB_DPPackage_UI_TypeSetting();
        //////    }
        //////    else
        //////    {
        //////        result = context.DB_DPPackage_UI_TypeSetting.First(x => x.ID == ndItem.ID);
        //////    }
        //////    result.DataMode = (short)ndItem.DataMode;
        //////    result.IntractionMode = (short)ndItem.IntractionMode;
        //////    result.IsMainType = ndItem.IsMainType;
        //////    result.DB_UI_TypeSettingID = ndItem.TypeUISetting.ID;
        //////    return result;
        //////}

        //////private DP_Package DPPackageDBToND(DB_DPPackage dbItem, bool complex)
        //////{
        //////    DP_Package result = new DP_Package(); ;

        //////    result.ID = dbItem.ID;
        //////    result.Name = dbItem.DB_Package.Name;
        //////    result.Description = dbItem.DB_Package.Description;
        //////    result.NDPackageID = dbItem.DB_Package.ID;

        //////    if (complex)
        //////    {
        //////        BizType bizType = new BizType();
        //////        // result.TypeConditions = new List<DataMaster.EntityRelations.ND_Type_TypeCondition>();
        //////        foreach (var item in dbItem.DB_Package.DB_Package_Type_TypeCondition)
        //////        {
        //////            if (item.DB_TypeConditionID != null && item.DB_TypeConditionID != Guid.Empty)
        //////            {//فعلا برای تایپ کاندیشن کلاس و تبدیل نداریم
        //////                //var typeCondition = bizType.TypeDBToND(item.DB_Type, complex);
        //////                //result.TypeConditionss.Add(typeCondition);
        //////            }
        //////            else
        //////            {
        //////                result.TypeOrTypeConditions.Add(new ND_TypeOrTypeCondition() { Type = bizType.TypeDBToND(item.DB_Type, complex) });
        //////            }
        //////            //var typeCondition = new DataMaster.EntityRelations.ND_Type_TypeCondition();


        //////        }

        //////        foreach (var item in dbItem.DB_Package.DB_Package_Relation)
        //////        {

        //////            DataMasterBusiness.EntityRelations.BizRelation bizRelation = new DataMasterBusiness.EntityRelations.BizRelation();
        //////            var relation = bizRelation.RelationDBToND(item.DB_Relation, false);
        //////            result.Relations.Add(relation);
        //////        }

        //////        BizTypeProperty bizTypeProperty = new BizTypeProperty();
        //////        //  result.Properties = new List<ND_Type_Property>();
        //////        foreach (var item in dbItem.DB_DPPackage_Type_Property)
        //////        {
        //////            result.Properties.Add(PackagePropertyDBToND(item, complex));
        //////        }
        //////        //  result.CalculatedProperties = new List<DP_PackageCalculatedProperty>();
        //////        foreach (var item in dbItem.DB_DPCalculatedProperty)
        //////        {
        //////            result.CalculatedProperties.Add(DPPackageCalculatedPropertyDBToND(item, complex));
        //////        }

        //////        //  result.Category = new List<DP_PackageCategory>();
        //////        foreach (var item in dbItem.DB_DPPackageCategory_DPPackage)
        //////        {
        //////            result.Category.Add(DP_PackageCategoryDBToND(item.DB_DPPackageCategory, complex));
        //////        }


        //////        ////جوری نوشته شود که اگر موجود نبود جنریت شود
        //////        foreach (var item in dbItem.DB_DPPackage_UI_TypeSetting)
        //////        {
        //////            BizUISetting bizUISetting = new BizUISetting();
        //////            result.PackageUITypeSettings.Add(PackageUITypeSettingsDBToND(item, complex));
        //////            //var typeUISettings = bizUISetting.GetUITypeSettings(item.DB_TypeID, item.DB_TypeConditionID);
        //////            //if (typeUISettings != null)
        //////            //    result.PackageUITypeSettings.Add(typeUISettings);
        //////        }

        //////        result.RelatedPackages = new List<DPPackageRelation>();
        //////        result.SpecifiedPackages = new List<DP_SpecifiedPackage>();



        //////        //result.GeneralSpecifiedPackage = new List<DP_SpecifiedPackage>();
        //////        //foreach (var item in dbItem.DB_DPSpecifiedPackage)
        //////        //{
        //////        //    bool isGeneral = true;
        //////        //    if (item.DB_DPEditPackage.Any())
        //////        //    {
        //////        //        isGeneral = false;
        //////        //    }
        //////        //    if (item.DB_DPSearchPackage.Any())
        //////        //    {
        //////        //        isGeneral = false;
        //////        //    }
        //////        //    if (item.DB_DPViewPackage.Any())
        //////        //    {
        //////        //        isGeneral = false;
        //////        //    }

        //////        //    if (isGeneral)
        //////        //    {
        //////        //        result.GeneralSpecifiedPackage.Add(DB_DPSpecifiedPackageDBToND(item, complex));
        //////        //    }

        //////        //}
        //////    }
        //////    return result;


        //////}

        //////private DP_PackageTypeUISettings PackageUITypeSettingsDBToND(DB_DPPackage_UI_TypeSetting dbItem, bool complex)
        //////{
        //////    DP_PackageTypeUISettings result = new DP_PackageTypeUISettings();
        //////    result.DataMode = (Enum_DPUI_TypeDataMode)dbItem.DataMode;
        //////    result.IntractionMode = (Enum_DPUI_TypeIntracionMode)dbItem.IntractionMode;
        //////    result.ID = dbItem.ID;
        //////    result.IsMainType = dbItem.IsMainType == true;
        //////    if (dbItem.DB_UI_TypeSetting != null)
        //////    {
        //////        result.TypeUISetting = new DPUI_TypeSettings();
        //////        result.TypeUISetting.TypeID = dbItem.DB_UI_TypeSetting.TypeID;
        //////        result.TypeUISetting.TypeConditionID = dbItem.DB_UI_TypeSetting.TypeConditionID;
        //////        BizUISetting bizUISetting = new BizUISetting();
        //////        foreach (var item in dbItem.DB_UI_TypeSetting.DB_UI_ItemSetting)
        //////        {
        //////            result.ItemSetting.Add(bizUISetting.PackageItemSettingDBToND(item, complex));
        //////        }

        //////    }
        //////    return result;
        //////}

        //////private DP_PackageProperty PackagePropertyDBToND(DB_DPPackage_Type_Property dbItem, bool complex)
        //////{
        //////    DP_PackageProperty result = new DP_PackageProperty();
        //////    result.PropertyID = dbItem.DB_Type_Property.DB_PropertyID;
        //////    result.TypeID = dbItem.DB_Type_Property.DB_TypeID;
        //////    result.TypeConditionID = dbItem.DB_TypeConditionID;
        //////    return result;
        //////}



        //////private DP_SpecifiedPackage DB_DPSpecifiedPackageDBToND(DB_DPSpecifiedPackage dbItem, bool complex)
        //////{
        //////    DP_SpecifiedPackage result = new DP_SpecifiedPackage();
        //////    BizType bizType = new BizType();
        //////    result.Types = new List<ND_Type>();
        //////    foreach (var item in dbItem.DB_DPSpecifiedPackage_Type)
        //////    {
        //////        result.Types.Add(bizType.TypeDBToND(item.DB_Type, complex));
        //////    }
        //////    BizProperty bizProperty = new BizProperty();
        //////    result.Properties = new List<ND_Property>();
        //////    foreach (var item in dbItem.DB_DPSpecifiedPackage_Property)
        //////    {
        //////        result.Properties.Add(bizProperty.PropertyDBToND(item.DB_Property, complex));
        //////    }
        //////    return result;
        //////}

        //////private DP_PackageCategory DP_PackageCategoryDBToND(DB_DPPackageCategory item, bool complex)
        //////{
        //////    DP_PackageCategory result = new DP_PackageCategory();
        //////    result.Name = item.Name;
        //////    return result;

        //////}

        //////private DP_PackageCalculatedProperty DPPackageCalculatedPropertyDBToND(DB_DPCalculatedProperty dbItem, bool complex)
        //////{
        //////    DP_PackageCalculatedProperty result = new DP_PackageCalculatedProperty(); ;

        //////    result.ID = dbItem.ID;
        //////    result.Name = dbItem.DB_Property.Name;
        //////    result.Title = dbItem.DB_Property.Title;
        //////    result.Description = dbItem.DB_Property.Description;
        //////    result.Type = dbItem.DB_Property.Type;
        //////    result.TypeCategory = dbItem.DB_Property.TypeCategory;
        //////    result.Formula = dbItem.Formula;
        //////    BizTypeProperty bizTypeProperty = new BizTypeProperty();
        //////    result.Properties = new List<ND_Type_Property>();
        //////    foreach (var item in dbItem.DB_DPCalculatedProperty_Type_Property)
        //////    {
        //////        result.Properties.Add(bizTypeProperty.TypePropertyDBToND(item.DB_Type_Property, complex));
        //////    }


        //////    return result;
        //////}


        //////public GenericResult<OperationResult> GenerateDPPackageStructure()
        //////{
        //////    GenericResult<OperationResult> result = new GenericResult<OperationResult>();

        //////    bool done = false;
        //////    bool faild = false;


        //////    //List<DB_Package> listPackage = new List<DB_Package>();
        //////    //using (IdeaEntities context = new IdeaEntities())
        //////    //{
        //////    //    listPackage = context.DB_Package.Where(x => !x.DB_DPPackage.Any()).ToList();
        //////    //}
        //////    using (IdeaEntities context = new IdeaEntities())
        //////    {
        //////        foreach (var item in context.DB_DPPackage)
        //////        {
        //////            try
        //////            {
        //////                if (!item.DB_DPPackageTreeStructure.Any())
        //////                {
        //////                    DP_PackageTreeStructure packageTreeItem = new DP_PackageTreeStructure();

        //////                    packageTreeItem.Name = item.DB_Package.Name;
        //////                    packageTreeItem.Package = new DP_Package();
        //////                    packageTreeItem.Package.ID = item.ID;
        //////                    UpdateDPPackageTreeStructure(packageTreeItem);
        //////                }

        //////                done = true;
        //////            }
        //////            catch (Exception ex)
        //////            {
        //////                result.BaseResult.Messages.Add("خطا در بروزرسانی پکیج " + item.DB_Package.Name + Environment.NewLine + ex.Message);
        //////                faild = true;
        //////            }

        //////        }
        //////    }
        //////    if (done && faild)
        //////        result.Result = OperationResult.PartiallyDone;
        //////    else if (done)
        //////        result.Result = OperationResult.Done;
        //////    else if (faild)
        //////        result.Result = OperationResult.Failed;
        //////    return result;
        //////}
        //////public Guid UpdateDPPackageTreeStructure(DP_PackageTreeStructure ndItem)
        //////{
        //////    try
        //////    {
        //////        using (IdeaEntities context = new IdeaEntities())
        //////        {
        //////            var editItem = DPPackageTreeStructureNDToDB(ndItem, context); ;

        //////            if (editItem.ID == Guid.Empty)
        //////            {
        //////                editItem.ID = Guid.NewGuid();
        //////                context.DB_DPPackageTreeStructure.Add(editItem);
        //////            }
        //////            context.SaveChanges();
        //////            return editItem.ID;
        //////        }

        //////    }
        //////    catch (Exception ex)
        //////    {
        //////        throw ex;
        //////    }
        //////}
        //////private DB_DPPackageTreeStructure DPPackageTreeStructureNDToDB(DP_PackageTreeStructure ndItem, IdeaEntities context)
        //////{
        //////    DB_DPPackageTreeStructure result;
        //////    if (ndItem.ID == Guid.Empty)
        //////    {
        //////        result = new DB_DPPackageTreeStructure();
        //////    }
        //////    else
        //////        result = context.DB_DPPackageTreeStructure.Where(x => x.ID == ndItem.ID).First();

        //////    result.DB_DPPackageID = ndItem.Package.ID;
        //////    result.Name = ndItem.Name;

        //////    return result;
        //////}

        //////public DP_ResultRelatedPackage GetRelatedPackage(DP_RequestRelatedPackage request)
        //////{
        //////    try
        //////    {
        //////        DP_ResultRelatedPackage result = new DP_ResultRelatedPackage();
        //////        //using (IdeaEntities context = new IdeaEntities())
        //////        //{
        //////        //    var editItem = DPPackageTreeStructureNDToDB(ndItem, context); ;

        //////        //    if (editItem.ID == Guid.Empty)
        //////        //    {
        //////        //        editItem.ID = Guid.NewGuid();
        //////        //        context.DB_DPPackageTreeStructure.Add(editItem);
        //////        //    }
        //////        //    context.SaveChanges();
        //////        //    return editItem.ID;
        //////        //}
        //////        return result;

        //////    }
        //////    catch (Exception ex)
        //////    {
        //////        throw ex;
        //////    }
        //////}







    }

    class PureItem
    {
        public string ObjectIdentity { set; get; }
        public string ObjectCategory { set; get; }
    }
}