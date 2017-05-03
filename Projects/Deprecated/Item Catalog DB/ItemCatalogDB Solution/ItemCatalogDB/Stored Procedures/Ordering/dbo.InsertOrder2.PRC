SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsertOrder2]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertOrder2]
GO


CREATE PROCEDURE dbo.InsertOrder2 
@Vendor_ID int,
@OrderType_ID tinyint, 
@ProductType_ID tinyint, 
@PurchaseLocation_ID int,
@ReceiveLocation_ID int,
@Transfer_SubTeam int,
@Transfer_To_SubTeam int,
@Fax_Order bit,
@Expected_Date datetime,
@CreatedBy int,
@Return_Order bit,
@CurrencyID int,
@NewOrderHeader_ID int OUTPUT
AS 

BEGIN
    SET NOCOUNT ON
    
    IF @ProductType_ID = 0
       SELECT @ProductType_ID = 1
    
	declare @store_no int
	select @store_no = store_no
	from vendor (nolock)
	where vendor_id = @receivelocation_id

	INSERT INTO OrderHeader (Vendor_ID, OrderType_ID, ProductType_ID, PurchaseLocation_ID, ReceiveLocation_ID, Transfer_SubTeam, Transfer_To_SubTeam, 
		Fax_Order, Expected_Date, CreatedBy, Return_Order,
		CurrencyID)
    VALUES (@Vendor_ID, @OrderType_ID, @ProductType_ID, @PurchaseLocation_ID, @ReceiveLocation_ID, @Transfer_SubTeam, @Transfer_To_SubTeam, 
		@Fax_Order, @Expected_Date, @CreatedBy, @Return_Order,
		@CurrencyID)

    SELECT @NewOrderHeader_ID = SCOPE_IDENTITY()
    
    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

