CREATE FUNCTION dbo.fn_ItemNetDiscountValidation
(
   @Store_No            int,
   @Item_Key            int,
   @Vendor_ID           int,
   @RegCost          decimal(10,4),
   @StartOrCurrentDate     smalldatetime,
   -- support the use of this function for validating the creation or the updating of existing deals.
   @EndDate          smalldatetime, -- null if not for validation
   @VendorDealHistory_ID   int,        -- null or <= 0 if not for validation or if validating a new deal
   @NotStackable        bit            -- 1 only if for validating a new or update to an existing nonstackable deal
)
   RETURNS decimal(10,4)
AS
-- **************************************************************************
-- Procedure: fn_ItemNetDiscountValidation()
--    Author: David Marine
--      Date: 11/01/2007
--
-- Description:
-- This proc performs two different functions:
--
-- 1. Returns net discount for an item. (it calls fn_ItemNetDiscount())
--    This functionality was left in when the proc was re-written in case
--    some other code calls this function rather than the correct fn_ItemNetDiscount()
--    function.
--
-- 2. To get the max discount for an item between two dates.  This is used
--    in EIM to check that a new discount being inserted will not cause the
--    cost of an item to go negative.
--
-- Modification History:
-- Date        Init TFS    Comment
-- 07/06/2010  BBB   xxxxx Applied coding standards and modification history
-- 07/08/2010  BBB   xxxxx Removed InsertDate conversion
-- 11/05/2010  RS    xxxxx Fixed date comparison condition to get only active promos.
-- 11/07/2010  RS    xxxxx Re-write - put Item Net Discount code back into fn_ItemNetDiscount()
--                         and simplified the logic for this proc.
-- 02/16/2011  TTL   996   Fixed bad start/end date comparisons when VendorDealHistory dates are being retrieved and inserted into @tmpdates table
--                         (these dates are then used to pull a net discount value).  See FIX section of TFS bug for full logic and problem details.


-- **************************************************************************
BEGIN
   --**************************************************************************
   --Declare internal variables
   --**************************************************************************
   DECLARE @NetDiscount          decimal(10,4);
   DECLARE @StoreItemVendorId    int;
   DECLARE @latestInsertDate     datetime;

   --**************************************************************************
   --Populate internal variables
   --**************************************************************************
   SET @VendorDealHistory_ID  = ISNULL(@VendorDealHistory_ID, -1)

   --**************************************************************************
   -- Get the StoreItemVendorId in advance to remove a JOIN in subsequent
   -- queries to speed it up.
   --**************************************************************************
   select @StoreItemVendorId = StoreItemVendorId from StoreItemVendor where Vendor_Id = @Vendor_Id and Store_No = @Store_No and Item_Key = @Item_Key;

   --**************************************************************************
   -- If the EndDate is NULL, we are just getting the net discount
   --**************************************************************************
   select @latestInsertDate = max(InsertDate) from VendorDealHistory where StoreItemVendorId = @StoreItemVendorId;

   --**************************************************************************
   -- If the EndDate is NULL, we are just getting the net discount
   --**************************************************************************
   IF @EndDate IS NULL
      BEGIN
      Set @NetDiscount = dbo.fn_ItemNetDiscount(@Store_No,@Item_Key,@Vendor_ID,@RegCost,@StartOrCurrentDate);
      END
   --**************************************************************************
   -- If the EndDate is NOT NULL, we must calculate the max discount that can
   -- occur between the start and end dates sent - so EIM can determine if
   -- the new discount it is inserting will cause the item cost to go negative.
   --**************************************************************************
   ELSE
      BEGIN
      SET @EndDate = ISNULL(@EndDate, @StartOrCurrentDate);

      declare @tmpdates table ( StartDate  smalldatetime );

      --***********************************************************************
      -- Get all the start / end dates for existing promo records between the
      -- start / end dates sent.
      --***********************************************************************
      insert into @tmpdates
         select distinct
            vdh.StartDate
         from
            VendorDealHistory vdh
         where
            vdh.StoreItemVendorId = @StoreItemVendorId
            and vdh.StartDate <= @StartOrCurrentDate
            and vdh.EndDate >= @EndDate
      union
         select distinct
            vdh.EndDate
         from
            VendorDealHistory vdh
         where
            vdh.StoreItemVendorId = @StoreItemVendorId
            and vdh.StartDate <= @StartOrCurrentDate
            and vdh.EndDate >= @EndDate;

      --***********************************************************************
      -- Call the fn_ItemNetDiscount() function for each date in the @tmpdates table
      -- and get the max discount.
      --***********************************************************************
      select
         @NetDiscount = max(dbo.fn_ItemNetDiscount(@Store_No,@Item_Key,@Vendor_ID,@RegCost,td.StartDate))
      from
         @tmpdates td;
      END

   --**************************************************************************
   --Return
   --**************************************************************************
   RETURN ISNULL(@NetDiscount,0);
END