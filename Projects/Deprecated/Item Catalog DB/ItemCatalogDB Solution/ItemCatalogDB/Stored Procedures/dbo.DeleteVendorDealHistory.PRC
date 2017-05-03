 SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'dbo.DeleteVendorDealHistory') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dbo.DeleteVendorDealHistory
GO


CREATE PROCEDURE dbo.DeleteVendorDealHistory
    @VendorDealHistoryID int
AS

BEGIN
    SET NOCOUNT ON
    
    DELETE VendorDealHistory 		
    FROM VendorDealHistory
	WHERE VendorDealHistoryID = @VendorDealHistoryID			
    
    SET NOCOUNT OFF
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


 