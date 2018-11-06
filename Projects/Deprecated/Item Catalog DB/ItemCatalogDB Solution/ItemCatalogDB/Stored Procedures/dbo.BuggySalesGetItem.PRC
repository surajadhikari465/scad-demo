SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[BuggySalesGetItem]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[BuggySalesGetItem]
GO


CREATE PROCEDURE dbo.BuggySalesGetItem
@mskUPC int
AS

SELECT Item_Key 
FROM ItemIdentifier 
WHERE Deleted_Identifier = 0 AND Identifier = @mskUPC 



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

