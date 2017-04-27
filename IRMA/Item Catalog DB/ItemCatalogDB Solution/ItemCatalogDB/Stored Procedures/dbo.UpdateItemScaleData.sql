/****** Object:  StoredProcedure [dbo].[UpdateItemScaleData]    Script Date: 08/29/2006 15:54:40 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[UpdateItemScaleData]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[UpdateItemScaleData]
GO

/****** Object:  StoredProcedure [dbo].[UpdateItemScaleData]    Script Date: 08/29/2006 15:54:40 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

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


 