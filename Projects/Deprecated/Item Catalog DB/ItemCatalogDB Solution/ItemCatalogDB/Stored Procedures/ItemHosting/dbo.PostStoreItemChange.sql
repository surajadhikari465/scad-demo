SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PostStoreItemChange]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dbo].[PostStoreItemChange] AS BEGIN SET NOCOUNT ON; END')
GO

ALTER PROCEDURE dbo.PostStoreItemChange 
    @Item_Key int, 
    @Store_No int, 
    @Restricted_Hours bit,
    @IBM_Discount bit,
    @NotAuthorizedForSale bit,
    @CompetitiveItem bit,
    @PosTare int,
    @LinkedItem int,
    @GrillPrint bit,
    @AgeCode int,
    @VisualVerify bit,
    @SrCitizenDiscount bit,
    @SubTeam_No int,
    @POSLinkCode varchar(10),
	@KitchenRoute_ID int,
	@Routing_Priority smallint,
	@Consolidate_Price_To_Prev_Item bit,
	@Print_Condiment_On_Receipt bit,
	@Age_Restrict bit,
	@AuthorizedItem bit,
	@MixMatch int,
	@Discountable bit,
	@Refresh bit,
	@LocalItem bit,
	@ItemSurcharge int
AS 

BEGIN
	-- Create the Price table relationship, if it does not already exist for the Store-Item
    IF NOT EXISTS(SELECT * FROM Price WHERE Item_Key = @Item_Key AND Store_No = @Store_No)
		INSERT INTO Price (Item_Key, Store_No) VALUES (@Item_Key, @Store_No)

	-- Update the values on the Price table that maintains Store-Item relationship data
    UPDATE Price 
    SET Restricted_Hours = @Restricted_Hours,
        IBM_Discount = @IBM_Discount,
        NotAuthorizedForSale = @NotAuthorizedForSale,
        CompFlag = @CompetitiveItem,
        PosTare = @PosTare,
        LinkedItem = @LinkedItem,
        GrilLPrint = @GrillPrint,
        AgeCode = @AgeCode,
        VisualVerify = @VisualVerify,
        SrCitizenDiscount = @SrCitizenDiscount,
        ExceptionSubTeam_No = @SubTeam_No,
        POSLinkCode = @POSLinkCode,
        KitchenRoute_ID = @KitchenRoute_ID,
		Routing_Priority = @Routing_Priority,
		Consolidate_Price_To_Prev_Item = @Consolidate_Price_To_Prev_Item,
		Print_Condiment_On_Receipt = @Print_Condiment_On_Receipt,
		Age_Restrict = @Age_Restrict,
		MixMatch = @MixMatch,
		Discountable = @Discountable,
		LocalItem = @LocalItem,
		ItemSurcharge = @ItemSurcharge
    WHERE Item_Key = @Item_Key AND Store_No = @Store_No
    
    -- Update the values on the StoreItem table that maintains Store-Item relationship data
    EXEC dbo.UpdateStoreItem @Item_Key, @Store_No, @AuthorizedItem, @Refresh
    
    IF @AuthorizedItem = 0
		UPDATE PriceBatchDetail
			SET Expired = 1,ReAuthFlag = 0
		WHERE 
			Item_Key = @Item_Key
			AND Store_No = @Store_No 
			AND ReAuthFlag = 1
			AND PriceBatchHeaderId IS NULL
			AND ItemChgTypeId = 1
			AND Expired = 0

	-- Create ItemLocale event for sending data to Icon for Mammoth purposes. Configuration is checked in stored procedure.
	EXEC [mammoth].[InsertItemLocaleChangeQueue] @Item_Key, @Store_No, 'ItemLocaleAddOrUpdate'

END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

