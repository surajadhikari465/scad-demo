
CREATE PROCEDURE [dbo].[UpdateItemPOSData]
	@Item_Key int,
	@Food_Stamps bit,
	@Price_Required bit,
	@Quantity_Required bit, 
	@QtyProhibit bit,
	@GroupList int,
	@Case_Discount bit,
	@Coupon_Multiplier bit,
	@FSA_Eligible bit,
	@Misc_Transaction_Sale smallint,
	@Misc_Transaction_Refund smallint,
	@Ice_Tare int,
	@Product_Code varchar(15),
	@Unit_Price_Category int
AS
 
BEGIN

/*********************************************************************************************
CHANGE LOG
DEV		DATE		TASK	Description
----------------------------------------------------------------------------------------------
DBS		20110125	1241	Merge Up FSA Changes

***********************************************************************************************/

	DECLARE @Product_CodeUpdated bit
	
	

	-- set the falgs to indicate whether interested fields got updated or not

	SELECT @Product_CodeUpdated = CASE	WHEN @Product_Code IS NOT NULL AND Product_Code IS NOT NULL AND @Product_Code <> Product_Code THEN 1
										WHEN (@Product_Code IS NULL AND Product_Code IS NOT NULL) OR (@Product_Code IS NOT NULL AND Product_Code IS NULL) THEN 1
										ELSE 0
										END
	FROM Item
	WHERE 
		Item_Key = @Item_Key

	-- UPDATE POS DATA IN THE ITEM TABLE FOR THE ASSOCIATED ITEM_KEY
	UPDATE Item SET 
        Food_Stamps = @Food_Stamps,
        Price_Required = @Price_Required,
        Quantity_Required = @Quantity_Required,
        QtyProhibit = @QtyProhibit,
        GroupList = @GroupList,
        Case_Discount = @Case_Discount,
		Coupon_Multiplier = @Coupon_Multiplier,
		Misc_Transaction_Sale = @Misc_Transaction_Sale,
		Misc_Transaction_Refund = @Misc_Transaction_Refund,
		Ice_Tare = @Ice_Tare,
		Product_Code = @Product_Code,
		FSA_Eligible = @FSA_Eligible,
		Unit_Price_Category = @Unit_Price_Category
	WHERE Item_Key = @Item_Key

	-- Queue event for mammoth to refresh its data.
	IF	@Product_CodeUpdated  = 1
	BEGIN
		EXEC [mammoth].[InsertItemLocaleChangeQueue] @Item_Key, NULL, 'ItemLocaleAddOrUpdate', NULL, NULL
	END
 
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateItemPOSData] TO [IRMAClientRole]
    AS [dbo];

