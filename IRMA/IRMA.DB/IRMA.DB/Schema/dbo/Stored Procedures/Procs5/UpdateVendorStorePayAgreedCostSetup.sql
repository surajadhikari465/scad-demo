CREATE PROCEDURE dbo.UpdateVendorStorePayAgreedCostSetup
    @vendor_id int
	,@store_no int
	,@effectiveDate smalldatetime
	,@deleteFlag bit
	,@Success bit = NULL OUTPUT

	/*
	Created by Tom Lux
	2009.07.01
	IRMA v3.5
	
	**Run this to test.**
	declare @effectiveDate smalldatetime
	select @effectiveDate = getdate()
	EXEC dbo.[UpdateVendorStorePayAgreedCostSetup] 1534, 808, @effectiveDate, 0 -- Data=FL, VendorID=UNFI, Store=Plantation, Effective Date=today, Delete Flag=false/0
	*/

AS
BEGIN
    SET NOCOUNT ON

	/*
	[For SQL Testing]
    declare @vendor_id int; select @vendor_id = 1534
	declare @store_no int; select @store_no = 808
	declare @effectiveDate smalldatetime
	select @effectiveDate = getdate()
	declare @deleteFlag bit; select @deleteFlag = 1
	declare @Success bit; select @success = 0

	*/

	if @deleteFlag = 1
	begin
		delete from payorderedcost
		where
			vendor_id = @vendor_id
			and store_no = @store_no
	end
	else
	begin

		/* Update effective date to ensure it has no time value. */
		declare @strDate varchar(8)
		select @strDate = convert(varchar, @effectiveDate, 112) -- Convert to ISO date string.
		select @effectiveDate = cast(@strDate as smalldatetime) -- Convert back to smalldatetime.

		if exists (select * from payorderedcost (nolock) where vendor_id = @vendor_id and store_no = @store_no)
		/* Update effective date in existing pay-agreed-cost entry. */
		begin
			update payorderedcost
			set beginDate = @effectiveDate
			where
				vendor_id = @vendor_id
				and store_no = @store_no
		end
		else
		/* Insert new pay-agreed-cost entry for vendor and store with effective (begin) date. */
		begin
			insert into payorderedcost(
				vendor_id
				,store_no
				,beginDate)
			values(
				@vendor_id
				,@store_no
				,@effectiveDate)
		end
	end


	/*
	Set the success flag:
		1) Whether or not the entry was deleted.
		2) Whether or not an entry exists that matches the passed values.
	*/
	if @deleteFlag = 1
	begin
		select @success = 
			case
				when (
					select count(1)
					from payorderedcost (nolock)
					where
						vendor_id = @vendor_id
						and store_no = @store_no) = 0 -- If count is zero, no records were found, so the delete was successful.
				then 1
				else 0
			end    
	end
	else
	begin
		select @success = 
			case
				when (
					select count(1)
					from payorderedcost (nolock)
					where
						vendor_id = @vendor_id
						and store_no = @store_no
						and beginDate = @effectiveDate) = 1 
				then 1 
				else 0
			end    
	end    
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateVendorStorePayAgreedCostSetup] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateVendorStorePayAgreedCostSetup] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateVendorStorePayAgreedCostSetup] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateVendorStorePayAgreedCostSetup] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateVendorStorePayAgreedCostSetup] TO [IRMAReportsRole]
    AS [dbo];

