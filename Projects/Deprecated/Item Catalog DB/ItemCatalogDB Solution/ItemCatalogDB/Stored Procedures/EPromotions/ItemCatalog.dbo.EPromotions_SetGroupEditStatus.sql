  /****** Object:  StoredProcedure [dbo].[EPromotions_SetGroupEditStatus]    Script Date: 05/31/2006 16:02:45 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[EPromotions_SetGroupEditStatus]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[EPromotions_SetGroupEditStatus]
GO
/****** Object:  StoredProcedure [dbo].[EPromotions_SetGroupEditStatus]    Script Date: 05/31/2006 16:02:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE  dbo.EPromotions_SetGroupEditStatus
	@GroupId int,
	@Status int
AS 

BEGIN
    SET NOCOUNT ON
    
	--if @Status = -99 then the group is being marked Available to edit. (null)	
    if @Status=-99 	 set @Status = null
	

	-- sets the IsEdited field for a specific Group. null = Available to be edited. integer > 0 = UserId of the user that is currently editing the group.
	Update ItemGroup 
	Set IsEdited = @Status
	Where Group_Id = @GroupId

    SET NOCOUNT OFF

END


GO
   