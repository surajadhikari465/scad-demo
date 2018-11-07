SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'dbo.GetVendorCostHistory') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dbo.GetVendorCostHistory
GO


CREATE PROCEDURE dbo.GetVendorCostHistory
    @VendorCostHistoryID int
AS

BEGIN
    SET NOCOUNT ON

    SELECT Promotional, UnitCost, UnitFreight, Package_Desc1, StartDate, EndDate, Promotional, FromVendor, MSRP
    FROM VendorCostHistory (rowlock)
    WHERE VendorCostHistoryID = @VendorCostHistoryID

    SET NOCOUNT OFF
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

