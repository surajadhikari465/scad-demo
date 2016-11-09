
CREATE PROCEDURE dbo.IMHA_Update_Promo
(
	@UpcNo varchar(20),
	@Vendor_Key varchar(20),
	@Warehouse varchar(20),
	@CaseQty int,
	@Package_Desc1 decimal(12,4),
	@CaseAmt decimal(12,4),
	@StartDate datetime,
	@EndDate datetime,
	@VendorDealCode varchar(3),
	@FromVendor int,
	@CostPromoCode int,
	@InsertDate datetime,
	@PromoRecCount int,
	@Debug int = 0
)
AS
--**************************************************************************
-- Procedure: IMHA_Update_Promo()
--    Author: Greg Bowles
--      Date: 05/28/2007
--
-- Description:
-- This procedure updates the IRMA VendorDealHistory table.
--
-- Change History:
-- Date			Init	Description
-- 07/07/2010	BBB		Added InsertDate to Insert; applied coding standards;
-- 10/05/2007	RS		Removed promo deletion code, this is now taken care of
--						in the IMHA_Update_Cost() routine before calling this
--						procedure.
-- 09/27/2007	RS		Added Case Uom name as an argument.
-- 06/07/2007	RS		Re-formatted, commented.
-- 05/23/2007	GB		Created.
--**************************************************************************
BEGIN
	SET NOCOUNT ON;
	--**************************************************************************
	-- Populate internal variables
	--**************************************************************************
	IF (@Debug = 1)
		PRINT 'IMHA_Update_Promo: Processing [' + @Vendor_Key + '], [' + @Warehouse + '], [' + @UpcNo + '] rec: [' + cast(@PromoRecCount as char(2)) + ']...'

	SET @FromVendor = 1

	IF @EndDate = ''
		SET @EndDate = null

   --**************************************************************************
   -- Add a new promo code entry if this one doesn't already exist
   --**************************************************************************
   IF NOT EXISTS (SELECT CostPromoCode FROM CostPromoCodeType WHERE CostPromoCode = @CostPromoCode)
		BEGIN
			IF (@Debug = 1)
				PRINT 'IMHA_Update_Promo: Adding new Promo Code [' + cast(@CostPromoCode as char(4)) + '] ...'

			DECLARE @CostPromoCodeDesc varchar(50)
			SET @CostPromoCodeDesc =  'Promo Code ' + CAST(@CostPromoCode as VARCHAR)
			EXEC InsertCostPromoCodeType @CostPromoCode, @CostPromoCodeDesc
		END

	IF (@Debug = 1)
		PRINT 'IMHA_Update_Promo: Inserting new promo record for [' + @Vendor_Key + '], [' + @Warehouse + '], [' + @UpcNo + '] ...'

   --**************************************************************************
   -- Main SQL
   --**************************************************************************
   INSERT INTO VendorDealHistory (StoreItemVendorID, CaseQty, Package_Desc1, CaseAmt, StartDate, EndDate, VendorDealTypeID, FromVendor, CostPromoCodeTypeID, InsertDate)
	   SELECT
		  SIV.StoreItemVendorID,
		  @CaseQty,
		  @Package_Desc1,
		  @CaseAmt,
		  ISNULL(@StartDate, CONVERT(datetime,CONVERT(varchar(255),GETDATE(),101))) as StartDate,
			ISNULL(cast(@EndDate as datetime), '2079-06-06') as EndDate,
			VDT.VendorDealTypeID,
			@FromVendor,
		  CPC.CostPromoCodeTypeID,
		  @InsertDate
	   FROM
		  StoreItemVendor			(nolock) siv
		  JOIN ItemIdentifier		(nolock) ii		ON (ii.Item_Key = siv.Item_Key)
		  JOIN Vendor				(nolock) v		ON (v.Vendor_Id = siv.Vendor_Id)
		  JOIN CostPromoCodeType	(nolock) cpc	ON (cpc.CostPromoCode = @CostPromoCode)
		  JOIN VendorDealType		(nolock) vdt	ON (vdt.Code = @VendorDealCode)
	   WHERE
		  ii.Identifier		= @UpcNo
		  AND v.Vendor_Key	= @Vendor_Key
END
