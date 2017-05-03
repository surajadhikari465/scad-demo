SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CheckForDuplicateInvLocationItem]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CheckForDuplicateInvLocationItem]
GO


CREATE PROCEDURE dbo.CheckForDuplicateInvLocationItem
@InvLocID int,
@ItemID int

AS

SET NOCOUNT ON

SELECT 
	Count(InvLocID) as Found
FROM 
	InventoryLocationItems (nolock)
WHERE 
	InvLocID = @InvLocID and Item_Key = @ItemID

SET NOCOUNT OFF
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
