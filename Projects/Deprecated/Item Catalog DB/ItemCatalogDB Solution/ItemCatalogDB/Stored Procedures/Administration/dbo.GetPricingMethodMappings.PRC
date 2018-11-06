SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[GetPricingMethodMappings]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetPricingMethodMappings]
GO


CREATE PROCEDURE dbo.GetPricingMethodMappings
AS 

SELECT PMI.POSFileWriterKey, PMI.PricingMethod_Key, PMI.PricingMethod_ID, PW.POSFileWriterCode, PM.PricingMethod_Name
FROM POSWriterPricingMethodId PMI
LEFT JOIN POSWriter PW
	 ON PW.POSFileWriterKey = PMI.POSFileWriterKey
LEFT JOIN PricingMethod PM
	 ON PM.PricingMethod_ID = PMI.PricingMethod_Key
	
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
