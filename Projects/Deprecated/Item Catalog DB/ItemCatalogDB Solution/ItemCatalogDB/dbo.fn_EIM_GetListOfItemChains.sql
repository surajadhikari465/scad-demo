IF EXISTS ( SELECT  *
            FROM    SYSOBJECTS
            WHERE   NAME = 'fn_EIM_GetListOfItemChains' ) 
    DROP FUNCTION dbo.fn_EIM_GetListOfItemChains
    GO
-- Return a comma concatonated string of chain ids for

-- the provided item key.

CREATE FUNCTION dbo.fn_EIM_GetListOfItemChains ( @Item_Key INT )
RETURNS VARCHAR(1000)
AS 
    BEGIN



        DECLARE @ChainDelimetedString AS VARCHAR(4000)

           

        SELECT  @ChainDelimetedString = COALESCE(@ChainDelimetedString + ', ',
                                                 '')
                + CAST(ItemChain.ItemChainID AS VARCHAR(200))
        FROM    ItemChainItem  (NOLOCK)
                JOIN ItemChain  (NOLOCK) ON ItemChain.ItemChainId = ItemChainItem.ItemChainId
        WHERE   ItemChainItem.Item_Key = @Item_Key
        ORDER BY ItemChainDesc



        RETURN @ChainDelimetedString

    

    END
GO


