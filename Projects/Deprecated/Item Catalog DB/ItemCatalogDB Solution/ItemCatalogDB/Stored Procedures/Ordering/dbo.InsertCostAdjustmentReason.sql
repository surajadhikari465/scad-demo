 SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsertCostAdjustmentReason]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertCostAdjustmentReason]
GO

CREATE PROCEDURE dbo.InsertCostAdjustmentReason
	@Description VARCHAR(50),
	@IsDefault BIT,
	@CostAdjustmentReason_ID INT OUTPUT
AS
BEGIN
    
	INSERT INTO CostAdjustmentReason	(
		Description,
		IsDefault
	)	VALUES	(
		@Description,
		@IsDefault
	)
    
	SET @CostAdjustmentReason_ID = SCOPE_IDENTITY()
END
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



