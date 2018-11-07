CREATE PROCEDURE [dbo].[GetOrderAllocItems]
    @Store_No int,
    @SubTeam_No int,
	@UserName varchar(35),
    @NonRetail int,
    @AdjustBOH bit,
	@IncludeInboundQty bit,
	@ExpectedDateStart Datetime,
	@ExpectedDateEnd Datetime,
	@PreOrderOption int
/* 

    grant exec on dbo.GetOrderAllocItems to IRMAAdminRole
    grant exec on dbo.GetOrderAllocItems to IRMAClientRole
    grant exec on dbo.GetOrderAllocItems to IRMASchedJobsRole
    grant exec on dbo.GetOrderAllocItems to public

*/
AS
BEGIN
    SET NOCOUNT ON
	--select * from dbo.fn_GetOrderAllocItems (@Store_No, @SubTeam_No,  @NonRetail)

	INSERT INTO dbo.tmpOrdersAllocateItems
			   (Store_No,
				SubTeam_No,
				UserName,
				Item_Key
			   ,Identifier
			   ,Item_Description
			   ,Category_Name
			   ,Pre_Order
			   ,PackSize
			   ,BOH
			   ,WOO
			   ,SOO
			   ,FIFODateTime)
		 SELECT
			   @Store_No, 
			   @SubTeam_No, 
			   @UserName, 
			   Item_Key, 
			   Identifier, 
			   Item_Description, 
			   Category_Name, 
			   Pre_Order, 
			   PackSize, 
			   BOH, 
			   WOO, 
			   SOO,
			   FiFoDate
	from dbo.fn_GetOrderAllocItems (@Store_No, @SubTeam_No, @UserName, @NonRetail, @AdjustBOH, @IncludeInboundQty, @ExpectedDateStart, @ExpectedDateEnd, @PreOrderOption)

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderAllocItems] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderAllocItems] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderAllocItems] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderAllocItems] TO [IRMAReportsRole]
    AS [dbo];

