 SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'dbo.InventoryValueDetail') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  drop procedure dbo.InventoryValueDetail
GO


CREATE PROCEDURE dbo.InventoryValueDetail
    @BusUnit int,
    @TeamNo int,
    @SubTeamNo int,
    @categoryID int,
	@Level3 int,
	@Level4 int,
	@Identifier varchar(13) --Changed the datatype to fix the bug 6109.	
AS 
/*

grant exec on dbo.InventoryValueDetail to IRMAClientRole
grant exec on dbo.InventoryValueDetail to IRMAReportsRole
grant exec on dbo.InventoryValueDetail to IRMAExcelRole

exec InventoryValueDetail 104, 106, 25, 56

*/
BEGIN
    SET NOCOUNT ON
    
SELECT 
BusUnit,
Team,
SubTeam,
Item_key,
Identifier,
Item_Description,
Unit_Name,
Unit_Abbreviation,
CaseCountOnHand,
CaseCountNotAvail,
LandedCaseCost,
ExtLandedCost,
CaseUpchargeAmt,
CaseUpchargePct,
LoadedCaseCost,
ExtLoadedCost,
PackSize 
from dbo.fn_InventoryValue (@BusUnit, @TeamNo, @SubTeamNo, @CategoryID, @Level3, @Level4, @Identifier) 
 
    SET NOCOUNT OFF
END

/*

use ItemCatalog_b16
select count(*) as Item from Item
select count(*) as ItemAdjustment from ItemAdjustment
select count(*) as ItemCaseHistory from ItemCaseHistory 
select count(*) as ItemCategory from ItemCategory
select count(*) as ItemHistory from ItemHistory
select count(*) as ItemIdentifier from ItemIdentifier
select count(*) as ItemUnit from ItemUnit
select count(*) as OnHand from OnHand

use ItemCatalog
select count(*) as Item from Item
select count(*) as ItemAdjustment from ItemAdjustment
select count(*) as ItemCaseHistory from ItemCaseHistory 
select count(*) as ItemCategory from ItemCategory
select count(*) as ItemHistory from ItemHistory
select count(*) as ItemIdentifier from ItemIdentifier
select count(*) as ItemUnit from ItemUnit
select count(*) as OnHand from OnHand

select * from Item
select * from ItemAdjustment
select * from ItemCaseHistory 
select * from ItemCategory
select * from ItemHistory
select * from ItemIdentifier
select * from OnHand

*/



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

