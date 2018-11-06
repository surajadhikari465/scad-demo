SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'dbo.UpdateVendorCostHistory') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dbo.UpdateVendorCostHistory
GO


CREATE PROCEDURE dbo.UpdateVendorCostHistory
    @VendorCostHistoryID int,
    @UnitCost smallmoney,
    @UnitFreight smallmoney,
    @Package_Desc1 decimal(9,4),
    @StartDate smalldatetime,
    @EndDate smalldatetime,
    @Promotional bit,
    @MSRP smallmoney
AS

BEGIN
    SET NOCOUNT ON

    UPDATE VendorCostHistory
    SET UnitCost = @UnitCost,
        UnitFreight = @UnitFreight,
        Package_Desc1 = @Package_Desc1,
        StartDate = ISNULL(@StartDate, CONVERT(smalldatetime,CONVERT(varchar(255),GETDATE(),101))),
        EndDate = ISNULL(@EndDate, '2079-06-06'),
        Promotional = @Promotional,
        MSRP = @MSRP
    FROM VendorCostHistory (rowlock)
    WHERE VendorCostHistoryID = @VendorCostHistoryID

    SET NOCOUNT OFF
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


