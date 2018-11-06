SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetItemIdentifersForItem]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetItemIdentifersForItem]
GO

CREATE PROCEDURE dbo.GetItemIdentifersForItem
    @Item_Key int
AS

BEGIN
    SET NOCOUNT ON
 
    SELECT II.Identifier
    FROM ItemIdentifier II (nolock)
    WHERE II.Item_Key = @Item_Key

    SET NOCOUNT OFF
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

--GRANT  EXECUTE  ON [dbo].[GetItemIdentifersForItem]  TO IRSUser
GO

