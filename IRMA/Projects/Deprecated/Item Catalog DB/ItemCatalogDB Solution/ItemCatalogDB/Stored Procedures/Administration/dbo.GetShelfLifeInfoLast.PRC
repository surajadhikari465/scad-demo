SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetShelfLifeInfoLast]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetShelfLifeInfoLast]
GO


CREATE PROCEDURE dbo.GetShelfLifeInfoLast
AS 

SELECT ShelfLife_Name, ShelfLife_ID, User_ID 
FROM ItemShelfLife
WHERE ShelfLife_ID = (SELECT MAX(ShelfLife_ID) FROM ItemShelfLife)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



