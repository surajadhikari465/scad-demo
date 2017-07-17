using Dapper;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;
using Mammoth.ItemLocale.Controller.DataAccess.Models;
using System.Collections.Generic;

namespace Mammoth.ItemLocale.Controller.DataAccess.Queries
{
    public class GetItemLocaleDataQuery : IQueryHandler<GetItemLocaleDataParameters, List<ItemLocaleEventModel>>
    {
        private IDbProvider db;

        public GetItemLocaleDataQuery(IDbProvider db)
        {
            this.db = db;
        }

        public List<ItemLocaleEventModel> Search(GetItemLocaleDataParameters parameters)
        {
            // This query will get all events regardless of whether the Store_No IS NULL or not.
            List<ItemLocaleEventModel> itemLocaleData = new List<ItemLocaleEventModel>();
            string sql = @"SET TRANSACTION ISOLATION LEVEL SNAPSHOT;

                            DECLARE @ExcludedStoreNo varchar(250);
                            SET @ExcludedStoreNo = (SELECT dbo.fn_GetAppConfigValue('LabAndClosedStoreNo','IRMA Client'));

                            -- For all the rows where ItemLocaleChangeQueue.Store_No IS NOT NULL
                            SELECT
	                            q.QueueID					as QueueId,
                                q.EventTypeID				as EventTypeId,
                                srm.Region_Code             as Region,
	                            q.Identifier				as ScanCode,
	                            s.BusinessUnit_ID			as BusinessUnitId,
                                s.Store_No                  as StoreNo,
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
	                            COALESCE(uiu.Unit_Name, ovu.Unit_Name, iu.Unit_Name)		as RetailUnit,
                                COALESCE(soe.ExtraText,sce.ExtraText, iet.ExtraText)		as ScaleExtraText,
	                            COALESCE(iov.SignRomanceTextLong, sa.SignRomanceTextLong)		as SignRomanceLong,
	                            COALESCE(iov.SignRomanceTextShort, sa.SignRomanceTextShort)		as SignRomanceShort,
	                            COALESCE(iov.Sign_Description, i.Sign_Description) as SignDescription,
	                            sa.UomRegulationTagUom		as TagUom,
	                            p.Discountable				as TmDiscount,
                                p.MSRPPrice                 as Msrp
                            FROM
	                            [mammoth].[ItemLocaleChangeQueue]	q
	                            INNER JOIN mammoth.ItemChangeEventType t on q.EventTypeID = t.EventTypeID
	                            INNER JOIN Item						i	on	q.Item_Key	= i.Item_Key
                                LEFT JOIN ItemIdentifier           ii  on  i.Item_Key  = ii.Item_Key
                                                                        AND q.Identifier = ii.Identifier
                                                                        AND ii.Deleted_Identifier = 0
                                LEFT JOIN Store						s	on	q.Store_No = s.Store_No
                                LEFT JOIN StoreRegionMapping        srm on  s.Store_No = srm.Store_No
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
                                LEFT JOIN ItemNutrition             nut  ON  nut.ItemKey=ii.Item_Key
                                LEFT JOIN Item_ExtraText            iet ON  iet.Item_ExtraText_ID = nut.Item_ExtraText_ID
                                LEFT JOIN ItemUomOverride           iuo ON  iuo.Item_Key = i.Item_Key
                                                                        AND iuo.Store_No = s.Store_No
	                            LEFT JOIN ItemUnit					uiu	on	iuo.Retail_Unit_ID	= uiu.Unit_ID 
                            WHERE
	                            q.InProcessBy = @JobInstance
                                AND q.Store_No IS NOT NULL

                            UNION ALL

                            -- For all the rows where ItemLocaleChangeQueue.Store_No IS NULL
                            -- Order of JOINs is different than above
                            SELECT
	                            q.QueueID					as QueueId,
                                q.EventTypeID				as EventTypeId,
                                srm.Region_Code             as Region,
	                            q.Identifier				as ScanCode,
	                            s.BusinessUnit_ID			as BusinessUnitId,
                                s.Store_No                  as StoreNo,
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
	                            COALESCE(uiu.Unit_Name, ovu.Unit_Name, iu.Unit_Name)		as RetailUnit,
                                COALESCE(soe.ExtraText,sce.ExtraText,iet.ExtraText)		as ScaleExtraText,
	                            COALESCE(iov.SignRomanceTextLong, sa.SignRomanceTextLong)		as SignRomanceLong,
	                            COALESCE(iov.SignRomanceTextShort, sa.SignRomanceTextShort)		as SignRomanceShort,
	                            COALESCE(iov.Sign_Description, i.Sign_Description) as SignDescription,
	                            sa.UomRegulationTagUom		as TagUom,
	                            p.Discountable				as TmDiscount,
                                p.MSRPPrice                 as Msrp
                            FROM
	                            [mammoth].[ItemLocaleChangeQueue]	q
	                            INNER JOIN mammoth.ItemChangeEventType t on q.EventTypeID = t.EventTypeID
	                            INNER JOIN Item						i	on	q.Item_Key	= i.Item_Key
                                LEFT JOIN ItemIdentifier           ii	on  i.Item_Key  = ii.Item_Key
                                                                        AND q.Identifier = ii.Identifier
                                                                        AND ii.Deleted_Identifier = 0
	                            LEFT JOIN Price                     p   on  i.Item_Key = p.Item_Key
	                            LEFT JOIN StoreItem				    si	on	i.Item_Key	= si.Item_Key
											                            AND p.Store_No = si.Store_No
	                            LEFT JOIN Store						s	on	p.Store_No = s.Store_No
                                LEFT JOIN StoreRegionMapping        srm on  s.Store_No = srm.Store_No
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
                                LEFT JOIN ItemNutrition             nut  ON  nut.ItemKey=ii.Item_Key
                                LEFT JOIN Item_ExtraText            iet ON  iet.Item_ExtraText_ID = nut.Item_ExtraText_ID
                                LEFT JOIN ItemUomOverride           iuo ON  iuo.Item_Key = i.Item_Key
                                                                        AND iuo.Store_No = s.Store_No
	                            LEFT JOIN ItemUnit					uiu	on	iuo.Retail_Unit_ID	= uiu.Unit_ID 
                            WHERE
	                            q.InProcessBy = @JobInstance
                                AND (s.WFM_Store = 1 OR s.Mega_Store = 1 )
                                AND (Internal = 1 AND BusinessUnit_ID IS NOT NULL)
                                AND s.Store_No NOT IN (SELECT Key_Value FROM dbo.fn_Parse_List(@ExcludedStoreNo, '|'))
	                            AND q.Store_No IS NULL;";

            itemLocaleData = this.db.Connection
                .Query<ItemLocaleEventModel>(sql,
                    new { JobInstance = parameters.Instance, Region = parameters.Region },
                    transaction: db.Transaction)
                .AsList();

            return itemLocaleData;
        }
    }
}
