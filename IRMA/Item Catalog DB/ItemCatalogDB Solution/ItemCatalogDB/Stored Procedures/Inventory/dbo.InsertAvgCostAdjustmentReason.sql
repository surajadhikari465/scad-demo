SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsertAvgCostAdjustmentReason]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertAvgCostAdjustmentReason]
GO


CREATE PROCEDURE dbo.InsertAvgCostAdjustmentReason

	@Description varchar(75),
	@Active bit

AS 

INSERT INTO AvgCostAdjReason 
	(
		Description,
		Active
	)
VALUES
   (
	   @Description,
	   @Active
	)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

