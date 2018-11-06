SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DeleteRoleConflict]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[DeleteRoleConflict]
GO

CREATE PROCEDURE dbo.DeleteRoleConflict
	@ConflictId int
AS 
	DELETE FROM dbo.RoleConflicts WHERE ConflictId = @ConflictId

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO   