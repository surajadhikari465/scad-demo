   /****** Object:  StoredProcedure [dbo].[EPromotions_UnlockGroup]    Script Date: 05/31/2006 16:02:45 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[EPromotions_UnlockGroup]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[EPromotions_UnlockGroup]
GO
/****** Object:  StoredProcedure [dbo].[EPromotions_UnlockGroup]    Script Date: 05/31/2006 16:02:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE  dbo.EPromotions_UnlockGroup
	@GroupId int
AS 

BEGIN
    SET NOCOUNT ON
 
 UPDATE Itemgroup 
 SET IsEdited=null
 WHERE Group_ID = @GroupId
 
 SET NOCOUNT OFF
 
END
GO