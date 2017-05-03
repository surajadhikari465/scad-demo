IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetPriceBatchDetailIDs]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetPriceBatchDetailIDs]
GO

CREATE PROCEDURE dbo.GetPriceBatchDetailIDs
    @ItemKeyList varchar(8000),
    @ItemKeyListSep char(1) 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DECLARE @List TABLE (Item_Key int)

    INSERT INTO @List (Item_Key)
    SELECT Key_Value 
    FROM dbo.fn_Parse_List(@ItemKeyList, @ItemKeyListSep) L

	SELECT 
		PriceBatchDetailID 
	FROM 
		PriceBatchDetail PBD
	INNER JOIN 
            @List L
            ON L.Item_Key = PBD.Item_Key
	WHERE 
		PriceBatchHeaderID IS NULL 
		and ItemChgTypeID = 2
END
GO
