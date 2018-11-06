SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetItemName]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetItemName]
GO


CREATE PROCEDURE dbo.GetItemName 
@Item_Key int 
AS 


SELECT Item.Item_Key, Item.Item_Description, Identifier
FROM Item INNER JOIN ItemIdentifier ON (ItemIdentifier.Item_Key = Item.Item_Key AND ItemIdentifier.Default_Identifier = 1)
WHERE Item.Item_Key = @Item_Key
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


