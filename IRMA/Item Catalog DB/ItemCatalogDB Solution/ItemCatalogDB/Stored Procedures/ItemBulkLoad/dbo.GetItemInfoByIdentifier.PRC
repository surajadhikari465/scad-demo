SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetItemInfoByIdentifier]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetItemInfoByIdentifier]
GO


CREATE PROCEDURE dbo.GetItemInfoByIdentifier
    @Identifier varchar(20)
AS 

BEGIN
    SET NOCOUNT ON
    
    SELECT Item.*
    FROM Item (NOLOCK)
    INNER JOIN
        ItemIdentifier (NOLOCK)
        ON ItemIdentifier.Item_Key = Item.Item_Key 
    WHERE ItemIdentifier.Identifier = @Identifier
     AND NOT Deleted_Identifier = 1
    
    SET NOCOUNT OFF

END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


