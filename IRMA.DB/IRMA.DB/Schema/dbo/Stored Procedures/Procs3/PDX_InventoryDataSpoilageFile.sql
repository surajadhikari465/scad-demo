CREATE PROCEDURE [dbo].[PDX_InventoryDataSpoilageFile]
AS 
BEGIN
SET NOCOUNT ON
set transaction isolation level read uncommitted

declare @IncludedStores as table (Store_No int)
declare @today as datetime, @yesterday as datetime

set @today = Convert(Date, getdate(), 102)
set @yesterday = Convert(Date, getdate() - 1, 102)

insert into @IncludedStores
select store_no
from   Store
where  mega_store = 1
union
select s.store_no
from   Store s
join   [dbo].[fn_Parse_List]([dbo].[fn_GetAppConfigValue]('WFMBannerStoresForOrdering', 'IRMA CLIENT'), '|') bs 
	   on s.BusinessUnit_ID = bs.Key_Value

;WITH CTE
AS (
select convert(varchar, ih.DateStamp, 112) as CALENDAR_DATE, RIGHT('0000000000000'+ISNULL(ii.Identifier,''),13) as UPC, s.BusinessUnit_ID as STORE_NUMBER, 
              case 
                     when Quantity = 0
                     then Weight
                     else Quantity
              End as SPOILAGE_UNITS,
			  st.SubTeam_Name as PROD_SUBTEAM,
			  case 
					when i.SubTeam_No = ih.SubTeam_No
					then 'Y'
					else 'N'
			  end as HOST_SUBTEAM_FLAG,
			  rtrim(iu.Unit_Name) as CASE_UOM
from itemhistory ih
join @IncludedStores ist on ih.Store_no = ist.Store_No
join Store s on ih.Store_No = s.Store_No
join ItemIdentifier ii on ih.Item_Key = ii.Item_key
join Item i on ii.item_key = i.Item_Key
join ValidatedScanCode vsc on vsc.ScanCode = ii.Identifier 
join SubTeam st on st.SubTeam_No = ih.SubTeam_No
join ItemUnit iu on iu.Unit_Id = i.Retail_Unit_ID
where Adjustment_ID = 1
and i.Retail_Sale = 1
and ((ih.Insert_Date < @today and ih.Insert_Date >= @yesterday)
 or (ih.Insert_Date < @today and ih.Insert_Date >= @today - 5
and vsc.InsertDate < @today and vsc.InsertDate > @yesterday))
and ii.Default_Identifier = 1
and ii.Deleted_Identifier = 0
and ii.Remove_Identifier = 0
)
select CALENDAR_DATE, UPC, STORE_NUMBER, SUM(SPOILAGE_UNITS) as SPOILAGE_UNITS, PROD_SUBTEAM, HOST_SUBTEAM_FLAG, CASE_UOM
from CTE
group by CALENDAR_DATE, UPC, STORE_NUMBER, PROD_SUBTEAM, HOST_SUBTEAM_FLAG, CASE_UOM
order by STORE_NUMBER, UPC, CALENDAR_DATE, PROD_SUBTEAM
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[PDX_InventoryDataSpoilageFile] TO [IRMAPDXExtractRole]
    AS [dbo];

