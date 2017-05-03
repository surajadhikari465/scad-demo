CREATE PROCEDURE dbo.UpdateItemScaleData
	@Item_Key int,
	@ScaleDesc1 varchar(64),
	@ScaleDesc2 varchar(64),
	@ScaleDesc3 varchar(64),
	@ScaleDesc4 varchar(64),
	@Ingredients varchar(3500),
	@ShelfLife_Length smallint,
	@ShelfLife_ID int,	
	@Tare int,
	@UseBy int,
	@ForcedTare char(1)
AS

BEGIN
	-- UPDATE SCALE DATA IN THE ITEM TABLE FOR THE ASSOCIATED ITEM_KEY
	UPDATE Item
	SET ScaleDesc1 = @ScaleDesc1,
		ScaleDesc2 = @ScaleDesc2,
		ScaleDesc3 = @ScaleDesc3,
		ScaleDesc4 = @ScaleDesc4,
		Ingredients = @Ingredients,
		ShelfLife_Length = @ShelfLife_Length,
		ShelfLife_ID = @ShelfLife_ID,		
		ScaleTare = @Tare,
		ScaleUseBy = @UseBy,
		ScaleForcedTare = @ForcedTare
	WHERE Item_Key = @Item_Key		
 
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateItemScaleData] TO [IRMAClientRole]
    AS [dbo];

