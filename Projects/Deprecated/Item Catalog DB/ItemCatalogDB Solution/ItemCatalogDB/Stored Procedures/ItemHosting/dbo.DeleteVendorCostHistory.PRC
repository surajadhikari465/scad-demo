 IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DeleteVendorCostHistory]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[DeleteVendorCostHistory]
GO
/****** Object:  StoredProcedure [dbo].[DeleteVendorCostHistory]    Script Date: 06/15/2009 12:06:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[DeleteVendorCostHistory]
    @VendorCostHistoryID int
AS

BEGIN
    SET NOCOUNT ON
	
	Update OrderItem Set VendorCostHistoryID = Null Where VendorCostHistoryID = @VendorCostHistoryID
    DELETE VendorCostHistory FROM VendorCostHistory (rowlock) WHERE VendorCostHistoryID = @VendorCostHistoryID

    SET NOCOUNT OFF
END
GO

BEGIN
GRANT EXECUTE ON [dbo].DeleteVendorCostHistory TO IRMAClientRole
END
Go