SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CheckForDuplicateVendors]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CheckForDuplicateVendors]
GO


CREATE PROCEDURE dbo.CheckForDuplicateVendors 
    @Vendor_ID int,
    @CompanyName varchar(255),
    @PS_Vendor_ID varchar(255),
    @PS_Address_Sequence varchar(255)
AS
BEGIN
    SET NOCOUNT ON
    
    SELECT COUNT(*) AS VendorCount 
    FROM Vendor
    WHERE 
		(
			RTRIM(CompanyName) = RTRIM(@CompanyName)
		OR 
			dbo.fn_TrimLeadingZeros(PS_Vendor_ID) = dbo.fn_TrimLeadingZeros(@PS_Vendor_ID)
		)		                  
    AND Vendor_Id <> @Vendor_ID
    
    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

