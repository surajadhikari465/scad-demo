/****** Object:  StoredProcedure [dbo].[Administration_GetSalePriceChgType]    Script Date: 05/19/2006 16:33:50 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Administration_GetSalePriceChgType]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Administration_GetSalePriceChgType]
GO

/****** Object:  StoredProcedure [dbo].[Administration_GetSalePriceChgType]    Script Date: 04/17/2008 14:28:53 ******/
CREATE PROCEDURE [dbo].[Administration_GetSalePriceChgType] 
AS 
BEGIN
    SET NOCOUNT ON

select pricechgtypeid, pricechgtypedesc 
from dbo.pricechgtype (nolock) 
where on_sale = 1
    
    SET NOCOUNT OFF
END


GO


























