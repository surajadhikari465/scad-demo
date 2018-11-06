if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetAllPriceTypes]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetAllPriceTypes]

GO

Create PROCEDURE [dbo].[GetAllPriceTypes] 

AS 
BEGIN
    
    SELECT 
		CAST(PriceChgTypeId AS int) AS PriceChgTypeId,
		PriceChgTypeDesc
    FROM 
		PriceChgType (NOLOCK)
    ORDER BY 
		Priority asc

END

GO

