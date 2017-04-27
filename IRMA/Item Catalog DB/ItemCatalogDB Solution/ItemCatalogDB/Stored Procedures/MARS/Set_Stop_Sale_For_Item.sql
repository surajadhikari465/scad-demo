IF NOT EXISTS (	SELECT * FROM dbo.sysobjects WHERE OBJECTPROPERTY(id, N'IsProcedure') = 1 AND
	id = OBJECT_ID(N'[dbo].[Set_Stop_Sale_For_Item]') 
)
BEGIN
	EXEC ('CREATE PROCEDURE dbo.Set_Stop_Sale_For_Item AS SELECT 1')
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[Set_Stop_Sale_For_Item](
	@StoreNumber int, 
	@ItemIdentifier varchar(20), 
	@StopSale bit
)
AS
	BEGIN
	SET NOCOUNT ON;
	
	DECLARE @CurrentTime datetime;
	SET @CurrentTime = CURRENT_TIMESTAMP;
		
	UPDATE 
		p
	SET 
		p.NotAuthorizedForSale = @StopSale
	FROM
		Price p
		JOIN ItemIdentifier ii ON p.Item_Key = ii.Item_Key
	WHERE 
		ii.Identifier = @ItemIdentifier
		AND p.Store_No = @StoreNumber

	--Delete PriceBatchDetail record from PriceAddUpdate Trigger in order to reduce noise from MARS requests
	DELETE PriceBatchDetail
	where PriceBatchDetail.PriceBatchDetailID = 
		(select p.PriceBatchDetailID from PriceBatchDetail p 
		join ItemIdentifier ii on p.Item_Key = ii.Item_Key
		where ii.Identifier = @ItemIdentifier
		and p.Store_No = @StoreNumber
		and p.Insert_Date >= @CurrentTime)	
END
go


