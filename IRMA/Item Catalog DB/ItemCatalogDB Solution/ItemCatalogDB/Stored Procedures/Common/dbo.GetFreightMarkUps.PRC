SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetFreightMarkUps]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetFreightMarkUps]
GO


CREATE PROCEDURE dbo.GetFreightMarkUps
@Vendor_ID int,
@Store_No int
AS

SELECT Vendor_ID, Store_No, FreightMarkUp 
FROM VendorStoreFreight
where Store_no = isnull(@Store_no,Store_No) and Vendor_ID = isnull(@Vendor_ID, Vendor_ID)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


