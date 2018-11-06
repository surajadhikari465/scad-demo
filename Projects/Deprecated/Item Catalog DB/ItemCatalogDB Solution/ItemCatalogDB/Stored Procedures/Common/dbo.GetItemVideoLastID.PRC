SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetItemVideoLastID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetItemVideoLastID]
GO


CREATE PROCEDURE dbo.GetItemVideoLastID
@Item_Key int
AS 

SELECT MAX(ItemVideo_ID) AS ItemVideo_ID
FROM ItemVideo
WHERE Item_Key = @Item_Key


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


