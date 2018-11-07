SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetItemVideo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetItemVideo]
GO


CREATE PROCEDURE dbo.GetItemVideo
@ItemVideo_ID int
AS 

SELECT PageDescription, PageText
FROM ItemVideo
WHERE ItemVideo_ID = @ItemVideo_ID


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


