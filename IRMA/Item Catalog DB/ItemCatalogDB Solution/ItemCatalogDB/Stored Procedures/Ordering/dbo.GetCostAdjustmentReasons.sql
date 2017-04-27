SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetCostAdjustmentReasons]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetCostAdjustmentReasons]
GO

CREATE PROCEDURE dbo.GetCostAdjustmentReasons
AS
BEGIN
    
    SELECT
		CostAdjustmentReason_ID,
		Description,
		IsDefault
    FROM
		CostAdjustmentReason
    
END
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO