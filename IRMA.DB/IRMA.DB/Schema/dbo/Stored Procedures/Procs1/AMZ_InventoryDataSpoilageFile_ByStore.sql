CREATE PROCEDURE [dbo].[AMZ_InventoryDataSpoilageFile_ByStore]
	@BusinessUnitID int,
	@RunAsDate datetime
AS 
BEGIN
SET NOCOUNT ON
set transaction isolation level read uncommitted

declare @today as datetime, @yesterday as datetime, @fiveDaysAgo as datetime

set @today = Convert(Date, @RunAsDate, 102)
set @yesterday = Convert(Date, @today - 1, 102)

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
join Store s on ih.Store_No = s.Store_No
join ItemIdentifier ii on ih.Item_Key = ii.Item_key
join Item i on ii.item_key = i.Item_Key
join SubTeam st on st.SubTeam_No = ih.SubTeam_No
join ItemUnit iu on iu.Unit_Id = i.Retail_Unit_ID
where Adjustment_ID = 1
and s.BusinessUnit_ID = @BusinessUnitID
and i.Retail_Sale = 1
and (ih.Insert_Date < @today and ih.Insert_Date >= @yesterday)
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
    ON OBJECT::[dbo].[AMZ_InventoryDataSpoilageFile_ByStore] TO [IRMAPDXExtractRole]
    AS [dbo];
GO