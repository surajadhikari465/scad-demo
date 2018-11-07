SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CheckPriceExist]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CheckPriceExist]
GO

CREATE PROCEDURE dbo.CheckPriceExist
	@Identifier varchar(20),
    @StoreList varchar(2000),
    @ListSeparator char(1)
AS
BEGIN
    SET NOCOUNT ON
    
    SELECT count(*) PriceCnt
    FROM Price
        INNER JOIN
            ItemIdentifier
            on ItemIdentifier.Item_key = Price.Item_Key
        INNER JOIN
            dbo.FN_Parse_List(@StoreList, @ListSeparator) Stores
            on Stores.Key_Value = Price.Store_no 
    WHERE Price.Price > 0 and ItemIdentifier.Identifier = @Identifier     
    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

