IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[PayOrderedCostDelete]'))
DROP TRIGGER [dbo].[PayOrderedCostDelete]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [PayOrderedCostDelete]
ON [dbo].[PayOrderedCost]
FOR Delete
AS 
BEGIN
	/*
	[Tom Lux, IRMA v3.5, 7/9/2009]
	*/
    DECLARE @Error_No int
    SELECT @Error_No = 0

	/*
	On Delete: Unsent orders for the vendor and store are set as pay-by-invoice (payByAgreedCost=0).

	[For Testing]
	-- Show all pay-by-agreed-cost store-vendors.
	select * from payorderedcost
	select * from store

	*NOTE: You should not insert/update directly into the PayOrderedCost table, unless you know to observe the date-only rule for the 'beginDate' field.
	*PayOrderedCost.BeginDate should be a pure date value with a time of 00:00:00.
	Use this SP (next 3 lines) for insert/update.

	-- The following line (for FL data) removes the UNFI vendor and Plantation (801) store pay-by-agreed-cost entry, making this store-vendor pay-by-invoice-cost.
	EXEC dbo.UpdateVendorStorePayAgreedCostSetup @store_no=801, @vendor_id=1534, @effectiveDate=null, @deleteFlag=1

	-- Get all unsent orders for test store and vendor.
	select oh.* from orderheader oh join dbo.fn_getunsentorders(801, 1534) uoh on oh.orderheader_id = uoh.orderheader_id
	-- Check order history (if orders should have been updated).
	select ohh.* from orderheaderhistory ohh join dbo.fn_getunsentorders(801, 1534) uoh on ohh.orderheader_id = uoh.orderheader_id order by insertdate desc, ohh.orderheader_id
	-- Show all pay-by-agreed-cost POs.
	select * from orderheader where paybyagreedcost = 1
	*/

	update
		orderheader
	set
		paybyagreedcost = 0
	where
		orderheader_id in (
			select
				orderheader_id
			from
				orderheader oh (nolock)
			join
				vendor rl (nolock)
				on rl.vendor_id = oh.receiveLocation_id
			join
				store s (nolock)
				on s.store_no = rl.store_no
			join
				deleted
				on deleted.store_no = rl.store_no and deleted.vendor_id = oh.vendor_id
			where
				sent = 0
		)
		and paybyagreedcost = 1 -- Only touch orders that need to be changed.

    SELECT @Error_No = @@ERROR

	IF @Error_No <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('PayOrderedCostDelete trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO
