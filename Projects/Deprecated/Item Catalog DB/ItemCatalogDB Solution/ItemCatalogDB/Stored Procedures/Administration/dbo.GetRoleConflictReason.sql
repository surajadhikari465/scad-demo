IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[GetRoleConflictReason]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[GetRoleConflictReason]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE dbo.GetRoleConflictReason
	@ConflictType varchar(1),
	@UserId int,
	@TitleId int
AS 
	IF @ConflictType = 'T'
		SELECT * FROM dbo.RoleConflictReason WHERE Title_Id = @TitleId AND ConflictType = 'T'
	
	IF @ConflictType = 'U'
		SELECT * FROM dbo.RoleConflictReason WHERE User_Id = @UserId AND ConflictType = 'U'

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO