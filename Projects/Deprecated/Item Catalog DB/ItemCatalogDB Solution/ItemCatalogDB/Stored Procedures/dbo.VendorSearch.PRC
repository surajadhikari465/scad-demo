SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[VendorSearch]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[VendorSearch]
GO


CREATE PROCEDURE dbo.VendorSearch 
@CompanyName varchar(52)

AS

BEGIN

SELECT @CompanyName = '%' + @CompanyName + '%'

select CompanyName, Vendor_ID, City, State
from Vendor 
where CompanyName LIKE @CompanyName
END 


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


