SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetPosChangesAggregated]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetPosChangesAggregated]
GO


CREATE PROCEDURE dbo.GetPosChangesAggregated
AS

SELECT DISTINCT Sales_Date, Store_No 
FROM POSChanges
WHERE Aggregated = 0
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



