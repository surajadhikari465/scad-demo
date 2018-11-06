SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsertOrder]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertOrder]
GO


CREATE PROCEDURE dbo.InsertOrder 
	@Vendor_ID int,
	@OrderType_ID tinyint,
	@ProductType_ID tinyint,
	@PurchaseLocation_ID int,
	@ReceiveLocation_ID int,
	@Transfer_SubTeam int,
	@Transfer_To_SubTeam int,
	@Fax_Order bit,
	@Expected_Days int,
	@CreatedBy int,
	@Return_Order tinyint, 
	@FromQueue bit = 0,
	@SupplyTransferToSubTeam int = null,
	@DSDOrder bit = 0,
	@InvoiceNumber varchar(30) =null
AS 

/*
	---------------------------------------------------------------------------------------------------------
	Revision History
	---------------------------------------------------------------------------------------------------------

	<Date>			<Developer>		<TFS>		<Change Description>
	======================================================
	4/21/10		Tom Lux				12521		Currency ID was not being set in the OrderHeader record because it was added to the select that pulls order-window data,
																	which may return no rows, leaving @CurrencyID NULL.  Reverted to setting currency via separate select after order-window query.
	10/04/12	Amudha Sethuraman	8101		DSDOrder was added as an optional input parameter to identify DSD orders created using WFM Mobile application.

	10/18/12	Hui Kou				7419		Invoice Number was addes as an optional input parameter to save invoice number to order header
	01/14/13    Hui Kou             9782        Increase Invoice number Varchar to 30, in case some invoice number can be very long
	01/16/13    Hui Kou             9782        Make @InvoiceNumber varchar(30)
	---------------------------------------------------------------------------------------------------------
*/
BEGIN
    SET NOCOUNT ON
    
    DECLARE @Expected_Date datetime
    
    IF @ProductType_ID = 0
		SELECT @ProductType_ID = 1
    DECLARE @CurrencyID int

    IF (@Expected_Days IS NOT NULL) SELECT @Expected_Date = CONVERT(varchar(12), DATEADD(DAY, @Expected_Days, GETDATE()), 101)

    DECLARE @OrderStart datetime, @OrderEnd datetime
    SELECT
		@OrderStart = OrderStart
		,@OrderEnd = CASE WHEN @Transfer_To_SubTeam IS NULL THEN OrderEnd ELSE OrderEndTransfers END
    FROM ZoneSubTeam ZST (NOLOCK)
    INNER JOIN Vendor (NOLOCK) ON Vendor.Vendor_ID = @Vendor_ID AND Vendor.Store_No = ZST.Supplier_Store_No AND ZST.SubTeam_No = @Transfer_SubTeam
    INNER JOIN Vendor RL (NOLOCK) ON RL.Vendor_ID = @ReceiveLocation_ID
    INNER JOIN Store (NOLOCK) ON Store.Store_No = RL.Store_No AND Store.Zone_ID = ZST.Zone_ID
    WHERE @Return_Order = 0
    AND NOT EXISTS (SELECT * FROM Users (NOLOCK) WHERE User_ID = @CreatedBy AND Warehouse = 1)

	-- Currency needs to be pulled here and not in the above select, as the query above may return no data, but we need a currency.
	SELECT @CurrencyID = CurrencyID
	FROM Vendor (NOLOCK) 
	WHERE Vendor_ID = @Vendor_ID
	
    IF (DATEDIFF(minute, 
                 CONVERT(varchar(255), ISNULL(@OrderStart,    CONVERT(smalldatetime, GETDATE())), 108), 
                 CONVERT(varchar(255), CONVERT(smalldatetime, GETDATE()),                         108)) >= 0) AND
       (DATEDIFF(minute, 
                 CONVERT(varchar(255), CONVERT(smalldatetime, GETDATE()),                         108), 
                 CONVERT(varchar(255), ISNULL(@OrderEnd,      CONVERT(smalldatetime, GETDATE())), 108)) >= 0)
    BEGIN
        IF @OrderStart IS NOT NULL AND DATEDIFF(day, GETDATE(), @Expected_Date) < 0
            RAISERROR(50003, 16, 1)
        ELSE
            INSERT INTO OrderHeader (Vendor_ID, OrderType_ID, ProductType_ID, PurchaseLocation_ID, ReceiveLocation_ID, Transfer_SubTeam, Transfer_To_SubTeam, 
				Fax_Order, Expected_Date, CreatedBy, Return_Order, FromQueue, SupplyTransferToSubTeam,
				CurrencyID, DSDOrder, InvoiceNumber)
            VALUES (@Vendor_ID, @OrderType_ID, @ProductType_ID, @PurchaseLocation_ID, @ReceiveLocation_ID, @Transfer_SubTeam, @Transfer_To_SubTeam, 
				@Fax_Order, @Expected_Date, @CreatedBy, @Return_Order, @FromQueue, @SupplyTransferToSubTeam,
				@CurrencyID, @DSDOrder, @InvoiceNumber)
    END
    ELSE
    BEGIN
        DECLARE @WindowStart varchar(255), @WindowEnd varchar(255)
        SELECT @WindowStart = CONVERT(varchar(255), @OrderStart, 108), @WindowEnd = CONVERT(varchar(255), @OrderEnd, 108)
        RAISERROR(50002, 16, 1, @WindowStart, @WindowEnd)
    END
    
    SELECT SCOPE_IDENTITY() as OrderHeader_ID

    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


