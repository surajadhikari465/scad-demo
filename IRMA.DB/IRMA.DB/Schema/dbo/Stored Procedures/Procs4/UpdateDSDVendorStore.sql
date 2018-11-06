CREATE PROCEDURE [dbo].[UpdateDSDVendorStore]
	 @vendor_id int
	,@store_no int
	,@effectiveDate smalldatetime
	,@AddFlag bit
	,@Success bit = NULL OUTPUT
AS
-- **************************************************************************
-- Procedure: UpdateDSDVendorStore()
--    Author: Hui Kou
--      Date: 09.25.2012
--
-- Description:
-- This procedure is called from the WFM Mobile app to return item data to the 
-- order interface
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 10.10.12		HK   	7419	Creation

-- **************************************************************************
BEGIN
    SET NOCOUNT ON  

	if @AddFlag = 0
	begin
		delete from DSDVendorStore
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

		if exists (select * from DSDVendorStore (nolock) where vendor_id = @vendor_id and store_no = @store_no)
		/* Update effective date in existing pay-agreed-cost entry. */
		begin
			update DSDVendorStore
			set beginDate = @effectiveDate
			where
				vendor_id = @vendor_id
				and store_no = @store_no
		end
		else
		/* Insert new pay-agreed-cost entry for vendor and store with effective (begin) date. */
		begin
			insert into DSDVendorStore(
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
	if @AddFlag = 0
	begin
		select @success = 
			case
				when (
					select count(1)
					from DSDVendorStore (nolock)
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
					from DSDVendorStore (nolock)
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
    ON OBJECT::[dbo].[UpdateDSDVendorStore] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateDSDVendorStore] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateDSDVendorStore] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateDSDVendorStore] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateDSDVendorStore] TO [IRMAReportsRole]
    AS [dbo];

