IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetVendorItemStatuses]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetVendorItemStatuses]
GO

CREATE PROCEDURE [dbo].[GetVendorItemStatuses]
AS
BEGIN
    SET NOCOUNT ON
    
    SELECT StatusID, StatusName
    FROM VendorItemStatuses
    
    SET NOCOUNT OFF
END
GO
