SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetItemMultipleUPCs]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetItemMultipleUPCs]
GO


CREATE PROCEDURE dbo.GetItemMultipleUPCs
@Item_Key int
AS

SELECT UPC
FROM ItemUPC
WHERE Item_Key = @Item_Key AND UPC <> '0'
ORDER BY UPC

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


