using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slaw.ItemLocale.Service
{
    public class ItemLocaleRepository
    {
        public IEnumerable<ItemLocaleModel> GetItemLocaleData(string region)
        {
            throw new NotImplementedException("Need to update the Model with the correct properties returned from the query instead of the properties that you copied from the Mammoth API Controller project.");
            IEnumerable<ItemLocaleModel> results = null;
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ItemCatalog_" + region].ConnectionString))
            {
                results = connection.Query<ItemLocaleModel>(
                    @"SET TRANSACTION ISOLATION LEVEL SNAPSHOT;

                    DECLARE @events table (QueueID int, EventTypeID int, Identifier nvarchar(13), Item_key int, Store_No int);

                    WITH Publish
	                AS
	                (
		                SELECT TOP(@NumberOfRows) 
			                InProcessBy
		                FROM
			                [mammoth].[ItemLocaleChangeQueue] WITH (ROWLOCK, READPAST, UPDLOCK)
		                WHERE
			                InProcessBy IS NULL
			                AND ProcessFailedDate IS NULL
		                ORDER BY
			                InsertDate
	                )

	                UPDATE Publish 
                    SET InProcessBy = @JobInstance
                    OUTPUT INSERTED.*
                    INTO @events

                    DECLARE @ExcludedStoreNo varchar(250);
                    SET @ExcludedStoreNo = (SELECT dbo.fn_GetAppConfigValue('LabAndClosedStoreNo','IRMA Client'));

                    -- For all the rows where ItemLocaleChangeQueue.Store_No IS NOT NULL
                    SELECT
	                    q.QueueID					as QueueId,
                        q.EventTypeID				as EventTypeId,
                        @Region                     as Region,
	                    q.Identifier				as ScanCode,
	                    s.BusinessUnit_ID			as BusinessUnitId,
                        CASE
		                    WHEN p.AgeCode = 1 THEN 18
		                    WHEN p.AgeCode = 2 THEN 21
		                    ELSE NULL
	                    END							as AgeRestriction,
                        CASE 
                            WHEN t.EventTypeName = 'ItemDelete' THEN 0
		                    ELSE si.Authorized
	                    END							as Authorized,
	                    p.IBM_Discount				as CaseDiscount,
	                    sa.UomRegulationChicagoBaby as ChicagoBaby,
	                    sa.ColorAdded				as ColorAdded,
	                    COALESCE(ivc.Origin_Name, co.Origin_Name)	as CountryOfProcessing,
	                    siv.DiscontinueItem			as Discontinued,
	                    p.ElectronicShelfTag		as ElectronicShelfTag,
	                    sa.Exclusive				as Exclusive,
	                    lt.LabelTypeDesc			as LabelTypeDescription,
	                    lsc.Identifier				as LinkedItem,
	                    p.LocalItem					as LocalItem,
	                    sa.Locality					as Locality,
                        ii.NumPluDigitsSentToScale  as NumberOfDigitsSentToScale,
	                    COALESCE(ovo.Origin_Name, oo.Origin_Name)	as Origin,
	                    i.Product_Code				as ProductCode,
	                    p.Restricted_Hours			as RestrictedHours,
	                    COALESCE(ovu.Unit_Name, iu.Unit_Name)		as RetailUnit,
                        COALESCE(soe.ExtraText,sce.ExtraText)		as ScaleExtraText,
	                    sa.SignRomanceTextLong		as SignRomanceLong,
	                    sa.SignRomanceTextShort		as SignRomanceShort,
	                    COALESCE(iov.Sign_Description, i.Sign_Description) as SignDescription,
	                    sa.UomRegulationTagUom		as TagUom,
	                    p.Discountable				as TmDiscount,
                        p.MSRPPrice                 as Msrp
                    FROM
	                    @events	q
	                    INNER JOIN mammoth.ItemChangeEventType t on q.EventTypeID = t.EventTypeID
	                    INNER JOIN Item						i	on	q.Item_Key	= i.Item_Key
                        LEFT JOIN ItemIdentifier           ii  on  i.Item_Key  = ii.Item_Key
                                                                AND q.Identifier = ii.Identifier
                                                                AND ii.Deleted_Identifier = 0
                        LEFT JOIN Store						s	on	q.Store_No = s.Store_No
	                    LEFT JOIN StoreItemVendor			siv	on	i.Item_Key	= siv.Item_Key
											                    AND s.Store_No = siv.Store_No
											                    AND siv.PrimaryVendor = 1
											                    AND siv.DeleteDate IS NULL
	                    LEFT JOIN Price                     p   on  i.Item_Key = p.Item_Key
                                                                AND s.Store_No = p.Store_No
	                    LEFT JOIN StoreItem				    si	on	s.Store_No	= si.Store_No
											                    AND i.Item_Key	= si.Item_Key
	                    LEFT JOIN ItemSignAttribute			sa	on	i.Item_Key	= sa.Item_Key
	                    LEFT JOIN ItemOrigin				co	on	i.CountryProc_ID = co.Origin_ID                     -- country of processing
	                    LEFT JOIN ItemOrigin				oo	on	i.Origin_ID = oo.Origin_ID		                    -- origin
	                    LEFT JOIN LabelType					lt	on	i.LabelType_ID = lt.LabelType_ID
	                    LEFT JOIN ItemScale					sc	on	i.Item_Key	= sc.Item_Key
	                    LEFT JOIN Scale_ExtraText			sce	on	sc.Scale_ExtraText_ID = sce.Scale_ExtraText_ID      -- scale extra text
	                    LEFT JOIN ItemOverride				iov	on	i.Item_Key	= iov.Item_Key
											                    AND iov.StoreJurisdictionID = s.StoreJurisdictionID
	                    LEFT JOIN ItemOrigin				ivc	on	iov.CountryProc_ID = ivc.Origin_ID	                -- alternate country of processing
	                    LEFT JOIN ItemOrigin				ovo	on	iov.Origin_ID = ovo.Origin_ID		                -- alternate origin
	                    LEFT JOIN ItemScaleOverride			iso	on	i.Item_Key	= iso.Item_Key
											                    AND s.StoreJurisdictionID = iso.StoreJurisdictionID
	                    LEFT JOIN Scale_ExtraText			soe	on	iso.Scale_ExtraText_ID = soe.Scale_ExtraText_ID     -- alternate scale extra text
	                    LEFT JOIN ItemUnit					iu	on	i.Retail_Unit_ID	= iu.Unit_ID                    -- retail unit
	                    LEFT JOIN ItemUnit					ovu	on	iov.Retail_Unit_ID	= ovu.Unit_ID                   -- alternate retail unit
                        LEFT JOIN ItemIdentifier            lsc on  p.LinkedItem = lsc.Item_Key
                                                                AND lsc.Default_Identifier = 1
                    WHERE
	                    q.InProcessBy = @JobInstance
                        AND q.Store_No IS NOT NULL

                    UNION ALL

                    -- For all the rows where ItemLocaleChangeQueue.Store_No IS NULL
                    -- Order of JOINs is different than above
                    SELECT
	                    q.QueueID					as QueueId,
                        q.EventTypeID				as EventTypeId,
                        @Region                     as Region,
	                    q.Identifier				as ScanCode,
	                    s.BusinessUnit_ID			as BusinessUnitId,
                        CASE
		                    WHEN p.AgeCode = 1 THEN 18
		                    WHEN p.AgeCode = 2 THEN 21
		                    ELSE NULL
	                    END							as AgeRestriction,
                        CASE 
                            WHEN t.EventTypeName = 'ItemDelete' THEN 0
		                    ELSE si.Authorized
	                    END							as Authorized,
	                    p.IBM_Discount				as CaseDiscount,
	                    sa.UomRegulationChicagoBaby as ChicagoBaby,
	                    sa.ColorAdded				as ColorAdded,
	                    COALESCE(ivc.Origin_Name, co.Origin_Name)	as CountryOfProcessing,
	                    siv.DiscontinueItem			as Discontinued,
	                    p.ElectronicShelfTag		as ElectronicShelfTag,
	                    sa.Exclusive				as Exclusive,
	                    lt.LabelTypeDesc			as LabelTypeDescription,
	                    lsc.Identifier				as LinkedItem,
	                    p.LocalItem					as LocalItem,
	                    sa.Locality					as Locality,
                        ii.NumPluDigitsSentToScale  as NumberOfDigitsSentToScale,
	                    COALESCE(ovo.Origin_Name, oo.Origin_Name)	as Origin,
	                    i.Product_Code				as ProductCode,
	                    p.Restricted_Hours			as RestrictedHours,
	                    COALESCE(ovu.Unit_Name, iu.Unit_Name)		as RetailUnit,
                        COALESCE(soe.ExtraText,sce.ExtraText)		as ScaleExtraText,
	                    sa.SignRomanceTextLong		as SignRomanceLong,
	                    sa.SignRomanceTextShort		as SignRomanceShort,
	                    COALESCE(iov.Sign_Description, i.Sign_Description) as SignDescription,
	                    sa.UomRegulationTagUom		as TagUom,
	                    p.Discountable				as TmDiscount,
                        p.MSRPPrice                 as Msrp
                    FROM
	                    @events	q
	                    INNER JOIN mammoth.ItemChangeEventType t on q.EventTypeID = t.EventTypeID
	                    INNER JOIN Item						i	on	q.Item_Key	= i.Item_Key
                        LEFT JOIN ItemIdentifier           ii	on  i.Item_Key  = ii.Item_Key
                                                                AND q.Identifier = ii.Identifier
                                                                AND ii.Deleted_Identifier = 0
	                    LEFT JOIN Price                     p   on  i.Item_Key = p.Item_Key
	                    LEFT JOIN StoreItem				    si	on	i.Item_Key	= si.Item_Key
											                    AND p.Store_No = si.Store_No
	                    LEFT JOIN Store						s	on	p.Store_No = s.Store_No
	                    LEFT JOIN StoreItemVendor			siv	on	i.Item_Key	= siv.Item_Key
											                    AND siv.PrimaryVendor = 1
											                    AND siv.DeleteDate IS NULL
											                    AND s.Store_No = siv.Store_No
	                    LEFT JOIN ItemSignAttribute			sa	on	i.Item_Key	= sa.Item_Key
	                    LEFT JOIN ItemOrigin				co	on	i.CountryProc_ID = co.Origin_ID                     -- country of processing
	                    LEFT JOIN ItemOrigin				oo	on	i.Origin_ID = oo.Origin_ID		                    -- origin
	                    LEFT JOIN LabelType					lt	on	i.LabelType_ID = lt.LabelType_ID
	                    LEFT JOIN ItemScale					sc	on	i.Item_Key	= sc.Item_Key
	                    LEFT JOIN Scale_ExtraText			sce	on	sc.Scale_ExtraText_ID = sce.Scale_ExtraText_ID      -- scale extra text
	                    LEFT JOIN ItemOverride				iov	on	i.Item_Key	= iov.Item_Key
											                    AND iov.StoreJurisdictionID = s.StoreJurisdictionID
	                    LEFT JOIN ItemOrigin				ivc	on	iov.CountryProc_ID = ivc.Origin_ID	                -- alternate country of processing
	                    LEFT JOIN ItemOrigin				ovo	on	iov.Origin_ID = ovo.Origin_ID		                -- alternate origin
	                    LEFT JOIN ItemScaleOverride			iso	on	i.Item_Key	= iso.Item_Key
											                    AND s.StoreJurisdictionID = iso.StoreJurisdictionID
	                    LEFT JOIN Scale_ExtraText			soe	on	iso.Scale_ExtraText_ID = soe.Scale_ExtraText_ID     -- alternate scale extra text
	                    LEFT JOIN ItemUnit					iu	on	i.Retail_Unit_ID	= iu.Unit_ID                    -- retail unit
	                    LEFT JOIN ItemUnit					ovu	on	iov.Retail_Unit_ID	= ovu.Unit_ID                   -- alternate retail unit
                        LEFT JOIN ItemIdentifier            lsc on  p.LinkedItem = lsc.Item_Key
                                                                AND lsc.Default_Identifier = 1
                    WHERE
	                    q.InProcessBy = @JobInstance
                        AND (s.WFM_Store = 1 OR s.Mega_Store = 1 )
                        AND (Internal = 1 AND BusinessUnit_ID IS NOT NULL)
                        AND s.Store_No NOT IN (SELECT Key_Value FROM dbo.fn_Parse_List(@ExcludedStoreNo, '|'))
	                    AND q.Store_No IS NULL;",
                    new { JobInstance = 1 });

            }

            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Icon"].ConnectionString))
            {
                var itemIds = connection.Query<Tuple<string, int>>(@"select scanCode ScanCode, itemID ItemId from dbo.ScanCode (nolock) where scanCode in (@ScanCodes)", new { ScanCodes = results.Select(il => il.ScanCode) })
                    .ToDictionary(t => t.Item1, t => t.Item2);

                foreach (var model in results)
                {
                    model.IconItemId = itemIds[model.ScanCode];
                }
            }

            return results;
        }

        public int SaveMessageHistory(string message)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Icon"].ConnectionString))
            {
                return connection.Query<int>(
                    @"INSERT app.MessageHistory(MessageTypeId, MessageStatusId, Message)
                    VALUES (1, 2, @Message)

                    SELECT SCOPE_IDENTITY()",
                    new { Message = message }).First();
            }
        }

        public void FinalizeItemLocaleRecords(string region, IEnumerable<ItemLocaleModel> itemLocaleModels)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ItemCatalog_" + region].ConnectionString))
            {
                connection.Execute("DELETE dbo.ItemLocaleChangeQueue WHERE InProcessBy = @JobInstance", new { JobInstance = 1 });
            }
        }
    }
}
