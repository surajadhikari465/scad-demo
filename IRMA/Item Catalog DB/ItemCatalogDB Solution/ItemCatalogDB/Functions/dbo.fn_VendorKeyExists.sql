IF EXISTS (SELECT * FROM sysobjects WHERE name = N'fn_VendorKeyExists')
	DROP FUNCTION fn_VendorKeyExists
GO

CREATE FUNCTION [dbo].[fn_VendorKeyExists]
(
	@VendorKey varchar(10)
)
RETURNS bit
AS

BEGIN  

	DECLARE @result bit
	
	SELECT @result = (SELECT CASE WHEN COUNT(*) = 0 THEN 0 ELSE 1 END
					  FROM Vendor WHERE Vendor_Key = @VendorKey)

	RETURN @result

END

GO 