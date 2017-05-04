   /****** Object:  StoredProcedure [dbo].[EPromotions_GetGroupEditStatus]    Script Date: 05/31/2006 16:02:45 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[EPromotions_GetGroupEditStatus]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[EPromotions_GetGroupEditStatus]
GO
/****** Object:  StoredProcedure [dbo].[EPromotions_GetGroupEditStatus]    Script Date: 05/31/2006 16:02:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE  dbo.EPromotions_GetGroupEditStatus
	@GroupId int
AS 

BEGIN
    SET NOCOUNT ON
	
	SELECT EditUserId=IsEdited, IsEdited=case when IsEdited is null then 'No' else 'Yes' end , EditedBy=case when IsEdited is not null then IsNull(FullName, 'Unknown') else null end
	FROM ItemGroup left join Users on ItemGroup.IsEdited = Users.User_Id
	WHERE Group_Id =  @GroupId
    SET NOCOUNT OFF

END


GO
   