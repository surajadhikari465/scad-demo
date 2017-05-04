SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PricingMethodList]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[PricingMethodList]
GO

-- this is only being used in EPromo currently.  

CREATE PROCEDURE dbo.PricingMethodList
AS 

SELECT PricingMethod_ID, PricingMethod_Name
FROM PricingMethod
ORDER By PricingMethod_ID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

