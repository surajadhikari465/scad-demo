SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetStoreVendor]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetStoreVendor]
GO


CREATE PROCEDURE dbo.GetStoreVendor
@Store_No int
AS 

SELECT MAX(Vendor_ID) AS Vendor_ID
FROM Vendor (NOLOCK) 
WHERE Store_No = @Store_No
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

