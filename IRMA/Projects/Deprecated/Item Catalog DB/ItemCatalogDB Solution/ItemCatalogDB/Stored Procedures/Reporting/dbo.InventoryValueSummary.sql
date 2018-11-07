
SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'dbo.InventoryValueSummary ') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  drop procedure dbo.InventoryValueSummary 
GO


CREATE PROCEDURE dbo.InventoryValueSummary 
-- was the old dbo.InventoryBalance SP

    @BusUnit int,
    @TeamNo int,
    @SubTeamNo int,
    @categoryID int,
	@Level3 int,
	@Level4 int,
	@Identifier varchar(13) --Changed the datatype to fix the bug 6109.		
AS
/*

grant exec on dbo.InventoryValueSummary  to IRMAClientRole
grant exec on dbo.InventoryValueSummary  to IRMAReportsRole
grant exec on dbo.InventoryValueSummary  to IRMAExcelRole

-- exec dbo.InventoryValueSummary 104, 106, 25

*/

BEGIN
    SET NOCOUNT ON

	select 
		BusUnit,
		Team,
		SubTeam,
		SUM(CaseCountOnHand) as CaseCountOnHand,
		SUM(CaseCountNotAvail) as CaseCountNotAvail,
		SUM(ExtLandedCost) as ExtLandedCost,
		SUM(ExtLoadedCost) as ExtLoadedCost
	from dbo.fn_InventoryValue (@BusUnit, @TeamNo, @SubTeamNo, @CategoryID, @Level3, @Level4, @Identifier)
	GROUP BY BusUnit, Team, SubTeam
 
    SET NOCOUNT OFF
END


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

