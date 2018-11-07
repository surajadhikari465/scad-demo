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
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_GetSalePriceChgType] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_GetSalePriceChgType] TO [IRMAClientRole]
    AS [dbo];

