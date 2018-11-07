CREATE PROCEDURE dbo.InsertAvgCostAdjustment

	@Item_Key int,
	@Store_No int,
	@SubTeam_No int,
	@AvgCost smallmoney,
	--@Quantity decimal(18,4), you know they are going to ask for this at some point
	@Reason int,
	@Comments varchar(1000),
	@User_ID int

AS 

INSERT INTO AvgCostHistory 
	(
		Item_Key,
		Store_No,
		SubTeam_No,
		Effective_Date,
		AvgCost,
		Quantity,
		Reason,
		Comments,
		[User_ID]
	)
VALUES
   (
	   @Item_Key,
	   @Store_No,
	   @SubTeam_No,
	   GetDate(),
	   @AvgCost,
	   dbo.fn_CurOnHoldQtyAvgCostHistory(@Item_Key, @Store_No, @SubTeam_No), -- adjust current qty on hand
	   @Reason,
	   @Comments,
	   @User_ID
	)
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertAvgCostAdjustment] TO [IRMAClientRole]
    AS [dbo];

