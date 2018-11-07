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
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertAvgCostAdjustmentReason] TO [IRMAClientRole]
    AS [dbo];

