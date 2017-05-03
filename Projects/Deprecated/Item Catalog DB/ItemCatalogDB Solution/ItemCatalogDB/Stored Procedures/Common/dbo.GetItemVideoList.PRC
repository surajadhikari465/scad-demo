SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetItemVideoList]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetItemVideoList]
GO


CREATE PROCEDURE dbo.GetItemVideoList
@Item_Key int
AS 

SELECT ItemVideo_ID, Sequence_ID, PageDescription
FROM ItemVideo
WHERE Item_Key = @Item_Key


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


