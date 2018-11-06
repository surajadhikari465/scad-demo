SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[InsertRoleConflictReason]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[InsertRoleConflictReason]
GO

CREATE PROCEDURE dbo.InsertRoleConflictReason
	@ConflictType varchar(1),
	@UserId int,
	@TitleId int,
	@Role1 varchar(50),
	@Role2 varchar(50),
	@Reason varchar(max),
	@InsertUserId int
AS 
	IF @ConflictType = 'T'
		INSERT INTO dbo.RoleConflictReason (ConflictType, User_Id, Title_ID, Role1, Role2, RoleConflictReason, InsertUserId, InsertDate) 
				VALUES (@ConflictType, NULL, @TitleId, @Role1, @Role2, @Reason, @InsertUserId, GETDATE())	
	
	IF @ConflictType = 'U'
		INSERT INTO dbo.RoleConflictReason (ConflictType, User_Id, Title_ID, Role1, Role2, RoleConflictReason, InsertUserId, InsertDate) 
			VALUES (@ConflictType, @UserId, NULL, @Role1, @Role2, @Reason, @InsertUserId, GETDATE())

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO