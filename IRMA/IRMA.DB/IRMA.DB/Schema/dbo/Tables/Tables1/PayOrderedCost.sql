CREATE TABLE [dbo].[PayOrderedCost] (
    [PayOrderedCostId] INT           IDENTITY (1, 1) NOT NULL,
    [Vendor_ID]        INT           NOT NULL,
    [Store_No]         INT           NOT NULL,
    [BeginDate]        SMALLDATETIME NOT NULL,
    CONSTRAINT [PK_PayOrderedCost] PRIMARY KEY CLUSTERED ([PayOrderedCostId] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_PayOrderedCost_Store] FOREIGN KEY ([Store_No]) REFERENCES [dbo].[Store] ([Store_No]),
    CONSTRAINT [FK_PayOrderedCost_Vendor] FOREIGN KEY ([Vendor_ID]) REFERENCES [dbo].[Vendor] ([Vendor_ID])
);


GO
CREATE TRIGGER [PayOrderedCostUpdate]
ON [dbo].[PayOrderedCost]
FOR Update
AS 
BEGIN
	/*
	[Tom Lux, IRMA v3.5, 7/9/2009]
	*/
    DECLARE @Error_No int
    SELECT @Error_No = 0

	/*
	On Update: If effective date is changed from today or date in the past to a future date, this effectively disables pay-by-agreed-cost,
	so unsent orders for the vendor and store are set as pay-by-invoice (payByAgreedCost=0).

	[For Testing]
	-- Show all pay-by-agreed-cost store-vendors.
	select * from payorderedcost
	select * from store

	*NOTE: You should not insert/update directly into the PayOrderedCost table, unless you know to observe the date-only rule for the 'beginDate' field.
	*PayOrderedCost.BeginDate should be a pure date value with a time of 00:00:00.
	Use this SP (next 3 lines) for insert/update.

	-- The following three lines set (for FL data) the UNFI vendor and Plantation (801) store as pay-by-agreed-cost for a future date
	-- and assume the entry already exists with a current or past effective date.  If row does not exist, run test below with today as effective date.
	declare @effectiveDate smalldatetime
	select @effectiveDate = getdate()+1
	EXEC dbo.UpdateVendorStorePayAgreedCostSetup @store_no=801, @vendor_id=1534, @effectiveDate=@effectiveDate, @deleteFlag=0

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
				inserted
				on inserted.store_no = rl.store_no and inserted.vendor_id = oh.vendor_id
			join
				deleted
				on deleted.payorderedcostid = inserted.payorderedcostid
			where
				inserted.begindate <> deleted.begindate -- Effective date was updated.
				and deleted.begindate <= cast(convert(varchar, getdate(), 112) as smalldatetime) -- Original date was active, meaning it was today or in the past.
				and inserted.begindate > getdate() -- New date is in the future.
				and sent = 0
		)
		and paybyagreedcost = 1 -- Only touch orders that need to be changed.

    SELECT @Error_No = @@ERROR

	IF @Error_No <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('PayOrderedCostUpdate trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
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
CREATE TRIGGER [PayOrderedCostInsert]
ON [dbo].[PayOrderedCost]
FOR INSERT
AS 
BEGIN
	/*
	[Tom Lux, IRMA v3.5, 7/9/2009]
	*/
    DECLARE @Error_No int
    SELECT @Error_No = 0

	/*
	On Insert: Unsent orders for the vendor and store are set as pay-agreed-cost (payByAgreedCost=1) if effective date is today.

	[For Testing]
	-- Show all pay-by-agreed-cost store-vendors.
	select * from payorderedcost
	select * from store

	*NOTE: You should not insert/update directly into the PayOrderedCost table, unless you know to observe the date-only rule for the 'beginDate' field.
	*PayOrderedCost.BeginDate should be a pure date value with a time of 00:00:00.
	Use this SP (next 3 lines) for insert/update.

	-- The following three lines set (for FL data) the UNFI vendor and Plantation (801) store as pay-by-agreed-cost.
	declare @effectiveDate smalldatetime
	select @effectiveDate = getdate()
	EXEC dbo.UpdateVendorStorePayAgreedCostSetup @store_no=801, @vendor_id=1534, @effectiveDate=@effectiveDate, @deleteFlag=0

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
		paybyagreedcost = 1
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
				inserted
				on inserted.store_no = rl.store_no and inserted.vendor_id = oh.vendor_id
			where
				inserted.begindate <= convert(varchar, getdate(), 112) -- If being (effective) date is today or prior, the pay-by-agreed-cost setting is live now.
				and sent = 0
		)
		and paybyagreedcost = 0 -- Only touch orders that need to be changed.

    SELECT @Error_No = @@ERROR

	IF @Error_No <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('PayOrderedCostInsert trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO
GRANT SELECT
    ON OBJECT::[dbo].[PayOrderedCost] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PayOrderedCost] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PayOrderedCost] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PayOrderedCost] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PayOrderedCost] TO [IRMAReportsRole]
    AS [dbo];

