SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CheckForDuplicateItemQuantity]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CheckForDuplicateItemQuantity]
GO


CREATE PROCEDURE dbo.CheckForDuplicateItemQuantity 
@Store_No int, 
@Item_Key int 
AS 

SELECT COUNT(*) AS ItemQuantityCount 
FROM ItemQuantity
WHERE Store_No = @Store_No AND Item_Key = @Item_Key
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



