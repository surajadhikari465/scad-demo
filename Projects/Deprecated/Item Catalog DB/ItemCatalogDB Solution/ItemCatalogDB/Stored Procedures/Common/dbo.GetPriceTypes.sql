SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetPriceTypes]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetPriceTypes]
GO


CREATE PROCEDURE [dbo].[GetPriceTypes] 
	@IncludeReg bit
AS 
BEGIN
    SET NOCOUNT ON
    
    SELECT 
		PriceChgTypeDesc,
		PriceChgTypeId,
		Priority,
		On_Sale,
		MSRP_Required,
		LineDrive,
		Competitive,
		LastUpdateTimestamp
    FROM 
		PriceChgType (NOLOCK)
	WHERE @IncludeReg >= 1 - On_Sale
	-- so if IncludeReg is zero, it won't include On_Sale = 1
    ORDER BY 
		Priority asc
    
    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


