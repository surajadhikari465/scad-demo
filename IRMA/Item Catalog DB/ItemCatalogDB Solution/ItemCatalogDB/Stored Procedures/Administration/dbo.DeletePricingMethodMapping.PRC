SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DeletePricingMethodMapping]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[DeletePricingMethodMapping]
GO


CREATE PROCEDURE dbo.DeletePricingMethodMapping
@POSFileWriterKey int,
@PricingMethod_Key int,
@PricingMethod_ID int
AS 

DELETE FROM POSWriterPricingMethodId
WHERE POSFileWriterKey = @POSFileWriterKey AND PricingMethod_Key = @PricingMethod_Key AND PricingMethod_ID = @PricingMethod_ID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO