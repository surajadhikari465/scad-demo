SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[InsertPricingMethodMapping]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[InsertPricingMethodMapping]
GO


CREATE PROCEDURE dbo.InsertPricingMethodMapping
@POSFileWriterKey int,
@PricingMethod_Key int,
@PricingMethod_ID int
AS 

INSERT INTO POSWriterPricingMethodId (POSFileWriterKey,PricingMethod_Key,PricingMethod_ID) 
VALUES (@POSFileWriterKey,@PricingMethod_Key,@PricingMethod_ID)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
