using DataAccess;
using MyConnectionManager;
using MyDataSearchManagerBusiness;
using MyModelManager;
using MyTargetDatabaseManager;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyExternalReportLibrary
{
    public class MyExternalReportHandler
    {
        public long GetExternalReportKey(DR_Requester requester, int reportID, int entityID, DP_SearchRepository searchItem)
        {
            BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
            SearchRequestManager searchRequestManager = new SearchRequestManager();
            var query = searchRequestManager.GetSelectFromExternal(requester, entityID, searchItem, true, ModelEntites.SecurityMode.View);
            var entity = bizTableDrivedEntity.GetSimpleEntity(requester, entityID);

            if (query != null)
            {
                //  var transactionresult = ConnectionManager.ExecuteTransactionalQueryItems(allQueryItems.Where(x => !string.IsNullOrEmpty(x.Query)).ToList());

                var guid = Guid.NewGuid().ToString();
                var id = 0;
                using (var model = new MyIdeaDataDBEntities())
                {
                    var newItem = new ExternalReportKeys();
                    newItem.DateTime = DateTime.Now;
                    newItem.RequesterID = requester.Identity;
                    newItem.ReportID = reportID;
                    model.ExternalReportKeys.Add(newItem);
                    model.SaveChanges();
                    id = newItem.ID;
                }
                if (id != 0)
                {
                    var select = "select " + query.Item1 + "," + id + " as ReportKey" + " " + query.Item2;
                    var tmpTableName = "xr_" + entity.TableName;
                    var inserted = TargetDatabaseManager.InsertFromQueryToTargetDB(entityID, tmpTableName, select);
                    if (inserted)
                        return id;
                }

            }
            return 0;
        }
    }
}


//اس پی ایجاد کننده جدول کلید برای گزارش
//USE[SampleDB]
//GO
///****** Object:  StoredProcedure [dbo].[sp_CreateReportTable]    Script Date: 4/7/2018 4:08:04 PM ******/
//SET ANSI_NULLS ON
//GO
//SET QUOTED_IDENTIFIER ON
//GO
//-- =============================================
//-- Author:		<Author,,Name>
//-- Create date: <Create Date,,>
//-- Description:	<Description,,>
//-- =============================================
//ALTER PROCEDURE[dbo].[sp_CreateReportTable]
//@ReportName varchar(512),
//@ReportKey varchar(128)
//AS
//BEGIN


//    SET NOCOUNT ON;
//	DECLARE @TableName nvarchar(256);
//set @TableName = 'xk_' + @ReportName;
//DECLARE @select nvarchar(max);
//DECLARE @from nvarchar(max);

//select @select = SelectClause, @from = FromClause from MyExternalReport.dbo.ExternalReportKeys where[Key]= +@reportKey;

//	--select @query1 = replace(@query1, '@ReportKey', '''' + @reportKey + '''' + ' as ReportKey')

//    IF(@select!='')

//        BEGIN
//            set @select= 'select ' + @select + ',''' + @reportKey + ''' as ReportKey';
//			DECLARE @filterQuery nvarchar(max) = @select+' ' +@from  ;
//				IF EXISTS(SELECT* FROM INFORMATION_SCHEMA.TABLES
//                   WHERE TABLE_NAME = @TableName)

//                   Begin
//                       declare @clear varchar(2000)

//                        set @clear = 'delete  from ' + @TableName + ' where ReportKey='''+@ReportKey+''''

//                        Exec(@clear)

//                           declare @sql varchar(2000)

//                        set @sql = 'Insert into ' + @TableName + ' ' + @filterQuery

//                        Exec(@sql)

//                   End
//                   Else

//                   BEGIN
//                     declare @sql1 varchar(2000)

//                    set @sql1 = 'Select * into ' + @TableName + ' from (' + @filterQuery + ') as aaa'

//                    Exec(@sql1)

//                    declare @sqlIdentityOFF varchar(2000)

//                    SET @sqlIdentityOFF = +'SET IDENTITY_INSERT ' + @TableName + ' OFF'

//                    EXEC(@sqlIdentityOFF)


//                   END
//        END

//    Else
//    BEGIN

//        DECLARE @Error nvarchar(1024) ='The external report with the key ''' + @ReportKey+''' not found';
//		THROW 90009, @Error, 1;  
//	END
//END










//اس پی معادل دیتاست برای هر گزارش 
//USE[SampleDB]
//GO
///****** Object:  StoredProcedure [dbo].[Report_City]    Script Date: 4/7/2018 4:08:47 PM ******/
//SET ANSI_NULLS ON
//GO
//SET QUOTED_IDENTIFIER ON
//GO
//-- =============================================
//-- Author:		<Author,,Name>
//-- Create date: <Create Date,,>
//-- Description:	<Description,,>
//-- =============================================
//ALTER PROCEDURE[dbo].[Report_City]
//	-- Add the parameters for the stored procedure here

//    @ReportKey varchar(128)
//AS
//BEGIN
//	-- SET NOCOUNT ON added to prevent extra result sets from
//	-- interfering with SELECT statements.
//    SET NOCOUNT ON;
//EXEC sp_CreateReportTable 'Report_City',@ReportKey



//SELECT* from city inner join xk_Report_City as filter on city.ID=filter.id where filter.ReportKey=@ReportKey
//END
