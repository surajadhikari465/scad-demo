 if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[fn_VendorExists]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[fn_VendorExists]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE  FUNCTION dbo.fn_VendorExists
	(@Vendor_ID int)
RETURNS bit
AS

BEGIN  
	DECLARE @return bit
    
    IF EXISTS (SELECT * FROM dbo.Vendor WHERE Vendor_ID = @Vendor_ID)
		SELECT @return = 1
	ELSE
		SELECT @return = 0
        
	RETURN @return
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO