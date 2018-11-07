SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetStoreIsDistribution]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetStoreIsDistribution]
GO


CREATE PROCEDURE dbo.GetStoreIsDistribution 
@Store_No int
AS 

SELECT Distribution_Center
FROM Store
WHERE Store_No = @Store_No

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO