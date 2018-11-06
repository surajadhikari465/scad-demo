/****** Object:  StoredProcedure [dbo].[GetCostPromoCodeTypes]    Script Date: 1/25/2007 11:06:56 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[GetCostPromoCodeTypes]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetCostPromoCodeTypes]
GO

/****** Object:  StoredProcedure [dbo].[GetCostPromoCodeTypes]    Script Date: 1/25/2007 11:06:56 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE dbo.GetCostPromoCodeTypes
AS
BEGIN
    SET NOCOUNT ON

	SELECT CostPromoCodeTypeID, 
		CostPromoCode, 
		(CONVERT(varchar, CostPromoCode) + ' - ' + CostPromoDesc) AS CostPromoDesc
    FROM CostPromoCodeType
	ORDER BY CostPromoCode
    
    SET NOCOUNT OFF
END
GO
  