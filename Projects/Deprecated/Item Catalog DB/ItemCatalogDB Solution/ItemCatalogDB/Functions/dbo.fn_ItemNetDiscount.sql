ALTER FUNCTION dbo.fn_ItemNetDiscount
(
   @Store_No int,
   @Item_Key int,
   @Vendor_ID int,
   @RegCost decimal(10,4),
   @CurrentDate smalldatetime
)

RETURNS decimal(10,4)
AS
-- **********************************************************************************************
-- Procedure: fn_ItemNetDiscount()
--    Author: Sam Gordon
--      Date: 02/20/2007
--
-- Description:
-- This function sums the discount amounts in the VendorDealHistory records
-- that are active for the @CurrentDate sent in. It only uses the last set
-- of records inserted that all have the same InsertDate.
--
-- Modification History:
-- Date			Init	TFS		Comment
-- 03/12/2013	BBB		11122	Added (nolocks) to prevent blocking
-- 1/11/2011	DBS		13813	Add Ron Savage's fix which gets the latest insertdate 
--								within the start and end date to avoid future sales
-- 1/11/2011	DBS		13813	Adjust between so it includes actual end date
-- 11/07/2010	RS		xxxxx	Re-write - put code that was moved to the other function
--								back in here where it belongs and simplified the logic for both procs.
-- 11/01/2007  DM		xxxxx	David Marine moved code to fn_ItemNetDiscountValidation().
-- **********************************************************************************************
BEGIN
   --**************************************************************************
   --Declare internal variables
   --**************************************************************************
   DECLARE @NetDiscount          decimal(10,4);
   DECLARE @StoreItemVendorId    int;
   DECLARE @latestInsertDate     datetime;

   --**************************************************************************
   -- Get the StoreItemVendorId in advance to remove a JOIN in subsequent
   -- queries to speed it up.
   --**************************************************************************
   select @StoreItemVendorId = StoreItemVendorId from StoreItemVendor (nolock) where Vendor_Id = @Vendor_Id and Store_No = @Store_No and Item_Key = @Item_Key;

   --**************************************************************************
   -- Get the latest insertDate, to identify the last set of promos inserted
   --**************************************************************************
	select 
		  @latestInsertDate = max(InsertDate) 
	   from 
		  VendorDealHistory (nolock)
	   where 
		  StoreItemVendorId = @StoreItemVendorId
		  and CONVERT(VARCHAR(10), @CurrentDate, 101) between StartDate and EndDate;
   --**************************************************************************
   -- Get the sum of the discounts that are active for the StartDate sent in
   --**************************************************************************
   SELECT
      @NetDiscount = SUM(CASE
                           WHEN vdt.CaseAmtType = '%' THEN
                              (vdh.CaseAmt / 100) * @RegCost
                           ELSE
                              vdh.CaseAmt
                         END)
   FROM
      VendorDealHistory    (nolock) vdh

      JOIN VendorDealType  (nolock) vdt
         ON (vdt.VendorDealTypeID = vdh.VendorDealTypeID )
   WHERE
      vdh.StoreItemVendorId   = @StoreItemVendorId
      and vdh.InsertDate      = @latestInsertDate
      and CONVERT(VARCHAR(10), @CurrentDate, 101) BETWEEN vdh.StartDate and vdh.EndDate;

   --**************************************************************************
   --Return
   --**************************************************************************
   RETURN ISNULL(@NetDiscount,0);
END;
go
